<script lang="ts" setup>
// import { RouterView } from 'vue-router';
import { reactive, ref } from 'vue';
import { Cpu, Expand, Fold } from '@element-plus/icons-vue'
import type { menuModel } from '@/models/sys/menuModel'

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
      "parentId": "0",
      "name": "system",
      "txt": "系统管理",
      "code": "01",
      "menuType": 1,
      "url": "/system",
      "icon": "setting",
      "iconSize": "24",
      "childs": [
        {
          "id": "101",
          "parentId": "1",
          "name": "user",
          "txt": "用户管理",
          "code": "0101",
          "menuType": 2,
          "url": "/system/user",
          "icon": "user",
          "iconSize": "20",
          "childs": []
        },
        {
          "id": "102",
          "parentId": "1",
          "name": "role",
          "txt": "角色管理",
          "code": "0102",
          "menuType": 2,
          "url": "/system/role",
          "icon": "team",
          "iconSize": "20",
          "childs": []
        },
        {
          "id": "103",
          "parentId": "1",
          "name": "menu",
          "txt": "菜单管理",
          "code": "0103",
          "menuType": 2,
          "url": "/system/menu",
          "icon": "menu",
          "iconSize": "20",
          "childs": []
        },
        {
          "id": "104",
          "parentId": "1",
          "name": "log",
          "txt": "日志管理",
          "code": "0104",
          "menuType": 2,
          "url": "/system/log",
          "icon": "file-text",
          "iconSize": "20",
          "childs": []
        }
      ]
    },
    {
      "id": "2",
      "parentId": "0",
      "name": "content",
      "txt": "内容管理",
      "code": "02",
      "menuType": 1,
      "url": "/content",
      "icon": "folder",
      "iconSize": "24",
      "childs": [
        {
          "id": "201",
          "parentId": "2",
          "name": "article",
          "txt": "文章管理",
          "code": "0201",
          "menuType": 2,
          "url": "/content/article",
          "icon": "file",
          "iconSize": "20",
          "childs": []
        },
        {
          "id": "202",
          "parentId": "2",
          "name": "category",
          "txt": "分类管理",
          "code": "0202",
          "menuType": 2,
          "url": "/content/category",
          "icon": "tags",
          "iconSize": "20",
          "childs": []
        },
        {
          "id": "203",
          "parentId": "2",
          "name": "comment",
          "txt": "评论管理",
          "code": "0203",
          "menuType": 2,
          "url": "/content/comment",
          "icon": "message",
          "iconSize": "20",
          "childs": []
        },
        {
          "id": "204",
          "parentId": "2",
          "name": "link",
          "txt": "友情链接",
          "code": "0204",
          "menuType": 2,
          "url": "/content/link",
          "icon": "link",
          "iconSize": "20",
          "childs": []
        }
      ]
    },
    {
      "id": "3",
      "parentId": "0",
      "name": "operation",
      "txt": "运营工具",
      "code": "03",
      "menuType": 1,
      "url": "/operation",
      "icon": "tool",
      "iconSize": "24",
      "childs": [
        {
          "id": "301",
          "parentId": "3",
          "name": "dashboard",
          "txt": "数据看板",
          "code": "0301",
          "menuType": 2,
          "url": "/operation/dashboard",
          "icon": "dashboard",
          "iconSize": "20",
          "childs": []
        },
        {
          "id": "302",
          "parentId": "3",
          "name": "push",
          "txt": "消息推送",
          "code": "0302",
          "menuType": 2,
          "url": "/operation/push",
          "icon": "notification",
          "iconSize": "20",
          "childs": []
        },
        {
          "id": "303",
          "parentId": "3",
          "name": "activity",
          "txt": "活动管理",
          "code": "0303",
          "menuType": 2,
          "url": "/operation/activity",
          "icon": "gift",
          "iconSize": "20",
          "childs": []
        },
        {
          "id": "304",
          "parentId": "3",
          "name": "survey",
          "txt": "问卷调研",
          "code": "0304",
          "menuType": 2,
          "url": "/operation/survey",
          "icon": "form",
          "iconSize": "20",
          "childs": []
        }
      ]
    },
    {
      "id": "4",
      "parentId": "0",
      "name": "analytics",
      "txt": "数据分析",
      "code": "04",
      "menuType": 1,
      "url": "/analytics",
      "icon": "bar-chart",
      "iconSize": "24",
      "childs": [
        {
          "id": "401",
          "parentId": "4",
          "name": "userAnalysis",
          "txt": "用户分析",
          "code": "0401",
          "menuType": 2,
          "url": "/analytics/user",
          "icon": "user",
          "iconSize": "20",
          "childs": []
        },
        {
          "id": "402",
          "parentId": "4",
          "name": "sales",
          "txt": "销售报表",
          "code": "0402",
          "menuType": 2,
          "url": "/analytics/sales",
          "icon": "shopping-cart",
          "iconSize": "20",
          "childs": []
        },
        {
          "id": "403",
          "parentId": "4",
          "name": "traffic",
          "txt": "流量分析",
          "code": "0403",
          "menuType": 2,
          "url": "/analytics/traffic",
          "icon": "line-chart",
          "iconSize": "20",
          "childs": []
        },
        {
          "id": "404",
          "parentId": "4",
          "name": "funnel",
          "txt": "转化漏斗",
          "code": "0404",
          "menuType": 2,
          "url": "/analytics/funnel",
          "icon": "funnel-plot",
          "iconSize": "20",
          "childs": []
        }
      ]
    },
    {
      "id": "5",
      "parentId": "0",
      "name": "shop",
      "txt": "商城模块",
      "code": "05",
      "menuType": 1,
      "url": "/shop",
      "icon": "store",
      "iconSize": "24",
      "childs": [
        {
          "id": "501",
          "parentId": "5",
          "name": "product",
          "txt": "商品管理",
          "code": "0501",
          "menuType": 2,
          "url": "/shop/product",
          "icon": "shopping",
          "iconSize": "20",
          "childs": []
        },
        {
          "id": "502",
          "parentId": "5",
          "name": "order",
          "txt": "订单管理",
          "code": "0502",
          "menuType": 2,
          "url": "/shop/order",
          "icon": "profile",
          "iconSize": "20",
          "childs": []
        },
        {
          "id": "503",
          "parentId": "5",
          "name": "stock",
          "txt": "库存管理",
          "code": "0503",
          "menuType": 2,
          "url": "/shop/stock",
          "icon": "inbox",
          "iconSize": "20",
          "childs": []
        },
        {
          "id": "504",
          "parentId": "5",
          "name": "promotion",
          "txt": "促销管理",
          "code": "0504",
          "menuType": 2,
          "url": "/shop/promotion",
          "icon": "tag",
          "iconSize": "20",
          "childs": []
        }
      ]
    },
    {
      "id": "6",
      "parentId": "0",
      "name": "monitor",
      "txt": "系统监控",
      "code": "06",
      "menuType": 1,
      "url": "/monitor",
      "icon": "eye",
      "iconSize": "24",
      "childs": [
        {
          "id": "601",
          "parentId": "6",
          "name": "health",
          "txt": "服务状态",
          "code": "0601",
          "menuType": 2,
          "url": "/monitor/health",
          "icon": "heart",
          "iconSize": "20",
          "childs": []
        },
        {
          "id": "602",
          "parentId": "6",
          "name": "sql",
          "txt": "SQL监控",
          "code": "0602",
          "menuType": 2,
          "url": "/monitor/sql",
          "icon": "database",
          "iconSize": "20",
          "childs": []
        },
        {
          "id": "603",
          "parentId": "6",
          "name": "cache",
          "txt": "缓存监控",
          "code": "0603",
          "menuType": 2,
          "url": "/monitor/cache",
          "icon": "rocket",
          "iconSize": "20",
          "childs": []
        },
        {
          "id": "604",
          "parentId": "6",
          "name": "job",
          "txt": "定时任务",
          "code": "0604",
          "menuType": 2,
          "url": "/monitor/job",
          "icon": "clock-circle",
          "iconSize": "20",
          "childs": []
        }
      ]
    }
  ]
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
