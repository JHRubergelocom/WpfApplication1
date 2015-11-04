using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace WpfApplication1
{
    public class KonfigurationParameter
    {
        private KonfigurationIx _ixConf;

        public KonfigurationIx ixConf
        {
            get
            {
                return _ixConf;
            }
            set
            {
                _ixConf = value;
            }
        }

        public KonfigurationParameter(KonfigurationIx ixConf)
        {
            this._ixConf = ixConf;
        }

        public KonfigurationParameter()
        {
            this._ixConf = new KonfigurationIx();
        }

        public KonfigurationParameter(XmlNode profileNode)
        {
            this._ixConf = new KonfigurationIx();

            foreach (XmlNode subNode in profileNode.ChildNodes)
            {
                switch (subNode.Name)
                {
                    case "ixConf":
                        _ixConf = new KonfigurationIx(subNode);
                        break;
                }
            }
        }

        private XmlElement CreateXMLNodeValue(XmlDocument xmlDoc, string nodeName, string nodeValue)
        {
            XmlElement node = xmlDoc.CreateElement(nodeName);
            node.InnerText = nodeValue;
            return node;
        }


        public XmlElement CreateXMLNode(XmlDocument xmlDoc, string profilename) 
        {
            XmlElement profileElem = CreateXMLNodeValue(xmlDoc, "profile", profilename);

            XmlElement nodeElem = ixConf.CreateXMLNode(xmlDoc);
            profileElem.AppendChild(nodeElem);

            return profileElem;

        }
    }

}
