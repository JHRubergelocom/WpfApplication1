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
    class Unittests
    {
        static Dictionary<string, string> GetUnittestApp(IXConnection ixConn)
        {
            String parentId = "ARCPATH[(E10E1000-E100-E100-E100-E10E10E10E00)]:/Business Solutions/development/ELOapps/ClientInfos";
            List<Sord> sordELOappsClientInfo = RepoUtils.FindChildren(parentId, ixConn, false);
            string configApp = "";
            string configId = "";
            foreach (Sord s in sordELOappsClientInfo)
            {
                string objId = s.id + "";
                EditInfo editInfo = ixConn.Ix.checkoutDoc(objId, null, EditInfoC.mbSordDoc, LockC.NO);
                DocVersion dv = editInfo.document.docs[0];
                string url = dv.url;
                Stream inputStream = ixConn.Download(url, 0, -1);
                string jsonString = new StreamReader(inputStream, Encoding.UTF8).ReadToEnd();
                jsonString = jsonString.Replace("namespace", "namespace1");
                JsonConfig config = JsonConfig.ReadToObject(jsonString);

                string webId = config.web.id;

                if (webId != null)
                {
                    if (webId.Contains("UnitTests"))
                    {
                        configApp = config.web.namespace1 + "." + config.web.id;
                        configId = config.id;

                    }
                }
            };
            Dictionary<string, string> dicApp = new Dictionary<string, string>();
            dicApp.Add("configApp", configApp);
            dicApp.Add("configId", configId);

            return dicApp;
        }

        public static void ShowUnittestsApp(KonfigurationIx ixConf)
        {

            try
            {
                IXConnFactory connFact = new IXConnFactory(ixConf.ixUrl, "Show Unittests", "1.0");
                IXConnection ixConn = connFact.Create(ixConf.user, ixConf.pwd, null, null);

                string ticket = ixConn.LoginResult.clientInfo.ticket;
                string ixUrl = ixConn.EndpointUrl;
                string appUrl = ixUrl.Replace("ix-", "wf-");
                appUrl = appUrl.Replace("/ix", "/apps/app");
                appUrl = appUrl + "/";
                Dictionary<string, string> dicApp = Unittests.GetUnittestApp(ixConn);
                appUrl = appUrl + dicApp["configApp"];
                appUrl = appUrl + "/?lang=de";
                appUrl = appUrl + "&ciId=" + dicApp["configApp"];
                appUrl = appUrl + "&ticket=" + ticket;
                appUrl = appUrl + "&timezone=Europe%2FBerlin";
                Http.OpenUrl(appUrl);

            }
            catch (byps.BException e)
            {
                if (e.Source != null)
                {
                    MessageBox.Show("Falsche Verbindungsdaten zu ELO \n" + e.Message, "ELO Connection", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                    Debug.WriteLine("byps.BException message: {0}", e.Message);
                }
            }
            catch (System.Net.WebException e)
            {
                if (e.Source != null)
                {
                    MessageBox.Show("Indexserver-Verbindung ungültig \n User: " + ixConf.user + "\n IxUrl: " + ixConf.ixUrl, "ELO Connection", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                    Debug.WriteLine("System.Net.WebException message: {0}", e.Message);
                }
            }
        }

        public static void ShowReportMatchUnittest(KonfigurationIx ixConf)
        {

            try
            {
                IXConnFactory connFact = new IXConnFactory(ixConf.ixUrl, "Show Report Match Unittest", "1.0");
                IXConnection ixConn = connFact.Create(ixConf.user, ixConf.pwd, null, null);

                List<string> jsTexts = RepoUtils.LoadTextDocs("ARCPATH[(E10E1000-E100-E100-E100-E10E10E10E00)]:/Business Solutions/_global/Unit Tests", ixConn);
                SortedDictionary<string, bool> dicRFs = RegisterFunctions.GetRFs(ixConn, jsTexts, ixConf.package);
                SortedDictionary<string, bool> dicASDirectRules = ASDirectRules.GetRules(ixConn, jsTexts, ixConf.package);
                SortedDictionary<string, bool> dicActionDefs = ActionDefinitions.GetActionDefs(ixConn, jsTexts, ixConf.package);

                string htmlDoc = Http.CreateHtmlReport(dicRFs, dicASDirectRules, dicActionDefs);

                string reportUrl = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\ReportElo";
                if (!Directory.Exists(reportUrl))
                {
                    Directory.CreateDirectory(reportUrl);
                }

                File.WriteAllText(Path.Combine(reportUrl, "Report.html"), htmlDoc);
                reportUrl = reportUrl + "\\Report.html";
                Http.OpenUrl(reportUrl);

            }
            catch (byps.BException e)
            {
                if (e.Source != null)
                {
                    MessageBox.Show("Falsche Verbindungsdaten zu ELO \n" + e.Message, "ELO Connection", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                    Debug.WriteLine("byps.BException message: {0}", e.Message);
                }
            }
            catch (System.Net.WebException e)
            {
                if (e.Source != null)
                {
                    MessageBox.Show("Indexserver-Verbindung ungültig \n User: " + ixConf.user + "\n IxUrl: " + ixConf.ixUrl, "ELO Connection", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                    Debug.WriteLine("System.Net.WebException message: {0}", e.Message);
                }
            }
        }

        public static bool Match(IXConnection ixConn, string uname, string package, List<string> jsTexts)
        {
            foreach (string jsText in jsTexts)
            {
                string[] jsLines = jsText.Split('\n');
                foreach (string line in jsLines)
                {
                    if (line.Contains(package))
                    {
                        if (line.Contains(uname))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }
    }
}
