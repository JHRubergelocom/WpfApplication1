using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;

namespace WpfApplication1
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private SortedDictionary<string, KonfigurationParameter> profiles;

        public MainWindow()
        {
            InitializeComponent();
            LoadKonfigurations();
        }

        public SortedDictionary<string, KonfigurationParameter> GetProfiles()
        {
            return profiles;
        }

        private void btnLoadEloScripte_Click(object sender, RoutedEventArgs e)
        {
            if (profiles.Count > 0)
            {
                string actvalue = cboProfile.SelectedValue.ToString();
                if (profiles.ContainsKey(actvalue))
                {
                    if (chkExportReferences.IsChecked.HasValue)
                    {
                        ExportElo.StartExportElo(actvalue, profiles[actvalue], (bool)chkExportReferences.IsChecked);
                        MessageBox.Show("Finished btnLoadEloScripte_Click");
                    }
                }
            }
        }

        private void btnNewProfile_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new NewProfilDialog(this);
            if (dialog.ShowDialog() == true)
            {
                // MessageBox.Show("You select Profilename: " + dialog.txtProfileName);
                KonfigurationParameter newprofile = new KonfigurationParameter(new KonfigurationIx("arcPath", "ixUrl", "user", "pwd"));
                string newprofilename = dialog.txtProfileName.Text;
                if (!profiles.ContainsKey(newprofilename))
                {
                    profiles.Add(newprofilename, newprofile);
                    SetValues(newprofile);
                    SaveKonfigurations();

                    InitKonfigurations(newprofilename);
                }

            }
            else
            {
                // MessageBox.Show("Dialog has been cancelled: ");
            }
        }

        private void LoadKonfigurations()
        {

            // aus XML-Datei laden
            profiles = new SortedDictionary<string, KonfigurationParameter>();
            string winPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\profiles.xml";
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(winPath);
                string profilename = "";

                foreach (XmlNode profileNode in doc.SelectNodes("/profiles/profile"))
                {
                    foreach (XmlNode subNode in profileNode.ChildNodes)
                    {
                        switch (subNode.Name)
                        {
                            case "#text":
                                profilename = subNode.InnerText;
                                break;
                        }
                    }
                    profiles.Add(profilename, new KonfigurationParameter(profileNode));
                }

            }
            catch (FileNotFoundException e)
            {
                Debug.WriteLine("FileNotFoundException: {0}", e.Message);
            }
            catch (Exception e)
            {
                Debug.WriteLine("Exception: {0}", e.Message);
            }
            /*
            if (profiles.Count == 0)
            {
                profiles.Add("Invoice",new KonfigurationParameter(new KonfigurationIx("ARCPATH[1]:/Administration/Business Solutions", "http://srvpdevbs01vm:8010/ix-invoice/ix", "Ruberg", "elo")));
                profiles.Add("Pubsec", new KonfigurationParameter(new KonfigurationIx("ARCPATH[1]:/Administration/Business Solutions", "http://srvpdevbs01vm:8020/ix-pubsec/ix", "Ruberg", "elo")));
                profiles.Add("Akadamie", new KonfigurationParameter(new KonfigurationIx("ARCPATH[1]:/Administration/Business Solutions", "http://PCRUBERG:9090/ix-Akadamie2/ix", "Administrator", "elo")));
                profiles.Add("Beispielarchiv", new KonfigurationParameter(new KonfigurationIx("ARCPATH[1]:/Administration/Business Solutions", "http://PCRUBERG:9090/ix-elo/ix", "Administrator", "elo")));
            }
             */ 
            // TODO

            InitKonfigurations();
        }

        private void SaveKonfigurations()
        {
            // In XML-Datei speichern

            // Create the XmlDocument.
            XmlDocument doc = new XmlDocument();
            doc.LoadXml("<profiles></profiles>");

            // Add a profile element.
            foreach (KeyValuePair<string, KonfigurationParameter> kvpProfile in profiles)
            {
                XmlElement profileElem = kvpProfile.Value.CreateXMLNode(doc, kvpProfile.Key);
                doc.DocumentElement.AppendChild(profileElem);
            }

            string winPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\profiles.xml";
            try
            {
                XmlWriter writer = XmlWriter.Create(winPath);
                doc.Save(writer);
                writer.Flush();
                writer.Close();
            }
            catch (Exception e)
            {
                Debug.WriteLine("Exception: {0}", e.Message);
            }
        }

        private void SetValues(KonfigurationParameter profile)
        {
            txtUser.Text = profile.ixConf.user;
            txtArcPath.Text = profile.ixConf.arcPath;
            txtIxUrl.Text = profile.ixConf.ixUrl;
            txtPwd.Password = profile.ixConf.pwd;
        }

        private KonfigurationParameter GetValues()
        {   
            KonfigurationParameter profile = new KonfigurationParameter(new KonfigurationIx(txtArcPath.Text, txtIxUrl.Text, txtUser.Text, txtPwd.Password));
            return profile;
        }

        private void SetStatusEnabled(bool status)
        {
            cboProfile.IsEnabled = status;
            txtArcPath.IsEnabled = status;
            txtIxUrl.IsEnabled = status;
            txtPwd.IsEnabled = status;
            txtUser.IsEnabled = status;

            btnDeleteProfile.IsEnabled = status;
            btnSaveProfile.IsEnabled = status;
            btnLoadEloScripte.IsEnabled = status;
            btnGenerateOneNotePages.IsEnabled = status;

        }

        private void FillCboProfile()
        {
            cboProfile.Items.Clear();
            foreach (KeyValuePair<string, KonfigurationParameter> kvpProfile in profiles)
            {
                cboProfile.Items.Add(kvpProfile.Key);
            }
            if (profiles.Count > 0)
            {
                SetStatusEnabled(true);
            }
            else
            {
                SetStatusEnabled(false);
            }
        }

        private void InitKonfigurations()
        {
            FillCboProfile();
            if (profiles.Count > 0)
            {
                cboProfile.SelectedIndex = 0;
            }
        }

        private void InitKonfigurations(string selectionValue)
        {
            FillCboProfile();
            if (profiles.Count > 0)
            {
                cboProfile.SelectedValue = selectionValue;
            }
        }

        private void cboProfile_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cboProfile.SelectedIndex == -1)
            {
                return;
            }
            if (cboProfile.SelectedValue == null)
            {
                return;
            }


            string actvalue = cboProfile.SelectedValue.ToString();

            if (profiles.ContainsKey(actvalue)) 
            {
                SetValues(profiles[actvalue]);
            }
        }

        private void btnSaveProfile_Click(object sender, RoutedEventArgs e)
        {
            string actvalue = cboProfile.SelectedValue.ToString();
            if (profiles.ContainsKey(actvalue))
            {
                profiles[actvalue] = GetValues(); 
                SaveKonfigurations();
            }
        }

        private void btnDeleteProfile_Click(object sender, RoutedEventArgs e)
        {
            if (cboProfile.SelectedIndex == -1)
            {
                return;
            }

            string actvalue = cboProfile.SelectedValue.ToString();
            int actindex = cboProfile.SelectedIndex;
            if (profiles.ContainsKey(actvalue))
            {
                profiles.Remove(actvalue);
                SetValues(new KonfigurationParameter());
                if (actindex > (profiles.Count - 1)) 
                {
                    actindex = actindex - 1;
                }

                SaveKonfigurations();
                InitKonfigurations();
                cboProfile.SelectedIndex = actindex;
            }

        }
    }
}
