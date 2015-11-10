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
        private KonfigurationOneNote _onenoteConf;

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
        public KonfigurationOneNote onenoteConf
        {
            get
            {
                return _onenoteConf;
            }
            set
            {
                _onenoteConf = value;
            }
        }

        public KonfigurationParameter(KonfigurationIx ixConf, KonfigurationOneNote onenoteConf)
        {
            this._ixConf = ixConf;
            this._onenoteConf = onenoteConf;
        }

        public KonfigurationParameter()
        {
            this._ixConf = new KonfigurationIx();
            this._onenoteConf = new KonfigurationOneNote();
        }

        public KonfigurationParameter(XmlNode profileNode)
        {
            this._ixConf = new KonfigurationIx();
            this._onenoteConf = new KonfigurationOneNote();

            foreach (XmlNode subNode in profileNode.ChildNodes)
            {
                switch (subNode.Name)
                {
                    case "ixConf":
                        _ixConf = new KonfigurationIx(subNode);
                        break;
                    case "onenoteConf":
                        _onenoteConf = new KonfigurationOneNote(subNode);
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

            nodeElem = onenoteConf.CreateXMLNode(xmlDoc);
            profileElem.AppendChild(nodeElem);

            return profileElem;

        }
    }

}
