using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Diagnostics;
using System.Xml;
using System.Text.RegularExpressions;
using System.IO;
using HtmlAgilityPack;
using System.Web;

namespace WpfApplication1
{
    class Page
    {
        private OneNoteBook book;
        public static int count = 0;

        XmlDocument node;
        public String id;
        public String linkPageId;
        String name;
        String idName = null;
        public int pageLevel;
        public Page parent;
        List<Page> childPages = new List<Page>();
        int imageIdx = 0;

        List<Paragraph> paragraphs = new List<Paragraph>();
        List<ContentImage> images = new List<ContentImage>();
        Dictionary<string, string> styles = new Dictionary<string, string>();
        public Dictionary<string, string> tagDef = new Dictionary<string, string>();
        public HashSet<string> tags = new HashSet<string>();
        ContentImage thumbnail = null; // thumbnail image

        public Page(XmlDocument node, String id, OneNoteBook book, KonfigurationOneNote onenoteConf)
        {
            this.book = book;
            this.node = node;
            this.id = id;

            // generate Page link
            String link;
            ExportOneNotePages.oneNote.GetHyperlinkToObject(this.id, null, out link);
            this.linkPageId = getLinkPageId(link);

            string strNamespace = "http://schemas.microsoft.com/office/onenote/2013/onenote";
            XmlNamespaceManager nsmgr = new XmlNamespaceManager(node.NameTable);
            nsmgr.AddNamespace("one", strNamespace);

            XmlNode pageNode = node.SelectSingleNode("//one:Page", nsmgr);
            if (pageNode.Attributes != null && pageNode.Attributes["name"] != null)
            {
                name = pageNode.Attributes["name"].InnerText;
            }

            // read quickstyle definitions
            XmlNodeList quickStyleNodes = node.SelectNodes("//one:QuickStyleDef", nsmgr);
            foreach (XmlNode quickStyleNode in quickStyleNodes)
            {
                if (quickStyleNode.Attributes["index"] != null && quickStyleNode.Attributes["name"] != null)
                {
                    styles.Add(quickStyleNode.Attributes["index"].InnerText, quickStyleNode.Attributes["name"].InnerText);
                }
            }

            // read tag definitions
            XmlNodeList tagDefNodes = node.SelectNodes("//one:TagDef", nsmgr);
            foreach (XmlNode tagDefNode in tagDefNodes)
            {
                if (tagDefNode.Attributes["index"] != null && tagDefNode.Attributes["name"] != null)
                {
                    tagDef.Add(tagDefNode.Attributes["index"].InnerText, tagDefNode.Attributes["name"].InnerText);
                }
            }

            // read tags
            XmlNodeList tagNodes = node.SelectNodes("//one:Tag", nsmgr);
            foreach (XmlNode tagNode in tagNodes)
            {
                if (tagNode.Attributes["index"] != null)
                {
                    String name = tagDef[tagNode.Attributes["index"].InnerText];
                    if (name != null)
                    {
                        tags.Add(name);
                    }
                }
            }

            // read paragraphs
            XmlNodeList oeNodes = node.SelectNodes("//one:OE", nsmgr);
            foreach (XmlNode oeNode in oeNodes)
            {
                if (!this.ignoreNodeIfNameInParents(oeNode, "one:Table")
                    && !this.ignoreNodeIfNameInParents(oeNode, "one:Title"))
                {
                    paragraphs.Add(new Paragraph(this, oeNode, onenoteConf));
                }
            }
        }

        public Boolean ignoreNodeIfNameInParents(XmlNode node, String ignoreName)
        {
            XmlNode curr = node;
            while (curr.ParentNode != null)
            {
                curr = curr.ParentNode;
                if (curr.Name.Equals(ignoreName))
                {
                    return true;
                }
            }
            return false;
        }

        public String getQuickStyleNameForId(string id)
        {
            return styles[id];
        }

