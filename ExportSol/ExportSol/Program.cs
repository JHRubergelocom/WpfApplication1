using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

using EloixClient.IndexServer;
using System.IO;

namespace ExportSol
{
    class Program {

        private static void FindChildren(IXConnection conn, string arcPath, string winPath, bool exportReferences)
        {

            FindInfo fi = null;
            FindResult fr = null;

            try
            {

                EditInfo ed = conn.Ix.checkoutSord(arcPath, EditInfoC.mbOnlyId, LockC.NO);
                int parentId = ed.sord.id;

                fi = new FindInfo();
                fi.findChildren = new FindChildren();
                fi.findChildren.parentId = Convert.ToString(parentId);
                fi.findChildren.endLevel = 1;
                SordZ sordZ = SordC.mbMin;
                

                int idx = 0;
                fr = conn.Ix.findFirstSords(fi, 1000, sordZ);
                while (true)
                {
                    foreach (Sord sord in fr.sords)
                    {
                        bool isFolder = sord.type < SordC.LBT_DOCUMENT;
                        bool isDocument = sord.type >= SordC.LBT_DOCUMENT && sord.type <= SordC.LBT_DOCUMENT_MAX;
                        bool isReference = sord.parentId != parentId;

                        bool doExportScript = false;
                        // Keine Referenzen ausgeben
                        if (!exportReferences)
                        {
                            if (!isReference)
                            {
                                doExportScript = true;
                            }
                        }
                        // Referenzen mit ausgeben
                        else
                        {
                            doExportScript = true;
                        }

                        if (doExportScript)
                        {
                            // Wenn Ordner rekursiv aufrufen
                            if (isFolder)
                            {
                                // Neuen Ordner in Windows anlegen, falls noch nicht vorhanden
                                string subFolderPath = winPath + "\\" + sord.name;
                                if (!Directory.Exists(subFolderPath))
                                {
                                    try
                                    {
                                        Directory.CreateDirectory(subFolderPath);
                                    }
                                    catch (System.IO.PathTooLongException e)
                                    {
                                        Console.WriteLine("Exception: " + e.Message + " " + subFolderPath);
                                        Debug.WriteLine("Exception: " + e.Message + " " + subFolderPath);
                                        return;
                                    }
                                }
                                FindChildren(conn, arcPath + "/" + sord.name, subFolderPath, exportReferences);
                            }

                            // Wenn Dokument Pfad und Name ausgeben
                            if (isDocument)
                            {
                                // Dokument aus Archiv downloaden und in Windows anlegen
                                ed = conn.Ix.checkoutDoc(Convert.ToString(sord.id), null, EditInfoC.mbDocument, LockC.NO);
                                DocVersion dv = ed.document.docs[0];
                                String outFile = winPath + "\\" + sord.name + "." + dv.ext;
                                if (File.Exists(outFile))
                                {
                                    File.Delete(outFile);
                                }
                                try
                                {
                                    conn.Download(dv.url, outFile);
                                    Console.WriteLine("Arcpath=" + arcPath + "/" + sord.name);
                                    Debug.WriteLine("Arcpath=" + arcPath + "/" + sord.name);
                                }
                                catch (System.IO.PathTooLongException e)
                                {
                                    Console.WriteLine("Exception: " + e.Message + " " + outFile);
                                    Debug.WriteLine("Exception: " + e.Message + " " + outFile);
                                    return;
                                }
                            }

                        }

                    }
                    if (!fr.moreResults) break;
                    idx += fr.sords.Length;
                    fr = conn.Ix.findNextSords(fr.searchId, idx, 1000, sordZ);
                }
            }
            catch (byps.BException e)
            {
                if (e.Source != null)
                {
                    Console.WriteLine("byps.BException message: {0}", e.Message);
                    Debug.WriteLine("byps.BException message: {0}", e.Message);
                }
            }
            catch (System.IO.DirectoryNotFoundException e)
            {
                if (e.Source != null)
                {
                    Console.WriteLine("System.IO.DirectoryNotFoundException message: {0}", e.Message);
                    Debug.WriteLine("System.IO.DirectoryNotFoundException message: {0}", e.Message);
                }
            }
            catch (System.NotSupportedException e)
            {
                if (e.Source != null)
                {
                    Console.WriteLine("System.NotSupportedException message: {0}", e.Message);
                    Debug.WriteLine("System.NotSupportedException message: {0}", e.Message);
                }
            }
            finally
            {
                if (fr != null)
                {
                    conn.Ix.findClose(fr.searchId);
                }
            }
        }

        static void Main(string[] args)
        {
            // Defaultwerte
            string arcPath = "ARCPATH[1]:/Administration/Business Solutions";
            string winPath = "E:/Temp/sol";
            string ixUrl = "http://srvpdevbs01vm:8010/ix-invoice/ix";
            string user = "Ruberg";
            string pwd = "elo";
            string exportref = "false";

            var cmdLineOptions = new ExportSol.CommandLineOptions();
            if (CommandLine.Parser.Default.ParseArguments(args, cmdLineOptions))
            {
                Debug.WriteLine("arcpath: {0}", cmdLineOptions.arcpath);
                Debug.WriteLine("winpath: {0}", cmdLineOptions.winpath);
                Debug.WriteLine("ixurl: {0}", cmdLineOptions.ixurl);
                Debug.WriteLine("user: {0}", cmdLineOptions.user);
                Debug.WriteLine("pwd: {0}", cmdLineOptions.pwd);
                Debug.WriteLine("exportref: {0}", cmdLineOptions.exportref);
                arcPath = cmdLineOptions.arcpath;
                winPath = cmdLineOptions.winpath;
                ixUrl = cmdLineOptions.ixurl;
                user = cmdLineOptions.user;
                pwd = cmdLineOptions.pwd;
                exportref = cmdLineOptions.exportref;
            }
            else
            {
                Debug.WriteLine("cmdLineOptions not avaible");
            }

            try
            {
                IXConnFactory connFact = new IXConnFactory(ixUrl, "ExportSol", "1.0");
                IXConnection conn = connFact.Create(user, pwd, null, null);

                // TODO Referenzen standardmäßig ignorieren
                if (exportref.Equals("false"))
                {
                    FindChildren(conn, arcPath, winPath, false);
                }
                else
                {
                    FindChildren(conn, arcPath, winPath, true);
                }
                // TODO

                Console.WriteLine("ticket=" + conn.LoginResult.clientInfo.ticket);
                conn.Logout();
            }
            catch (byps.BException e)
            {
                if (e.Source != null)
                {
                    Console.WriteLine("byps.BException message: {0}", e.Message);
                    Debug.WriteLine("byps.BException message: {0}", e.Message);
                }
            }
            catch (System.Net.WebException e)
            {
                if (e.Source != null)
                {
                    Console.WriteLine("System.Net.WebException message: {0}", e.Message);
                    Debug.WriteLine("System.Net.WebException message: {0}", e.Message);
                }
            }
        }
    }
}
