using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using System.Windows;
using EloixClient.IndexServer;

namespace WpfApplication1
{
    class RegisterFunctions
    {
        public static SortedDictionary<string, bool> GetRFs(IXConnection ixConn, List<string> jsTexts, string package)
        {
            String parentId = "ARCPATH[(E10E1000-E100-E100-E100-E10E10E10E00)]:/Business Solutions/" + package + "/IndexServer Scripting Base";
            if (package.Equals(""))
            {
                parentId = "ARCPATH[(E10E1000-E100-E100-E100-E10E10E10E00)]:/IndexServer Scripting Base/_ALL/business_solutions";
            }
            List<Sord> sordRFInfo = RepoUtils.FindChildren(parentId, ixConn, true);
            SortedDictionary<string, bool> dicRFs = new SortedDictionary<string, bool>();
            foreach (Sord s in sordRFInfo)
            {
                string objId = s.id + "";
                EditInfo editInfo = ixConn.Ix.checkoutDoc(objId, null, EditInfoC.mbSordDoc, LockC.NO);
                if (editInfo.document.docs.Length > 0)
                {
                    DocVersion dv = editInfo.document.docs[0];
                    string url = dv.url;
                    Stream inputStream = ixConn.Download(url, 0, -1);
                    string jsText = new StreamReader(inputStream, Encoding.UTF8).ReadToEnd();
                    string[] jsLines = jsText.Split('\n');
                    foreach (string line in jsLines)
                    {
                        if (line.Contains("function RF_"))
                        {
                            string[] rf = line.Split();
                            string rfName = rf[1];
                            string[] rfNames = rfName.Split('(');
                            rfName = rfNames[0];
                            if (!rfName.Equals("*"))
                            {
                                if (!dicRFs.ContainsKey(rfName))
                                {
                                    bool match = Unittests.Match(ixConn, rfName, package, jsTexts);
                                    dicRFs.Add(rfName, match);
                                }
                            }
                        }
                    }    
                }
            };
            return dicRFs;
        }

    }
}
