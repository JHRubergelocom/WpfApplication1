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
        private bool _exportReferences;
        private string _maskName;

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
        public bool exportReferences
        {
            get
            {
                return _exportReferences;
            }
            set
            {
                _exportReferences = value;
            }
        }
        public string maskName
        {
            get
            {
                return _maskName;
            }
            set
            {
                _maskName = value;
            }
        }

        public KonfigurationIx(string arcPath, string ixUrl, string user, string pwd, bool exportReferences, string maskName)
        {
            this._arcPath = arcPath;
            this._ixUrl = ixUrl;
            this._user = user;
            this._pwd = pwd;
            this._exportReferences = exportReferences;
            this._maskName = maskName;
        }

        public KonfigurationIx()
        {
            this._arcPath = "";
            this._ixUrl = "";
            this._user = "";
            this._pwd = "";
            this._exportReferences = false;
            this._maskName = "";
        }

        public KonfigurationIx(XmlNode ixConfNode)
        {
            this._arcPath = "";
            this._ixUrl = "";
            this._user = "";
            this._pwd = "";
            this._exportReferences = false;
            this._maskName = "";

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

                    case "exportReferences":
                        _exportReferences = bool.Parse(subNode.InnerText);                       
                        break;

                    case "maskName":
                        _maskName = subNode.InnerText;
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

            nodeElem = CreateXMLNodeValue(xmlDoc, "exportReferences", exportReferences.ToString());
            ixConfElem.AppendChild(nodeElem);

            nodeElem = CreateXMLNodeValue(xmlDoc, "maskName", maskName);
            ixConfElem.AppendChild(nodeElem);

            return ixConfElem;

        }

    }
}
