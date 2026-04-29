import type { InternalAxiosRequestConfig, AxiosRequestConfig, AxiosResponse } from 'axios'

export interface ReqInterceptors {
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
