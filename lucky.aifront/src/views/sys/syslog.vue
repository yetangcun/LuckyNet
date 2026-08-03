<template>
  <div class="pg_top">
    <div class="pg_query">
      <el-text class="lbStl">关键字:</el-text>
      <el-input class="commonInput" v-model="state.query.reqUrl" placeholder="请求地址"></el-input>
      <el-input class="commonInput" v-model="state.query.reqIp" placeholder="客户端IP"></el-input>
      <el-select class="commonInput" v-model="state.query.reqType" placeholder="请选择">
        <el-option v-for="item in state.methodKv" :key="item.value" :label="item.label" :value="item.value"></el-option>
      </el-select>
      <el-select class="commonInput" v-model="state.query.status" placeholder="请选择">
        <el-option v-for="item in state.statusKv" :key="item.value" :label="item.label" :value="item.value"></el-option>
      </el-select>

      <el-button type="primary" :icon="Search" class="btnStl" @click="getList">查询</el-button>
    </div>
    <div class="pg_grid">
      <div class="grid_div">
        <el-table :data="state.tbData" v-loading="state.loading" height="100%">
            <el-table-column type="selection" width="55" />
            <!-- <el-table-column label="Date" width="120">
              <template #default="scope">{{ scope.row.date }}</template>
            </el-table-column> -->
            <el-table-column property="reqUrl" label="地址" width="257" />
            <el-table-column property="status" label="状态" width="88">
              <template #default="scope">
                <div style="display: flex; align-items: center">
                  <span v-if="scope.row.status==1" style="color: green;">成功</span>
                  <span v-else style="color: orangered;">失败</span>
                </div>
              </template>
            </el-table-column>
            <el-table-column property="reqIp" label="客户端IP" width="166" show-overflow-tooltip/>
            <el-table-column property="reqParam" label="请求参数" width="257" show-overflow-tooltip/>
            <el-table-column property="reqType" label="请求方式" width="88" />
            <el-table-column property="logMsg" label="异常提示" width="199" show-overflow-tooltip />
            <el-table-column property="reqUser" label="用户" width="166" />
            <el-table-column property="reqTime" label="操作时间" width="166" show-overflow-tooltip />
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
import type { selNumKV, selKV } from '@/models/common/selectKV';
import type { logQueryModel, logInfoModel } from '@/models/sys/logModel'

import { systemReq } from '@/utils/reqUtil';

const state = reactive<{
  loading:boolean,
  query:logQueryModel,
  statusKv: selNumKV[],
  methodKv: selKV[],
  tbData: logInfoModel[]
}>({
  loading: false,
  query: {
    pageIndex: 1,
    pageSize: 20,
    reqType: '',
    status: -1,
    reqUrl: '',
    reqIp: '',
    beginTime: '',
    endTime: '',
    total: 0
  },
  methodKv: [
    {
      label: '请选择',
      value: '',
    },
    {
      label: 'GET',
      value: 'GET'
    },
    {
      label: 'POST',
      value: 'POST'
    },
    {
      label: 'PUT',
      value: 'PUT'
    },
    {
      label: 'DELETE',
      value: 'DELETE'
    },
    {
      label: '登录',
      value: 'LOGIN'
    },
    {
      label: '退出',
      value: 'QUIT'
    }
  ],
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
  state.tbData = []
  systemReq.axiosIns.get('api/sys/SysLog/pages', { params:state.query })
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

const hdlCurrentChange = (val:number) => {
  // console.log('val: '+val)
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
