using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace WpfApplication1
{
    public class KonfigurationOneNoteTags
    {
        private string _expandedTag;
        private string _importantTag;
        private string _criticalTag;
        private string _warningTag;
        private string _cautionTag;
        private string _thumbnailTag;

        public string expandedTag
        {
            get
            {
                return _expandedTag;
            }
            set
            {
                _expandedTag = value;
            }
        }

        public string importantTag
        {
            get
            {
                return _importantTag;
            }
            set
            {
                _importantTag = value;
            }
        }

        public string criticalTag
        {
            get
            {
                return _criticalTag;
            }
            set
            {
                _criticalTag = value;
            }
        }

        public string warningTag
        {
            get
            {
                return _warningTag;
            }
            set
            {
                _warningTag = value;
            }
        }

        public string cautionTag
        {
            get
            {
                return _cautionTag;
            }
            set
            {
                _cautionTag = value;
            }
        }

        public string thumbnailTag
        {
            get
            {
                return _thumbnailTag;
            }
            set
            {
                _thumbnailTag = value;
            }
        }

        public KonfigurationOneNoteTags(string expandedTag, string importantTag, string criticalTag, string warningTag, string cautionTag, string thumbnailTag)
        {
            this._expandedTag = expandedTag;
            this._importantTag = importantTag;
            this._criticalTag = criticalTag;
            this._warningTag = warningTag;
            this._cautionTag = cautionTag;
            this._thumbnailTag = thumbnailTag;
        }

        public KonfigurationOneNoteTags()
        {
            this._expandedTag = "";
            this._importantTag = "";
            this._criticalTag = "";
            this._warningTag = "";
            this._cautionTag = "";
            this._thumbnailTag = "";
        }

        public KonfigurationOneNoteTags(XmlNode onenotetagsConfNode)
        {
            this._expandedTag = "";
            this._importantTag = "";
            this._criticalTag = "";
            this._warningTag = "";
            this._cautionTag = "";
            this._thumbnailTag = "";

            foreach (XmlNode subNode in onenotetagsConfNode.ChildNodes)
            {
                switch (subNode.Name)
                {
                    case "expandedTag":
                        _expandedTag = subNode.InnerText;
                        break;

                    case "importantTag":
                        _importantTag = subNode.InnerText;
                        break;

                    case "criticalTag":
                        _criticalTag = subNode.InnerText;
                        break;

                    case "warningTag":
                        _warningTag = subNode.InnerText;
                        break;

                    case "cautionTag":
                        _cautionTag = subNode.InnerText;
                        break;

                    case "thumbnailTag":
                        _thumbnailTag = subNode.InnerText;
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
            XmlElement ixConfElem = CreateXMLNodeValue(xmlDoc, "onenotetagsConf", "");

            XmlElement nodeElem = CreateXMLNodeValue(xmlDoc, "expandedTag", expandedTag);
            ixConfElem.AppendChild(nodeElem);

            nodeElem = CreateXMLNodeValue(xmlDoc, "importantTag", importantTag);
            ixConfElem.AppendChild(nodeElem);

            nodeElem = CreateXMLNodeValue(xmlDoc, "criticalTag", criticalTag);
            ixConfElem.AppendChild(nodeElem);

            nodeElem = CreateXMLNodeValue(xmlDoc, "warningTag", warningTag);
            ixConfElem.AppendChild(nodeElem);

            nodeElem = CreateXMLNodeValue(xmlDoc, "cautionTag", cautionTag);
            ixConfElem.AppendChild(nodeElem);

            nodeElem = CreateXMLNodeValue(xmlDoc, "thumbnailTag", thumbnailTag);
            ixConfElem.AppendChild(nodeElem);

            return ixConfElem;

        }
    }
}
