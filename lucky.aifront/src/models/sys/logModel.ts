import type { pageInfo } from '@/models/common/pageInfo'

export interface logQueryModel extends pageInfo {
  pageIndex: number
  pageSize: number
  reqType: string
  status: number
  reqUrl: string
  reqIp: string
  beginTime: string
  endTime: string
}

export interface logInfoModel {
  id: number
  reqType: string
  status: number
  reqUrl: string
  reqIp: string
  reqParam: string
  reqUser: string
  reqTime: string,
  msg: string
}
