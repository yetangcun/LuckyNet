using Lucky.BaseModel.Enum;

namespace Common.CoreLib.Model.Option
{
    /// <summary>
    /// 系统管理模块数据库配置
    /// </summary>
    public class SysDbOption : DbDefaultOption
    {
        /// <summary>
        /// 是否迁移
        /// </summary>
        public bool IsMigration { get; set; } = false;
    }

    /// <summary>
    /// 协议管理模块数据库配置
    /// </summary>
    public class PrtclDbOption : DbDefaultOption
    {
    }


    /// <summary>
    /// 数据库参数选项
    /// </summary>
    public class DbDefaultOption
    {
        /// <summary>
        /// 是否只读
        /// </summary>
        public bool? IsReadOnly { get; set; } = false;

        ///// <summary>
        ///// 企业编号
        ///// </summary>
        //public string? EnterpriseCode { get; set; }
        ///// <summary>
        ///// 业务编码
        ///// </summary>
        //public string? BusinessCode { get; set; }

        /// <summary>
        /// 数据库类型
        /// </summary>
        public DatabaseType DbType { get; set; }

        /// <summary>
        /// 主库连接串
        /// </summary>
        public required string MasterConnString { get; set; }

        /// <summary>
        /// 从库连接串
        /// </summary>
        public string? SlaveConnString { get; set; }

        /// <summary>
        /// 默认超时时间|秒
        /// </summary>
        public int TimeOuts { get; set; } = 20;
    }

    /// <summary>
    /// click数据库配置项
    /// </summary>
    public class ClkhouseOption
    {
        /// <summary>
        /// 主机
        /// </summary>
        public required string Host { get; set; }

        /// <summary>
        /// 端口
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public required string User { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public required string Password { get; set; }

        /// <summary>
        /// 数据库名
        /// </summary>
        public required string Database { get; set; }
    }
}
