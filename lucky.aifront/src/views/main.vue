<script lang="ts" setup>
// import { RouterView } from 'vue-router';
import { onMounted, reactive, ref } from 'vue';
import { Cpu, Expand, Fold } from '@element-plus/icons-vue'
import type { menuModel } from '@/models/sys/menuModel'

import { useRouter } from 'vue-router';
import { systemReq } from '@/utils/reqUtil'
import SelfMenu from '@/compenents/SelfMenu.vue';
import { ElMessageBox } from 'element-plus'
// import { useGlbStateStore } from '@/stores/glbstate'

const router = useRouter()
const to_pg = (obj: menuModel) => {
  md.menus.forEach((e:menuModel) => {
    if (e.children && e.children.length>0) {
      e.children.forEach((c:menuModel) => {
        if (c.menuType == 3)
          c.isSelect = false
        else if (c.children && c.children.length > 0) {
          c.children.forEach((ch:menuModel) => {
            if (ch.menuType == 3) ch.isSelect = false
          })
        }
      })
    }
  })
  obj.isSelect = true
  // console.log(obj.url)
  router.push(obj.url)
  localStorage.setItem('currSelMenu', obj.id)
}

const quit_hdl = () => {
  ElMessageBox.confirm(
    '确定退出系统吗?',
    '退出登录',
    {
      confirmButtonText: '确定',
      cancelButtonText: '取消',
      type: 'warning',
    }
  ).then(() => {
      localStorage.setItem('tkn', '')
      router.replace('/')
    })
    .catch(() => { })
}

// const glbstate = useGlbStateStore()
// console.log(glbstate.usrInfo.avatar + ' --- ' + glbstate.usrInfo.name)

