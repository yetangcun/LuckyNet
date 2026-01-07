using Lucky.BaseModel.Model;
using System.Linq.Expressions;
using Lucky.BaseModel.Model.Entity;

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
        Task<(int, List<T>)> GetPageListAsync<T>(Expression<Func<T, bool>> where, PageInfo page) where T : class;

        /// <summary>
        /// 获取分页列表
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <typeparam name="TResult">结果类型</typeparam>
        /// <param name="where">查询条件</param>
        /// <param name="selector">选择器</param>
        /// <param name="page">分页参数</param>
        Task<(int, List<TResult>)> GetPagesAsync<T, TResult>(Expression<Func<T, bool>>? where, Expression<Func<T, TResult>> selector, PageInfo page) where T : class;

        /// <summary>
        /// 获取最大值
        /// </summary>
        Task<TField?> MaxAsync<T, TField>(Expression<Func<T, bool>>? where, Expression<Func<T, TField>> field) where T : class;

        /// <summary>
        /// 添加
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        Task<T> AddAsync<T>(T entity) where T : class;

        /// <summary>
        /// 批量添加
        /// </summary>
        Task<int> AddRangeAsync<T>(List<T> entities) where T : class;

        /// <summary>
        /// 删除
        /// </summary>
        Task<int> DeleteAsync<T>(T entity) where T : class;

        /// <summary>
        /// 批量删除
        /// </summary>
        Task<int> DeleteRangeAsync<T>(List<T> entities) where T : class;

        /// <summary>
        /// 修改
        /// </summary>
        Task<int> UpdateAsync<T>(T entity) where T : class;

        /// <summary>
        /// 批量修改
        /// </summary>
        Task<int> UpdateRangeAsync<T>(List<T> entities) where T : class;

        /// <summary>
        /// 条件查询实体列表
        /// </summary>
        Task<List<T>> GetListAsync<T>(Expression<Func<T, bool>>? where) where T : class;

        /// <summary>
        /// 条件查询结果列表
        /// </summary>
        Task<List<TResult>> GetListAsync<T, TResult>(Expression<Func<T, bool>>? where, Expression<Func<T, TResult>> selectors) where T : class;

        /// <summary>
        /// 根据Id查询实体对象
        /// </summary>
        Task<T?> GetByIdAsync<T, TKey>(TKey id) where T : BaseCommonEntity<TKey>;

        /// <summary>
        /// 根据Id查询结果对象
        /// </summary>
        Task<TResult?> GetByIdAsync<T, TKey, TResult>(TKey id, Expression<Func<T, TResult>> selectors) where T : BaseCommonEntity<TKey>;

        /// <summary>
        /// 获取单个字段值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TField"></typeparam>
        /// <param name="where"></param>
        /// <param name="field"></param>
        Task<TField?> GetScalarAsync<T, TField>(Expression<Func<T, bool>>? where, Expression<Func<T, TField>> field) where T : class;

        #region 直接sql操作

        /// <summary>
        /// 执行sql操作
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        Task<int> ExecuteSqlAsync(string sql, params object[] parameters);

        /// <summary>
        /// 执行sql事务操作
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        Task<int> ExecuteSqlTransactionAsync(string sql, params object[] parameters);

        /// <summary>
        /// 执行查询sql
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        Task<List<TResult>> SqlQueryParams<TResult>(string sql, params object[] parameters);

        /// <summary>
        /// 执行查询sql
        /// 此方法不会存在sql注入问题
        /// </summary>
        Task<List<TResult>> SqlQueryAsync<TResult>(FormattableString sql);

        #endregion
    }
}
