<script lang="ts" setup>
// import { RouterView } from 'vue-router';
import { onMounted, reactive, ref } from 'vue';
import { Cpu, Expand, Fold } from '@element-plus/icons-vue'
import type { menuModel } from '@/models/sys/menuModel'

// import { useGlbStateStore } from '@/stores/glbstate'
import { systemReq } from '@/utils/reqUtil'
import SelfMenu from '@/compenents/SelfMenu.vue';

// const glbstate = useGlbStateStore()

// console.log(glbstate.usrInfo.avatar + ' --- ' + glbstate.usrInfo.name)

const lg_title = ref(import.meta.env.VITE_SYS_LOG_TITLE)
const md = reactive<{
  loading:boolean,
  navState:boolean,
  layout:number,
  modules:menuModel[],
  menus:menuModel[]
}>({
  loading: false,
  navState: true,
  layout:1,   // 布局类型 1全部左侧 2顶部模块+左侧子菜单
  modules: [],
  menus:[
    {
      "id": "1",
      "parent_id": "0",
      "name": "系统管理",
      "code": "01",
      "menu_type": 1,
      "path": "/system",
      "icon": "icon-system-locked",
      "icon_size": "19",
      "isExpand":false,
      "childs": [
        {
          "id": "101",
          "parent_id": "1",
          "name": "用户管理",
          "code": "0101",
          "menu_type": 3,
          "path": "/system/user",
          "icon": "icon-yonghuguanli",
          "icon_size": "21",
          "childs": [],
          "isExpand":false
        },
        {
          "id": "102",
          "parent_id": "1",
          "name": "角色管理",
          "code": "0102",
          "menu_type": 3,
          "path": "/system/role",
          "icon": "icon-jiaose",
          "icon_size": "18",
          "childs": [],
          "isExpand":false
        },
        {
          "id": "103",
          "parent_id": "1",
          "name": "菜单管理",
          "code": "0103",
          "menu_type": 3,
          "path": "/system/menu",
          "icon": "icon-view-module",
          "icon_size": "18",
          "childs": [],
          "isExpand":false
        },
        {
          "id": "104",
          "parent_id": "1",
          "name": "日志管理",
          "code": "0104",
          "menu_type": 3,
          "path": "/system/log",
          "icon": "icon-MenuIcon-rizhiguanli-heise",
          "icon_size": "20",
          "childs": [],
          "isExpand":false
        }
      ]
    },
    {
      "id": "2",
      "parent_id": "0",
      "name": "任务管理",
      "code": "02",
      "menu_type": 1,
      "path": "/content",
      "icon": "icon-task-time",
      "icon_size": "21",
      "isExpand":false,
      "childs": [
        {
          "id": "201",
          "parent_id": "2",
          "name": "文章管理",
          "code": "0201",
          "menu_type": 3,
          "path": "/content/article",
          "icon": "file",
          "icon_size": "20",
          "childs": [],
      "isExpand":false
        },
        {
          "id": "202",
          "parent_id": "2",
          "name": "分类管理",
          "code": "0202",
          "menu_type": 3,
          "path": "/content/category",
          "icon": "tags",
          "icon_size": "20",
          "childs": [],
      "isExpand":false
        },
        {
          "id": "203",
          "parent_id": "2",
          "name": "评论管理",
          "code": "0203",
          "menu_type": 3,
          "path": "/content/comment",
          "icon": "message",
          "icon_size": "20",
          "childs": [],
      "isExpand":false
        },
        {
          "id": "204",
          "parent_id": "2",
          "name": "友情链接",
          "code": "0204",
          "menu_type": 3,
          "path": "/content/link",
          "icon": "link",
          "icon_size": "20",
          "childs": [],
      "isExpand":false
        }
      ]
    },
    {
      "isExpand":false,
      "id": "3",
      "parent_id": "0",
      "name": "移动端管理",
      "code": "03",
      "menu_type": 1,
      "path": "/operation",
      "icon": "icon-shouji",
      "icon_size": "20",
      "childs": [
        {
          "id": "301",
          "parent_id": "3",
          "name": "数据看板",
          "code": "0301",
          "menu_type": 3,
          "path": "/operation/dashboard",
          "icon": "dashboard",
          "icon_size": "20",
          "childs": [],
      "isExpand":false
        },
        {
          "id": "302",
          "parent_id": "3",
          "name": "消息推送",
          "code": "0302",
          "menu_type": 3,
          "path": "/operation/push",
          "icon": "notification",
          "icon_size": "20",
          "childs": [],
      "isExpand":false
        },
        {
          "id": "303",
          "parent_id": "3",
          "name": "活动管理",
          "code": "0303",
          "menu_type": 3,
          "path": "/operation/activity",
          "icon": "gift",
          "icon_size": "20",
          "childs": [],
      "isExpand":false
        },
        {
          "id": "304",
          "parent_id": "3",
          "name": "问卷调研",
          "code": "0304",
          "menu_type": 3,
          "path": "/operation/survey",
          "icon": "form",
          "icon_size": "20",
          "childs": [],
      "isExpand":false
        }
      ]
    },
    {
      "id": "4",
      "parent_id": "0",
      "name": "统计分析",
      "code": "04",
      "menu_type": 1,
      "path": "/analytics",
      "icon": "icon-tubiao",
      "icon_size": "22",
      "isExpand":false,
      "childs": [
        {
          "id": "401",
          "parent_id": "4",
          "name": "用户分析",
          "code": "0401",
          "menu_type": 3,
          "path": "/analytics/user",
          "icon": "user",
          "icon_size": "20",
          "childs": [],
      "isExpand":false
        },
        {
          "id": "402",
          "parent_id": "4",
          "name": "销售报表",
          "code": "0402",
          "menu_type": 3,
          "path": "/analytics/sales",
          "icon": "shopping-cart",
          "icon_size": "20",
          "childs": [],
      "isExpand":false
        },
        {
          "id": "403",
          "parent_id": "4",
          "name": "流量分析",
          "code": "0403",
          "menu_type": 3,
          "path": "/analytics/traffic",
          "icon": "line-chart",
          "icon_size": "20",
          "childs": [],
      "isExpand":false
        },
        {
          "id": "404",
          "parent_id": "4",
          "name": "转化漏斗",
          "code": "0404",
          "menu_type": 3,
          "path": "/analytics/funnel",
          "icon": "funnel-plot",
          "icon_size": "20",
          "childs": [],
      "isExpand":false
        }
      ]
    }
  ]
})

