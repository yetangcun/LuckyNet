export interface logQueryModel {
  pageNum: number
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
