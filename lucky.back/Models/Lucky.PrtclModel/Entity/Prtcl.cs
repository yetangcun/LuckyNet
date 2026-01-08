using SqlSugar;
using System;
using System.Text;
using System.Collections.Generic;

namespace Lucky.PrtclModel.Entity
{
    [SugarTable("prtcl")]
    public class Prtcl
    {
        /// <summary>
        /// 主键
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, ColumnName = "id")]
        public int id { get; set; }


        /// <summary>
        /// 名称
        /// </summary>
        [SugarColumn(Length = 40, ColumnName = "name", ColumnDescription = "名称", IsNullable = false)]
        public string? name { get; set; }
    }
}
