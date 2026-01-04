using System;
using System.Text;
using System.Collections.Generic;

namespace Data.EFCore.Rpsty
{
    /// <summary>
    /// 通用仓储接口
    /// </summary>
    public interface ICommonRpsty
    {
        /// <summary>
        /// 设置数据库只读
        /// </summary>
        /// <param name="isReadOnly"></param>
        void SetDbReadOnly(bool isReadOnly = false);
    }
}
