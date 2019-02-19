using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Diagnostics;
using System.Web;
using System.Xml;

namespace WpfApplication1
{
    class Paragraph
    {
        Page page;
        XmlNode node;
        String quickStyleIndex = "p"; // default is p == paragraph
        List<Content> contents = new List<Content>();
        List<string> tags = new List<string>();
        bool isHome = false; // default is false
        String preLocaleTag = "sol.locale:";

        public Paragraph(Page page, XmlNode node, KonfigurationOneNote onenoteConf)
        {
            this.page = page;
            this.node = node;
            if (node.Attributes["quickStyleIndex"] != null)
            {
                this.quickStyleIndex = page.getQuickStyleNameForId(node.Attributes["quickStyleIndex"].InnerText);
            }
            readTagsForParagraph();
            addAllChildNodesAsContent(onenoteConf);
        }


        private void readTagsForParagraph()
        {
            foreach (XmlNode childNode in node.ChildNodes)
            {
                switch (childNode.Name)
                {
                    case "one:Tag":
                        if (childNode.Attributes["index"] != null)
                        {
                            String name = page.tagDef[childNode.Attributes["index"].InnerText];
                            if (name != null)
                            {
                                tags.Add(name);
                            }
                        }
                        break;
                }
            }
        }

        private void addAllChildNodesAsContent(KonfigurationOneNote onenoteConf)
        {
            foreach (XmlNode childNode in node.ChildNodes)
            {
                switch (childNode.Name)
                {

                    case "one:Table":
                        contents.Add(new ContentTable(childNode, page, onenoteConf));
                        break;
                    case "one:T":
                        contents.Add(new ContentText(childNode, page));
                        break;
                    case "one:List":
                        contents.Add(new ContentList(childNode, page));
                        break;
                    case "one:Image":
                        // Check, if thumbnail tag is set
                        if (tags.Contains(onenoteConf.onenoteTags.thumbnailTag))
                        {
                            Debug.WriteLine("thumbnailTag");
                            // Set image as thumbnail to page and set paragraph to home
                            ContentImage img = new ContentImage(childNode, page);
                            page.setThumbnail(img);
                            this.isHome = true;
                        }
                        else
                        {
                            // check for locale tag
                            if (checkLocationTag(onenoteConf))
                            {
                                ContentImage img = new ContentImage(childNode, page);
                                contents.Add(img);
                                page.addImage(img);
                            }
                        }
                        break;
                    default:
                        Debug.WriteLine("ERROR: Unknown type for paragraph: " + childNode.Name);
                        // ignore type
                        break;
                }
            }
        }

        private String getLang(KonfigurationOneNote onenoteConf)
        {
            String lang = "DE";
            try
            {
                lang = onenoteConf.lang;
            }
            catch (Exception)
            {
            }
            return lang;
        }

        private Boolean checkLocationTag(KonfigurationOneNote onenoteConf)
        {
            // Check location, if selected
            String lang = getLang(onenoteConf);
            Boolean addImage = false;
            List<string> localeTags = new List<string>();
            String textTag = "";
            int i;

            if (tags.Count == 0)
            {
                addImage = true;
            }
            else
            {
                for (i = 0; i < tags.Count; i++)
                {
                    textTag = tags[i];
                    if (textTag.StartsWith(preLocaleTag))
                    {
                        localeTags.Add(textTag);
                    }
                }
                if (localeTags.Count == 0)
                {
                    addImage = true;
                }
                else
                {
                    for (i = 0; i < localeTags.Count; i++)
                    {
                        textTag = localeTags[i];
                        if (textTag.EndsWith(lang))
                        {
                            addImage = true;
                        }
                    }
                }
            }
            return addImage;
        }

        public String render(KonfigurationOneNote onenoteConf)
        {
            // ignore paragraph if home
            if (this.isHome)
            {
                return "";
            }

            String paragraph = "";
            foreach (Content content in contents)
            {
                paragraph += content.render(onenoteConf);
            }

            // apply styles to paragraph
            paragraph = renderStyles(paragraph);

            // apply message types to content
            paragraph = renderMessageTypes(paragraph, tags, onenoteConf);

            return paragraph;
        }

        private String renderStyles(String paragraphContent)
        {
            String prefix = "";
            String surfix = "";

            Boolean isEmpty = paragraphContent.Trim().Equals("");

            switch (quickStyleIndex)
            {
                case "h1":
                    if (!isEmpty) prefix = "## ";
                    break;
                case "h2":
                    if (!isEmpty) prefix = "### ";
                    break;
                case "h3":
                    if (!isEmpty) prefix = "#### ";
                    break;
                case "h4":
                    if (!isEmpty) prefix = "##### ";
                    break;
                case "h5":
                    if (!isEmpty) prefix = "###### ";
                    break;
                case "h6":
                    if (!isEmpty) prefix = "####### ";
                    break;
                case "code":
                    prefix = "    ";
                    paragraphContent = HttpUtility.HtmlDecode(paragraphContent);
                    break;
                case "blockquote":
                    prefix = "> ";
                    break;
                default:
                    break;
            }

            return prefix + paragraphContent + surfix;
        }

        // apply message types to content
        private string renderMessageTypes(String paragraphContent, List<string> tags, KonfigurationOneNote onenoteConf)
        {
            string pContent = "";
            string tagclass = "";
            foreach (string entry in tags)
            {


                // special tags
                if (entry.Equals(onenoteConf.onenoteTags.importantTag))
                {
                    tagclass = "tag_important";
                }

                if (entry.Equals(onenoteConf.onenoteTags.criticalTag))
                {
                    tagclass = "tag_critical";
                }

                if (entry.Equals(onenoteConf.onenoteTags.warningTag))
                {
                    tagclass = "tag_warning";
                }

                if (entry.Equals(onenoteConf.onenoteTags.cautionTag))
                {
                    tagclass = "tag_caution";
                }

                if (!tagclass.Equals(""))
                {
                    pContent = "<span class=\"" + tagclass + "\">" + paragraphContent + "</span>";
                }
                else
                {
                    pContent = paragraphContent;
                }

            }
            if (tags.Count == 0)
            {
                pContent = paragraphContent;
            }
            return pContent;
        }

        public bool firstContentIsText()
        {
            if (contents.Count > 0 && contents[0] is ContentText
               && this.quickStyleIndex.Equals("p"))
            {
                return true;
            }
            return false;
        }
    }
}
