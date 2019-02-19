using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using EloixClient.IndexServer;

namespace WpfApplication1
{
    class ActionDefinitions
    {
        public static SortedDictionary<string, bool> GetActionDefs(IXConnection ixConn, List<string> jsTexts, string package)
        {
            String parentId = "ARCPATH[(E10E1000-E100-E100-E100-E10E10E10E00)]:/Business Solutions/" + package + "/Action definitions";
            if (package.Equals(""))
            {
                parentId = "ARCPATH[(E10E1000-E100-E100-E100-E10E10E10E00)]:/Business Solutions/_global/Action definitions";
            }
            List<Sord> sordActionDefInfo = RepoUtils.FindChildren(parentId, ixConn, true);
            SortedDictionary<string, bool> dicActionDefs = new SortedDictionary<string, bool>();
            foreach (Sord s in sordActionDefInfo)
            {
                string actionDef = s.name;
                string[] rf = actionDef.Split('.');
                actionDef = rf[rf.Length-1];
                actionDef = "actions." + actionDef;

                if (!dicActionDefs.ContainsKey(actionDef))
                {
                    bool match = Unittests.Match(ixConn, actionDef, package, jsTexts);
                    dicActionDefs.Add(actionDef, match);
                }
            };
            return dicActionDefs;
        }

    }
}