const lg_title = ref(import.meta.env.VITE_SYS_LOG_TITLE)
const md = reactive<{
  loading:boolean,
  layout:number,
  currNav:string,  // 当前选中展开的目录
  modules:menuModel[],
  menus:menuModel[],
  isNavExpand:boolean,
  navWdth:string
}>({
  loading: false,
  currNav: '',
  layout:1,   // 布局类型 1全部左侧 2顶部模块+左侧子菜单
  modules: [],
  // menus:[
  //   {
  //     "id": "1",
  //     "parent_id": "0",
  //     "name": "系统管理",
  //     "code": "01",
  //     "menu_type": 1,
  //     "path": "/system",
  //     "icon": "icon-system-locked",
  //     "icon_size": "20",
  //     "isExpand":true,
  //     "isSelect":false,
  //     "childs": [
  //       {
  //         "id": "101",
  //         "parent_id": "1",
  //         "name": "用户管理",
  //         "code": "0101",
  //         "menu_type": 3,
  //         "path": "/sys/user",
  //         "icon": "icon-yonghuguanli",
  //         "icon_size": "21",
  //         "childs": [],
  //     "isSelect":false,
  //         "isExpand":false
  //       },
  //       {
  //         "id": "102",
  //         "parent_id": "1",
  //         "name": "角色权限管理",
  //         "code": "0102",
  //         "menu_type": 3,
  //         "path": "/sys/permission",
  //         "icon": "icon-jiaose",
  //         "icon_size": "18",
  //         "childs": [],
  //     "isSelect":false,
  //         "isExpand":false
  //       },
  //       {
  //         "id": "103",
  //         "parent_id": "1",
  //         "name": "菜单管理",
  //         "code": "0103",
  //         "menu_type": 3,
  //         "path": "/sys/menu",
  //         "icon": "icon-view-module",
  //         "icon_size": "18",
  //         "childs": [],
  //     "isSelect":false,
  //         "isExpand":false
  //       },
  //       {
  //         "id": "104",
  //         "parent_id": "1",
  //         "name": "日志管理",
  //         "code": "0104",
  //         "menu_type": 3,
  //         "path": "/sys/loginlog",
  //         "icon": "icon-MenuIcon-rizhiguanli-heise",
  //         "icon_size": "20",
  //         "childs": [],
  //     "isSelect":false,
  //         "isExpand":false
  //       }
  //     ]
  //   },
  //   {
  //     "id": "2",
  //     "parent_id": "0",
  //     "name": "任务管理",
  //     "code": "02",
  //     "menu_type": 1,
  //     "path": "/",
  //     "icon": "icon-task-time",
  //     "icon_size": "22",
  //     "isExpand":false,
  //     "isSelect":false,
  //     "childs": [
  //       {
  //         "id": "201",
  //         "parent_id": "2",
  //         "name": "文章管理",
  //         "code": "0201",
  //         "menu_type": 3,
  //         "path": "/sys/org",
  //         "icon": "icon-article",
  //         "icon_size": "19",
  //         "childs": [],
  //         "isSelect":false,
  //         "isExpand":false
  //       },
  //       {
  //         "id": "202",
  //         "parent_id": "2",
  //         "name": "分类管理",
  //         "code": "0202",
  //         "menu_type": 3,
  //         "path": "/sys/org",
  //         "icon": "icon-gengduo",
  //         "icon_size": "20",
  //         "childs": [],
  //         "isSelect":false,
  //         "isExpand":false
  //       },
  //       {
  //         "id": "203",
  //         "parent_id": "2",
  //         "name": "评论管理",
  //         "code": "0203",
  //         "menu_type": 3,
  //         "path": "/sys/org",
  //         "icon": "icon-pinglun",
  //         "icon_size": "20",
  //         "childs": [],
  //         "isSelect":false,
  //         "isExpand":false
  //       },
  //       {
  //         "id": "204",
  //         "parent_id": "2",
  //         "name": "友情链接",
  //         "code": "0204",
  //         "menu_type": 3,
  //         "path": "/sys/org",
  //         "icon": "icon-lianjie",
  //         "icon_size": "20",
  //         "childs": [],
  //         "isSelect":false,
  //         "isExpand":false
  //       }
  //     ]
  //   },
  //   {
  //     "isExpand":false,
  //     "id": "3",
  //     "parent_id": "0",
  //     "name": "移动端管理",
  //     "code": "03",
  //     "menu_type": 1,
  //     "path": "/operation",
  //     "icon": "icon-shouji",
  //     "icon_size": "21",
  //     "isSelect":false,
  //     "childs": [
  //       {
  //         "id": "301",
  //         "parent_id": "3",
  //         "name": "数据看板",
  //         "code": "0301",
  //         "menu_type": 3,
  //         "path": "/operation/dashboard",
  //         "icon": "icon-shujujianguan",
  //         "icon_size": "20",
  //         "childs": [],
  //         "isSelect":false,
  //         "isExpand":false
  //       },
  //       {
  //         "id": "302",
  //         "parent_id": "3",
  //         "name": "消息推送",
  //         "code": "0302",
  //         "menu_type": 3,
  //         "path": "/operation/push",
  //         "icon": "icon-fenxiang1",
  //         "icon_size": "20",
  //         "childs": [],
  //         "isSelect":false,
  //         "isExpand":false
  //       },
  //       {
  //         "id": "303",
  //         "parent_id": "3",
  //         "name": "活动管理",
  //         "code": "0303",
  //         "menu_type": 3,
  //         "path": "/operation/activity",
  //         "icon": "icon-huodongchouhua",
  //         "icon_size": "20",
  //         "childs": [],
  //         "isSelect":false,
  //         "isExpand":false
  //       },
  //       {
  //         "id": "304",
  //         "parent_id": "3",
  //         "name": "问卷调研",
  //         "code": "0304",
  //         "menu_type": 3,
  //         "path": "/operation/survey",
  //         "icon": "icon-dengji",
  //         "icon_size": "20",
  //         "childs": [],
  //         "isSelect":false,
  //         "isExpand":false
  //       }
  //     ]
  //   },
  //   {
  //     "id": "4",
  //     "parent_id": "0",
  //     "name": "统计分析",
  //     "code": "04",
  //     "menu_type": 1,
  //     "path": "/analytics",
  //     "icon": "icon-tubiao",
  //     "icon_size": "23",
  //     "isExpand":false,
  //     "isSelect":false,
  //     "childs": [
  //       {
  //         "id": "401",
  //         "parent_id": "4",
  //         "name": "用户分析",
  //         "code": "0401",
  //         "menu_type": 3,
  //         "path": "/analytics/user",
  //         "icon": "icon-MenuIcon-renyuanguanli-heise",
  //         "icon_size": "20",
  //         "childs": [],
  //         "isSelect":false,
  //         "isExpand":false
  //       },
  //       {
  //         "id": "402",
  //         "parent_id": "4",
  //         "name": "销售报表",
  //         "code": "0402",
  //         "menu_type": 3,
  //         "path": "/analytics/sales",
  //         "icon": "icon-yunyingguize",
  //         "icon_size": "21",
  //         "childs": [],
  //         "isSelect":false,
  //         "isExpand":false
  //       },
  //       {
  //         "id": "403",
  //         "parent_id": "4",
  //         "name": "流量分析",
  //         "code": "0403",
  //         "menu_type": 3,
  //         "path": "/analytics/traffic",
  //         "icon": "icon-celve",
  //         "icon_size": "18",
  //         "childs": [],
  //         "isSelect":false,
  //         "isExpand":false
  //       },
  //       {
  //         "id": "404",
  //         "parent_id": "4",
  //         "name": "转化漏斗",
  //         "code": "0404",
  //         "menu_type": 3,
  //         "path": "/analytics/funnel",
  //         "icon": "icon-shaixuan",
  //         "icon_size": "20",
  //         "childs": [],
  //         "isSelect":false,
  //         "isExpand":false
  //       }
  //     ]
  //   },
  //   {
  //     "id": "5",
  //     "parent_id": "0",
  //     "name": "测试007",
  //     "code": "05",
  //     "menu_type": 1,
  //     "path": "/sys/org",
  //     "icon": "icon-tubiao",
  //     "icon_size": "23",
  //     "isExpand":false,
  //     "isSelect":false,
  //     "childs": [
  //       {
  //         "id": "502",
  //         "parent_id": "5",
  //         "name": "测试00071",
  //         "code": "0502",
  //         "menu_type": 3,
  //         "path": "/sys/org",
  //         "icon": "icon-tubiao",
  //         "icon_size": "20",
  //         "isExpand":false,
  //         "isSelect":false,
  //         "childs": []
  //       },
  //       {
  //         "id": "501",
  //         "parent_id": "5",
  //         "name": "测试00070",
  //         "code": "0501",
  //         "menu_type": 2,
  //         "path": "/sys/org",
  //         "icon": "icon-tubiao",
  //         "icon_size": "20",
  //         "isExpand":false,
  //         "isSelect":false,
  //         "childs": [
  //           {
  //             "id": "50101",
  //             "parent_id": "501",
  //             "name": "测试00007",
  //             "code": "050101",
  //             "menu_type": 3,
  //             "path": "/sys/org",
  //             "icon": "icon-tubiao",
  //             "icon_size": "20",
  //             "isExpand":false,
  //             "isSelect":false,
  //             "childs": []
  //           },
  //           {
  //             "id": "50102",
  //             "parent_id": "501",
  //             "name": "测试000072",
  //             "code": "050102",
  //             "menu_type": 3,
  //             "path": "/sys/org",
  //             "icon": "icon-tubiao",
  //             "icon_size": "20",
  //             "isExpand":false,
  //             "isSelect":false,
  //             "childs": []
  //           }
  //         ]
  //       },
  //       {
  //         "id": "503",
  //         "parent_id": "5",
  //         "name": "测试00072",
  //         "code": "0503",
  //         "menu_type": 2,
  //         "path": "/sys/org",
  //         "icon": "icon-tubiao",
  //         "icon_size": "20",
  //         "isExpand":false,
  //         "isSelect":false,
  //         "childs": [
  //           {
  //             "id": "50301",
  //             "parent_id": "503",
  //             "name": "测试000073",
  //             "code": "050301",
  //             "menu_type": 3,
  //             "path": "/sys/org",
  //             "icon": "icon-tubiao",
  //             "icon_size": "20",
  //             "isExpand":false,
  //             "isSelect":false,
  //             "childs": []
  //           },
  //           {
  //             "id": "50302",
  //             "parent_id": "503",
  //             "name": "测试000075",
  //             "code": "050302",
  //             "menu_type": 3,
  //             "path": "/sys/org",
  //             "icon": "icon-tubiao",
  //             "icon_size": "20",
  //             "isExpand":false,
  //             "isSelect":false,
  //             "childs": []
  //           }
  //         ]
  //       }
  //     ]
  //   }
  // ],
  menus:[],
  isNavExpand:true,
  navWdth:'199px'
})

