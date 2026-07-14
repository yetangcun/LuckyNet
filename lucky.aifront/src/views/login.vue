<template>
  <div id="page_top">
    <div id="page_header" :style="{ borderBottomColor: selColor , borderBottomWidth: '2px', borderBottomStyle: 'solid' }">
      <div
        :style="{
          display: 'flex',
          opacity: 0.8,
          margin: '0px 0px 0px 11px',
          color: selColor,
          alignItems: 'center',
          flexDirection: 'row',
          fontStyle: 'italic'}
        "
      >
        <el-icon :size="61" :color="selColor">
          <Cpu class="common_large_icon" />
        </el-icon>
        <span :style="{ color: selColor ,display: 'flex', fontSize: '29px', textShadow: `0 0 32px ${selColor}` }"> AI NEXT </span>
      </div>
      <div style="display: flex">
        <span
          style="
            background-color: blue;
            width: 46px;
            height: 27px;
            cursor: pointer;
            border-radius: 4px;
            border: 2px solid transparent;
          "
          id="span1"
          @click="chgBackColor(1)"
        />
        <span
          style="
            background-color: purple;
            width: 46px;
            height: 27px;
            cursor: pointer;
            margin: 0px 11px;
            border-radius: 4px;
            border: 1px solid transparent;
          "
          id="span2"
          @click="chgBackColor(2)"
        />
        <span
          style="
            background-color: #285616;
            width: 46px;
            height: 27px;
            cursor: pointer;
            border-radius: 4px;
            border: 1px solid transparent;
          "
          id="span3"
          @click="chgBackColor(3)"
        />
      </div>
    </div>
    <div id="page_center">
      <div id="page_form">
        <div class="title_line">
          <el-icon :size="88" :color="'white'"><Cpu /> </el-icon>
        </div>
        <div class="common_line">
          <Avatar class="common_icon" :style="{color:selColor}" />
          <!-- <user-outlined class="common_icon" /> -->
          <input
            type="text"
            class="common_input"
            placeholder="请输入用户名"
            v-model="reqlogin.account"
            @keydown.enter="loginHandle"
          />
        </div>
        <div class="common_line">
          <Unlock class="common_icon" :style="{color:selColor}" />
          <!-- <unlock-outlined class="common_icon" /> -->
          <input
            type="password"
            class="common_input"
            placeholder="请输入密码"
            v-model="reqlogin.passwd"
            @keydown.enter="loginHandle"
          />
        </div>
        <div class="btn_line">
          <div style="display: flex; width: 100%; background-color: white; border-radius: 4px">
            <el-button class="common_btn" @click="loginHandle" :disabled="loading" :style="{color:selColor}">
              <div style="display: flex">
                <span
                  style="font-weight: 600; font-size: 23px; font-family: '楷体'"
                  v-loading="loading"
                  >登 录</span
                >
              </div>
            </el-button>
          </div>
          <div
            style="
              display: flex;
              justify-content: center;
              height: 18px;
              align-items: center;
              margin-top: 27px;
              color: white;
              padding: 1px 0px 6px 0px;
              font-size: 13px;
              font-family: '正楷';
            "
          >
            <span>{{ tipMsg }}</span>
          </div>
        </div>
      </div>
    </div>
    <div id="page_footer" :style="{boxShadow:`0px 0px 12px 0px ${selColor}`}">
      <label :style="{color:selColor}">AI Next Copyright@{{ years }} owned by Wuxiaojun</label>
      <!-- <label>AI Next Copyright@{{ years }} wholly owned by Wuxiaojun</label> -->
    </div>
  </div>
</template>

<script lang="ts" setup>
import { reactive, ref } from 'vue'
import { systemReq } from '../utils/reqUtil'
import type { reqLogin } from '../models/sys/reqLogin'
import { useRouter } from 'vue-router'
import { Md5 } from 'ts-md5'
import axiosReq from '@/utils/axiosUtil'
import { ElMessage } from 'element-plus'
import { Avatar, Unlock, Cpu } from '@element-plus/icons-vue'
// import { useGlbStateStore } from '@/stores/glbstate'

const router = useRouter()
const loading = ref(false)
const years = ref(new Date().getFullYear().toString())
const tipMsg = ref('请登录')
const reqlogin = reactive<reqLogin>({
  account: '',
  passwd: '',
  validateCode: '',
})
const selColor = ref('blue')

// const glbStore = useGlbStateStore()

