using LinqKit;
using Lucky.BaseModel.Model;
using Lucky.SysModel.Entity;
using System.Linq.Expressions;
using Lucky.SysModel.Model.Input;
using Lucky.SysModel.Model.Output;
using Lucky.SysService.Rpsty.IRpsty;
using Lucky.SysService.Service.IService;

namespace Lucky.SysService.Service
{
    public class SysOrgService : ISysOrgService
    {
        private readonly ISysRpsty<SysOrg, int> _roleRpsty;

        public SysOrgService(
            ISysRpsty<SysOrg, int> roleRpsty
            )
        {
            _roleRpsty = roleRpsty;
        }

        public async Task<bool> Del(int id, long uid)
        {
            throw new NotImplementedException();
        }

        public async Task<List<SysOrgOutput>> GetList(SysOrgQueryInput input)
        {
            var where = PredicateBuilder.New<SysOrg>(x => !x.IsDel); // 初始化为 true
            if (!string.IsNullOrEmpty(input.Name))
            {
                where = where.And(x => x.Name.Contains(input.Name));
            }
            if (!string.IsNullOrEmpty(input.Code))
            {
                where = where.And(x => x.Code!.Contains(input.Code));
            }
            if (input.ParentId.HasValue)
            {
                where = where.And(x => x.ParentId == input.ParentId);
            }

            Expression<Func<SysOrg, SysOrgOutput>> expr = x => new SysOrgOutput()  // 1、这是最直接、最可控、最高效的方式
            {
                Id = x.Id,
                Name = x.Name,
                Code = x.Code,
                Remark = x.Remark,
                OrgType = x.OrgType,
                Leader = x.LeaderId.ToString(),
                Phone = x.Phone
            };

            return await _roleRpsty.GetListAsync(where, expr);
        }

        public async Task<List<SysOrgOutputTree>> GetOrgTree(SysOrgQueryInput input)
        {
            var where = PredicateBuilder.New<SysOrg>(x => !x.IsDel); // 初始化为 true
            if (!string.IsNullOrEmpty(input.Name))
            {
                where = where.And(x => x.Name.Contains(input.Name));
            }
            if (!string.IsNullOrEmpty(input.Code))
            {
                where = where.And(x => x.Code!.Contains(input.Code));
            }
            if (input.ParentId.HasValue)
            {
                where = where.And(x => x.ParentId == input.ParentId);
            }

            Expression<Func<SysOrg, SysOrgOutput>> expr = x => new SysOrgOutput()  // 1、这是最直接、最可控、最高效的方式
            {
                Id = x.Id,
                Name = x.Name,
                Code = x.Code,
                Remark = x.Remark,
                OrgType = x.OrgType,
                Leader = x.LeaderId.ToString(),
                Phone = x.Phone
            };
            
            var data = await _roleRpsty.GetListAsync(where, expr);

            var lst = new List<SysOrgOutputTree>();

            var rootNodes = data.Where(x => x.Pid == null || x.Pid == 0 || x.Pid == -1).ToList();

            // 递归构建树形结构
            foreach (var rootNode in rootNodes)
            {
                var rootTreeNode = new SysOrgOutputTree
                {
                    Id = rootNode.Id,
                    Name = rootNode.Name,
                    Code = rootNode.Code,
                    Remark = rootNode.Remark,
                    OrgType = rootNode.OrgType,
                    Leader = rootNode.Leader,
                    Phone = rootNode.Phone,
                    Childs = new List<SysOrgOutputTree>()
                };
                if (rootTreeNode.Childs != null && rootTreeNode.Childs.Count > 0)
                    BuildTree(rootTreeNode, data);
                lst.Add(rootTreeNode);
            }

            return lst;
        }

        public async Task<List<TreeSelectKV>> GetOrgTreeSel(SysOrgQueryInput req)
        {
            var lst = new List<TreeSelectKV>();
            return lst;
        }

        public async Task<bool> Opt(SysOrgOptInput input, long uid)
        {
            throw new NotImplementedException();
        }

        private void BuildTree(SysOrgOutputTree node, List<SysOrgOutput> data)
        {
            var children = data.Where(x => x.Pid == node.Id).ToList();
            foreach (var child in children)
            {
                var childNode = new SysOrgOutputTree
                {
                    Id = child.Id,
                    Name = child.Name,
                    Code = child.Code,
                    Remark = child.Remark,
                    OrgType = child.OrgType,
                    Leader = child.Leader,
                    Phone = child.Phone,
                    Childs = new List<SysOrgOutputTree>()
                };
                if (childNode.Childs != null && childNode.Childs.Count > 0)
                    BuildTree(childNode, data);
                node.Childs!.Add(childNode);
            }
        }

        public async Task<SysOrgOutput> Get(int id)
        {
            throw new NotImplementedException();
        }
    }
}
