using System.Text.Encodings.Web;
using System.Text.Json.Serialization;

namespace System.Text.Json
{
    public static class JapxSerializationOptions
    {
        static JapxSerializationOptions()
        {
            Default = new JsonSerializerOptions()
            {
                // JsonSerializerDefaults.Web配置：
                //_propertyNameCaseInsensitive = true; // 不区分大小写的属性匹配
                //_jsonPropertyNamingPolicy = JsonNamingPolicy.CamelCase; // 对属性名称使用camel大小写
                //_numberHandling = JsonNumberHandling.AllowReadingFromString; // 允许或写入带引号的数字

                // 忽略空值
                //IgnoreNullValues = true,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                // HTML不转义
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };
            Default.Converters.Add(new CultureCustomConverter());
            Default.Converters.Add(new TimeZoneInfoConverter());
            Default.Converters.Add(new TimeSpanConverter());
        }

        public static JsonSerializerOptions Default { get; private set; }
    }
}
