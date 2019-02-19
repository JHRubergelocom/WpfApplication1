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
    class Webclient
    {
        public static void StartWebclient(KonfigurationIx ixConf)
        {

            try
            {
                IXConnFactory connFact = new IXConnFactory(ixConf.ixUrl, "Start Webclient", "1.0");
                IXConnection ixConn = connFact.Create(ixConf.user, ixConf.pwd, null, null);

                string ticket = ixConn.LoginResult.clientInfo.ticket;
                string ixUrl = ixConn.EndpointUrl;
                string webclientUrl = ixUrl.Replace("ix-", "web-");
                webclientUrl = webclientUrl.Replace("/ix", "");
                webclientUrl = webclientUrl + "/?lang=de";
                webclientUrl = webclientUrl + "&ticket=" + ticket;
                webclientUrl = webclientUrl + "&timezone=Europe%2FBerlin";
                Http.OpenUrl(webclientUrl);

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
