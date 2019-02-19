using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Xml;

namespace WpfApplication1
{
    class ContentText : Content
    {
        public ContentText(XmlNode node, Page page) : base(node, page) { }

        public override String render(KonfigurationOneNote onenoteConf)
        {
            String richText = node.InnerText;
            return richText;
        }
    }
}
