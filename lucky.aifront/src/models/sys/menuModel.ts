export interface menuModel {
  id: string,
  name: string,  // 名称
  parent_id:string,
  menu_type: number, // 1模块 2分组 3菜单
  code: string,   // 编码
  path: string,   // 路由地址
  icon: string,   // 图标
  icon_size: string,  // 图标大小尺寸
  childs: menuModel[],
  isExpand: boolean,   // 是否展开
  isSelect: boolean    // 是否选中
}
