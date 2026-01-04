using System.Text;
using System.Buffers;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Common.CoreLib.Util
{
    /// <summary>
    /// Json任何类型读取到字符串属性
    /// 因为 System.Text.Json 必须严格遵守类型一致，当非字符串读取到字符属性时报错：
    /// The JSON value could not be converted to System.String.
    /// </summary>
    public class StringConverter : JsonConverter<string>
    {
        public override string Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.String)
            {
                return reader.GetString();
            }
            else
            {
                //非字符类型，返回原生内容
                return GetRawPropertyValue(reader);
            }

            throw new JsonException();
        }

        public override void Write(Utf8JsonWriter writer, string value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value);
        }

        /// <summary>
        /// 非字符类型，返回原生内容
        /// </summary>
        /// <param name="jsonReader"></param>
        /// <returns></returns>
        private static string GetRawPropertyValue(Utf8JsonReader jsonReader)
        {
            ReadOnlySpan<byte> utf8Bytes = jsonReader.HasValueSequence ?
            jsonReader.ValueSequence.ToArray() :
            jsonReader.ValueSpan;
            return Encoding.UTF8.GetString(utf8Bytes);
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
        /// <returns></returns>
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
    /// 
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
