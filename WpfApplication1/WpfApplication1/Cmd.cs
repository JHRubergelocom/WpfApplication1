using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Windows;

namespace WpfApplication1
{
    class Cmd
    {
        public static void ExecuteCmd(KonfigurationCmd cmdConf)
        {
            try
            {
                Process proThis = new Process();
                ProcessStartInfo psiThis = new ProcessStartInfo();
                psiThis.FileName = cmdConf.cmdCommand;
                psiThis.Arguments = cmdConf.cmdArguments;
                psiThis.WorkingDirectory = cmdConf.cmdWorkingDir;
                proThis.StartInfo = psiThis;
                proThis.Start();
                proThis.WaitForExit();
            } 
            catch (System.InvalidOperationException e)
            {
                if (e.Source != null)
                {
                    MessageBox.Show("Cmd Konfiguration ungültig \n Command: " + cmdConf.cmdCommand + 
                                    "\n Arguments: " + cmdConf.cmdArguments +
                                    "\n WorkingDir: " + cmdConf.cmdWorkingDir, 
                                    "Execute Cmd", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                    Debug.WriteLine("System.InvalidOperationException message: {0}", e.Message);
                }

            }

        }
    }
}
