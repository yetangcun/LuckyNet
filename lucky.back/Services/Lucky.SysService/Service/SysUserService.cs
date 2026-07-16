using Common.CoreLib.Extension.Common;
using LinqKit;
using Lucky.BaseModel;
using Lucky.BaseModel.Enum;
using Lucky.BaseModel.Model;
using Lucky.SysModel.Entity;
//using System.Linq.Expressions;
using Lucky.SysModel.Model.Input;
using Lucky.SysModel.Model.Output;
using Lucky.SysService.Rpsty.IRpsty;
using Lucky.SysService.Service.IService;

namespace Lucky.SysService.Service
{
    /// <summary>
    /// 用户服务
    /// </summary>
    public class SysUserService : ISysUserService
    {
        private readonly ISysRpsty<SysUser, long> _usrRpsty;
        private readonly ISysRpsty<SysRole, int> _roleRpsty;
        private readonly ISysRpsty<SysMenu, int> _menuRpsty;
        private readonly ISysRpsty<SysUserRole, long> _usrRoleRpsty;
        private readonly ISysRpsty<SysRoleMenu, int?> _roleMenuRpsty;

        /// <summary>
        /// 构造函数
        /// </summary>
        public SysUserService(
            ISysRpsty<SysUser, long> usrRpsty,
            ISysRpsty<SysRole, int> roleRpsty,
            ISysRpsty<SysMenu, int> menuRpsty,
            ISysRpsty<SysRoleMenu, int?> roleMenuRpsty,
            ISysRpsty<SysUserRole, long> usrRoleRpsty)
        {
            _usrRpsty = usrRpsty;
            _roleRpsty = roleRpsty;
            _menuRpsty = menuRpsty;
            _usrRoleRpsty = usrRoleRpsty;
            _roleMenuRpsty = roleMenuRpsty;
        }

        public async Task<(int, List<SysUserOutput>)> GetList(SysUserQueryInput req)
        {
            var where = PredicateBuilder.New<SysUser>(x => !x.IsDel); // 初始化为 true
            if (!string.IsNullOrWhiteSpace(req.Txt))
                where = where.And(x => x.Account.Contains(req.Txt) || (!string.IsNullOrWhiteSpace(x.RealName) && x.RealName.Contains(req.Txt)));

            if (req.Status.HasValue)
                where = where.And(x => x.Status == req.Status);

            /**************测试方法*****************/
            var maxId = await _usrRpsty.MaxAsync(where, x => x.Id);   // 获取用户最大Id
            var maxRoleId = await _roleRpsty.MaxAsync(null, x => x.Id);  // 查询角色最大Id

            var likeStr = $"%{req.Txt}%";
            var roleObj = await _roleRpsty.SqlQueryAsync<SysRoleOutput>($"select id,name,remark from sys_role where name like {likeStr};");
            var userObj = await _usrRpsty.SqlQueryAsync<SysUserOutput>($"select id,account,avatar,realname,password from sys_user where account like {likeStr} or realname like {likeStr};");

            //var outModel = await _usrRpsty.GetByIdAsync<SysUser, long, SysUserOutput>(2, x => new SysUserOutput()
            //{
            //    id = x.Id,
            //    account = x.Account,
            //    realname = x.RealName
            //});
            //var lst = await _usrRpsty.GetListAsync(where);
            //var lists = await _usrRpsty.GetListAsync(where, x => new SysUserOutput()
            //{
            //    id = x.Id,
            //    account = x.Account,
            //    realname = x.RealName
            //});
            /**************测试方法*****************/

            var pgInfo = new PageInfo()
            {
                PageIndex = req.PageIndex,
                PageSize = req.PageSize,
                Sort = req.Sort,
                SortType = req.SortType
            };

            //Expression<Func<SysUser, SysUserOutput>> expr = x => new SysUserOutput()  // 1、这是最直接、最可控、最高效的方式
            //{
            //    Uid = x.Id,
            //    Account = x.Account,
            //    Name = x.RealName
            //};

            var expr = SimpleMappingExtensions.AutoMap<SysUser, SysUserOutput>();  // 2、这是最简便的方式
            var res = await _usrRpsty.GetPagesAsync(where, expr, pgInfo);

            return (res.Item1, res.Item2);
        }

