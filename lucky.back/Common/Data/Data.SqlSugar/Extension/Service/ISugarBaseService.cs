using Lucky.BaseModel.Model;
using System.Linq.Expressions;

namespace Data.SqlSugar.Extension.Service
{
    public interface ISugarBaseService
    {
        Task<(int, List<TOut>?)> GetPageListAsync<Tin, TOut>(Expression<Func<Tin, bool>>? where, Expression<Func<Tin, TOut>> selectors, PageInfo page);

        Task<(int, List<TOut>)> GetPages<Tin, TOut>(Expression<Func<Tin, bool>>? where, Expression<Func<Tin, TOut>> selectors, PageInfo page);

        void CreateTables(string[] dllNames);
    }
}
