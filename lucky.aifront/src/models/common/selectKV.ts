export interface selKV {
  label:string,
  value:string
}

export interface treeSelKV {
  label:string,
  value:string,
  children:treeSelKV[]
}