        public void saveMDFile(KonfigurationOneNote onenoteConf)
        {
            System.IO.Directory.CreateDirectory(this.getPagePath(onenoteConf));
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(this.getPagePath(onenoteConf) + @"README.md"))
            {
                file.WriteLine("# " + this.name);
                file.WriteLine("");
                foreach (Paragraph paragraph in paragraphs)
                {
                    String renderedText = paragraph.render(onenoteConf);

                    renderedText = this.RenderInternalLinks(renderedText);

                    file.WriteLine(renderedText);
                }

                file.Flush();
                file.Close();
            }

            foreach (Page page in childPages)
            {
                page.saveMDFile(onenoteConf);
            }

            foreach (ContentImage image in images)
            {
                image.saveImage(onenoteConf);
            }

            // rename thumbnail image as "icon.png"
            if (thumbnail != null)
            {
                thumbnail.renameToIcon();
                thumbnail.saveImage(onenoteConf);
            }

            try
            {
                this.readImages(onenoteConf);
            }
            catch (Exception e)
            {
                Debug.WriteLine("ERROR: reading images failed " + e.Message);
            }


            Debug.WriteLine("Finished page");
        }

        public void addChildPage(Page page)
        {
            childPages.Add(page);
            page.parent = this;
        }

        override public String ToString()
        {
            return "Page: " + name;
        }

        public String getCfgId()
        {
            if (this.idName == null)
            {
                Regex rgx = new Regex("[^a-zA-Z0-9]");
                String idname = rgx.Replace(this.name, "_");
                Page.count++;
                this.idName = "p" + Page.count + "_" + idname;
            }
            return this.idName;
        }

        public String getPageJsonConfig(KonfigurationOneNote onenoteConf)
        {
            String itemsCfg = "{"
                  + "\"name\": \"" + JSONHelpers.EscapeStringValue(this.getCfgId()) + "\","
                  + "\"url\": \"" + JSONHelpers.EscapeStringValue(this.getRenderPagePath()) + "\","
                  + "\"title\": \"" + JSONHelpers.EscapeStringValue(this.name) + "\","
                  + "\"description\": \"" + JSONHelpers.EscapeStringValue(this.getShortDesc(onenoteConf)) + "\""
                  + "}";

            if (childPages.Count > 0)
            {
                foreach (Page page in childPages)
                {

                    itemsCfg = itemsCfg + "," + page.getPageJsonConfig(onenoteConf);
                }
                bool expanded = false;
                if (this.tags.Contains(onenoteConf.onenoteTags.expandedTag))
                {
                    expanded = true;
                }
                return "{"
                + "\"title\": \"" + JSONHelpers.EscapeStringValue(this.name) + "\","
                + (expanded ? "\"expanded\": true," : "")
                + "\"items\": [" + itemsCfg + "]"
                + "}";
            }
            else
            {
                return itemsCfg;
            }
        }

        public void addImage(ContentImage image)
        {
            this.images.Add(image);
        }

        public int getNextImageId()
        {
            this.imageIdx++;
            return this.imageIdx++;
        }

        public String getPagePath(KonfigurationOneNote onenoteConf)
        {
            return ExportOneNotePages.GetOutPutDir(onenoteConf.notebook) + @"\" + this.getCfgId() + @"\";
        }

        public String getRenderPagePath()
        {
            return @"" + this.getCfgId() + @"/";
        }

