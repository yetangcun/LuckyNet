export interface cfgInfoModel {
  id: string;
  name: string;
  code: string;
  value: string;
  cfgType: string;
  status: number;
  sort: number;
}


export interface cfgInfoQueryModel {
  name: string;
  cfgType: string;
}
