using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using EloixClient.IndexServer;

namespace WpfApplication1
{
    class RepoUtils
    {
        public static List<Sord> FindChildren(String objId, IXConnection ixConn, bool references)
        {
            Console.WriteLine("FindChildren: objId " + objId, " ixConn " + ixConn);
            try
            {
                ixConn.Ix.checkoutSord(objId, SordC.mbAll, LockC.NO);
            }
            catch (Exception)
            {
                return new List<Sord>();
            }

            List<Sord> children = new List<Sord>();
            FindInfo findInfo = new FindInfo();
            FindChildren findChildren = new FindChildren();
            FindByType findByType = new FindByType();
            FindByIndex findByIndex = new FindByIndex();
            Boolean includeReferences = references;
            SordZ sordZ = SordC.mbAll;
            Boolean recursive = true;
            int level = 3;
            ObjKey[] objKeys = new ObjKey[] { };
            findChildren.parentId = objId;
            findChildren.mainParent = !includeReferences;
            findChildren.endLevel = (recursive) ? level : 1;
            findInfo.findChildren = findChildren;
            findInfo.findByIndex = findByIndex;

            FindResult findResult = new FindResult();
            try
            {
                int idx = 0;
                findResult = ixConn.Ix.findFirstSords(findInfo, 1000, sordZ);
                while (true)
                {
                    for (int i = 0; i < findResult.sords.Length; i++)
                    {
                        children.Add(findResult.sords[i]);
                    }
                    if (!findResult.moreResults)
                    {
                        break;
                    }
                    idx += findResult.sords.Length;
                    findResult = ixConn.Ix.findNextSords(findResult.searchId, idx, 1000, sordZ);
                }
            }
            finally
            {
                if (findResult != null)
                {
                    ixConn.Ix.findClose(findResult.searchId);
                }
            }
            return children;
        }

        public static List<string> LoadTextDocs(String parentId, IXConnection ixConn)
        {
            List<Sord> sordRFInfo = RepoUtils.FindChildren(parentId, ixConn, true);
            List<string> docTexts = new List<string>();

            foreach (Sord s in sordRFInfo)
            {
                string objId = s.id + "";
                EditInfo editInfo = ixConn.Ix.checkoutDoc(objId, null, EditInfoC.mbSordDoc, LockC.NO);
                if (editInfo.document.docs.Length > 0)
                {
                    DocVersion dv = editInfo.document.docs[0];
                    string url = dv.url;
                    Stream inputStream = ixConn.Download(url, 0, -1);
                    string docText = new StreamReader(inputStream, Encoding.UTF8).ReadToEnd();
                    docTexts.Add(docText);
                }
            };
            return docTexts;
        }

    }
}
