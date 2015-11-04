using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace WpfApplication1
{
    public class KonfigurationIx
    {
        private string _arcPath;
        private string _ixUrl;
        private string _user;
        private string _pwd;

        public string arcPath
        {
            get
            {
                return _arcPath;
            }
            set
            {
                _arcPath = value;
            }
        }

        public string ixUrl
        {
            get
            {
                return _ixUrl;
            }
            set
            {
                _ixUrl = value;
            }
        }
        public string user
        {
            get
            {
                return _user;
            }
            set
            {
                _user = value;
            }
        }
        public string pwd
        {
            get
            {
                return _pwd;
            }
            set
            {
                _pwd = value;
            }
        }

        public KonfigurationIx(string arcPath, string ixUrl, string user, string pwd)
        {
            this._arcPath = arcPath;
            this._ixUrl = ixUrl;
            this._user = user;
            this._pwd = pwd;
        }

        public KonfigurationIx()
        {
            this._arcPath = "";
            this._ixUrl = "";
            this._user = "";
            this._pwd = "";
        }

        public KonfigurationIx(XmlNode ixConfNode)
        {
            this._arcPath = "";
            this._ixUrl = "";
            this._user = "";
            this._pwd = "";

            foreach (XmlNode subNode in ixConfNode.ChildNodes)
            {
                switch (subNode.Name)
                {
                    case "arcPath":
                        _arcPath = subNode.InnerText;
                        break;

                    case "user":
                        _user = subNode.InnerText;
                        break;

                    case "ixUrl":
                        _ixUrl = subNode.InnerText;
                        break;

                    case "pwd":
                        _pwd = subNode.InnerText;
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

        public XmlElement CreateXMLNode(XmlDocument xmlDoc) 
        {
            XmlElement ixConfElem = CreateXMLNodeValue(xmlDoc, "ixConf", "");

            XmlElement nodeElem = CreateXMLNodeValue(xmlDoc, "arcPath", arcPath);
            ixConfElem.AppendChild(nodeElem);

            nodeElem = CreateXMLNodeValue(xmlDoc, "ixUrl", ixUrl);
            ixConfElem.AppendChild(nodeElem);

            nodeElem = CreateXMLNodeValue(xmlDoc, "user", user);
            ixConfElem.AppendChild(nodeElem);

            nodeElem = CreateXMLNodeValue(xmlDoc, "pwd", pwd);
            ixConfElem.AppendChild(nodeElem);

            return ixConfElem;

        }

    }
}
