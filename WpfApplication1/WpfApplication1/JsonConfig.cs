using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.IO;

namespace WpfApplication1
{
    [DataContract]
    internal class JsonSelector
    {
        [DataMember]
        internal string[] ids = new string[] { };

        [DataMember]
        internal string arcPath = "";
    }

    [DataContract]
    internal class JsonView
    {
        [DataMember]
        internal string show = "";

        [DataMember]
        internal string focus = "";

        [DataMember]
        internal string type = "";
    }

    [DataContract]
    internal class JsonWeb
    {
        [DataMember]
        internal string id = "";

        [DataMember]
        internal string namespace1 = "";

        [DataMember]
        internal bool eloSession = false;
    }

    [DataContract]
    internal class JsonConfig
    {
        [DataMember]
        internal JsonSelector selector = new JsonSelector();

        [DataMember]
        internal JsonView view = new JsonView();

        [DataMember]
        internal JsonWeb web = new JsonWeb();

        [DataMember]
        internal string id = "";

        // Serialize config to a JSON stream.  
        public static string WriteFromObject(JsonConfig config)
        {
            //Create a stream to serialize the object to.  
            MemoryStream ms = new MemoryStream();

            // Serializer the User object to the stream.  
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(JsonConfig));
            ser.WriteObject(ms, config);
            byte[] json = ms.ToArray();
            ms.Close();
            return Encoding.UTF8.GetString(json, 0, json.Length);
        }

        // Deserialize a JSON stream to a User object.  
        public static JsonConfig ReadToObject(string json)
        {
            JsonConfig deserializedConfig = new JsonConfig();
            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(json));
            DataContractJsonSerializer ser = new DataContractJsonSerializer(deserializedConfig.GetType());
            deserializedConfig = ser.ReadObject(ms) as JsonConfig;
            ms.Close();
            return deserializedConfig;
        }
    }
}
