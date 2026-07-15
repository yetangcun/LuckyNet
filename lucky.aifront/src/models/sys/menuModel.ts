export interface menuModel {
  id: string,
  name: string,  // 名称
  parentId:string,
  menuType: number, // 1模块 2分组 3菜单
  code: string,   // 编码
  url: string,   // 路由地址
  icon: string,   // 图标
  status: number,  // 状态 0禁用 1正常 2隐藏
  sort: number,    // 排序
  iconSize: string,  // 图标大小尺寸
  children: menuModel[],
  isExpand: boolean,   // 是否展开
  isSelect: boolean,   // 是否选中
}

export interface menuOptModel {
  id: string,
  name: string,  // 菜单名称
  parentId:string,
  menuType: number, // 1模块 2分组 3菜单
  code: string,   // 菜单编码
  url: string,   // 路由地址
  icon: string,   // 图标
  status: number,  // 状态 0禁用 1正常 2隐藏
  sort: number,   // 排序
  iconSize: string,  // 图标大小尺寸
}

export interface menuQueryModel {
  name: string,   // 菜单名称
}
