using Lucky.BaseModel.Enum;
using Common.CoreLib.Model.Option;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace Data.EFCore.Cxt
{
    /// <summary>
    /// 通用数据库上下文实现
    /// </summary>
    public class CommonCxt : DbContext, ICommonCxt
    {
        private DbDefaultOption? _opt;

        public bool SetDbOption(DbDefaultOption opt)
        {
            _opt = opt;
            return true;
        }

        /// <summary>
        ///  获取数据库上下文
        /// </summary>
        /// <typeparam name="TCxt"></typeparam>
        /// <typeparam name="TOpt"></typeparam>
        public TCxt GetDbCxt<TCxt, TOpt>(TOpt opt) where TCxt : CommonCxt where TOpt : DbDefaultOption
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
            var connString = _opt!.IsReadOnly!.Value ? _opt.SlaveConnString : _opt.MasterConnString;
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
                    //optBlder.UseSqlServer(connString, blder =>
                    //{
                    //    blder.CommandTimeout(_opt.TimeOuts);
                    //    blder.MaxBatchSize(6000);
                    //});
                    optBlder.UseSqlServer(connString);
                    break;
                case DatabaseType.Postgresql:
                    //optBlder.UseNpgsql(connString, blder =>
                    //{
                    //    blder.CommandTimeout(_opt.TimeOuts);
                    //    blder.MaxBatchSize(6000);
                    //});
                    optBlder.UseNpgsql(connString);
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

        /// <summary>
        /// 初始化库、表结构
        /// </summary>
        public bool InitTable()
        {
            try
            {
                var tis = this.GetType().Name;
                var res = this.Database.EnsureCreated(); // 创建数据库、 创建表结构

                return res;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return  false;
            }
        }

        public bool InitData()
        {
            try
            {
                var sqlBld = new StringBuilder();
                sqlBld.AppendLine($"insert into sys_user(id, account, password, realname, email, avatar, phone, status, is_del, create_time, create_uid) values(1,'xxiao','xiaoxiao','潇潇','666@888.999','http://689.com/666.png','13713688888',1,false,'2026-01-09 16:16:16',1);");
                sqlBld.AppendLine($"insert into sys_user(id, account, password, realname, email, avatar, phone, status, is_del, create_time, create_uid) values(2,'xyz','xiaoxiao','字母哥','698@666.698','http://666.com/999.png','13713699999',1,false,'2026-01-09 16:16:16',1);");
                sqlBld.AppendLine($"insert into sys_role(id, name, word, sort, status, remark, is_del, create_time, create_uid) values(1,'超级管理员','SuperAdmin',1,1,'',false,'2026-01-09 16:16:16',1);");
                sqlBld.AppendLine($"insert into sys_role(id, name, word, sort, status, remark, is_del, create_time, create_uid) values(2,'管理员','Admin',2,1,'',false,'2026-01-09 16:18:19',1);");
                sqlBld.AppendLine($"insert into sys_user_role(user_id, role_id) values(1,1);");
                sqlBld.AppendLine($"insert into sys_user_role(user_id, role_id) values(2,2);");
                var sql = sqlBld.ToString(); var optRes = this.Database.ExecuteSqlRaw(sql);
                return optRes > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            finally
            {
                Database.CloseConnection();
            }
        }
    }
}
