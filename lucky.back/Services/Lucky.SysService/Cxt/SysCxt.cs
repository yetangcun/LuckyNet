using Data.EFCore.Cxt;
using Lucky.SysModel.Entity;
using Microsoft.EntityFrameworkCore;

namespace Lucky.SysService.Cxt
{
    /// <summary>
    /// 系统管理模块数据库上下文
    /// </summary>
    public class SysCxt : CommonCxt, ISysCxt
    {
        /// <summary>
        /// 系统用户
        /// </summary>
        public DbSet<SysUser> SysUsers { get; set; }

        ///// <summary>
        ///// 组织机构
        ///// </summary>
        //public DbSet<SysOrg> SysOrgs { get; set; }

        /// <summary>
        /// 角色
        /// </summary>
        public DbSet<SysRole> SysRoles { get; set; }

        ///// <summary>
        ///// 菜单
        ///// </summary>
        //public DbSet<SysMenu> SysMenus { get; set; }

        ///// <summary>
        ///// 日志
        ///// </summary>
        //public DbSet<SysLog> SysLogs { get; set; }

        ///// <summary>
        ///// 配置
        ///// </summary>
        //public DbSet<SysConfig> SysConfigs { get; set; }

        ///// <summary>
        ///// 用户角色
        ///// </summary>
        //public DbSet<SysUserRole> SysUserRoles { get; set; }

        ///// <summary>
        ///// 角色菜单
        ///// </summary>
        //public DbSet<SysRoleMenu> SysRoleMenus { get; set; }

        ///// <summary>
        ///// 用户菜单
        ///// </summary>
        //public DbSet<SysUserMenu> SysUserMenus { get; set; }

        ///// <summary>
        ///// 用户组织
        ///// </summary>
        //public DbSet<SysUserOrg> SysUserOrgs { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<SysUser>(entity =>
            {
                entity.ToTable("sys_user").HasQueryFilter(r => !r.IsDel);
            });

            //modelBuilder.Entity<SysOrg>().ToTable("sys_org").HasQueryFilter(r => !r.IsDel); ;

            //modelBuilder.Entity<SysRole>().ToTable("sys_role").HasQueryFilter(r => !r.IsDel);

            //modelBuilder.Entity<SysMenu>().ToTable("sys_menu").HasQueryFilter(r => !r.IsDel); ;

            //modelBuilder.Entity<SysConfig>().ToTable("sys_config").HasQueryFilter(r => !r.IsDel); ;

            //modelBuilder.Entity<SysLog>().ToTable("sys_log");

            //modelBuilder.Entity<SysUserRole>().ToTable("sys_user_role");

            //modelBuilder.Entity<SysRoleMenu>().ToTable("sys_role_menu");

            //modelBuilder.Entity<SysUserMenu>().ToTable("sys_user_menu");

            //modelBuilder.Entity<SysUserOrg>().ToTable("sys_user_org");
        }
    }
}