onMounted(() => { // 初始化加载
  md.loading = true
  systemReq.axiosIns.get('api/sys/SysUser/Permissions')
  .then((res: any) => { // console.log(res)
    md.loading = false
    const currSelMenu = localStorage.getItem('currSelMenu')
    let currUrl = ''
    if (currSelMenu) {
      res.Data.permissions.forEach((e:menuModel) => {
         if (e.children && e.children.length>0) {
            e.children.forEach((c:menuModel) => {
              if (c.id == currSelMenu) {
                c.isSelect = true
                e.isExpand = true
                md.currNav = e.id
                currUrl = c.url
                return
              }
              else if (c.children && c.children.length>0) {
                c.children.forEach((ch:menuModel) => {
                  if (ch.id == currSelMenu) {
                    ch.isSelect = true
                    c.isExpand = true
                    e.isExpand = true
                    md.currNav = e.id
                    currUrl = ch.url
                    return
                  }
                })
              }
            })
          }
      })
    }
    md.menus = res.Data.permissions
    if (currUrl)
      router.push(currUrl)
  })
  .catch(ex=>{
    console.log(ex.message)
    md.loading = false
  })
})

const expandOr = () => {
  if (md.isNavExpand) {
    md.menus.forEach(e=>{
      if (e.isExpand)
         md.currNav = e.id
      e.isExpand = false
    })
  }
  else if (md.currNav) {
    md.menus.forEach(e=>{
      if (e.id == md.currNav && e.isExpand == false) {
         e.isExpand = true
         return
      }
    })
  }
  md.isNavExpand = !md.isNavExpand
  md.navWdth = md.isNavExpand?'199px':'66px'
}

