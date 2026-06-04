export interface usrInfoModel {
  uid: string,
  name: string,
  nickName: string,
  roleName: string,
  avatar: string,
  layout: number,
  org: string,
  createTime:string,
  createUser:string
}

export interface usrQueryModel {
  txt: string,
  orgId: string
}
