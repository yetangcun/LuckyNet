using Lucky.BaseModel.Model;
using System.Linq.Expressions;
using Lucky.BaseModel.Model.Entity;

namespace Data.EFCore.Rpsty
{
    /// <summary>
    /// 通用仓储接口
    /// </summary>
    public interface ICommonRpsty<T> where T : class
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
        Task<(int, List<T>)> GetPageListAsync(Expression<Func<T, bool>> where, PageInfo page);

        /// <summary>
        /// 获取分页列表
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <typeparam name="TResult">结果类型</typeparam>
        /// <param name="where">查询条件</param>
        /// <param name="selector">选择器</param>
        /// <param name="page">分页参数</param>
        Task<(int, List<TResult>)> GetPagesAsync<TResult>(Expression<Func<T, bool>>? where, Expression<Func<T, TResult>> selector, PageInfo page);

        /// <summary>
        /// 获取最大值
        /// </summary>
        Task<TField?> MaxAsync<TField>(Expression<Func<T, bool>>? where, Expression<Func<T, TField>> field);

        /// <summary>
        /// 添加
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        Task<T> AddAsync(T entity);

        /// <summary>
        /// 批量添加
        /// </summary>
        Task<int> AddRangeAsync(List<T> entities);

        /// <summary>
        /// 删除
        /// </summary>
        Task<int> DeleteAsync(T entity);

        /// <summary>
        /// 批量删除
        /// </summary>
        Task<int> DeleteRangeAsync(List<T> entities);

        /// <summary>
        /// 修改
        /// </summary>
        Task<int> UpdateAsync(T entity);

        /// <summary>
        /// 批量修改
        /// </summary>
        Task<int> UpdateRangeAsync(List<T> entities);

        /// <summary>
        /// 条件查询实体列表
        /// </summary>
        Task<List<T>> GetListAsync(Expression<Func<T, bool>>? where);

        /// <summary>
        /// 条件查询结果列表
        /// </summary>
        Task<List<TResult>> GetListAsync<TResult>(Expression<Func<T, bool>>? where, Expression<Func<T, TResult>> selectors);

        /// <summary>
        /// 根据Id查询实体对象
        /// </summary>
        Task<TEntity?> GetByIdAsync<TEntity, TKey>(TKey id) where TEntity : BaseCommonEntity<TKey>;

        /// <summary>
        /// 根据Id查询结果对象
        /// </summary>
        Task<TResult?> GetByIdAsync<TEntity, TKey, TResult>(TKey id, Expression<Func<TEntity, TResult>> selectors) where TEntity : BaseCommonEntity<TKey>;

        /// <summary>
        /// 获取单个字段值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TField"></typeparam>
        /// <param name="where"></param>
        /// <param name="field"></param>
        Task<TField?> GetScalarAsync<TField>(Expression<Func<T, bool>>? where, Expression<Func<T, TField>> field);

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