        public async Task<(int, List<SysUserInfoOutput>)> GetPages(SysUserQueryInput req)
        {
            var where = PredicateBuilder.New<SysUser>(x => !x.IsDel); // 初始化为 true
            if (!string.IsNullOrWhiteSpace(req.Txt))
                where = where.And(x => x.Account.Contains(req.Txt) || (!string.IsNullOrWhiteSpace(x.RealName) && x.RealName.Contains(req.Txt)));

            if (req.Status.HasValue)
                where = where.And(x => x.Status == req.Status);

            /**************测试方法*****************/
            //var maxId = await _usrRpsty.MaxAsync(where, x => x.Id);   // 获取用户最大Id
            //var maxRoleId = await _roleRpsty.MaxAsync(null, x => x.Id);  // 查询角色最大Id

            //var likeStr = $"%{req.Txt}%";
            //var roleObj = await _roleRpsty.SqlQueryAsync<SysRoleOutput>($"select id,name,remark from sys_role where name like {likeStr};");
            //var userObj = await _usrRpsty.SqlQueryAsync<SysUserOutput>($"select id,account,avatar,realname,password from sys_user where account like {likeStr} or realname like {likeStr};");

            //var outModel = await _usrRpsty.GetByIdAsync<SysUser, long, SysUserOutput>(2, x => new SysUserOutput()
            //{
            //    id = x.Id,
            //    account = x.Account,
            //    realname = x.RealName
            //});
            //var lst = await _usrRpsty.GetListAsync(where);
            //var lists = await _usrRpsty.GetListAsync(where, x => new SysUserOutput()
            //{
            //    id = x.Id,
            //    account = x.Account,
            //    realname = x.RealName
            //});
            /**************测试方法*****************/

            var pgInfo = new PageInfo()
            {
                PageIndex = req.PageIndex,
                PageSize = req.PageSize,
                Sort = req.Sort,
                SortType = req.SortType
            };

            //Expression<Func<SysUser, SysUserOutput>> expr = x => new SysUserOutput()  // 1、这是最直接、最可控、最高效的方式
            //{
            //    Uid = x.Id,
            //    Account = x.Account,
            //    Name = x.RealName
            //};

            var expr = SimpleMappingExtensions.AutoMap<SysUser, SysUserInfoOutput>();  // 2、这是最简便的方式
            var res = await _usrRpsty.GetPagesAsync(where, expr, pgInfo);

            return (res.Item1, res.Item2);
        }

