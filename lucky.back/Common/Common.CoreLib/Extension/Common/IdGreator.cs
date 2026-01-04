using Lucky.BaseModel;
using Yitter.IdGenerator;

namespace Common.CoreLib.Extension.Common
{
    /// <summary>
    /// 分布式Id生成
    /// SeqBitLength=6 Id是16位; SeqBitLength=10 Id是17位; SeqBitLength=12 Id是18位
    /// 如果超过5W个/s，低于50W个/s，推荐修改：SeqBitLength=10
    /// 如果超过50W个/s，接近500W个/s，推荐修改：SeqBitLength=12
    /// 增加 SeqBitLength 会让性能更高，但生成的 ID 会更长
    /// </summary>
    public class IdGreator
    {
        static IdGreator()
        {
            var workerId = GlobalConstant.MachineNo;  // 不同服务器(物理机)设置不同的值
            var opts = new IdGeneratorOptions(workerId);
            opts.WorkerIdBitLength = 6; // 最多支持64个节点
            opts.SeqBitLength = 6; // 默认值6，限制每毫秒生成的ID个数。若生成速度超过5万个/秒，建议加大 SeqBitLength 到 10
            opts.BaseTime = DateTime.Parse("1999-12-31 23:59:59");  // 基准时间

            // 设置参数配置（务必调用，否则参数设置不生效）：
            YitIdHelper.SetIdGenerator(opts);
        }

        /// <summary>
        /// 生成Id
        /// </summary>
        public static long GetNxtId()
        {
            return YitIdHelper.NextId();
        }
    }
}
