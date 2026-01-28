// 完全的模块声明
declare module 'crypto-js/md5' {
  const MD5: (message: string) => {
    toString(): string
  }
  export default MD5
}
