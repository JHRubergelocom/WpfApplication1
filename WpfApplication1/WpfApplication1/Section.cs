using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Diagnostics;
using System.Xml;
using Microsoft.Office.Interop.OneNote;

namespace WpfApplication1
{
    class Section
    {

        OneNoteBook book;
        String id;
        String name;
        List<Page> pages = new List<Page>();
        XmlNode node;

        public Section(XmlNode node, OneNoteBook book, KonfigurationOneNote onenoteConf)
        {
            this.book = book;
            this.node = node;

            if (node.Attributes != null && node.Attributes["ID"] != null)
            {
                id = node.Attributes["ID"].InnerText;
            }
            if (node.Attributes != null && node.Attributes["name"] != null)
            {
                name = node.Attributes["name"].InnerText;
            }

            addPages(onenoteConf);
        }

        private void addPages(KonfigurationOneNote onenoteConf)
        {
            int lastLevel = 1;
            Page parentPage = null;
            Page lastPage = null;

            foreach (XmlNode childNode in node.ChildNodes)
            {
                switch (childNode.Name)
                {
                    case "one:Page":
                        Page page = getPage(childNode, onenoteConf);

                        // get level
                        if (childNode.Attributes["pageLevel"] != null)
                        {
                            int level = Int16.Parse(childNode.Attributes["pageLevel"].InnerText);
                            page.pageLevel = level;
                            if (level > lastLevel)
                            {
                                parentPage = lastPage;
                            }
                            if (level < lastLevel)
                            {
                                int diff = lastLevel - level;
                                if (level != 1)
                                {
                                    parentPage = lastPage.parent;
                                    for (int i = diff; i > 0; i--)
                                    {
                                        if (parentPage != null)
                                        {
                                            parentPage = parentPage.parent;
                                        }
                                    }
                                }
                                else
                                {
                                    parentPage = null;
                                }
                            }
                            lastLevel = level;
                        }

                        if (!page.ignore(onenoteConf))
                        {
                            if ((lastPage == null || parentPage == null) && page != null)
                            {
                                pages.Add(page);
                            }
                            else
                            {
                                parentPage.addChildPage(page);
                            }
                        }
                        else
                        {
                            Debug.WriteLine("Ignoring Page: " + page.id);
                        }

                        lastPage = page;
                        break;
                    default:
                        Debug.WriteLine("ERROR: Unknown type for section element: " + childNode.Name);
                        // ignore type
                        break;
                }
            }
        }



        private Page getPage(XmlNode node, KonfigurationOneNote onenoteConf)
        {
            if (node.Attributes["ID"] != null)
            {
                String pageContentXml;
                ExportOneNotePages.oneNote.GetPageContent(node.Attributes["ID"].InnerText, out pageContentXml, PageInfo.piAll);

                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(pageContentXml);

                Page page = new Page(xmlDoc, node.Attributes["ID"].InnerText, this.book, onenoteConf);
                return page;
            }

            return null;
        }

        public void savePages(KonfigurationOneNote onenoteConf)
        {
            foreach (Page page in pages)
            {
                page.saveMDFile(onenoteConf);
            }

            Debug.WriteLine("Finished page");
        }

        public String getPagesCfg(KonfigurationOneNote onenoteConf)
        {
            String itemsCfg = "";
            bool expanded = false;

            foreach (Page page in pages)
            {
                if (page.tags.Contains(onenoteConf.onenoteTags.expandedTag))
                {
                    expanded = true;
                }
                if (!itemsCfg.Equals(""))
                {
                    itemsCfg += ",";
                }
                itemsCfg += page.getPageJsonConfig(onenoteConf);
            }

            return "{"
            + "\"title\": \"" + JSONHelpers.EscapeStringValue(this.name) + "\","
            + (expanded ? "\"expanded\": true," : "")
            + "\"items\": [" + itemsCfg + "]"
            + "}";
        }

        override public String ToString()
        {
            return "Section: " + name;
        }

        public Page findPage(String onenodeId)
        {
            foreach (Page page in this.pages)
            {
                Page found = page.findPage(onenodeId);
                if (found != null)
                {
                    return found;
                }
            }
            return null;
        }
    }
}
