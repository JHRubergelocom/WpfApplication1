using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml;
using EloixClient.IndexServer;

namespace WpfApplication1
{
    class ASDirectRules
    {
        public static SortedDictionary<string, bool> GetRules(IXConnection ixConn, List<string> jsTexts, string package)
        {
            String parentId = "ARCPATH[(E10E1000-E100-E100-E100-E10E10E10E00)]:/Business Solutions/" + package + "/ELOas Base/Direct";
            if (package.Equals(""))
            {
                parentId = "ARCPATH[(E10E1000-E100-E100-E100-E10E10E10E00)]:/ELOas Base/Direct";
            }

            List<Sord> sordRuleInfo = RepoUtils.FindChildren(parentId, ixConn, true);
            SortedDictionary<string, bool> dicRules = new SortedDictionary<string, bool>();
            foreach (Sord s in sordRuleInfo)
            {
                string objId = s.id + "";
                EditInfo editInfo = ixConn.Ix.checkoutDoc(objId, null, EditInfoC.mbSordDoc, LockC.NO);
                if (editInfo.document.docs.Length > 0)
                {
                    DocVersion dv = editInfo.document.docs[0];
                    string url = dv.url;
                    Stream inputStream = ixConn.Download(url, 0, -1);
                    string xmlText = new StreamReader(inputStream, Encoding.UTF8).ReadToEnd();

                    try
                    {
                        XmlDocument doc = new XmlDocument();
                        doc.LoadXml(xmlText);
                        string rulesetname = "";
                        foreach (XmlNode nameNode in doc.SelectNodes("ruleset/base/name"))
                        {
                            foreach (XmlNode subNode in nameNode.ChildNodes)
                            {
                                switch (subNode.Name)
                                {
                                    case "#text":
                                        rulesetname = subNode.InnerText;
                                        break;
                                }
                            }
                        }
                        if (!dicRules.ContainsKey(rulesetname))
                        {
                            bool match = Unittests.Match(ixConn, rulesetname, package, jsTexts);
                            dicRules.Add(rulesetname, match);
                        }
                    }
                    catch (XmlException e)
                    {
                        Debug.WriteLine("Exception: {0}", e.Message);
                    }

                }
            };
            return dicRules;
        }
    }
}
