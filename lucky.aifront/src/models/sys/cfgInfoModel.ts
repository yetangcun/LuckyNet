export interface cfgInfoModel {
  id: number;
  name: string;
  code: string;
  value: string;
  cfgType: string;
  typeName: string;
  status: number;
  sort: number;
}

export interface cfgOptModel {
  id: number;
  name: string;
  value: string;
  code: string;
  cfgType: string;
  typeName: string;
  status: number;
  sort: number;
}

export interface cfgInfoQueryModel {
  name: string;
  cfgType: string;
}