const chgBackColor = (type: number) => {
  const formEle: HTMLElement | null = document.getElementById('page_form')
  const span1Ele: HTMLElement | null = document.getElementById('span1')
  const span2Ele: HTMLElement | null = document.getElementById('span2')
  const span3Ele: HTMLElement | null = document.getElementById('span3')

  if (!formEle) return
  if (!span1Ele || !span2Ele || !span3Ele) return


  if (type == 1) {
    span2Ele.style.border = '1px solid transparent'
    span3Ele.style.border = '1px solid transparent'
    span1Ele.style.border = '2px solid blue'
    formEle.style.backgroundColor = 'blue'
    selColor.value = 'blue'
    return
  }
  if (type == 2) {
    span1Ele.style.border = '1px solid transparent'
    span3Ele.style.border = '1px solid transparent'
    span2Ele.style.border = '2px solid purple'
    formEle.style.backgroundColor = 'purple'
    selColor.value = 'purple'
    return
  }
  if (type == 3) {
    span1Ele.style.border = '1px solid transparent'
    span2Ele.style.border = '1px solid transparent'
    span3Ele.style.border = '2px solid #285616'
    formEle.style.backgroundColor = '#285616'
    selColor.value = '#285616'
    return
  }
}

const loginHandle = () => {
  tipMsg.value = '请登录'
  if (!reqlogin.account) {
    tipMsg.value = '请输入用户名'
    return
  }
  if (!reqlogin.passwd) {
    tipMsg.value = '请输入密码'
    return
  }

  loading.value = true
  tipMsg.value = '正在登录...'
  const pwdMd5 = Md5.hashStr(reqlogin.passwd)
  const accountBase64 = btoa(reqlogin.account)
  systemReq.axiosIns
    .post('api/sys/SysUser/loginHdl', { Account: accountBase64, Passwd: pwdMd5 })
    .then((res: any) => {
      loading.value = false
      if (res.Code != 200) {
        tipMsg.value = '登录失败'
        ElMessage({
          type: 'error',
          message: res.Msg,
        })
        return
      }

      localStorage.setItem('tkn', res.Data.tkn)

      // glbStore.setUsrInfo(res.Data)

      axiosReq.runCounts = 0 // 初始化成默认值

      router.replace('/index')
    })
    .catch((err: unknown) => {
      loading.value = false
      tipMsg.value = '登录失败'
      console.log(err)
    })
}
</script>

<style scoped>
#page_top {
  display: flex;
  flex: 1;
  flex-direction: column;
  align-content: space-between;
  background-color: transparent;
}
#page_header {
  display: flex;
  opacity: 0.7;
  max-height: 68px;
  min-height: 68px;
  padding: 0px 26px;
  align-items: center;
  background-color: #f1f1f1;
  justify-content: space-between;
  /* border-bottom: 2px solid rgb(141, 67, 141); */
}
#page_center {
  display: flex;
  flex: 1;
  opacity: 0.6;
  align-items: center;
  justify-content: center;
}
#page_form {
  display: flex;
  padding: 16px 49px;
  border-radius: 8px;
  flex-direction: column;
  background-color: blue;
  box-shadow: 0px 0px 16px white inset;
}
.common_line {
  margin: 21px 0;
  display: flex;
  padding: 6px;
  border: none;
  border-radius: 2px;
  align-items: center;
  background-color: white;
  justify-content: flex-start;
  border-bottom: 1px solid gray;
}
.title_line {
  display: flex;
  padding: 1px;
  justify-content: center;
  margin: 19px 0px 17px 0px;
}

.btn_line {
  display: flex;
  margin: 14px 0 0 0;
  padding: 6px 0px;
  flex-direction: column;
  justify-content: center;
}

.common_input {
  display: flex;
  border: none;
  outline: none;
  height: 36px;
  width: 256px;
  padding: 6px 4px 0px 11px;
  font-size: 18px;
  font-weight: 500;
  text-decoration: none;
}

.common_btn {
  cursor: pointer;
  display: flex;
  border: none;
  height: 49px;
  flex: 1;
  font-size: 20px;
  background-color: white;
  font-family: '微软雅黑';
  justify-content: center;
  align-items: center;
  border-radius: 4px;
}
.common_large_icon {
  margin: 1px 7px 0px 0px;
  font-weight: bold;
  stroke: dimgray;
  stroke-width: 11;
}
.common_icon {
  width: 32px;
  margin: 3px 1px 0px 4px;
  font-weight: bold;
  stroke: dimgray;
  stroke-width: 11;
}
#page_footer {
  display: flex;
  padding: 26px;
  opacity: 0.4;
  color: black;
  font-size: 18px;
  /* font-weight: bold; */
  box-shadow: 0px 0px 11px #f1f1f1 inset;
  justify-content: center;
  background-color: #f3eeee;
}
</style>
