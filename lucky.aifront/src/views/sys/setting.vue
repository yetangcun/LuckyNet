<template>
  <div class="pg_top">
    <div class="pg_query">
      <el-text class="lbStl">关键字:</el-text>
      <el-input class="commonInput" v-model="state.query.name" placeholder="名称|编码"></el-input>
      <el-text class="lbStl">配置类型:</el-text>
      <el-select class="commonInput" v-model="state.query.cfgType"
      :options="state.selKv" :props="props" placeholder="请选择"/>
      <el-button type="primary" :icon="Search" class="btnStl">查询</el-button>
    </div>
    <div class="pg_grid">
      <el-table :data="state.tbData" style="display: flex; flex: 1;width: auto; height: 100%; flex-wrap: wrap; flex-shrink: 1;">
          <el-table-column type="selection" width="55" />
          <el-table-column property="cfgType" label="配置类型" width="120" />
          <el-table-column property="typeName" label="类型名" width="120" />
          <!-- <el-table-column property="sex" label="性别" width="88">
            <template #default="scope">
              <div style="display: flex; align-items: center">
                <span v-if="scope.row.sex==1">男</span>
                <span v-else>女</span>
              </div>
            </template>
          </el-table-column> -->
          <el-table-column property="name" label="选项名" width="137" show-overflow-tooltip/>
          <el-table-column property="value" label="选项值" width="111" />
          <el-table-column property="code" label="编码" width="101" />
          <el-table-column property="status" label="状态" width="88"/>
          <el-table-column property="sort" label="排序" width="88" show-overflow-tooltip />
          <el-table-column label="日期" width="168">
            <template #default="scope">{{ scope.row.createTime }}</template>
          </el-table-column>
          <el-table-column label="操作" min-width="150" show-overflow-tooltip>
            <template #default="scope">
              <div>
                <el-button @click="btnEdit(scope.row.id)" type="primary">编辑</el-button>
                <el-button @click="btnDel(scope.row.id)" type="danger">删除</el-button>
              </div>
            </template>
          </el-table-column>
      </el-table>
    </div>
  </div>
  <el-dialog
    v-model="state.dlgVisible"
    :title="state.dlgTitle"
    width="500"
    draggable
    overflow
  >
    <span>It's a overflow draggable Dialog</span>
    <template #footer>
      <div class="dialog-footer">
        <el-button @click="state.dlgVisible = false">取消</el-button>
        <el-button type="primary" @click="state.dlgVisible = false">确定</el-button>
      </div>
    </template>
  </el-dialog>
</template>

<script lang="ts" setup>
import { onMounted, reactive } from 'vue';
import { Search } from '@element-plus/icons-vue'
import { ElMessage, ElMessageBox } from 'element-plus'
import type { selKV } from '@/models/common/selectKV';
import type { cfgInfoModel, cfgInfoQueryModel, cfgOptModel } from '@/models/sys/cfgInfoModel'

import { systemReq } from '@/utils/reqUtil';

const props = {
  value: 'value',
  label: 'label'
}

const state = reactive<{
  loading:boolean,
  dlgTitle:string,
  selKv: selKV[],
  opt: cfgOptModel,
  dlgVisible:boolean,
  query:cfgInfoQueryModel,
  tbData: cfgInfoModel[]
}>({
  loading: false,
  dlgTitle: '',
  dlgVisible: false,
  query: {
    name: '',
    cfgType: '',
  },
  selKv: [
    {
      label: '请选择',
      value: ''
    }
  ],
  tbData: [],
  opt: {
    id: '',
    name: '',
    code: '',
    value: '',
    sort: 0,
    status: 1,
    cfgType: '',
    typeName: ''
  }
})

const getList = () => {
  systemReq.axiosIns.get('api/sys/SysUser/pages', { params:state.query })
  .then((res:any) => {
    console.log(res.Data)
    state.tbData = res.Data
  })
  .catch((err:any)=>{
    console.log(err)
  })
}

const btnAdd = () => {
  state.dlgTitle = '新增用户'
  state.dlgVisible = true
}

const btnEdit = (id:string) => {
  state.dlgTitle = '编辑用户'
  state.dlgVisible = true
  // console.log(id)
}

const btnDel = (id:string) => {
  // console.log(id)
  ElMessageBox.confirm(
    '确认删除?',
    '警告',
    {
      confirmButtonText: '确认',
      cancelButtonText: '取消',
      type: 'warning',
    }
  ).then(() => {
      ElMessage({
        type: 'success',
        message: '删除成功',
      })
    }).catch(() => {
      ElMessage({
        type: 'info',
        message: '取消删除',
      })
    })
}

onMounted(() => {
  getList()
})

</script>

<style scoped>

</style>
