using System;
using System.Text;
using System.Collections.Generic;

namespace Lucky.SysModel.Model.Input
{
    /// <summary>
    /// 查询入参
    /// </summary>
    public class SysOrgQueryInput
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        ///  编码
        /// </summary>
        public string? Code { get; set; }

        /// <summary>
        /// 父级ID
        /// </summary>
        public string? ParentId { get; set; }
    }

    /// <summary>
    /// 操作入参
    /// </summary>
    public class SysOrgOptInput
    {
    }
}
