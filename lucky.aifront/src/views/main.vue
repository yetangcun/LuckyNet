<script lang="ts" setup>
// import { RouterView } from 'vue-router';
import { onMounted, reactive, ref } from 'vue';
import { Cpu, Expand, Fold } from '@element-plus/icons-vue'
import type { menuModel } from '@/models/sys/menuModel'

import { useRouter } from 'vue-router';
import { systemReq } from '@/utils/reqUtil'
import SelfMenu from '@/compenents/SelfMenu.vue';
import { ElMessageBox } from 'element-plus'
import type { TabsPaneContext, TabPaneName } from 'element-plus'
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
  md.currSelMenu = obj.id

  if (md.selMenuns.length > 0) {
    let isExist = false
    md.selMenuns.forEach((e:menuModel) => {
      if (e.id == obj.id) {
        isExist = true
        e.isSelect = true
      }
      else e.isSelect = false
    })
    if (!isExist)
      md.selMenuns.push(obj)
  }
  else md.selMenuns.push(obj) // console.log(obj.url)
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

const lg_title = ref(import.meta.env.VITE_SYS_LOG_TITLE)
const md = reactive<{
  loading:boolean,
  layout:number,
  currNav:string,  // 当前选中展开的目录
  currSelMenu:string,  // 当前选中菜单
  modules:menuModel[],
  menus:menuModel[],
  selMenuns:menuModel[],
  isNavExpand:boolean,
  navWdth:string
}>({
  loading: false,
  currNav: '',
  currSelMenu: '-1',
  layout:1,   // 布局类型 1全部左侧 2顶部模块+左侧子菜单
  modules: [],
  menus:[],
  selMenuns:[{
    id: '-1',
    name: '首页',
    parentId: '0',
    code: '-1',
    url: '/sys/home',
    icon: 'icon-shouye',
    menuType: 3,
    status: 1,
    sort: 1,
    iconSize: '16',
    children: [],
    isExpand: false,
    isSelect: false
  }],
  isNavExpand:true,
  navWdth:'199px'
})

const tab_click = (tab:TabsPaneContext) => { // console.log(tab.props.name) 单击选中tab
  md.currSelMenu =  tab.props.name!.toString()
  md.selMenuns.forEach((f:menuModel) => {
    if (f.id == tab.props.name) {
      f.isSelect = true
      router.push(f.url)
      asyncSelectMenu(f.id)
    }
    else f.isSelect = false
  })
  localStorage.setItem('currSelMenu', md.currSelMenu)
}

const cls_tab = (tab:TabPaneName) => { // 关闭tab
  const idx = md.selMenuns.findIndex((e:menuModel) => e.id == tab) // console.log(tab, idx)
  if (idx >= 0) {
    md.selMenuns.splice(idx, 1)
    if (md.selMenuns.length > 0) {
      const lastIdx = idx - 1 >= 0 ? idx - 1 : 0
      const lastMenu = md.selMenuns[lastIdx]
      if (lastMenu) {
        md.currSelMenu = lastMenu.id; router.push(lastMenu.url)
        asyncSelectMenu(lastMenu.id)
        return
      }
    }
    md.currSelMenu = '-1'
    router.push('/sys/home')
  }
}

const asyncSelectMenu = (id: string) => { // 状态复位
  md.menus.forEach((e:menuModel) => {
    e.isSelect = false
    if (e.children && e.children.length>0) {
      e.isExpand = false
      e.children.forEach((c:menuModel) => {
        c.isSelect = false
        if (c.children && c.children.length > 0){
          c.isExpand = false
          c.children.forEach((ch:menuModel) => {
            ch.isSelect = false
          })
        }
      })
    }
  })

  // 根据id选中菜单
  md.menus.forEach((e:menuModel) => {
    if (e.children && e.children.length>0) {
      e.children.forEach((c:menuModel) => {
        if (c.id == id) {
          c.isSelect = true
          md.currNav = e.id
          e.isExpand = true
          return
        }
        else if (c.children && c.children.length > 0) {
          c.children.forEach((ch:menuModel) => {
            if (ch.id == id){
              ch.isSelect = true
              c.isExpand = true
              e.isExpand = true
              md.currNav = e.id
              return
            }
          })
        }
      })
    }
    else {
      e.isSelect = false
      if (e.id == md.currSelMenu) e.isSelect = true
    }
  })
}

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
                md.currNav = e.id; currUrl = c.url
                md.currSelMenu = c.id
                md.selMenuns.push(c)
                return
              }
              else if (c.children && c.children.length>0) {
                c.children.forEach((ch:menuModel) => {
                  if (ch.id == currSelMenu) {
                    ch.isSelect = true
                    c.isExpand = true
                    e.isExpand = true
                    md.currNav = e.id; currUrl = ch.url
                    md.currSelMenu = ch.id
                    md.selMenuns.push(ch)
                    return
                  }
                })
              }
            })
          }
      })
    }

    md.menus = res.Data.permissions // console.log(md.menus, res.Data.permissions)
    if (currUrl)
      router.push(currUrl)
    else
      router.push('/sys/home')
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
      <div id="r_tab">
        <el-tabs v-model="md.currSelMenu" type="card" @tab-click="tab_click" @tab-remove="cls_tab">
          <el-tab-pane v-for="(e, i) in md.selMenuns" :key="i" :label="e.name" :name="e.id" :closable="e.id!='-1'">
            <template #label>
              <div>
                <span :class="'iconfont '+ e.icon" style="font-size: 16px; margin-top: 2px;"></span><!-- <el-icon :size="20" :color="'white'"><Cpu /></el-icon> -->
                <span style="padding-left: 10px;">{{e.name}}</span>
              </div>
            </template>
          </el-tab-pane>

        </el-tabs>
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
  min-width: 0;   /* 关键666 */
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
  background-color: #e4e7ed;
  border-bottom: 1px solid snow;
  box-shadow: 0px 0px 11px 0px whitesmoke inset;
}
#r_content {
  display: flex;
  flex: 1;
  padding: 6px;
  min-height: 0px;  /* 关键666 */
  overflow: hidden;
  flex-direction: column;
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

:deep(.el-tabs__header) {
  /* background-color: #3964fe; */
  color: white;
  border-bottom: 1px solid #e4e7ed;
  margin: 1px 0px 0px 0px;
}
:deep(.el-tabs__item) {
  padding: 0px 11px;
}
</style>
