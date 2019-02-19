using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Xml;

namespace WpfApplication1
{
    abstract class Content
    {
        protected XmlNode node;
        protected Page page;
        public Content()
        {

        }
        public Content(XmlNode node, Page page)
        {
            this.node = node;
            this.page = page;
        }

        public abstract String render(KonfigurationOneNote onenoteConf);
    }
}
