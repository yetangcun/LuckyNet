import type { pageInfo } from '@/models/common/pageInfo'

export interface usrInfoModel {
  uid: string,
  realname: string,
  nickName: string,
  roleName: string,
  avatar: string,
  layout: number,
  org: string,
  createTime:string,
  createUser:string
}

export interface usrInfoCache {
  uid: string,
  name: string,
  nickName: string,
  roleName: string,
  avatar: string,
  layout: number,
  org: string
}

export interface usrQueryModel extends pageInfo {
  txt: string,
  orgId: string
}


export interface usrOptModel {
  id: string,
  account: string,
  name: string,
  roleIds: number [],
  roleId: string,
  orgId: string,
  avatar: string,
  status: number,
  sex: number,
  phone: string,
  addr: string
}
