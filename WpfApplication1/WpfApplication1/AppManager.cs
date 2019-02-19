using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Diagnostics;
using EloixClient.IndexServer;

namespace WpfApplication1
{
    class AppManager
    {
        public static void StartAppManager(KonfigurationIx ixConf)
        {

            try
            {
                IXConnFactory connFact = new IXConnFactory(ixConf.ixUrl, "Start AppManager", "1.0");
                IXConnection ixConn = connFact.Create(ixConf.user, ixConf.pwd, null, null);

                string ticket = ixConn.LoginResult.clientInfo.ticket;
                string ixUrl = ixConn.EndpointUrl;
                string appManagerUrl = ixUrl.Replace("ix-", "wf-");
                appManagerUrl = appManagerUrl.Replace("/ix", "/apps/app");
                appManagerUrl = appManagerUrl + "/elo.webapps.AppManager";
                appManagerUrl = appManagerUrl + "/?lang=de";
                appManagerUrl = appManagerUrl + "&ticket=" + ticket;
                appManagerUrl = appManagerUrl + "&timezone=Europe%2FBerlin";
                Http.OpenUrl(appManagerUrl);

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
