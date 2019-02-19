using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Diagnostics;
using System.Xml;

namespace WpfApplication1
{
    class OneNoteBook
    {
        List<Section> sections = new List<Section>();
        XmlNode node;

        public OneNoteBook(XmlNode node, KonfigurationOneNote onenoteConf)
        {
            this.node = node;
            addSections(onenoteConf);
        }

        private void addSections(KonfigurationOneNote onenoteConf)
        {
            foreach (XmlNode childNode in node.ChildNodes)
            {
                switch (childNode.Name)
                {
                    case "one:Section":
                        // if not deleted
                        // if (childNode.Attributes[""])
                        sections.Add(new Section(childNode, this, onenoteConf));
                        break;
                    default:
                        Debug.WriteLine("ERROR: Unknown type for section element: " + childNode.Name);
                        // ignore type
                        break;
                }
            }
        }

        public void saveOneNoteBook(KonfigurationOneNote onenoteConf)
        {
            foreach (Section section in sections)
            {
                section.savePages(onenoteConf);
            }

            Debug.WriteLine("Finished section");
        }

        public void saveOneNoteBookCfg(KonfigurationOneNote onenoteConf)
        {
            System.IO.Directory.CreateDirectory(ExportOneNotePages.GetOutPutDir(onenoteConf.notebook));
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(ExportOneNotePages.GetOutPutDir(onenoteConf.notebook) + @"\guides.json"))
            {
                file.Write(getBookCfg(onenoteConf));

                file.Flush();
                file.Close();
            }

            Debug.WriteLine("Finished Config");
        }

        public String getBookCfg(KonfigurationOneNote onenoteConf)
        {
            String itemsCfg = "";

            foreach (Section section in sections)
            {
                if (!itemsCfg.Equals(""))
                {
                    itemsCfg += ",";
                }
                itemsCfg += section.getPagesCfg(onenoteConf);
            }

            return "["
                + itemsCfg
            + "]";
        }

        public Page findPage(String onenodeId)
        {
            foreach (Section section in this.sections)
            {
                Page page = section.findPage(onenodeId);
                if (page != null)
                {
                    return page;
                }
            }
            return null;
        }

    }
}
