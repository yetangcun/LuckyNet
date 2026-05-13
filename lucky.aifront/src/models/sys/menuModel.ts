export interface menuModel {
  id: string,
  name: string,  // 英文
  txt: string,    // 中文
  parentId:string,
  menuType: number, // 1模块 2分组 3菜单
  code: string,  // 编码
  url: string,   // 路由地址
  icon: string,  // 图标
  iconSize: string,  // 图标大小尺寸
  childs: menuModel[]
}
