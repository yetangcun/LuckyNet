using Lucky.BaseModel.Enum;
using SqlSugar;

namespace Lucky.PrtclModel.Entity
{
    /// <summary>
    /// grpc协议
    /// </summary>
    [SugarTable("prtcl_grpc")]
    public class PrtclGrpc
    {
        /// <summary>
        /// id
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, ColumnName = "id", IsIdentity = false)]
        public int id { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [SugarColumn(Length = 20, ColumnName = "name", ExtendedAttribute = PropertyDetail.NOTNULL)]
        public string? name { get; set; }
    }
}
