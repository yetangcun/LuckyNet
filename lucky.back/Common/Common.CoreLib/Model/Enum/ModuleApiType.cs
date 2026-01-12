namespace Common.CoreLib.Model.Enum
{
    /// <summary>
    /// api模块信息
    /// </summary>
    public class ModuleApiInfo : Attribute
    {
        /// <summary>
        /// 标题
        /// </summary>
        public string? Title { get; set; }

        /// <summary>
        /// 版本
        /// </summary>
        public string? Version { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string? Description { get; set; }
    }

    /// <summary>
    /// api模块分组
    /// </summary>
    public enum ModuleApiType
    {
        /// <summary>
        /// 系统管理
        /// </summary>
        [ModuleApiInfo(Description = "系统管理模块", Title = "系统管理", Version = "1.0")]
        sys = 0,

        /// <summary>
        /// 消息队列
        /// </summary>
        [ModuleApiInfo(Description = "消息队列模块", Title = "消息队列", Version = "1.0")]
        mq = 1,

        /// <summary>
        /// IoT
        /// </summary>
        [ModuleApiInfo(Description = "IoT服务模块", Title = "IoT服务", Version = "1.0")]
        IoT
    }
}
