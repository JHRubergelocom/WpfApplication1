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

            if (Properties.Settings.Default.ActualConfiguration != "")
            {
                cboProfile.SelectedValue = Properties.Settings.Default.ActualConfiguration;
                string actvalue = cboProfile.SelectedValue.ToString();
                if (profiles.ContainsKey(actvalue))
                {
                    SetValues(profiles[actvalue]);
                }
            }
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
                        ExportElo.StartExportElo(actvalue, profiles[actvalue].ixConf);
                        MessageBox.Show("Finished btnLoadEloScripte_Click");
                    }
                }
            }
        }

        private void btnGenerateOneNotePages_Click(object sender, RoutedEventArgs e)
        {
            if (profiles.Count > 0)
            {
                string actvalue = cboProfile.SelectedValue.ToString();
                if (profiles.ContainsKey(actvalue))
                {
                    if (chkRenderImages.IsChecked.HasValue)
                    {
                        ExportOneNotePages.StartExportOneNotePages(actvalue, profiles[actvalue].onenoteConf);
                        MessageBox.Show("Finished btnGenerateOneNotePages_Click");
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
                KonfigurationParameter newprofile = new KonfigurationParameter(new KonfigurationIx("arcPath", "ixUrl", "user", "pwd", false, "maskName", "package"), 
                                                                               new KonfigurationOneNote("notebook", "ignoreTags", true, "lang",
                                                                               new KonfigurationOneNoteTags("expandedTag", "importantTag", "criticalTag", "warningTag", "cautionTag", "thumbnailTag")),
                                                                               new KonfigurationCmd("gitpullall.cmd", "", "E:\\Git\\solutions"),
                                                                               new KonfigurationCmd("powershell.exe", "E:\\Git\\solutions\\learning.git\\elopullunittests.ps1", "E:\\Git\\solutions\\learning.git"),
                                                                               new KonfigurationCmd("powershell.exe", "E:\\Git\\solutions\\learning.git\\elopreparelearning.ps1", "E:\\Git\\solutions\\learning.git"),
                                                                               new KonfigurationCmd("powershell.exe", "E:\\Git\\solutions\\learning.git\\elopulllearning.ps1", "E:\\Git\\solutions\\learning.git")
                                                                               );
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
            chkExportReferences.IsChecked = profile.ixConf.exportReferences;
            txtMaskName.Text = profile.ixConf.maskName;
            txtPackage.Text = profile.ixConf.package;

            txtNotebook.Text = profile.onenoteConf.notebook;
            txtIgnoreTags.Text = profile.onenoteConf.ignoreTags;
            chkRenderImages.IsChecked = profile.onenoteConf.renderImages;
            txtLang.Text = profile.onenoteConf.lang;

            txtExpandedTag.Text = profile.onenoteConf.onenoteTags.expandedTag;
            txtImportantTag.Text = profile.onenoteConf.onenoteTags.importantTag;
            txtCriticalTag.Text = profile.onenoteConf.onenoteTags.criticalTag;
            txtWarningTag.Text = profile.onenoteConf.onenoteTags.warningTag;
            txtCautionTag.Text = profile.onenoteConf.onenoteTags.cautionTag;
            txtThumbnailTag.Text = profile.onenoteConf.onenoteTags.thumbnailTag;

            txtGitPullAllCmd.Text = profile.cmdGitPullAllConf.cmdCommand;
            txtGitPullAllDir.Text = profile.cmdGitPullAllConf.cmdWorkingDir;

            txtEloPullUnittestCmd.Text = profile.cmdEloPullUnittestConf.cmdCommand;
            txtEloPullUnittestArgs.Text = profile.cmdEloPullUnittestConf.cmdArguments;
            txtEloPullUnittestDir.Text = profile.cmdEloPullUnittestConf.cmdWorkingDir;

            txtEloPrepareCmd.Text = profile.cmdEloPrepareConf.cmdCommand;
            txtEloPrepareArgs.Text = profile.cmdEloPrepareConf.cmdArguments;
            txtEloPrepareDir.Text = profile.cmdEloPrepareConf.cmdWorkingDir;

            txtEloPullPackageCmd.Text = profile.cmdEloPullPackageConf.cmdCommand;
            txtEloPullPackageArgs.Text = profile.cmdEloPullPackageConf.cmdArguments;
            txtEloPullPackageDir.Text = profile.cmdEloPullPackageConf.cmdWorkingDir;

        }

        private KonfigurationParameter GetValues()
        {
            KonfigurationParameter profile = new KonfigurationParameter(new KonfigurationIx(txtArcPath.Text, txtIxUrl.Text, txtUser.Text, txtPwd.Password, (bool)chkExportReferences.IsChecked, txtMaskName.Text, txtPackage.Text),
                                                                        new KonfigurationOneNote(txtNotebook.Text, txtIgnoreTags.Text, (bool)chkRenderImages.IsChecked, txtLang.Text,
                                                                        new KonfigurationOneNoteTags(txtExpandedTag.Text, txtImportantTag.Text, txtCriticalTag.Text, txtWarningTag.Text, txtCautionTag.Text, txtThumbnailTag.Text)),
                                                                        new KonfigurationCmd(txtGitPullAllCmd.Text, "", txtGitPullAllDir.Text),
                                                                        new KonfigurationCmd(txtEloPullUnittestCmd.Text, txtEloPullUnittestArgs.Text, txtEloPullUnittestDir.Text),
                                                                        new KonfigurationCmd(txtEloPrepareCmd.Text, txtEloPrepareArgs.Text, txtEloPrepareDir.Text),
                                                                        new KonfigurationCmd(txtEloPullPackageCmd.Text, txtEloPullPackageArgs.Text, txtEloPullPackageDir.Text)
                                                                        );
            return profile;
        }

        private void SetStatusEnabled(bool status)
        {
            cboProfile.IsEnabled = status;
            txtArcPath.IsEnabled = status;
            txtIxUrl.IsEnabled = status;
            txtPwd.IsEnabled = status;
            txtUser.IsEnabled = status;
            chkExportReferences.IsEnabled = status;
            txtMaskName.IsEnabled = status;
            txtPackage.IsEnabled = status;

            txtNotebook.IsEnabled = status;
            txtIgnoreTags.IsEnabled = status;
            chkRenderImages.IsEnabled = status;
            txtLang.IsEnabled = status;

            txtExpandedTag.IsEnabled = status;
            txtImportantTag.IsEnabled = status;
            txtCriticalTag.IsEnabled = status;
            txtWarningTag.IsEnabled = status;
            txtCautionTag.IsEnabled = status;
            txtThumbnailTag.IsEnabled = status;

            btnDeleteProfile.IsEnabled = status;
            btnSaveProfile.IsEnabled = status;
            btnLoadEloScripte.IsEnabled = status;
            btnGenerateOneNotePages.IsEnabled = status;

            txtGitPullAllCmd.IsEnabled = status;
            txtGitPullAllDir.IsEnabled = status;

            txtEloPullUnittestCmd.IsEnabled = status;
            txtEloPullUnittestArgs.IsEnabled = status;
            txtEloPullUnittestDir.IsEnabled = status;

            txtEloPrepareCmd.IsEnabled = status;
            txtEloPrepareArgs.IsEnabled = status;
            txtEloPrepareDir.IsEnabled = status;

            txtEloPullPackageCmd.IsEnabled = status;
            txtEloPullPackageArgs.IsEnabled = status;
            txtEloPullPackageDir.IsEnabled = status;

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

        private void btnShowUnittests_Click(object sender, RoutedEventArgs e)
        {
            if (profiles.Count > 0)
            {
                string actvalue = cboProfile.SelectedValue.ToString();
                if (profiles.ContainsKey(actvalue))
                {
                    Unittests.ShowUnittestsApp(profiles[actvalue].ixConf);
                }
            }
        }

        private void btnWebclient_Click(object sender, RoutedEventArgs e)
        {
            if (profiles.Count > 0)
            {
                string actvalue = cboProfile.SelectedValue.ToString();
                if (profiles.ContainsKey(actvalue))
                {
                    Webclient.StartWebclient(profiles[actvalue].ixConf);
                }
            }

        }

        private void btnAdminConsole_Click(object sender, RoutedEventArgs e)
        {
            if (profiles.Count > 0)
            {
                string actvalue = cboProfile.SelectedValue.ToString();
                if (profiles.ContainsKey(actvalue))
                {
                    AdminConsole.StartAdminConsole(profiles[actvalue].ixConf);
                }
            }
        }

        private void mainClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            string actvalue = cboProfile.SelectedValue.ToString();
            Properties.Settings.Default.ActualConfiguration = actvalue;
            Properties.Settings.Default.Save();
        }

        private void btnUnittest_Click(object sender, RoutedEventArgs e)
        {
            if (profiles.Count > 0)
            {
                string actvalue = cboProfile.SelectedValue.ToString();
                if (profiles.ContainsKey(actvalue))
                {
                    Unittests.ShowReportMatchUnittest(profiles[actvalue].ixConf);
                }
            }

        }

        private void btnAppManager_Click(object sender, RoutedEventArgs e)
        {
            if (profiles.Count > 0)
            {
                string actvalue = cboProfile.SelectedValue.ToString();
                if (profiles.ContainsKey(actvalue))
                {
                    AppManager.StartAppManager(profiles[actvalue].ixConf);
                }
            }

        }

        private void btnKnowledgeBoard_Click(object sender, RoutedEventArgs e)
        {
            if (profiles.Count > 0)
            {
                string actvalue = cboProfile.SelectedValue.ToString();
                if (profiles.ContainsKey(actvalue))
                {
                    KnowledgeBoard.ShowKnowledgeBoard(profiles[actvalue].ixConf);
                }
            }
        }

        private void btnGitPullAll_Click(object sender, RoutedEventArgs e)
        {
            if (profiles.Count > 0)
            {
                string actvalue = cboProfile.SelectedValue.ToString();
                if (profiles.ContainsKey(actvalue))
                {
                    Cmd.ExecuteCmd(profiles[actvalue].cmdGitPullAllConf);
                }
            }
        }

        private void btnEloPullUnittest_Click(object sender, RoutedEventArgs e)
        {
            if (profiles.Count > 0)
            {
                string actvalue = cboProfile.SelectedValue.ToString();
                if (profiles.ContainsKey(actvalue))
                {
                    Cmd.ExecuteCmd(profiles[actvalue].cmdEloPullUnittestConf);
                }
            }
        }

        private void btnEloPrepare_Click(object sender, RoutedEventArgs e)
        {
            if (profiles.Count > 0)
            {
                string actvalue = cboProfile.SelectedValue.ToString();
                if (profiles.ContainsKey(actvalue))
                {
                    Cmd.ExecuteCmd(profiles[actvalue].cmdEloPrepareConf);
                }
            }

        }

        private void btnEloPullPackage_Click(object sender, RoutedEventArgs e)
        {
            if (profiles.Count > 0)
            {
                string actvalue = cboProfile.SelectedValue.ToString();
                if (profiles.ContainsKey(actvalue))
                {
                    Cmd.ExecuteCmd(profiles[actvalue].cmdEloPullPackageConf);
                }
            }

        }
    }
}
