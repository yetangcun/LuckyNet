export interface cfgInfoModel {
  id: string;
  name: string;
  code: string;
  value: string;
  cfgType: string;
  typeName: string;
  status: number;
  sort: number;
}

export interface cfgOptModel {
  id: string;
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
