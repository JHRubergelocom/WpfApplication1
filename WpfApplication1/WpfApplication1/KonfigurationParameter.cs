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

        public KonfigurationParameter(string arcPath, string ixUrl, string user, string pwd)
        {
            this._arcPath = arcPath;
            this._ixUrl = ixUrl;
            this._user = user;
            this._pwd = pwd;
        }

        public KonfigurationParameter()
        {
            this._arcPath = "";
            this._ixUrl = "";
            this._user = "";
            this._pwd = "";
        }

        public KonfigurationParameter(XmlNode profileNode)
        {
            this._arcPath = "";
            this._ixUrl = "";
            this._user = "";
            this._pwd = "";

            foreach (XmlNode subNode in profileNode.ChildNodes)
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

        public XmlElement CreateXMLNode(XmlDocument xmlDoc, string profilename) 
        {
            XmlElement profileElem = CreateXMLNodeValue(xmlDoc, "profile", profilename);

            XmlElement nodeElem = CreateXMLNodeValue(xmlDoc, "arcPath", arcPath);
            profileElem.AppendChild(nodeElem);

            nodeElem = CreateXMLNodeValue(xmlDoc, "ixUrl", ixUrl);
            profileElem.AppendChild(nodeElem);

            nodeElem = CreateXMLNodeValue(xmlDoc, "user", user);
            profileElem.AppendChild(nodeElem);

            nodeElem = CreateXMLNodeValue(xmlDoc, "pwd", pwd);
            profileElem.AppendChild(nodeElem);

            return profileElem;

        }
    }

}
