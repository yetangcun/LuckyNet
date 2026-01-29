// import MD5 from 'crypto-js/*'
import CryptoJS from 'crypto-js'

// MD5加密
export function getMd5(str: string) {
  const md5Pwd = CryptoJS.MD5(str).toString()
  return md5Pwd
}

// AES加密
export function encryptAes(str: string, key: string) {
  const aesPwd = CryptoJS.AES.encrypt(str, key).toString()
  return aesPwd
}

// AES解密
export function decryptAes(str: string, key: string) {
  const aesSource = CryptoJS.AES.decrypt(str, key).toString(CryptoJS.enc.Utf8)
  return aesSource
}

// SHA256加密
export function sha256(str: string) {
  const sha256Pwd = CryptoJS.SHA256(str).toString()
  return sha256Pwd
}

// 字符串转16进制
export function str2Hex(str: string) {
  const wordArr = CryptoJS.enc.Utf8.parse(str)
  const hex = CryptoJS.enc.Hex.stringify(wordArr)
  return hex
}

// 16进制转字符串
export function hex2Str(hex: string) {
  const wordArr = CryptoJS.enc.Hex.parse(hex)
  const str = CryptoJS.enc.Utf8.stringify(wordArr)
  return str
}

// 字符串转Base64
export function getBase64(str: string) {
  const wordArr = CryptoJS.enc.Utf8.parse(str)
  const base64 = CryptoJS.enc.Base64.stringify(wordArr)
  return base64
}

// Base64转字符串
export function base642Str(base64: string) {
  const wordArr = CryptoJS.enc.Base64.parse(base64)
  const str = CryptoJS.enc.Utf8.stringify(wordArr)
  return str
}