        public async Task<SysUserInfoOutput?> GetUserInfo(long id)
        {
            //Expression<Func<SysUser, SysUserOutput>> expr = x => new SysUserOutput()  // 1、这是最直接、最可控、最高效的方式
            //{
            //    Uid = x.Id,
            //    Account = x.Account,
            //    Name = x.RealName
            //};
            var expr = SimpleMappingExtensions.AutoMap<SysUser, SysUserInfoOutput>();  // 2、这是最简便的方式
            var userObj = await _usrRpsty.GetByIdAsync(id, expr);
            return userObj;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="input"></param>
        public async Task<bool> Add(SysUserOptInput input, long optId)
        {
            var id = IdGreator.GetNxtId();

            var sysObj = new SysUser()
            {
                Account = input.Account,
                RealName = input.Name,
                Sex = input.Sex.HasValue ? input.Sex.Value : SexType.None,
                PassWord = GlobalConstant.DFT_PWD,
                Phone = input.Phone,
                Status = input.Status ?? 1,
                Id = id,
                CreateTime = DateTime.Now,
                CreateUid = optId
            };

            var res = await _usrRpsty.AddAsync(sysObj);

            return res != null;
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="SysUserOptInput"></param>
        public async Task<bool> Edit(SysUserOptInput input, long optId)
        {
            if (!input.Id.HasValue) return false;
            var entity = await _usrRpsty.GetByIdAsync(input.Id.Value);
            if (entity == null) return false;

            entity.RealName = input.Name;

            if(input.Status!=null && input.Status.HasValue)
                entity.Status = input.Status.Value;

            if (input.Sex != null && input.Sex.HasValue)
                entity.Sex = input.Sex.Value;

            if (!string.IsNullOrWhiteSpace(input.Phone))
                entity.Phone = input.Phone;

            entity.UpdateUid = optId;
            entity.UpdateTime = DateTime.Now;

            var res = await _usrRpsty.UpdateAsync(entity);

            return res > 0;
        }

        /// <summary>
        /// 删除
        /// 软删除
        /// </summary>
        /// <param name="id"></param>
        /// <param name="optId"></param>
        public async Task<bool> Del(long id, long optId)
        {
            var entity = await _usrRpsty.GetByIdAsync(id);
            if (entity == null) return false;

            entity.IsDel = true;
            entity.DelTime = DateTime.Now;
            entity.DelUid = optId;

            var res = await _usrRpsty.UpdateAsync(entity);

            return res > 0;
        }

        /// <summary>
        /// 登录系统
        /// </summary>
        /// <param name="account"></param>
        public async Task<SysUserOutput?> Dologin(string account)
        {
            return await _usrRpsty.SqlSingleQueryAsync<SysUserOutput>($"select id,account,password,realname,avatar from sys_user where account={account} and is_del=false");
        }

        /// <summary>
        /// 获取用户权限
        /// </summary>
        /// <param name="uid"></param>
        public async Task<UsrData> GetPermissions(long uid)
        {
            var usrData = new UsrData();
            var usrInfo = await _usrRpsty.GetByIdAsync(uid);

            usrData.layout = 1;
            usrData.uid = uid.ToString();
            usrData.name = usrInfo!.RealName;
            usrData.avatar = usrInfo.Avatar;

            var usrRoles = await _usrRoleRpsty.GetListAsync(x => x.UserId == uid);
            if (!usrRoles.Any())
                return usrData;

            var roleIds = usrRoles.Select(x => x.RoleId);
            var superRole = await _roleRpsty.GetListAsync(x => roleIds.Contains(x.Id) && x.RoleType == RoleType.Super); // 是否超级管理员
            if (superRole.Any())
            {
                var menus = await _menuRpsty.GetListAsync(x => !x.IsDel);
                usrData.permissions = GetMenuTree(menus);
            }
            else
            {
                var roleMenus = await _roleMenuRpsty.GetListAsync(x => roleIds.Contains(x.RoleId));
                if (roleMenus.Any())
                {
                    var menuIds = roleMenus.Select(x => x.MenuId);
                    var menus = await _menuRpsty.GetListAsync(x => menuIds.Contains(x.Id));
                    usrData.permissions = GetMenuTree(menus);
                }

            }

            return usrData;
        }

        private List<SysUserPermissionOutput> GetMenuTree(List<SysMenu> menus)
        {
            var topMenus = menus.Where(x => x.ParentId == null || x.ParentId == 0 || x.ParentId == -1).OrderBy(o => o.Sort);
            var lst = new List<SysUserPermissionOutput>();
            foreach (var item in topMenus)
            {
                var parentEle = new SysUserPermissionOutput()
                {
                    id = item.Id,
                    name = item.Name,
                    code = item.Code ?? string.Empty,
                    url = item.Path,
                    icon = item.Icon,
                    iconSize = item.IconSize,
                    menuType = item.MenuType,
                    parent_id = item.ParentId?.ToString() ?? string.Empty
                };

                GetMenuTrees(parentEle, menus);

                lst.Add(parentEle);
            }
            lst[0].isExpand = true;
            return lst;
        }

        private void GetMenuTrees(SysUserPermissionOutput pEle, IEnumerable<SysMenu> menus)
        {
            if (menus == null || menus.Count() < 1) return;

            if (pEle.children == null) pEle.children = new List<SysUserPermissionOutput>();

            var childs = menus.Where(m=>m.ParentId == pEle.id).OrderBy(o => o.Sort);

            foreach (var itm in childs)
            {
                var tmp = new SysUserPermissionOutput()
                {
                    id = itm.Id,
                    name = itm.Name,
                    code = itm.Code ?? string.Empty,
                    url = itm.Path,
                    icon = itm.Icon,
                    iconSize = itm.IconSize,
                    menuType = itm.MenuType,
                    parent_id = itm.ParentId?.ToString() ?? string.Empty
                };
                if (itm.MenuType != BaseModel.Enum.MenuType.Menu)
                {
                    var chlds = menus.Where(m => m.ParentId == itm.Id);
                    GetMenuTrees(tmp, menus);
                }
                pEle.children.Add(tmp);
            }
        }
    }
}
