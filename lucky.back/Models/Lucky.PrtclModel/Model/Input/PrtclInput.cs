using System;
using System.Text;
using Lucky.BaseModel.Model;
using System.Collections.Generic;

namespace Lucky.PrtclModel.Model.Input
{
    /// <summary>
    /// 查询输入
    /// </summary>
    public class PrtclQueryInput : PageInfo
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string? Name { get; set; }
    }

    public class PrtclOptInput
    {
    }
}
