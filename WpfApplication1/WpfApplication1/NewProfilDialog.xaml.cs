using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace WpfApplication1
{
    /// <summary>
    /// Interaktionslogik für NewProfilDialog.xaml
    /// </summary>
    public partial class NewProfilDialog : Window
    {
        private SortedDictionary<string, KonfigurationParameter> profiles;

        public NewProfilDialog(MainWindow parent)
        {
            this.profiles = parent.GetProfiles();
            InitializeComponent();
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (DialogResult.HasValue && DialogResult.Value)
            {
                if (profiles.ContainsKey(txtProfileName.Text))
                {
                    MessageBox.Show("Profilename: " + txtProfileName.Text + " ist schon vorhanden!");
                    e.Cancel = true;
                }
            }
        }
    }
}
