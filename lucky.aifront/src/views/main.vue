<script lang="ts" setup>
// import { RouterView } from 'vue-router';
import { Cpu } from '@element-plus/icons-vue'
import { ref } from 'vue';

const lg_title = ref(import.meta.env.VITE_SYS_LOG_TITLE)

console.log(lg_title)

</script>

<template>
  <div id="pg_top">
    <div id="pg_l">
      <div id="l_header">
          <el-icon :size="46" :color="'white'"><Cpu /> </el-icon>
          <span>・</span>
          <!-- <span style="font-size: 23px; font-style: italic;">AI NEXT</span> -->
          <span style="font-size: 23px; font-style: italic;">{{lg_title}}</span>
      </div>
      <div id="l_nav"></div>
    </div>
    <div id="pg_r">
      <div id="r_header"></div>
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
  background-color: deepskyblue;
}
#pg_l {
  display: flex;
  min-width: 168px;
  max-width: 199px;
  background-color: blueviolet;
}
#pg_r {
  display: flex;
  flex: 1;
  padding: 10px 20px;
  background-color: lightgray;
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
</style>
