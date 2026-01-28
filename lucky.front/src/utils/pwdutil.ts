import MD5 from 'crypto-js/md5'
export function getPwd(str: string) {
  const md5Pwd = MD5(str).toString()
  return md5Pwd
}
