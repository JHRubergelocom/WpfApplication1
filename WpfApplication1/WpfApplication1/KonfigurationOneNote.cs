using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace WpfApplication1
{
    class KonfigurationOneNote
    {
        private string _notebook;
        private string _ignoreTags;
        private string _renderImages;
        private string _expandedTag;
        private string _importantTag;
        private string _criticalTag;
        private string _warningTag;
        private string _cautionTag;
        private string _thumbnailTag;

        public string notebook
        {
            get
            {
                return _notebook;
            }
            set
            {
                _notebook = value;
            }
        }

        public string ignoreTags
        {
            get
            {
                return _ignoreTags;
            }
            set
            {
                _ignoreTags = value;
            }
        }

        public string renderImages
        {
            get
            {
                return _renderImages;
            }
            set
            {
                _renderImages = value;
            }
        }

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

        public KonfigurationOneNote(string notebook, string ignoreTags, string renderImages, string expandedTag, string importantTag, string criticalTag, string warningTag, string cautionTag, string thumbnailTag)
        {
            this._notebook = notebook;
            this._ignoreTags = ignoreTags;
            this._renderImages = renderImages;
            this._expandedTag = expandedTag;
            this._importantTag = importantTag;
            this._criticalTag = criticalTag;
            this._warningTag = warningTag;
            this._cautionTag = cautionTag;
            this._thumbnailTag = thumbnailTag;
        }

        public KonfigurationOneNote()
        {
            this._notebook = "";
            this._ignoreTags = "";
            this._renderImages = "";
            this._expandedTag = "";
            this._importantTag = "";
            this._criticalTag = "";
            this._warningTag = "";
            this._cautionTag = "";
            this._thumbnailTag = "";
        }

        public KonfigurationOneNote(XmlNode onenoteConfNode)
        {
            this._notebook = "";
            this._ignoreTags = "";
            this._renderImages = "";
            this._expandedTag = "";
            this._importantTag = "";
            this._criticalTag = "";
            this._warningTag = "";
            this._cautionTag = "";
            this._thumbnailTag = "";

            foreach (XmlNode subNode in onenoteConfNode.ChildNodes)
            {
                switch (subNode.Name)
                {
                    case "notebook":
                        _notebook = subNode.InnerText;
                        break;

                    case "ignoreTags":
                        _ignoreTags = subNode.InnerText;
                        break;

                    case "renderImages":
                        _renderImages = subNode.InnerText;
                        break;

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
            XmlElement ixConfElem = CreateXMLNodeValue(xmlDoc, "onenoteConf", "");

            XmlElement nodeElem = CreateXMLNodeValue(xmlDoc, "notebook", notebook);
            ixConfElem.AppendChild(nodeElem);

            nodeElem = CreateXMLNodeValue(xmlDoc, "ignoreTags", ignoreTags);
            ixConfElem.AppendChild(nodeElem);

            nodeElem = CreateXMLNodeValue(xmlDoc, "renderImages", renderImages);
            ixConfElem.AppendChild(nodeElem);

            nodeElem = CreateXMLNodeValue(xmlDoc, "expandedTag", expandedTag);
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
