using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

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

        /// <summary>
        ///  获取分页列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="where"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        Task<(int, List<T>)> GetPageListAsync<T>(Expression<Func<T, bool>> where, int pageIndex, int pageSize) where T : class;
    }
}
