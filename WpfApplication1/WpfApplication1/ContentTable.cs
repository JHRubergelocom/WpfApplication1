using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Diagnostics;
using System.Xml;

namespace WpfApplication1
{
    class ContentTable : Content
    {
        List<List<List<Paragraph>>> table = new List<List<List<Paragraph>>>();

        public ContentTable(XmlNode node, Page page, KonfigurationOneNote onenoteConf)
        {
            this.node = node;
            this.page = page;

            this.createTableStructure(onenoteConf);
        }

        public void createTableStructure(KonfigurationOneNote onenoteConf)
        {
            foreach (XmlNode childNode in node.ChildNodes)
            {
                switch (childNode.Name)
                {
                    case "one:Row":
                        table.Add(createTableRow(childNode, onenoteConf));
                        break;
                    default:
                        Debug.WriteLine("ERROR: Unknown type for table: " + childNode.Name);
                        // ignore type
                        break;
                }
            }
        }

        public List<List<Paragraph>> createTableRow(XmlNode node, KonfigurationOneNote onenoteConf)
        {
            List<List<Paragraph>> row = new List<List<Paragraph>>();
            foreach (XmlNode childNode in node.ChildNodes)
            {
                switch (childNode.Name)
                {
                    case "one:Cell":
                        if (childNode.FirstChild != null)
                        {
                            List<Paragraph> column = new List<Paragraph>();
                            XmlNodeList oeNodes = childNode.FirstChild.ChildNodes;
                            foreach (XmlNode oeNode in oeNodes)
                            {
                                switch (oeNode.Name)
                                {
                                    case "one:OE":
                                        Paragraph paragraph = new Paragraph(this.page, oeNode, onenoteConf);
                                        column.Add(paragraph);
                                        break;
                                    default:
                                        break;
                                }
                            }

                            row.Add(column);
                        }
                        break;
                    default:
                        Debug.WriteLine("ERROR: Unknown type for table: " + childNode.Name);
                        // ignore type
                        break;
                }
            }
            return row;
        }

        public override String render(KonfigurationOneNote onenoteConf)
        {
            String tableOutput = "";

            foreach (List<List<Paragraph>> row in this.table)
            {
                Boolean isHeader = tableOutput.Equals("");
                String rowOutput = "<tr>";
                String headingOutputPre = "";
                String headingOutputAfter = "";
                foreach (List<Paragraph> cell in row)
                {
                    String content = "";
                    foreach (Paragraph paragraph in cell)
                    {
                        if (!content.Equals(""))
                        {
                            content += "<br />";
                        }
                        content += paragraph.render(onenoteConf);
                    }
                    rowOutput += "<td>" + content + "</td>";

                }
                rowOutput += "</tr>";
                if (isHeader)
                {
                    headingOutputPre = "<table><thead>";
                    headingOutputAfter = "</thead><tbody>";
                }
                tableOutput += headingOutputPre + rowOutput + headingOutputAfter;
            }

            tableOutput += "</tbody></table>";

            return tableOutput;
        }
    }
}