        public void readImages(KonfigurationOneNote onenoteConf)
        {
            if (onenoteConf.renderImages == false)
            {
                return;
            }

            // read images from word export file since emf conversion does not handle antialiasing 
            if (images.Count == 0)
            {
                return;
            }

            string zipPath = ExportOneNotePages.GetTempDir(onenoteConf.notebook) + @"\" + this.getCfgId() + this.getCfgId() + ".docx";

            if (File.Exists(zipPath))
            {
                return;
            }
            ExportOneNotePages.oneNote.Publish(this.id, zipPath, Microsoft.Office.Interop.OneNote.PublishFormat.pfWord);
            string extractPath = ExportOneNotePages.GetTempDir(onenoteConf.notebook) + @"\" + this.getCfgId() + @"\";

            if (Directory.Exists(extractPath))
            {
                Directory.Delete(extractPath, true);
            }

            System.IO.Compression.ZipFile.ExtractToDirectory(zipPath, extractPath);

            try
            {
                String temp = ExportOneNotePages.GetTempDir(onenoteConf.notebook) + @"\" + this.getCfgId() + @"\word\media\";
                String[] filePaths = Directory.GetFiles(temp);
                int idx = 0;
                foreach (String file in filePaths)
                {
                    if (file.StartsWith(temp + "image"))
                    {
                        File.Move(file, getPagePath(onenoteConf) + images[idx].filename);
                    }
                    idx++;
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine("ERROR: Saving Images from Word failed. " + ex.Message);
            }
        }

        private string RenderInternalLinks(string html)
        {
            Regex regex = new Regex("href=\"(?<Link>.*?)\"",
                RegexOptions.IgnoreCase
                | RegexOptions.CultureInvariant
                | RegexOptions.IgnorePatternWhitespace
                | RegexOptions.Compiled);

            MatchCollection ms = regex.Matches(html);

            StringBuilder output = new StringBuilder(html);

            foreach (Match ma in ms)
            {
                if (ma.Value.StartsWith("href=\"onenote"))
                {
                    // find page id
                    String internalId = "";

                    if (ma.Value.Contains("page-id"))
                    {
                        String pageId = getLinkPageId(ma.Value);
                        Page page = book.findPage(pageId);
                        if (page != null)
                        {
                            internalId = page.getCfgId();
                        }
                    }
                    output = output.Replace(ma.Value, "href=\"#!/guide/" + internalId + "\"");
                }
            }

            return output.ToString();
        }

        private String getLinkPageId(String link)
        {
            string[] sp = link.Split(new string[] { "page-id={", "}" }, StringSplitOptions.None);
            if (sp.Count() >= 3)
            {
                String pageId = "{" + sp[2] + "}";
                return pageId;
            }
            return "";
        }

        public Page findPage(String onenodeId)
        {
            if (this.linkPageId.Equals(onenodeId))
            {
                return this;
            }
            foreach (Page page in this.childPages)
            {
                Page found = page.findPage(onenodeId);
                if (found != null)
                {
                    return found;
                }
            }
            return null;
        }

        public bool ignore(KonfigurationOneNote onenoteConf)
        {
            IEnumerator<String> enumerator = this.tags.GetEnumerator();
            while (enumerator.MoveNext())
            {
                if (onenoteConf.ignoreTags.Contains(enumerator.Current))
                {
                    return true;
                }
            }
            return false;
        }

        public String getShortDesc(KonfigurationOneNote onenoteConf)
        {
            foreach (Paragraph paragraph in paragraphs)
            {
                if (paragraph.firstContentIsText())
                {

                    String renderedText = paragraph.render(onenoteConf);
                    try
                    {
                        HtmlDocument doc = new HtmlDocument();
                        doc.LoadHtml(renderedText);
                        var text = doc.DocumentNode.SelectNodes("text()").Select(node => node.InnerText);
                        StringBuilder output = new StringBuilder();
                        foreach (string line in text)
                        {
                            output.AppendLine(line);
                        }
                        string textOnly = HttpUtility.HtmlDecode(output.ToString());
                        textOnly = textOnly.Replace("\r\n", "");
                        textOnly = textOnly.Replace("\n", "");
                        return textOnly;

                    }
                    catch (Exception ex) 
                    {
                        Debug.WriteLine(ex.Message);
                    }

                }
            }
            return "";
        }

        public void setThumbnail(ContentImage img)
        {
            this.thumbnail = img;
        }
    }
}