onMounted(() => { // 初始化加载
  systemReq.axiosIns.get('api/sys/SysUser/Permissions')
  .then(res=>{
    console.log(res)
  })
  .catch(ex=>{
    console.log(ex.message)
  })
})

console.log(lg_title)

</script>

<template>
  <div id="pg_top" v-loading="md.loading">
    <div id="pg_l">
      <div id="l_header">
          <el-icon :size="46" :color="'white'"><Cpu /> </el-icon>
          <span>・</span>
          <!-- <span style="font-size: 23px; font-style: italic;">AI NEXT</span> -->
          <span style="font-size: 23px; font-style: italic;">{{lg_title}}</span>
      </div>
      <div id="l_nav">
        <self-menu :menus="md.menus"></self-menu>
        <!-- <div v-for="vl in md.menus" :key="vl.code">

        </div> -->
      </div>
      <div id="l_footer">
        <el-icon :size="26" :color="'white'" style="cursor: pointer;">
          <Fold v-if="md.navState" />
          <Expand v-else />
        </el-icon>
      </div>
    </div>
    <div id="pg_r">
      <div id="r_header">
        <div v-if="md.modules && md.modules.length>0">

        </div>
        <div>
          <span class="iconfont icon-xihuan icnstl"></span>
          <span class="iconfont icon-xihuan icnstl"></span>
          <span class="iconfont icon-xihuan icnstl"></span>
          <span class="iconfont icon-xihuan icnstl"></span>
          <span class="iconfont icon-xihuan icnstl"></span>
          <span class="iconfont icon-xihuan icnstl"></span>
          <span class="iconfont icon-xihuan icnstl"></span>
          <span class="iconfont icon-xihuan icnstl"></span>
          <span class="iconfont icon-xihuan icnstl"></span>
          <span class="iconfont icon-xihuan icnstl"></span>
          <span class="iconfont icon-xihuan icnstl"></span>
          <span class="iconfont icon-xihuan icnstl"></span>
          <span class="iconfont icon-xihuan icnstl"></span>
          <span class="iconfont icon-xihuan icnstl"></span>
          <span class="iconfont icon-xihuan icnstl"></span>
          <span class="iconfont icon-xihuan icnstl"></span>
          <span class="iconfont icon-xihuan icnstl"></span>
          <span class="iconfont icon-xihuan icnstl"></span>
          <span class="iconfont icon-xihuan icnstl"></span>
          <span class="iconfont icon-xihuan icnstl"></span>
          <span class="iconfont icon-xihuan icnstl"></span>
          <span class="iconfont icon-xihuan icnstl"></span>
          <span class="iconfont icon-xihuan icnstl"></span>
          <span class="iconfont icon-xihuan icnstl"></span>
          <span class="iconfont icon-xihuan icnstl"></span>
          <span class="iconfont icon-xihuan icnstl"></span>
          <span class="iconfont icon-xihuan icnstl"></span>
          <span class="iconfont icon-xihuan icnstl"></span>
          <span class="iconfont icon-xihuan icnstl"></span>
          <span class="iconfont icon-xihuan icnstl"></span>
          <span class="iconfont icon-xihuan icnstl"></span>
          <span class="iconfont icon-xihuan icnstl"></span>
          <span class="iconfont icon-xihuan icnstl"></span>
          <span class="iconfont icon-xihuan icnstl"></span>
          <span class="iconfont icon-xihuan icnstl"></span>
          <span class="iconfont icon-xihuan icnstl"></span>
          <span class="iconfont icon-xihuan icnstl"></span>
          <span class="iconfont icon-xihuan icnstl"></span>
          <span class="iconfont icon-xihuan icnstl"></span>
          <span class="iconfont icon-xihuan icnstl"></span>
          <span class="iconfont icon-xihuan icnstl"></span>
          <span class="iconfont icon-xihuan icnstl"></span>
          <span class="iconfont icon-xihuan icnstl"></span>
          <span class="iconfont icon-xihuan icnstl"></span>
          <span class="iconfont icon-xihuan icnstl"></span>
          <span class="iconfont icon-xihuan icnstl"></span>
          <span class="iconfont icon-xihuan icnstl"></span>
          <span class="iconfont icon-xihuan icnstl"></span>
          <span class="iconfont icon-xihuan icnstl"></span>
          <span class="iconfont icon-xihuan icnstl"></span>
          <span class="iconfont icon-xihuan icnstl"></span>
          <span class="iconfont icon-xihuan icnstl"></span>
          <span class="iconfont icon-xihuan icnstl"></span>
          <span class="iconfont icon-xihuan icnstl"></span>
          <span class="iconfont icon-xihuan icnstl"></span>
          <span class="iconfont icon-xihuan icnstl"></span>
          <span class="iconfont icon-xihuan icnstl"></span>
          <span class="iconfont icon-xihuan icnstl"></span>
          <span class="iconfont icon-xihuan icnstl"></span>
          <span class="iconfont icon-xihuan icnstl"></span>
          <span class="iconfont icon-xihuan icnstl"></span>
          <span class="iconfont icon-xihuan icnstl"></span>
        </div>
      </div>
      <div id="r_content">
        <router-view></router-view>
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
  min-width: 199px;
  max-width: 211px;
  flex-direction: column;
  background-color: #3964fe;
}
#pg_r {
  display: flex;
  flex: 1;
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
  border-bottom: 1px solid lightgray;
}
#r_header {
  display: flex;
  justify-content: flex-start;
  align-items: center;
  padding-left: 16px;
  min-height: 66px;
  max-height: 66px;
  width: 100%;
  background-color: lightgray;
  border-bottom: 1px solid snow;
}
#l_nav {
  display: flex;
  flex: 1;
  align-items: flex-start;
  flex-direction: column;
  justify-content: flex-start;
  padding: 10px 0px 0px 10px;
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
  border-top: 1px solid lightgray;
}

.icnstl {
  font-size: 32px;
  color: #3964fe;
  padding: 0px 6px 0px 0px;
}
</style>
