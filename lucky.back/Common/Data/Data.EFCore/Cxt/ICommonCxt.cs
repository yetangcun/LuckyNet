using Common.CoreLib.Model.Option;

namespace Data.EFCore.Cxt
{
    /// <summary>
    /// 通用数据库上下文接口
    /// </summary>
    public interface ICommonCxt
    {
        /// <summary>
        /// 获取上下文
        /// </summary>
        /// <returns></returns>
        TCxt GetDbCxt<TCxt, TOpt>(TOpt option) where TCxt : CommonCxt where TOpt : EfcoreOption;
    }
}
