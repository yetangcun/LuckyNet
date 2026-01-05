using Common.CoreLib.Model.Option;
using Lucky.BaseModel.Enum;
using Microsoft.EntityFrameworkCore;

namespace Data.EFCore.Cxt
{
    /// <summary>
    /// 通用数据库上下文实现
    /// </summary>
    public class CommonCxt : DbContext, ICommonCxt
    {
        private DbOption? _opt;

        /// <summary>
        ///  获取数据库上下文
        /// </summary>
        /// <typeparam name="TCxt"></typeparam>
        /// <typeparam name="TOpt"></typeparam>
        public TCxt GetDbCxt<TCxt, TOpt>(TOpt opt) where TCxt : CommonCxt where TOpt : DbOption
        {
            _opt = opt;
            return (TCxt)this;
        }

        /// <summary>
        ///  配置约定
        /// </summary>
        /// <param name="configurationBuilder"></param>
        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            base.ConfigureConventions(configurationBuilder);
        }

        /// <summary>
        ///  配置数据库
        /// </summary>
        /// <param name="optBlder"></param>
        protected override void OnConfiguring(DbContextOptionsBuilder optBlder)
        {
            // base.OnConfiguring(optionsBuilder);
            var connString = _opt.IsReadOnly.Value ? _opt.SlaveConnString : _opt.MasterConnString;
            switch (_opt.DbType)
            {
                case DatabaseType.Mysql:
                    //optBlder.UseMySql(connString, ServerVersion.AutoDetect(connString), blder =>  // 使用Pomelo. EntityFrameworkCore.MySql 时需添加此配置
                    //{
                    //    blder.CommandTimeout(_opt.TimeOuts);
                    //});
                    //optBlder.UseMySQL(connString, blder =>
                    //{
                    //    blder.CommandTimeout(_opt.TimeOuts);
                    //    blder.MaxBatchSize(6000);
                    //}).UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
                    break;
                case DatabaseType.Sqlserver:
                    optBlder.UseSqlServer(connString, blder =>
                    {
                        blder.CommandTimeout(_opt.TimeOuts);
                        blder.MaxBatchSize(6000);
                    });
                    break;
                case DatabaseType.Postgresql:
                    optBlder.UseNpgsql(connString, blder =>
                    {
                        blder.CommandTimeout(_opt.TimeOuts);
                        blder.MaxBatchSize(6000);
                    });
                    break;
                case DatabaseType.Sqlite:
                    optBlder.UseSqlServer(connString, blder =>
                    {
                        blder.CommandTimeout(_opt.TimeOuts);
                        blder.MaxBatchSize(6000);
                    });
                    break;
            }
        }

        /// <summary>
        ///  配置模型
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
