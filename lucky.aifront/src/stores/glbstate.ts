import { ref } from 'vue'
import { defineStore } from 'pinia'
import type { usrInfoModel } from '@/models/sys/usrInfoModel'

export const useGlbStateStore = defineStore('glbstate', () => {
  const usrInfo = ref<usrInfoModel>({
    uid:'',
    name:'',
    nickName:'',
    roleName:'',
    avatar:'',
    layout:1
  })

  function setUsrInfo(usr:usrInfoModel) {
    usrInfo.value.uid = usr.uid
    usrInfo.value.name = usr.name
    usrInfo.value.roleName = usr.roleName
    usrInfo.value.nickName = usr.nickName
    usrInfo.value.avatar = usr.avatar
    usrInfo.value.layout = usr.layout
  }

  return { usrInfo, setUsrInfo }
})
