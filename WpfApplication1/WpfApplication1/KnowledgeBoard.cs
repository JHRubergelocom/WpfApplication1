using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows;
using System.Diagnostics;
using EloixClient.IndexServer;

namespace WpfApplication1
{
    class KnowledgeBoard
    {
        static Dictionary<string, string> GetKnowledgeBoard(IXConnection ixConn)
        {
            String parentId = "ARCPATH[(E10E1000-E100-E100-E100-E10E10E10E00)]:/Business Solutions/knowledge/ELOapps/ClientInfos";
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

                string id = config.id;

                if (id != null)
                {
                    if (id.Contains("tile-sol-knowledge-board"))
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

        public static void ShowKnowledgeBoard(KonfigurationIx ixConf)
        {

            try
            {
                IXConnFactory connFact = new IXConnFactory(ixConf.ixUrl, "Show KnowledgeBoard", "1.0");
                IXConnection ixConn = connFact.Create(ixConf.user, ixConf.pwd, null, null);

                string ticket = ixConn.LoginResult.clientInfo.ticket;
                string ixUrl = ixConn.EndpointUrl;
                string appUrl = ixUrl.Replace("ix-", "wf-");
                appUrl = appUrl.Replace("/ix", "/apps/app");
                appUrl = appUrl + "/";
                Dictionary<string, string> dicApp = KnowledgeBoard.GetKnowledgeBoard(ixConn);
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
    }
}
