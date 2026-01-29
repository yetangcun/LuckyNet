import axios from 'axios'
import type { AxiosInstance } from 'axios'
import { ElMessage } from 'element-plus'
import router from '@/router'

import type { InternalAxiosRequestConfig, AxiosRequestConfig, AxiosResponse } from 'axios'

interface ReqInterceptors {
  // 请求拦截
  requestInterceptorHandle?: (conf: AxiosRequestConfig) => InternalAxiosRequestConfig
  requestInterceptorCatch?: (err: unknown) => unknown

  // 响应拦截
  responseInterceptorHandle?: (conf: AxiosResponse) => AxiosResponse
  responseInterceptorCatch?: (err: unknown) => unknown
}

export interface AxiosReqConf extends AxiosRequestConfig {
  interceptors?: ReqInterceptors
}

class axiosReq {
  static runCounts = 0 // 超时重新登录,重复响应处理方法
  axiosIns: AxiosInstance

  constructor(conf: AxiosReqConf) {
    this.axiosIns = axios.create(conf)

    if (conf.interceptors == null || !conf.interceptors.requestInterceptorHandle)
      this.axiosIns.interceptors.request.use(
        (req: InternalAxiosRequestConfig) => {
          const token = localStorage.getItem('curraccesstoken')
          if (token) req.headers!.Authorization = `Bearer ${token}`
          else if (req.url != 'api/system/User/LoginHandleAsync') router.replace('/')
          return req
        },
        (err) => err,
      )
    else
      this.axiosIns.interceptors.request.use(
        conf.interceptors.requestInterceptorHandle,
        conf.interceptors.requestInterceptorCatch,
      )

    if (conf.interceptors == null || !conf.interceptors.responseInterceptorHandle)
      this.axiosIns.interceptors.response.use(
        (res: AxiosResponse) => {
          const data = res.data
          if (data.Code == 401) {
            router.replace('/')
            axiosReq.runCounts += 1
            if (axiosReq.runCounts < 2) {
              ElMessage({
                type: 'error',
                message: '登录超时,请重新登录!',
              })
            }
            return
          }

          const newToken = res.headers['fresh_token']
          if (newToken) localStorage.setItem('curraccesstoken', newToken)
          return res.data
        },
        (err) => {
          if (err.code == 'ERR_NETWORK') {
            ElMessage({
              type: 'error',
              message: '网络错误, 请检查网络连接和后台服务是否运行!',
            })
            return
          }
          if (err.code != 'ERR_BAD_RESPONSE' && err.response.status == 401) {
            router.replace('/')
            axiosReq.runCounts += 1
            if (axiosReq.runCounts < 2) {
              ElMessage({
                type: 'error',
                message: '登录超时,请重新登录',
              })
            }
            return
          }

          return err
        },
      )
    else
      this.axiosIns.interceptors.response.use(
        conf.interceptors.responseInterceptorHandle,
        conf.interceptors.responseInterceptorCatch,
      )
  }
}

export default axiosReq
