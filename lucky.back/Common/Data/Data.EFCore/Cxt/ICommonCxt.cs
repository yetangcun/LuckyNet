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
        TCxt GetDbCxt<TCxt, TOpt>(TOpt option) where TCxt : CommonCxt where TOpt : DbDefaultOption;

        /// <summary>
        ///  初始化
        /// </summary>
        /// <param name="opt"></param>
        /// <returns></returns>
        bool SetDbOption(DbDefaultOption opt);

        /// <summary>
        /// 初始化库、表结构
        /// </summary>
        /// <returns></returns>
        bool InitDbTable();

        /// <summary>
        /// 初始化数据
        /// </summary>
        /// <returns></returns>
        bool InitData();
    }
}
