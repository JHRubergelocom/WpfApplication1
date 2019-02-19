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
        private KonfigurationCmd _cmdGitPullAllConf;
        private KonfigurationCmd _cmdEloPullUnittestConf;
        private KonfigurationCmd _cmdEloPrepareConf;
        private KonfigurationCmd _cmdEloPullPackageConf;

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
        public KonfigurationCmd cmdGitPullAllConf
        {
            get
            {
                return _cmdGitPullAllConf;
            }
            set
            {
                _cmdGitPullAllConf = value;
            }
        }
        public KonfigurationCmd cmdEloPullUnittestConf
        {
            get
            {
                return _cmdEloPullUnittestConf;
            }
            set
            {
                _cmdEloPullUnittestConf = value;
            }
        }
        public KonfigurationCmd cmdEloPrepareConf
        {
            get
            {
                return _cmdEloPrepareConf;
            }
            set
            {
                _cmdEloPrepareConf = value;
            }
        }
        public KonfigurationCmd cmdEloPullPackageConf
        {
            get
            {
                return _cmdEloPullPackageConf;
            }
            set
            {
                _cmdEloPullPackageConf = value;
            }
        }


        public KonfigurationParameter(KonfigurationIx ixConf, KonfigurationOneNote onenoteConf, KonfigurationCmd cmdGitPullAllConf, KonfigurationCmd cmdEloPullUnittestConf, KonfigurationCmd cmdEloPrepareConf, KonfigurationCmd cmdEloPullPackageConf)
        {
            this._ixConf = ixConf;
            this._onenoteConf = onenoteConf;
            this._cmdGitPullAllConf = cmdGitPullAllConf;
            this._cmdEloPullUnittestConf = cmdEloPullUnittestConf;
            this._cmdEloPrepareConf = cmdEloPrepareConf;
            this._cmdEloPullPackageConf = cmdEloPullPackageConf;
        }

        public KonfigurationParameter()
        {
            this._ixConf = new KonfigurationIx();
            this._onenoteConf = new KonfigurationOneNote();
            this._cmdGitPullAllConf = new KonfigurationCmd();
            this._cmdEloPullUnittestConf = new KonfigurationCmd();
            this._cmdEloPrepareConf = new KonfigurationCmd();
            this._cmdEloPullPackageConf = new KonfigurationCmd();
        }

        public KonfigurationParameter(XmlNode profileNode)
        {
            this._ixConf = new KonfigurationIx();
            this._onenoteConf = new KonfigurationOneNote();
            this._cmdGitPullAllConf = new KonfigurationCmd();
            this._cmdEloPullUnittestConf = new KonfigurationCmd();
            this._cmdEloPrepareConf = new KonfigurationCmd();
            this._cmdEloPullPackageConf = new KonfigurationCmd();

            foreach (XmlNode subNode in profileNode.ChildNodes)
            {
                string cmdName = "";

                switch (subNode.Name)
                {
                    case "ixConf":
                        _ixConf = new KonfigurationIx(subNode);
                        break;
                    case "onenoteConf":
                        _onenoteConf = new KonfigurationOneNote(subNode);
                        break;
                    case "cmdConf":
                        foreach (XmlNode subSubNode in subNode.ChildNodes)
                        {
                            if (subSubNode.Name == "#text")
                            {
                                cmdName = subSubNode.InnerText;
                            }
                        }

                        switch (cmdName)
                        {
                            case "gitPullAll":
                                _cmdGitPullAllConf = new KonfigurationCmd(subNode);
                                break;
                            case "eloPullUnittest":
                                _cmdEloPullUnittestConf = new KonfigurationCmd(subNode);
                                break;
                            case "eloPrepare":
                                _cmdEloPrepareConf = new KonfigurationCmd(subNode);
                                break;
                            case "eloPullPackage":
                                _cmdEloPullPackageConf = new KonfigurationCmd(subNode);
                                break;
                        }
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

            nodeElem = cmdGitPullAllConf.CreateXMLNode(xmlDoc, "gitPullAll");
            profileElem.AppendChild(nodeElem);

            nodeElem = cmdEloPullUnittestConf.CreateXMLNode(xmlDoc, "eloPullUnittest");
            profileElem.AppendChild(nodeElem);

            nodeElem = cmdEloPrepareConf.CreateXMLNode(xmlDoc, "eloPrepare");
            profileElem.AppendChild(nodeElem);

            nodeElem = cmdEloPullPackageConf.CreateXMLNode(xmlDoc, "eloPullPackage");
            profileElem.AppendChild(nodeElem);

            return profileElem;

        }
    }

}
