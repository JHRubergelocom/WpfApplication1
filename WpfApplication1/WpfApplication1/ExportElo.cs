using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EloixClient.IndexServer;
using System.IO;
using System.Diagnostics;

namespace WpfApplication1
{
    class ExportElo
    {
        public static void FindChildren(IXConnection conn, string arcPath, string winPath, bool exportReferences, string maskName)
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
                        // Keine Referenzen ausgeben bei Dokumenten
                        if (!exportReferences && isDocument)
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

                        // Prüfen, ob bestimmte Verschlagwortungsmaske angegeben
                        if (doExportScript && isDocument)
                        {
                            if (maskName.Trim().Equals(""))
                            {
                                doExportScript = true;
                            }
                            else
                            {
                                if (sord.maskName.Equals(maskName))
                                {
                                    doExportScript = true;
                                }
                                else
                                {
                                    doExportScript = false;
                                }
                            }
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
                                FindChildren(conn, arcPath + "/" + sord.name, subFolderPath, exportReferences, maskName);
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
                                    Console.WriteLine("Arcpath=" + arcPath + "/" + sord.name + "  Maskname=" + sord.maskName);
                                    Debug.WriteLine("Arcpath=" + arcPath + "/" + sord.name + "  Maskname=" + sord.maskName);
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

        public static void StartExportElo(string profilename, KonfigurationIx ixConf)
        {
            try
            {
                IXConnFactory connFact = new IXConnFactory(ixConf.ixUrl, "StartExportElo", "1.0");
                IXConnection conn = connFact.Create(ixConf.user, ixConf.pwd, null, null);
                string winPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\ExportElo\\" + profilename;
                if (!Directory.Exists(winPath)) 
                {
                    Directory.CreateDirectory(winPath);
                }

                FindChildren(conn, ixConf.arcPath, winPath, ixConf.exportReferences, ixConf.maskName);

                Debug.WriteLine("ticket=" + conn.LoginResult.clientInfo.ticket);
                conn.Logout();
            }
            catch (byps.BException e)
            {
                if (e.Source != null)
                {
                    Debug.WriteLine("byps.BException message: {0}", e.Message);
                }
            }
            catch (System.Net.WebException e)
            {
                if (e.Source != null)
                {
                    Debug.WriteLine("System.Net.WebException message: {0}", e.Message);
                }
            }
        }
    }
}
