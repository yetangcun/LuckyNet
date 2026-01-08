namespace Lucky.BaseModel.Enum
{
    /// <summary>
    /// 数据库类型枚举
    /// </summary>
    public enum DatabaseType
    {
        /// <summary>
        /// mysql
        /// </summary>
        Mysql = 0,

        /// <summary>
        /// sqlserver
        /// </summary>
        Sqlserver = 1,

        /// <summary>
        /// sqlite
        /// </summary>
        Sqlite = 2,

        /// <summary>
        /// Oracle
        /// </summary>
        Oracle = 3,

        /// <summary>
        /// postgresql
        /// </summary>
        Postgresql = 4,

        /// <summary>
        /// 达梦
        /// </summary>
        Dameng = 5,

        /// <summary>
        /// ClickHouse
        /// </summary>
        ClickHouse = 13,

        /// <summary>
        /// 华为云数据库
        /// </summary>
        GaussDB = 18,

        /// <summary>
        /// 蚂蚁集团数据库
        /// </summary>
        OceanBase = 19,

        /// <summary>
        /// 阿里云数据库
        /// </summary>
        PolarDB = 22,

        /// <summary>
        /// Doris
        /// </summary>
        Doris = 23,

        /// <summary>
        /// mongodb
        /// </summary>
        Mongodb = 32,
    }
}
