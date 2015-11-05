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
        private KonfigurationOneNoteTags _onenoteTags;

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

        public KonfigurationOneNoteTags onenoteTags
        {
            get
            {
                return _onenoteTags;
            }
            set
            {
                _onenoteTags = value;
            }
        }


        public KonfigurationOneNote(string notebook, string ignoreTags, string renderImages, KonfigurationOneNoteTags onenoteTags)
        {
            this._notebook = notebook;
            this._ignoreTags = ignoreTags;
            this._renderImages = renderImages;
            this._onenoteTags = onenoteTags;
        }

        public KonfigurationOneNote()
        {
            this._notebook = "";
            this._ignoreTags = "";
            this._renderImages = "";
            this._onenoteTags = new KonfigurationOneNoteTags();
        }

        public KonfigurationOneNote(XmlNode onenoteConfNode)
        {
            this._notebook = "";
            this._ignoreTags = "";
            this._renderImages = "";
            this._onenoteTags = new KonfigurationOneNoteTags();

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

                    case "onenotetagsConf":
                        _onenoteTags = new KonfigurationOneNoteTags(subNode);
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

            nodeElem = onenoteTags.CreateXMLNode(xmlDoc);
            ixConfElem.AppendChild(nodeElem);

            return ixConfElem;

        }
    }
}
