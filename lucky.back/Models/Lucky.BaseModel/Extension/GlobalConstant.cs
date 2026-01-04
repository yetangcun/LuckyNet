namespace Lucky.BaseModel
{
    /// <summary>
    /// 全局静态常量
    /// </summary>
    public class GlobalConstant
    {
        /// <summary>
        /// 机器编号
        /// </summary>
        public const ushort MachineNo = 1;

        #region AES 密钥
        //public const string AES_KY = "aiot2kyaes666666";  // 16位
        //public const string AES_IV = "aiot2ivaes888888";  // 16位

        /// <summary>
        /// AES 密钥
        /// </summary>
        public const string AES_KY = "6666666666666666";  // 16位

        /// <summary>
        /// AES 向量
        /// </summary>
        public const string AES_IV = "8888888888888888";  // 16位
        #endregion

    }
}
