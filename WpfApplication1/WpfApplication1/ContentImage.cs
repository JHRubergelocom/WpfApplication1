using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Diagnostics;
using System.Xml;
using System.Runtime.InteropServices;
using System.IO;

namespace WpfApplication1
{
    class ContentImage : Content
    {

        [DllImport("Image.dll")]
        public static extern void Foo_Delete(IntPtr value);

        XmlNode data;
        String width;
        String height;
        String originFormat;
        String format;
        public String filename;
        String originFilename;
        Boolean requiresConversion = false;
        String altText;

        public ContentImage(XmlNode node, Page page)
        {
            this.node = node;
            this.page = page;

            format = node.Attributes["format"].InnerText;
            originFormat = node.Attributes["format"].InnerText;
            // alternative text
            try
            {
                altText = node.Attributes["alt"].InnerText;
                if (altText.StartsWith("Computergenerierter Alternativtext:"))
                {
                    altText = altText.Substring(altText.IndexOf(":") + 1);
                    altText = altText.Trim();
                }
            }
            catch (Exception)
            {
                altText = "";
            }

            foreach (XmlNode childNode in node.ChildNodes)
            {
                switch (childNode.Name)
                {
                    case "one:Size":
                        width = childNode.Attributes["width"].InnerText;
                        height = childNode.Attributes["height"].InnerText;
                        break;
                    case "one:Data":
                        data = childNode;
                        break;
                    default:
                        Debug.WriteLine("ERROR: Unknown type for paragraph: " + childNode.Name);
                        // ignore type
                        break;
                }
            }

            this.readFilename();
        }

        public void readFilename()
        {
            String ext;
            switch (this.originFormat.ToLower())
            {
                case ("emf"):
                    ext = "png";
                    this.requiresConversion = true;
                    break;
                default:
                    ext = this.originFormat.ToLower();
                    break;
            }

            String idname = "img" + page.getNextImageId();
            this.filename = idname + "." + ext;
            this.originFilename = idname + "." + this.originFormat.ToLower();

        }

        public override String render(KonfigurationOneNote onenoteConf)
        {
            String filePath = this.page.getRenderPagePath();
            if (altText.Trim().Equals(""))
            {
                return "{@img " + filename + "}";
            }
            return "{@img " + filename + " " + altText + "}"; // alt-text aus xml-Knoten
        }

        public void saveImage(KonfigurationOneNote onenoteConf)
        {
            String filePath = this.page.getPagePath(onenoteConf);
            String fn = this.filename;

            if (this.requiresConversion)
            {
                fn = this.originFilename;
            }
            File.WriteAllBytes(filePath + @"\" + fn, Convert.FromBase64String(data.InnerText));


        }

        // rename thumbnail image
        public void renameToIcon()
        {
            this.filename = "icon." + (this.filename.Split('.')[1]);
            this.originFilename = "icon." + (this.originFilename.Split('.')[1]);
        }
    }
}
