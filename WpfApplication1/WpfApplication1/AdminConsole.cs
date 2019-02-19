using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Windows;
using EloixClient.IndexServer;

namespace WpfApplication1
{
    class AdminConsole
    {
        public static void StartAdminConsole(KonfigurationIx ixConf)
        {

            try
            {
                IXConnFactory connFact = new IXConnFactory(ixConf.ixUrl, "Start AdminConsole", "1.0");
                IXConnection ixConn = connFact.Create(ixConf.user, ixConf.pwd, null, null);

                string ticket = ixConn.LoginResult.clientInfo.ticket;
                string ixUrl = ixConn.EndpointUrl;
                string[] adminConsole = ixUrl.Split('/');
                string adminConsoleUrl = adminConsole[0] + "//" + adminConsole[2] + "/AdminConsole";
                adminConsoleUrl = adminConsoleUrl + "/?lang=de";
                adminConsoleUrl = adminConsoleUrl + "&ticket=" + ticket;
                adminConsoleUrl = adminConsoleUrl + "&timezone=Europe%2FBerlin";
                Http.OpenUrl(adminConsoleUrl);

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
