using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Diagnostics;
using Microsoft.Office.Interop.OneNote;
using System.Xml;

namespace WpfApplication1
{
    class ExportOneNotePages
    {
        public static Application oneNote;

        public static string GetOutPutDir(string notebook)
        {
            string outputPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\OneNoteExport\\" + notebook + "\\output";
            return outputPath;
        }

        public static string GetTempDir(string notebook)
        {
            string tempPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\OneNoteExport\\" + notebook + "\\temp";
            return tempPath;
        }

        public static void StartExportOneNotePages(string profilename, KonfigurationOneNote onenoteConf)
        {
            oneNote = new Application();
            String oneNoteBooksC;
            Application onApplication = new Application();
            onApplication.GetHierarchy(null,
                HierarchyScope.hsPages, out oneNoteBooksC);

            XmlDocument oneNoteBooksXml = new XmlDocument();
            oneNoteBooksXml.LoadXml(oneNoteBooksC);
            string strNamespace = "http://schemas.microsoft.com/office/onenote/2013/onenote";
            XmlNamespaceManager nsmgr = new XmlNamespaceManager(oneNoteBooksXml.NameTable);
            nsmgr.AddNamespace("one", strNamespace);
            XmlNode xmlOneNoteBook = oneNoteBooksXml.SelectSingleNode("//one:Notebook[@name='" + onenoteConf.notebook + "']", nsmgr);

            OneNoteBook book = new OneNoteBook(xmlOneNoteBook, onenoteConf);
            book.saveOneNoteBook(onenoteConf);
            book.saveOneNoteBookCfg(onenoteConf);

            Debug.WriteLine("Done.");
        }
    }
}
