using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace WpfApplication1
{
    public class KonfigurationCmd
    {
        private string _cmdCommand;
        private string _cmdArguments;
        private string _cmdWorkingDir;

        public string cmdCommand
        {
            get
            {
                return _cmdCommand;
            }
            set
            {
                _cmdCommand = value;
            }
        }

        public string cmdArguments
        {
            get
            {
                return _cmdArguments;
            }
            set
            {
                _cmdArguments = value;
            }
        }

        public string cmdWorkingDir
        {
            get
            {
                return _cmdWorkingDir;
            }
            set
            {
                _cmdWorkingDir = value;
            }
        }

        public KonfigurationCmd(string cmdCommand, string cmdArguments, string cmdWorkingDir)
        {
            this._cmdCommand = cmdCommand;
            this._cmdArguments = cmdArguments;
            this._cmdWorkingDir = cmdWorkingDir;
        }

        public KonfigurationCmd()
        {
            this._cmdCommand = "";
            this._cmdArguments = "";
            this._cmdWorkingDir = "";
        }

        public KonfigurationCmd(XmlNode cmdConfNode)
        {
            this._cmdCommand = "";
            this._cmdArguments = "";
            this._cmdWorkingDir = "";

            foreach (XmlNode subNode in cmdConfNode.ChildNodes)
            {
                switch (subNode.Name)
                {
                    case "cmdCommand":
                        _cmdCommand = subNode.InnerText;
                        break;

                    case "cmdArguments":
                        _cmdArguments = subNode.InnerText;
                        break;

                    case "cmdWorkingDir":
                        _cmdWorkingDir = subNode.InnerText;
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

        public XmlElement CreateXMLNode(XmlDocument xmlDoc, string cmdName) 
        {
            XmlElement ixConfElem = CreateXMLNodeValue(xmlDoc, "cmdConf", cmdName);

            XmlElement nodeElem = CreateXMLNodeValue(xmlDoc, "cmdCommand", cmdCommand);
            ixConfElem.AppendChild(nodeElem);

            nodeElem = CreateXMLNodeValue(xmlDoc, "cmdArguments", cmdArguments);
            ixConfElem.AppendChild(nodeElem);

            nodeElem = CreateXMLNodeValue(xmlDoc, "cmdWorkingDir", cmdWorkingDir);
            ixConfElem.AppendChild(nodeElem);

            return ixConfElem;

        }

    }
}
