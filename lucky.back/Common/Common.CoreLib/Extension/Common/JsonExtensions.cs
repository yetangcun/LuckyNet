using System.Text.Json;
using System.Text.Encodings.Web;
using System.Text.Json.Serialization;

namespace Common.CoreLib.Extension.Common
{
    /// <summary>
    /// Json扩展
    /// </summary>
    public static class JsonExtensions
    {
        private static readonly JsonSerializerOptions dftOpts = new()
        {
            WriteIndented = true,
            PropertyNamingPolicy = null,
            PropertyNameCaseInsensitive = true,
            ReferenceHandler = ReferenceHandler.IgnoreCycles,
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };

        static JsonExtensions()
        {
            dftOpts.Converters.Add(new DateTimeJsonConverter());
            dftOpts.Converters.Add(new Util.StringConverter());
            dftOpts.Converters.Add(new DateTimeJsonConverter("yyyy-MM-dd HH:mm:ss"));
        }

        /// <summary>
        /// 序列化
        /// </summary>
        public static string ToJson<T>(this T obj)
        {
            return JsonSerializer.Serialize(obj, dftOpts);
        }

        /// <summary>
        /// 反序列化
        /// </summary>
        public static T? ToObj<T>(this string json)
        {
            return JsonSerializer.Deserialize<T>(json, dftOpts);
        }
    }

    /// <summary>
    /// 自定义时间格式
    /// </summary>
    public class DateTimeJsonConverter : JsonConverter<DateTime>
    {
        private readonly string _dateFormatString;

        /// <summary>
        /// 默认格式
        /// </summary>
        public DateTimeJsonConverter()
        {
            _dateFormatString = "yyyy-MM-dd HH:mm:ss";
        }

        /// <summary>
        /// 自定义格式
        /// </summary>
        /// <param name="dateFormatString"></param>
        public DateTimeJsonConverter(string dateFormatString)
        {
            _dateFormatString = dateFormatString;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="typeToConvert"></param>
        /// <param name="options"></param>
        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return DateTime.Parse(reader.GetString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="value"></param>
        /// <param name="options"></param>
        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToUniversalTime().ToString(_dateFormatString));
        }
    }

    /// <summary>
    /// 布尔转换
    /// </summary>
    public class BoolJsonConverter : JsonConverter<bool>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="typeToConvert"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public override bool Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.True || reader.TokenType == JsonTokenType.False)
                return reader.GetBoolean();

            return bool.Parse(reader.GetString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="value"></param>
        /// <param name="options"></param>
        public override void Write(Utf8JsonWriter writer, bool value, JsonSerializerOptions options)
        {
            writer.WriteBooleanValue(value);
        }
    }
}
