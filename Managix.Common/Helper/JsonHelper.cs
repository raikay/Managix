using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Managix.Infrastructure
{
    public class JsonHelper
    {
        public static string SerializeObject(object obj)
        {
            return JsonConvert.SerializeObject(obj);

        }

        public static T DeserializeObject<T>(string jsonStr)
        {
            return JsonConvert.DeserializeObject<T>(jsonStr);
        }
    }
}
