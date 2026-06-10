import { ref } from 'vue'
import { defineStore } from 'pinia'
import type { usrInfoCache } from '@/models/sys/usrInfoModel'

export const useGlbStateStore = defineStore('glbstate', () => {
  const usrInfo = ref<usrInfoCache>({
    uid:'',
    name:'',
    nickName:'',
    roleName:'',
    avatar:'',
    layout:1,
    org:''
  })

  function setUsrInfo(usr:usrInfoCache) {
    usrInfo.value.uid = usr.uid
    usrInfo.value.name = usr.name
    usrInfo.value.roleName = usr.roleName
    usrInfo.value.nickName = usr.nickName
    usrInfo.value.avatar = usr.avatar
    usrInfo.value.layout = usr.layout
  }

  return { usrInfo, setUsrInfo }
})
