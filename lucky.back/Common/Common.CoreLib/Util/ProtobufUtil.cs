using Microsoft.Extensions.Logging;
using ProtoBuf;

namespace Common.CoreLib.Util
{
    /// <summary>
    /// Protobuf工具类
    /// </summary>
    public class ProtobufUtil
    {
        private readonly ILogger<ProtobufUtil> _logger;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger"></param>
        public ProtobufUtil(ILogger<ProtobufUtil> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// 对象转字节
        /// 此对象T必须是被ProtoBuf.ProtoContract特性修饰的类
        /// 也就是在定义的类上加上属性标识：[ProtoContract]
        /// [ProtoContract]
        /// </summary>
        public byte[] Tobytes<T>(T obj)
        {
            try
            {
                using (var ms = new MemoryStream())
                {
                    Serializer.Serialize(ms, obj);
                    var buffer = ms.GetBuffer();
                    var bts = new byte[ms.Length];
                    Array.Copy(buffer, bts, ms.Length);
                    return bts;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ProtobufUtil.Tobytes");
            }

            return new byte[0];
        }

        /// <summary>
        /// 字节转对象
        /// </summary>
        public T? ToObj<T>(byte[] buffBytes)
        {
            if (buffBytes == null || buffBytes.Length == 0)
                return default;

            try
            {
                using (var ms = new MemoryStream(buffBytes))
                {
                    return Serializer.Deserialize<T>(ms);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ProtobufUtil.ToObj");
            }

            return default;
        }
    }
}
