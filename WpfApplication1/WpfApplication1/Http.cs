using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace WpfApplication1
{
    class Http
    {
        public static void OpenUrl(string url)
        {
            try
            {
                Process.Start(url);
            }
            catch (Exception)
            {
                Process.Start("IExplore.exe", url);
            }
        }

        public static string CreateHtmlHead(string title)
        {
            string htmlHead = "  <head>\n";
            htmlHead += "    <title>" + title + "</title>\n";
            htmlHead += "  </head>\n";
            return htmlHead;
        }

        public static string CreateHtmlStyle()
        {
            string htmlStyle = "  <style>\n";
             
            htmlStyle += "body {\n";
            htmlStyle += "  font-family: 'Segoe UI', Verdana, 'sans serif';\n";
            htmlStyle += "  margin: 15px;\n";
		    htmlStyle += "  font-size: 12px;\n";
            htmlStyle += "}\n";
            htmlStyle += "table {\n";
		    htmlStyle += "  font-size: 12px;\n";
            htmlStyle += "  padding-left: 10px;\n";
            htmlStyle += "  border-width: 0px;\n";
            htmlStyle += "  border-collapse: collapse;\n";
            htmlStyle += "  border-color: #A2A2A2;\n";
            htmlStyle += "}\n";
            htmlStyle += "table td {\n";
            htmlStyle += "  padding: 3px 7px;\n";
            htmlStyle += "}\n";
            htmlStyle += "table tr {\n";
            htmlStyle += "  white-space: nowrap;\n";
            htmlStyle += "}\n";
            htmlStyle += ".tdh {\n";
            htmlStyle += "  font-weight: bold;\n";
            htmlStyle += "  padding: 5px 5px 5px 5px;\n";
            htmlStyle += "  background-color: #A2A2A2;\n";
            htmlStyle += "  border-top-width: 1px;\n";
            htmlStyle += "  border-left-width: 1px;\n";
            htmlStyle += "  border-right-width: 1px;\n";
            htmlStyle += "  border-bottom-width: 1px;\n";
            htmlStyle += "  border-color: grey;\n";
            htmlStyle += "  border-style: solid;\n";
            htmlStyle += "  border-collapse: collapse;\n";
            htmlStyle += "}\n";
            htmlStyle += ".td1 {\n";
            htmlStyle += "  background-color: white;\n";
            htmlStyle += "  border-top-width: 0px;\n";
            htmlStyle += "  border-left-width: 1px;\n";
            htmlStyle += "  border-right-width: 1px;\n";
            htmlStyle += "  border-bottom-width: 0px;\n";
            htmlStyle += "  border-color: grey;\n";
            htmlStyle += "  border-style: solid;\n";
            htmlStyle += "  border-collapse: collapse;\n";
            htmlStyle += "}\n";
            htmlStyle += ".td2 {\n";
            htmlStyle += "  background-color: #A2A2A2;\n";
            htmlStyle += "  border-top-width: 0px;\n";
            htmlStyle += "  border-left-width: 1px;\n";
            htmlStyle += "  border-right-width: 1px;\n";
            htmlStyle += "  border-bottom-width: 0px;\n";
            htmlStyle += "  border-color: grey;\n";
            htmlStyle += "  border-style: solid;\n";
            htmlStyle += "  border-collapse: collapse;\n";
            htmlStyle += "}\n";
            htmlStyle += ".td1b {\n";
            htmlStyle += "  background-color: white;\n";
            htmlStyle += "  border-top-width: 0px;\n";
            htmlStyle += "  border-left-width: 1px;\n";
            htmlStyle += "  border-right-width: 1px;\n";
            htmlStyle += "  border-bottom-width: 1px;\n";
            htmlStyle += "  border-color: grey;\n";
            htmlStyle += "  border-style: solid;\n";
            htmlStyle += "  border-collapse: collapse;\n";
            htmlStyle += "}\n";
            htmlStyle += ".td2b {\n";
            htmlStyle += "  background-color: #A2A2A2;\n";
            htmlStyle += "  border-top-width: 0px;\n";
            htmlStyle += "  border-left-width: 1px;\n";
            htmlStyle += "  border-right-width: 1px;\n";
            htmlStyle += "  border-bottom-width: 1px;\n";
            htmlStyle += "  border-color: grey;\n";
            htmlStyle += "  border-style: solid;\n";
            htmlStyle += "  border-collapse: collapse;\n";
            htmlStyle += "}\n";
            htmlStyle += ".td1r {\n";
            htmlStyle += "  color: red;\n";
            htmlStyle += "  background-color: white;\n";
            htmlStyle += "  border-top-width: 0px;\n";
            htmlStyle += "  border-left-width: 1px;\n";
            htmlStyle += "  border-right-width: 1px;\n";
            htmlStyle += "  border-bottom-width: 0px;\n";
            htmlStyle += "  border-color: grey;\n";
            htmlStyle += "  border-style: solid;\n";
            htmlStyle += "  border-collapse: collapse;\n";
            htmlStyle += "}\n";
            htmlStyle += ".td2r {\n";
            htmlStyle += "  color: red;\n";
            htmlStyle += "  background-color: #A2A2A2;\n";
            htmlStyle += "  border-top-width: 0px;\n";
            htmlStyle += "  border-left-width: 1px;\n";
            htmlStyle += "  border-right-width: 1px;\n";
            htmlStyle += "  border-bottom-width: 0px;\n";
            htmlStyle += "  border-color: grey;\n";
            htmlStyle += "  border-style: solid;\n";
            htmlStyle += "  border-collapse: collapse;\n";
            htmlStyle += "}\n";
            htmlStyle += ".td1br {\n";
            htmlStyle += "  color: red;\n";
            htmlStyle += "  background-color: white;\n";
            htmlStyle += "  border-top-width: 0px;\n";
            htmlStyle += "  border-left-width: 1px;\n";
            htmlStyle += "  border-right-width: 1px;\n";
            htmlStyle += "  border-bottom-width: 1px;\n";
            htmlStyle += "  border-color: grey;\n";
            htmlStyle += "  border-style: solid;\n";
            htmlStyle += "  border-collapse: collapse;\n";
            htmlStyle += "}\n";
            htmlStyle += ".td2br {\n";
            htmlStyle += "  color: red;\n";
            htmlStyle += "  background-color: #A2A2A2;\n";
            htmlStyle += "  border-top-width: 0px;\n";
            htmlStyle += "  border-left-width: 1px;\n";
            htmlStyle += "  border-right-width: 1px;\n";
            htmlStyle += "  border-bottom-width: 1px;\n";
            htmlStyle += "  border-color: grey;\n";
            htmlStyle += "  border-style: solid;\n";
            htmlStyle += "  border-collapse: collapse;\n";
            htmlStyle += "}\n";
            htmlStyle += "h1 {\n";
            htmlStyle += "  padding: 30px 0px 0px 0px;\n";
            htmlStyle += "  font-size: 16px;\n";
            htmlStyle += "  font-weight: bold;\n";
            htmlStyle += "}\n";

            htmlStyle += "  </style>\n";
            return htmlStyle;
        }

        public static string CreateHtmlTable(string header, List<string> cols, List<List<string>> rows)
        {
            string htmlTable = "    <h1>" + header + "</h1>\n";
            htmlTable += "    <div class='container'>\n";
            htmlTable += "      <table border='2'>\n";
            htmlTable += "        <colgroup>\n";
            foreach(string col in cols)
            {
                htmlTable += "          <col width='100'>\n";
            }
            htmlTable += "        </colgroup>\n";
            htmlTable += "        <tr>\n";
            foreach (string col in cols)
            {
                htmlTable += "          <td class = 'tdh' align='left' valign='top'>" + col + "</td>\n";
            }
            htmlTable += "        </tr>\n";

            int i = 0;
            foreach (List<string> row in rows)
            {
                string td = "td2";
                if ((i % 2) == 0)
                {
                    td = "td1";
                }
                if (i == (rows.Count - 1))
                {
                    td += "b";
                }

                htmlTable += "        <tr>\n";

                foreach (string cell in row)
                {
                    if(cell.Equals("False"))
                    {
                        td += "r";
                    }
                }    
                foreach (string cell in row)
                {
                    htmlTable += "          <td class = '" + td + "' align='left' valign='top'>" + cell + "</td>\n";
                }

                htmlTable += "        </tr>\n";
                i++;
            }
            htmlTable += "      </table>\n";
            htmlTable += "    </div>\n";

            return htmlTable;
        }

        public static string CreateHtmlReport(SortedDictionary<string, bool> dicRFs, SortedDictionary<string, bool> dicASDirectRules, SortedDictionary<string, bool> dicActionDefs)
        {
            string htmlDoc = "<html>\n";
            string htmlHead = Http.CreateHtmlHead("Register Functions matching Unittest");
            string htmlStyle = Http.CreateHtmlStyle();
            string htmlBody = "<body>\n";

            List<string> cols = new List<string>();
            cols.Add("RF");
            cols.Add("Unittest");
            List<List<string>> rows = new List<List<string>>();
            foreach (KeyValuePair<string, bool> kvp in dicRFs)
            {
                List<string> row = new List<string>();
                row.Add(kvp.Key);
                row.Add(kvp.Value.ToString());
                rows.Add(row);
            }
            string htmlTable = Http.CreateHtmlTable("Register Functions matching Unittest", cols, rows);
            htmlBody += htmlTable;

            cols = new List<string>();
            cols.Add("AS Direct Rule");
            cols.Add("Unittest");
            rows = new List<List<string>>();
            foreach (KeyValuePair<string, bool> kvp in dicASDirectRules)
            {
                List<string> row = new List<string>();
                row.Add(kvp.Key);
                row.Add(kvp.Value.ToString());
                rows.Add(row);
            }
            htmlTable = Http.CreateHtmlTable("AS Direct Rules matching Unittest", cols, rows);
            htmlBody += htmlTable;

            cols = new List<string>();
            cols.Add("Action Definition");
            cols.Add("Unittest");
            rows = new List<List<string>>();
            foreach (KeyValuePair<string, bool> kvp in dicActionDefs)
            {
                List<string> row = new List<string>();
                row.Add(kvp.Key);
                row.Add(kvp.Value.ToString());
                rows.Add(row);
            }
            htmlTable = Http.CreateHtmlTable("Action Definitions matching Unittest", cols, rows);
            htmlBody += htmlTable;


            htmlBody += "</body>\n";
            htmlDoc += htmlHead;
            htmlDoc += htmlStyle;
            htmlDoc += htmlBody;
            htmlDoc += "</html>\n";

            return htmlDoc;
        }

    }
}
