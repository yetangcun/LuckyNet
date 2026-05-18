<script lang="ts" setup>
// import { RouterView } from 'vue-router';
import { onMounted, reactive, ref } from 'vue';
import { Cpu, Expand, Fold } from '@element-plus/icons-vue'
import type { menuModel } from '@/models/sys/menuModel'

// import { useGlbStateStore } from '@/stores/glbstate'
import { systemReq } from '@/utils/reqUtil'

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
      "icon": "setting",
      "icon_size": "24",
      "childs": [
        {
          "id": "101",
          "parent_id": "1",
          "name": "user",
          "code": "0101",
          "menu_type": 2,
          "path": "/system/user",
          "icon": "user",
          "icon_size": "20",
          "childs": []
        },
        {
          "id": "102",
          "parent_id": "1",
          "name": "role",
          "code": "0102",
          "menu_type": 2,
          "path": "/system/role",
          "icon": "team",
          "icon_size": "20",
          "childs": []
        },
        {
          "id": "103",
          "parent_id": "1",
          "name": "menu",
          "code": "0103",
          "menu_type": 2,
          "path": "/system/menu",
          "icon": "menu",
          "icon_size": "20",
          "childs": []
        },
        {
          "id": "104",
          "parent_id": "1",
          "name": "日志管理",
          "code": "0104",
          "menu_type": 2,
          "path": "/system/log",
          "icon": "file-text",
          "icon_size": "20",
          "childs": []
        }
      ]
    },
    {
      "id": "2",
      "parent_id": "0",
      "name": "内容管理",
      "code": "02",
      "menu_type": 1,
      "path": "/content",
      "icon": "folder",
      "icon_size": "24",
      "childs": [
        {
          "id": "201",
          "parent_id": "2",
          "name": "文章管理",
          "code": "0201",
          "menu_type": 2,
          "path": "/content/article",
          "icon": "file",
          "icon_size": "20",
          "childs": []
        },
        {
          "id": "202",
          "parent_id": "2",
          "name": "分类管理",
          "code": "0202",
          "menu_type": 2,
          "path": "/content/category",
          "icon": "tags",
          "icon_size": "20",
          "childs": []
        },
        {
          "id": "203",
          "parent_id": "2",
          "name": "评论管理",
          "code": "0203",
          "menu_type": 2,
          "path": "/content/comment",
          "icon": "message",
          "icon_size": "20",
          "childs": []
        },
        {
          "id": "204",
          "parent_id": "2",
          "name": "友情链接",
          "code": "0204",
          "menu_type": 2,
          "path": "/content/link",
          "icon": "link",
          "icon_size": "20",
          "childs": []
        }
      ]
    },
    {
      "id": "3",
      "parent_id": "0",
      "name": "运营工具",
      "code": "03",
      "menu_type": 1,
      "path": "/operation",
      "icon": "tool",
      "icon_size": "24",
      "childs": [
        {
          "id": "301",
          "parent_id": "3",
          "name": "数据看板",
          "code": "0301",
          "menu_type": 2,
          "path": "/operation/dashboard",
          "icon": "dashboard",
          "icon_size": "20",
          "childs": []
        },
        {
          "id": "302",
          "parent_id": "3",
          "name": "消息推送",
          "code": "0302",
          "menu_type": 2,
          "path": "/operation/push",
          "icon": "notification",
          "icon_size": "20",
          "childs": []
        },
        {
          "id": "303",
          "parent_id": "3",
          "name": "活动管理",
          "code": "0303",
          "menu_type": 2,
          "path": "/operation/activity",
          "icon": "gift",
          "icon_size": "20",
          "childs": []
        },
        {
          "id": "304",
          "parent_id": "3",
          "name": "问卷调研",
          "code": "0304",
          "menu_type": 2,
          "path": "/operation/survey",
          "icon": "form",
          "icon_size": "20",
          "childs": []
        }
      ]
    },
    {
      "id": "4",
      "parent_id": "0",
      "name": "数据分析",
      "code": "04",
      "menu_type": 1,
      "path": "/analytics",
      "icon": "bar-chart",
      "icon_size": "24",
      "childs": [
        {
          "id": "401",
          "parent_id": "4",
          "name": "用户分析",
          "code": "0401",
          "menu_type": 2,
          "path": "/analytics/user",
          "icon": "user",
          "icon_size": "20",
          "childs": []
        },
        {
          "id": "402",
          "parent_id": "4",
          "name": "销售报表",
          "code": "0402",
          "menu_type": 2,
          "path": "/analytics/sales",
          "icon": "shopping-cart",
          "icon_size": "20",
          "childs": []
        },
        {
          "id": "403",
          "parent_id": "4",
          "name": "流量分析",
          "code": "0403",
          "menu_type": 2,
          "path": "/analytics/traffic",
          "icon": "line-chart",
          "icon_size": "20",
          "childs": []
        },
        {
          "id": "404",
          "parent_id": "4",
          "name": "转化漏斗",
          "code": "0404",
          "menu_type": 2,
          "path": "/analytics/funnel",
          "icon": "funnel-plot",
          "icon_size": "20",
          "childs": []
        }
      ]
    },
    {
      "id": "5",
      "parent_id": "0",
      "name": "商城模块",
      "code": "05",
      "menu_type": 1,
      "path": "/shop",
      "icon": "store",
      "icon_size": "24",
      "childs": [
        {
          "id": "501",
          "parent_id": "5",
          "name": "商品管理",
          "code": "0501",
          "menu_type": 2,
          "path": "/shop/product",
          "icon": "shopping",
          "icon_size": "20",
          "childs": []
        },
        {
          "id": "502",
          "parent_id": "5",
          "name": "订单管理",
          "code": "0502",
          "menu_type": 2,
          "path": "/shop/order",
          "icon": "profile",
          "icon_size": "20",
          "childs": []
        },
        {
          "id": "503",
          "parent_id": "5",
          "name": "库存管理",
          "code": "0503",
          "menu_type": 2,
          "path": "/shop/stock",
          "icon": "inbox",
          "icon_size": "20",
          "childs": []
        },
        {
          "id": "504",
          "parent_id": "5",
          "name": "促销管理",
          "code": "0504",
          "menu_type": 2,
          "path": "/shop/promotion",
          "icon": "tag",
          "icon_size": "20",
          "childs": []
        }
      ]
    },
    {
      "id": "6",
      "parent_id": "0",
      "name": "系统监控",
      "code": "06",
      "menu_type": 1,
      "path": "/monitor",
      "icon": "eye",
      "icon_size": "24",
      "childs": [
        {
          "id": "601",
          "parent_id": "6",
          "name": "服务状态",
          "code": "0601",
          "menu_type": 2,
          "path": "/monitor/health",
          "icon": "heart",
          "icon_size": "20",
          "childs": []
        },
        {
          "id": "602",
          "parent_id": "6",
          "name": "SQL监控",
          "code": "0602",
          "menu_type": 2,
          "path": "/monitor/sql",
          "icon": "database",
          "icon_size": "20",
          "childs": []
        },
        {
          "id": "603",
          "parent_id": "6",
          "name": "缓存监控",
          "code": "0603",
          "menu_type": 2,
          "path": "/monitor/cache",
          "icon": "rocket",
          "icon_size": "20",
          "childs": []
        },
        {
          "id": "604",
          "parent_id": "6",
          "name": "job",
          "code": "0604",
          "menu_type": 2,
          "path": "/monitor/job",
          "icon": "clock-circle",
          "icon_size": "20",
          "childs": []
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
        <div v-for="vl in md.menus" :key="vl.code">

        </div>
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
        <div></div>
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
  min-width: 168px;
  max-width: 199px;
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
</style>
