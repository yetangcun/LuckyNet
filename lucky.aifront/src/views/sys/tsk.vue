<template>
  <div class="pg_top">
    <div class="pg_query">
      <el-text class="lbStl">关键字:</el-text>
      <el-input class="commonInput" v-model="state.query.txt" placeholder="名称|账号"></el-input>
      <el-text class="lbStl">状态:</el-text>
      <el-select class="commonInput" v-model="state.query.status" :options="state.sttKv" placeholder="请选择" :props="props" clearable/>
      <el-button type="primary" :icon="Search" class="btnStl" @click="getList">查询</el-button>
      <el-button type="danger" :icon="Plus" class="btnStl" @click="btnAdd">新增</el-button>
    </div>
    <div class="pg_grid">
      <div class="grid_div">
        <el-table :data="state.tbData" row-key="id" v-loading="state.loading" height="100%">
            <el-table-column type="selection" width="66" />
            <!-- <el-table-column label="Date" width="120">
              <template #default="scope">{{ scope.row.date }}</template>
            </el-table-column> -->
            <el-table-column property="name" label="名称" width="166" show-overflow-tooltip />
            <el-table-column property="code" label="编码" width="166" show-overflow-tooltip />
            <el-table-column property="status" label="状态" width="88">
              <template #default="scope">
                <div style="display: flex; align-items: center">
                  <span v-if="scope.row.status==1" style="color: green">启用</span>
                  <span v-else style="color: red">禁用</span>
                </div>
              </template>
            </el-table-column>
            <el-table-column property="cron" label="策略" width="137" show-overflow-tooltip />" />
            <el-table-column property="paramModel" label="参数模型" width="257" show-overflow-tooltip/>
            <el-table-column property="createTime" label="创建时间" width="166" />
            <el-table-column property="updateTime" label="更新时间" width="166" show-overflow-tooltip />
            <el-table-column property="createUser" label="创建人" width="137"/>
            <el-table-column property="updateUser" label="更新人" width="137"/>
            <el-table-column label="设置" width="257">
              <template #default="scope">
                <div>
                  <el-switch v-model="scope.row.status" :inactive-value="0" :active-value="1" inactive-text="禁用" active-text="启用" @change="setStatus(scope.row.id)" />
                </div>
              </template>
            </el-table-column>
            <el-table-column label="操作" min-width="176">
              <template #default="scope">
                <div>
                  <el-button @click="btnEdit(scope.row.id)" type="primary">编辑</el-button>
                  <el-button @click="btnDel(scope.row.id)" type="danger">删除</el-button>
                  <el-button @click="btnDetail(scope.row.id)" type="warning">执行记录</el-button>
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
      style="display: flex; justify-content: flex-end; margin: 10px 10px 10px 1px;"
      layout="total, sizes, prev, pager, next"
      @current-change="hdlCurrentChange"
    />
    </div>
  </div>
  <el-dialog
    v-model="state.dlgVisible"
    :title="state.dlgTitle"
    width="600"
    draggable
    overflow
  >
    <div style="display: flex; flex: 1; padding: 10px 6px 6px 6px;">
      <el-form :model="state.opt" label-width="auto" style="width: 100%;">
          <el-form-item label="名称" placeholder="请输入姓名" prop="name">
            <el-input v-model="state.opt.name" />
          </el-form-item>
          <el-form-item label="编码" placeholder="业务模块+业务功能@用英文单词">
            <el-input v-model="state.opt.code" />
          </el-form-item>
          <el-form-item label="策略" placeholder="请输入昵称">
            <el-input v-model="state.opt.cron" />
          </el-form-item>
          <el-form-item label="状态" placeholder="请选择性别">
            <el-radio-group v-model="state.opt.status">
              <el-radio :value="1"> 启用 </el-radio>
              <el-radio :value="0"> 禁用 </el-radio>
            </el-radio-group>
          </el-form-item>
          <el-form-item label="参数模型">
            <el-input v-model="state.opt.paramModel" type="textarea" :rows="6" placeholder="请输入参数模型" />
          </el-form-item>
          <el-form-item label="备注">
            <el-input v-model="state.opt.remark" type="textarea" :rows="3" placeholder="请输入参数模型"  />
          </el-form-item>
      </el-form>
    </div>
    <template #footer>
      <div class="dialog-footer">
        <el-button @click="state.dlgVisible = false">取消</el-button>
        <el-button type="primary" @click="btnSave">确定</el-button>
      </div>
    </template>
  </el-dialog>
</template>

