using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Xml;

namespace WpfApplication1
{
    class ContentList : Content
    {
        public ContentList(XmlNode node, Page page) : base(node, page) { }

        public override String render(KonfigurationOneNote onenoteConf)
        {
            XmlNode firstChild = node.FirstChild;
            if (firstChild.Name == "one:Bullet"
                && firstChild.Attributes["bullet"] != null)
            {
                /**
                 * List Templates:
                 * -   List Item
                 * *   List Item
                 */
                switch (firstChild.Attributes["bullet"].InnerText)
                {
                    case "25":
                        return "-   ";
                    default:
                        return "*   ";
                }
            }
            else if (firstChild.Name == "one:Number"
                && firstChild.Attributes["text"] != null)
            {
                /**
                 * Numeric List Templates:
                 * 1.   List Item
                 * 2.   List Item
                 */
                return firstChild.Attributes["text"].InnerText + "   ";
            }
            return "";
        }
    }
}