</script>

<template>
  <div id="pg_top" v-loading="md.loading" element-loading-text="正在加载...">
    <div id="pg_l" :style="{maxWidth:md.navWdth, minWidth:md.navWdth}">
      <div id="l_header">
          <el-icon :size="46" :color="'white'"><Cpu /> </el-icon>
          <span v-show="md.isNavExpand">・</span>
          <span v-show="md.isNavExpand" style="font-size: 23px; font-style: italic;">{{lg_title}}</span>
      </div>
      <div id="l_nav">
        <self-menu :menus="md.menus" :is-expand="md.isNavExpand" @to-pg="to_pg"/>
      </div>
      <div id="l_footer">
        <el-icon :size="26" :color="'white'" style="cursor: pointer;" @click="expandOr">
          <Fold v-if="md.isNavExpand" />
          <Expand v-else />
        </el-icon>
      </div>
    </div>
    <div id="pg_r">
      <div id="r_header">
        <div style="display: flex; flex: 1;">
          <div v-if="md.modules && md.modules.length>0"></div>
        </div>
        <div style="display: flex; padding-right: 40px;">
          <el-dropdown placement="bottom">
            <el-avatar :size="50" :src="'https://avatars.githubusercontent.com/u/7288459?v=4'" />
            <template #dropdown>
              <el-dropdown-menu>
                <el-dropdown-item>
                   <div class="drp_itm" @click="quit_hdl">
                      <span class="iconfont icon-logout" style="font-size: 19px;"></span>
                      <span style="display: flex; padding-left: 10px;">退出</span>
                   </div>
                </el-dropdown-item>
                <el-dropdown-item>
                   <div class="drp_itm">
                      <span class="iconfont icon-user2" style="font-size: 23px; margin-left: -3px;"></span>
                      <span style="display: flex; padding-left: 6px;">个人中心</span>
                   </div>
                </el-dropdown-item>
                <el-dropdown-item>
                   <div class="drp_itm">
                      <span class="iconfont icon-shezhi" style="font-size: 18px;"></span>
                      <span style="display: flex; padding-left: 10px;">系统设置</span>
                   </div>
                </el-dropdown-item>
              </el-dropdown-menu>
            </template>
          </el-dropdown>
        </div>
      </div>
      <div id="r_content">
        <!-- <RouterView /> -->
        <router-view />
        <!-- <router-view/> -->
      </div>
    </div>
  </div>
</template>

<style scoped>
#pg_top {
  display: flex;
  flex: 1;
  margin: 0px;
  width: 100%;
  height: 100%;
  padding: 0px;
  overflow: hidden;
  background-color: transparent;
}
#pg_l {
  display: flex;
  /* min-width: 199px;
  max-width: 199px; */
  overflow: hidden;
  flex-direction: column;
  background-color: #3964fe;
}
#pg_r {
  display: flex;
  flex: 1;
  flex-direction: column;
  background-color: transparent;
}
#l_header {
  display: flex;
  align-items: center;
  justify-content: center;
  min-height: 66px;
  max-height: 66px;
  width: 100%;
  color: snow;
  /* border-bottom: 1px solid cornflowerblue; */
  box-shadow: 0px 0px 14px 0px cornflowerblue inset;
}
#r_header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding-left: 16px;
  min-height: 67px;
  max-height: 67px;
  width: 100%;
  background-color: lightgray;
  border-bottom: 1px solid snow;
  box-shadow: 0px 0px 11px 0px whitesmoke inset;
}
#r_content {
  display: flex;
  flex: 1;
  padding: 6px;
}
#l_nav {
  display: flex;
  flex: 1;
  align-items: flex-start;
  flex-direction: column;
  justify-content: flex-start;
  overflow: auto;
}
#r_nav {
  display: flex;
  flex: 1;
  padding: 6px;
}
#l_footer {
  display: flex;
  padding: 10px 0px;
  align-items: center;
  justify-content: center;
  /* border-top: 1px solid cornflowerblue; */
  box-shadow: 0px 0px 14px 0px cornflowerblue inset;
}

.icnstl {
  opacity: 0.8;
  font-size: 32px;
  /* color: #49cc90; */
  color: #3964fe;
  padding: 0px 6px 0px 0px;
}

.drp_itm {
  display: flex;
  justify-content: flex-start;
  align-items: center;
}
</style>
