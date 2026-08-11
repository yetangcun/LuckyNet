<template>
  <div class="pg_top">
    <div class="pg_query">
      <el-text class="lbStl">关键字:</el-text>
      <el-input class="commonInput" v-model="state.query.txt" placeholder="类型名|选项名" clearable></el-input>
      <el-text class="lbStl">配置类型:</el-text>
      <el-select class="commonInput" v-model="state.query.cfgType"
      :options="state.typeKv" :props="props" placeholder="请选择" clearable/>
      <el-button type="primary" :icon="Search" class="btnStl" @click="getList">查询</el-button>
      <el-button type="danger" :icon="Plus" class="btnStl" @click="btnAdd">新增</el-button>
    </div>
    <div class="pg_grid">
      <div class="grid_div">
        <el-table :data="state.tbData" row-key="id" height="100%" v-loading="state.loading">
            <el-table-column type="selection" width="55" />
            <el-table-column property="cfgType" label="配置类型" width="137" show-overflow-tooltip/>
            <el-table-column property="typeName" label="类型名" width="137" show-overflow-tooltip />
            <!-- <el-table-column property="sex" label="性别" width="88">
              <template #default="scope">
                <div style="display: flex; align-items: center">
                  <span v-if="scope.row.sex==1">男</span>
                  <span v-else>女</span>
                </div>
              </template>
            </el-table-column> -->
            <el-table-column property="name" label="选项名" width="137" show-overflow-tooltip/>
            <el-table-column property="value" label="选项值" width="137" show-overflow-tooltip />
            <el-table-column property="code" label="编码" width="137" show-overflow-tooltip />
            <el-table-column property="status" label="状态" width="88"/>
            <el-table-column property="sort" label="排序" width="88" />
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
  <el-dialog v-model="state.dlgVisible" :title="state.dlgTitle" width="500" draggable>
    <el-form :model="state.opt" label-width="auto" style="width: 100%;">
      <el-form-item label="配置类型" placeholder="请输入名称">
        <el-select v-model="state.opt.cfgType" @change="getTypeName"
        :options="state.typeKv" :props="props" placeholder="请选择">
        </el-select>
      </el-form-item>
      <el-form-item label="类型名" placeholder="请输入名称">
        <el-input v-model="state.opt.typeName" placeholder="请输入名称"></el-input>
      </el-form-item>
      <el-form-item label="选项名" placeholder="请输入名称">
        <el-input v-model="state.opt.name" placeholder="请输入名称"></el-input>
      </el-form-item>
      <el-form-item label="选项值" placeholder="请输入名称">
        <el-input v-model="state.opt.value" placeholder="请输入名称"></el-input>
      </el-form-item>
      <el-form-item label="编码" placeholder="请输入名称">
        <el-input v-model="state.opt.code" placeholder="请输入名称"></el-input>
      </el-form-item>
      <el-form-item label="排序" placeholder="请输入名称">
        <el-input v-model="state.opt.sort" placeholder="请输入名称"></el-input>
      </el-form-item>
      <el-form-item label="状态" placeholder="请输入名称">
        <el-select v-model="state.opt.status"
        :options="state.statusKv" :props="props" placeholder="请选择">
        </el-select>
      </el-form-item>
    </el-form>
    <template #footer>
      <div class="dialog-footer">
        <el-button @click="btnCancel">取消</el-button>
        <el-button type="primary" @click="btnSave">确定</el-button>
      </div>
    </template>
  </el-dialog>
</template>

<script lang="ts" setup>
import { onMounted, reactive } from 'vue';
import { Plus, Search } from '@element-plus/icons-vue'
import { ElMessage, ElMessageBox } from 'element-plus'
import type { selKV, selNumKV } from '@/models/common/selectKV';
import type { cfgInfoModel, cfgInfoQueryModel, cfgOptModel } from '@/models/sys/cfgInfoModel'

import { systemReq } from '@/utils/reqUtil';

const props = {
  value: 'value',
  label: 'label'
}

const state = reactive<{
  loading:boolean,
  dlgTitle:string,
  typeKv: selKV[],
  statusKv: selNumKV[],
  opt: cfgOptModel,
  dlgVisible:boolean,
  query:cfgInfoQueryModel,
  tbData: cfgInfoModel[]
}>({
  loading: false,
  dlgTitle: '',
  dlgVisible: false,
  query: {
    txt: '',
    cfgType: '',
    total: 0,
    pageIndex: 1,
    pageSize: 20
  },
  typeKv: [
    {
      label: '请选择',
      value: ''
    }
  ],
  statusKv: [
    {
      label: '请选择',
      value: -1
    },
    {
      label: '启用',
      value: 1
    },
    {
      label: '禁用',
      value: 0
    }
  ],
  tbData: [],
  opt: {
    id: -1,
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
  systemReq.axiosIns.get('api/sys/SysConfig/pages', { params:state.query })
  .then((res:any) => {
    // console.log(res.Data)
    state.query.total = res.Total
    state.tbData = res.Data
  })
  .catch((err:any)=>{
    console.log(err)
  })
}

const btnAdd = () => {
  state.dlgTitle = '新增配置'
  state.dlgVisible = true
  state.opt = {
    id: -1,
    name: '',
    code: '',
    value: '',
    sort: 0,
    status: 1,
    cfgType: '',
    typeName: ''
  }
}

const btnEdit = (id:string) => {
  state.dlgTitle = '编辑配置'
  state.dlgVisible = true
  // console.log(id)
  systemReq.axiosIns.get(`api/sys/SysConfig/${id}`).then((res:any) => {
    state.opt = res.Data
  }).catch((err:any)=>{
    console.log(err)
  })
}

const btnCancel = () => {
  state.dlgVisible = false
  state.opt = {
    id: -1,
    name: '',
    code: '',
    value: '',
    sort: 0,
    status: 1,
    cfgType: '',
    typeName: ''
  }
}

const getTypeName = () => {
  if (state.opt.cfgType && state.typeKv)  {
    state.opt.typeName = state.typeKv.find((item:selKV) => item.value === state.opt.cfgType)?.label??''
  } else {
    state.opt.typeName = ''
  }
}

const btnSave = () => {
  if(!state.opt.name || !state.opt.value) {
    ElMessage({
      type: 'warning',
      message: '请填写完整信息',
    })
    return
  }
  systemReq.axiosIns.post(`api/sys/SysConfig`, state.opt).then((res:any) => {
    state.dlgVisible = false
    ElMessage({
      type: 'success',
      message: '保存成功!',
    })
    getList()
    if (!state.opt.cfgType) {
      getTypes()
    }
  }).catch((err:any)=>{
    console.log(err)
  })
}

const hdlSizeChange = (val:number) => {
  state.query.pageSize = val
  getList()
}

const hdlCurrentChange = (val:number) => {
  state.query.pageIndex = val
  getList()
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
      systemReq.axiosIns.delete(`api/sys/SysConfig/${id}`).then((res:any) => {
        ElMessage({
          type: 'success',
          message: '删除成功!',
        })
        getList()
      }).catch((err:any)=>{
        console.log(err)
      })
    }).catch(() => {
      ElMessage({
        type: 'info',
        message: '取消删除',
      })
    })
}

const getTypes = () => {
  state.typeKv = [
    {
      label: '请选择',
      value: ''
    }
  ]
  systemReq.axiosIns.get('api/sys/SysConfig/list').then((res:any) => {
    state.typeKv = res.Data
  }).catch((err:any)=>{
    console.log(err)
  })
}

onMounted(() => {
  getList()
  getTypes()
})

</script>

<style scoped>

</style>
