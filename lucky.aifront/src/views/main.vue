<script lang="ts" setup></script>

<template>
  <div id="pg_top">
    <div id="pg_l"></div>
    <div id="pg_r"></div>
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
  padding: 16px 0px 16px 16px;
  background-color: blueviolet;
}
#pg_r {
  display: flex;
  flex: 1;
  padding: 10px 20px;
  background-color: lightgray;
}
</style>
