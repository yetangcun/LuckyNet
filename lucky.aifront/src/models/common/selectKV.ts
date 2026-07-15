export interface selKV {
  label:string,
  value:string
}

export interface treeSelKV {
  label:string,
  value:string,
  children:treeSelKV[]
}

export interface selNumKV {
  label:string,
  value:number
}

