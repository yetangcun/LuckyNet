import type { pageInfo } from '@/models/common/pageInfo'

export interface tskModel {
  id: number;
  name: string;
  code: string;
  status: number;
  createTime: string;
  updateTime: string;
  creatUser: string;
  updatUser: string;
  remark: string;
  paramModel: string;
  cron: string;
}

export interface tskQueryModel extends pageInfo {
  txt: string;
  status: number;
}

export interface tskOptModel {
  id: number;
  name: string;
  code: string;
  status: number;
  remark: string;
  paramModel: string;
  cron: string;
}

export interface tskRecordModel {
  id: string;
  tskId: string;
  status: number;
  tskParam: string;
  tskMsg: string;
  startTime: string;
  endTime: string;
}

export interface tskRecordQueryModel {
  tskId: string;
  status: number;
  startTime: string;
  endTime: string;
}