<script lang="ts" setup>
import { onMounted, reactive, nextTick, inject } from 'vue';
import { Plus, Search } from '@element-plus/icons-vue'
import { ElMessage, ElMessageBox } from 'element-plus'
import type { selNumKV } from '@/models/common/selectKV';
import type { tskQueryModel, tskOptModel, tskModel } from '@/models/sys/tskModel'

import { systemReq } from '@/utils/reqUtil';

const props = {
  value: 'value',
  label: 'label'
}

const state = reactive<{
  loading:boolean,
  dlgTitle:string,
  dlgVisible:boolean,
  query:tskQueryModel,
  sttKv: selNumKV[],
  opt: tskOptModel,
  tbData: tskModel[]
}>({
  loading: false,
  dlgTitle: '',
  dlgVisible:false,
  query: {
    txt: '',
    status: -1,
    pageIndex: 1,
    pageSize: 20,
    total: 0
  },
  sttKv: [
    {
      label: '请选择',
      value: -1
    },
    {
      label: '禁用',
      value: 0
    },
    {
      label: '启用',
      value: 1
    }
  ],
  tbData: [],
  opt: {
    id: -1,
    name: '',
    code: '',
    status: 0,
    cron: '',
    paramModel: '',
    remark: ''
  }
})

const getList = () => {
  state.loading = true
  systemReq.axiosIns.get('api/sys/SysTsk/pages', { params:state.query })
  .then((res:any) => {
    state.loading = false
    // console.log(res.Data)
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

const setStatus = (val:number) => {
  systemReq.axiosIns.put(`api/sys/SysTsk/status/${val}`)
  .then((res:any) => {
    ElMessage.success('操作成功')
  })
  .catch((err:any)=>{
    console.log(err)
  })
  // getList()
}

const hdlCurrentChange = (val:number) => {
  console.log('val: '+val)
  state.query.pageIndex = val
  getList()
}

const btnAdd = () => { // 新增
  state.dlgTitle = '新增任务'
  state.dlgVisible = true
  state.opt.id = undefined
  state.opt.name = ''
  state.opt.code = ''
  state.opt.status = 0
  state.opt.cron = ''
  state.opt.paramModel = ''
  state.opt.remark = ''
}

const btnEdit = (id:number) => {  // 编辑
  state.dlgTitle = '编辑任务'
  state.dlgVisible = true
  // console.log(id)
  state.opt.id = id
  systemReq.axiosIns.get(`api/sys/SysTsk/${id}`)
  .then((res:any) => {
    // console.log(res.Data)
    state.opt.name = res.Data.name
    state.opt.code = res.Data.code
    state.opt.remark = res.Data.remark
    state.opt.cron = res.Data.cron
    state.opt.paramModel = res.Data.paramModel
    state.opt.status = res.Data.status
  })
  .catch((err:any)=>{
    console.log(err)
  })
}

const btnDel = (id:string) => { // console.log(id)
  ElMessageBox.confirm(
    '确认删除?',
    '警告',
    {
      confirmButtonText: '确认',
      cancelButtonText: '取消',
      type: 'warning',
    }
  ).then(() => {
      systemReq.axiosIns.delete(`api/sys/SysTsk/${id}`)
      .then((res:any) => {
        // console.log(res)
        getList()
        ElMessage({
          type: 'success',
          message: '删除成功',
        })
      })
      .catch((err:any)=>{
        console.log(err)
      })
    }).catch(() => {
      ElMessage({
        type: 'info',
        message: '取消删除',
      })
    })
}

const btnSave = () => {
  if(!state.opt.name || !state.opt.code) {
    ElMessage({
      type: 'warning',
      message: '请填写完整信息',
    })
    return
  }

  if (!state.opt.id) state.opt.id = 0
  systemReq.axiosIns.post('api/sys/SysTsk', state.opt)
  .then((res:any) => {
    state.dlgVisible = false
    if (res.Data) {
      getList()
      ElMessage({
        type: 'success',
        message: '保存成功',
      })
      return
    }
    else if(res.Msg) {
      ElMessage({
        type: 'error',
        message: res.Msg,
      })
    }
  })
  .catch((err:any)=>{
    console.log(err)
  })
}

const tik2pg = inject('tik2pg')
const btnDetail = (id:number) => {
  if (tik2pg) {
      // console.log('btnDetail: '+id)
      tik2pg('24', id)
  }
}

onMounted(async () => {
  getList()
  nextTick(() => {
    state.query.status = -1
  })
})

</script>

<style scoped>

</style>
