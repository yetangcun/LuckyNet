import type {pageInfo} from "../common/pageInfo";

export interface PermissionModel {
  id: string,
  name: string,
  word: string,
  status: number,
  sort: number,
  roleType: number,
  remark: string,
}

export interface PermissionQueryModel extends pageInfo {
  name: string,
}

export interface PermissionOptModel {
  id: string,
  name: string,  // 名称
  word: string,
  status: number,
  sort: number,
  roleType: number,
  remark: string,
}
