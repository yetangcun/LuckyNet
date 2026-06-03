<template>
  <div class="pg_top">
    <div class="pg_query">
      <el-text class="lbStl">关键字:</el-text>
      <el-input class="commonInput" v-model="state.query.txt" placeholder="名称|账号"></el-input>
      <el-text class="lbStl">组织机构:</el-text>
      <el-select class="commonInput" v-model="state.query.orgId"
      :options="state.selKv" :props="props">

      </el-select>
      <el-button type="primary" :icon="Search">查询</el-button>
    </div>
    <div class="pg_grid">

    </div>
  </div>
</template>

<script lang="ts" setup>
import { reactive } from 'vue';
import { Search } from '@element-plus/icons-vue'
import type { selKV } from '@/models/common/selectKV';
import type { usrQueryModel } from '@/models/sys/usrInfoModel'

const props = {
  value: 'id',
  label: 'label',
  options: 'options',
  disabled: 'disabled',
}
const state = reactive<{
  query:usrQueryModel,
  selKv: selKV[]
}>({
  query: {
    txt: '',
    orgId: ''
  },
  selKv: [
    {
      label: '请选择',
      id: ''
    }
  ]
})

</script>

<style scoped>

/*
.pg_top {
  display: flex;
  flex: 1;
  width: 100%;
  height: 100%;
} */

</style>
