<template>
  <div class="pg_top">
    <div class="pg_query">
      <el-text class="lbStl">关键字:</el-text>
      <el-input class="commonInput" v-model="state.query.tskId" placeholder="任务id" clearable/>
      <el-select class="commonInput" v-model="state.query.status" placeholder="请选择" clearable>
        <el-option v-for="item in state.statusKv" :key="item.value" :label="item.label" :value="item.value"></el-option>
      </el-select>
      <el-button type="primary" :icon="Search" class="btnStl" @click="getList">查询</el-button>
    </div>
    <div class="pg_grid">
      <div class="grid_div">
        <el-table :data="state.tbData" v-loading="state.loading" height="100%">
            <el-table-column type="selection" width="55" />
            <el-table-column property="tskId" label="任务id" width="257" />
            <el-table-column property="status" label="状态" width="88">
              <template #default="scope">
                <div style="display: flex; align-items: center">
                  <span v-if="scope.row.status==1" style="color: green;">成功</span>
                  <span v-else style="color: orangered;">失败</span>
                </div>
              </template>
            </el-table-column>
            <el-table-column property="tskParam" label="参数" width="166" show-overflow-tooltip/>
            <el-table-column property="startTime" label="开始时间" width="166" show-overflow-tooltip/>
            <el-table-column property="endTime" label="完成时间" width="166" show-overflow-tooltip />
            <el-table-column property="tskMsg" label="执行信息" width="257" show-overflow-tooltip/>
            <el-table-column label="操作" min-width="156">
              <template #default="scope">
                <div>
                  <el-button @click="btnScan(scope.row.id)" type="primary">详情</el-button>
                </div>
              </template>
            </el-table-column>
        </el-table>
      </div>
      <el-pagination
      v-model:current-page="state.query.pageIndex"
      v-model:page-size="state.query.pageSize"
      :size="'default'"
      :background="true"
      :total="state.query.total"
      @size-change="hdlSizeChange"
      :page-sizes="[20, 50, 100, 300, 500, 1000]"
      layout="total, sizes, prev, pager, next"
      style="display: flex; justify-content: flex-end; align-items: center; margin: 10px;"
      @current-change="hdlCurrentChange"
      />
    </div>
  </div>
</template>

<script lang="ts" setup>
import { onMounted, reactive } from 'vue';
import { Search } from '@element-plus/icons-vue'
import type { selNumKV } from '@/models/common/selectKV';
import type { tskRecordQueryModel, tskRecordModel } from '@/models/sys/tskModel'

import { useRoute } from 'vue-router'

import { systemReq } from '@/utils/reqUtil';

const route = useRoute()
let tskId = route.query.otherId?route.query.otherId.toString():''

const state = reactive<{
  loading:boolean,
  query:tskRecordQueryModel,
  statusKv: selNumKV[],
  tbData: tskRecordModel[]
}>({
  loading: false,
  query: {
    tskId: '',
    status: -1,
    startTime: '',
    endTime: '',
    pageIndex: 1,
    pageSize: 20,
    total: 0
  },
  statusKv: [
    {
      label: '请选择',
      value: -1,
    },
    {
      label: '成功',
      value: 1
    },
    {
      label: '失败',
      value: 0
    }
  ],
  tbData: [],
})

const getList = () => {
  state.loading = true
  state.tbData = [] // console.log('tskId: '+tskId)
  if (tskId) {
    state.query.tskId = tskId
    tskId = ''
  }
  systemReq.axiosIns.get('api/sys/SysTsk/record/pages', { params:state.query })
  .then((res:any) => {
    state.loading = false
    // console.log(res)
    state.query.total = res.Total
    state.tbData = res.Data
  })
  .catch((err:any)=>{
    state.loading = false
    console.log(err)
  })
}

const hdlSizeChange = (val:number) => {
  state.query.pageSize = val
  getList()
}

const hdlCurrentChange = (val:number) => { // console.log('val: '+val)
  state.query.pageIndex = val
  getList()
}

const btnScan = (id:number) => {
  console.log(id)
}

onMounted(async () => {
  getList()
})

</script>

<style scoped>

/*
.pg_top {
  display: flex;
  flex: 1;
  width: 100%;
  height: 100%;
}
*/

/* 表格包裹层：自动填充剩余空间 */

</style>
