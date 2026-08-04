<template>
  <div class="pg_top">
    <div class="pg_query">
      <el-text class="lbStl">关键字:</el-text>
      <el-input class="commonInput" v-model="state.query.txt" placeholder="名称|账号"></el-input>
      <el-text class="lbStl">组织机构:</el-text>
      <el-tree-select
        v-model="state.query.orgId"
        :data="state.orgKv"
        check-strictly
        placeholder="请选择"
        class="commonInput"
        :render-after-expand="false"/>
      <!-- <el-select class="commonInput" v-model="state.query.orgId"
      :options="state.selKv" :props="props" placeholder="请选择"/> -->
      <el-button type="primary" :icon="Search" class="btnStl" @click="getList">查询</el-button>
      <el-button type="danger" :icon="Plus" class="btnStl" @click="btnAdd">新增</el-button>
    </div>
    <div class="pg_grid">
      <div class="grid_div">
        <el-table :data="state.tbData" v-loading="state.loading" height="100%">
            <el-table-column type="selection" width="55" />
            <!-- <el-table-column label="Date" width="120">
              <template #default="scope">{{ scope.row.date }}</template>
            </el-table-column> -->
            <el-table-column property="realname" label="姓名" width="120" />
            <el-table-column property="sex" label="性别" width="88">
              <template #default="scope">
                <div style="display: flex; align-items: center">
                  <span v-if="scope.row.sex==1">男</span>
                  <span v-else>女</span>
                </div>
              </template>
            </el-table-column>
            <el-table-column property="account" label="昵称" width="120" />
            <el-table-column property="roleName" label="角色" width="166" show-overflow-tooltip/>
            <el-table-column property="phone" label="联系方式" width="120" />
            <el-table-column property="avatar" label="头像" width="166" show-overflow-tooltip/>
            <el-table-column property="org" label="组织机构" width="120" show-overflow-tooltip />
            <el-table-column label="日期" width="137">
              <template #default="scope">{{ scope.row.createTime }}</template>
            </el-table-column>
            <el-table-column property="createUser" label="创建人" width="120"/>
            <el-table-column
              property="addr"
              label="地址"
              width="240" show-overflow-tooltip
            />
            <el-table-column label="操作" min-width="176">
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
      style="display: flex; justify-content: flex-end; margin: 10px 10px 10px 1px;"
      layout="total, sizes, prev, pager, next"
      @current-change="hdlCurrentChange"
    />
    </div>
  </div>
  <el-dialog
    v-model="state.dlgVisible"
    :title="state.dlgTitle"
    width="500"
    draggable
    overflow
  >
    <div style="display: flex; flex: 1; padding: 10px 6px 6px 6px;">
      <el-form :model="state.opt" label-width="auto" style="width: 100%;">
          <el-form-item label="姓名" placeholder="请输入姓名" prop="name">
            <el-input v-model="state.opt.name" />
          </el-form-item>
          <el-form-item label="账号" placeholder="请输入账号">
            <el-input v-model="state.opt.account" />
          </el-form-item>
          <el-form-item label="昵称" placeholder="请输入昵称">
            <el-input v-model="state.opt.name" />
          </el-form-item>
          <el-form-item label="性别" placeholder="请选择性别">
            <el-radio-group v-model="state.opt.sex">
              <el-radio :value="1"> 男 </el-radio>
              <el-radio :value="2"> 女 </el-radio>
            </el-radio-group>
          </el-form-item>
          <el-form-item label="角色">
            <el-select v-model="state.opt.roleIds" multiple
            :options="state.roleKv" placeholder="请选择角色">
            </el-select>
          </el-form-item>
          <el-form-item label="所属组织" placeholder="请选择组织">
            <el-tree-select
              v-model="state.opt.orgId"
              :data="state.orgKv"
              check-strictly
              placeholder="请选择"
              :render-after-expand="false"/>
          </el-form-item>
          <el-form-item label="联系方式">
            <el-input v-model="state.opt.phone" />
          </el-form-item>
          <el-form-item label="地址">
            <el-input v-model="state.opt.addr" />
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
import { onMounted, reactive } from 'vue';
import { Plus, Search } from '@element-plus/icons-vue'
import { ElMessage, ElMessageBox } from 'element-plus'
import type { treeSelKV, selNumKV } from '@/models/common/selectKV';
import type { usrQueryModel, usrInfoModel, usrOptModel } from '@/models/sys/usrInfoModel'

import { systemReq } from '@/utils/reqUtil';

// const props = {
//   value: 'id',
//   label: 'label',
//   options: 'options',
//   disabled: 'disabled',
// }

const state = reactive<{
  loading:boolean,
  dlgTitle:string,
  dlgVisible:boolean,
  query:usrQueryModel,
  orgKv: treeSelKV[],
  roleKv: selNumKV[],
  opt: usrOptModel,
  tbData: usrInfoModel[]
}>({
  loading: false,
  dlgTitle: '',
  dlgVisible:false,
  query: {
    txt: '',
    orgId: '',
    pageIndex: 1,
    pageSize: 20,
    total: 0
  },
  orgKv: [
    {
      label: '请选择',
      value: '',
      children: []
    }
  ],
  roleKv: [
    {
      label: '请选择',
      value: -1
    }
  ],
  tbData: [],
  opt: {
    id: '',
    name: '',
    roleId: '',
    roleIds: [],
    account: '',
    orgId: '',
    avatar: '',
    status: 0,
    sex: 1,
    phone: '',
    addr: ''
  }
})

const getList = () => {
  state.loading = true
  systemReq.axiosIns.get('api/sys/SysUser/pages', { params:state.query })
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

const getRoleKv = async () => { // 获取角色下拉框
  systemReq.axiosIns.get('api/sys/SysRole/roleSels')
  .then((res:any) => {
    // console.log(res.Data)
    state.roleKv = res.Data
  })
  .catch((err:any)=>{
    console.log(err)
  })
}

const getOrgKv = async () => {  // 获取组织树
  systemReq.axiosIns.get('api/sys/SysOrg/treeSels')
  .then((res:any) => {
    // console.log(res.Data)
    state.orgKv = res.Data
  })
  .catch((err:any)=>{
    console.log(err)
  })
}

const hdlSizeChange = (val:number) => {
  state.query.pageSize = val
  getList()
}

const hdlCurrentChange = (val:number) => {
  console.log('val: '+val)
  state.query.pageIndex = val
  getList()
}

const btnAdd = () => { // 新增
  state.dlgTitle = '新增用户'
  state.dlgVisible = true
  state.opt.id = null
  state.opt.name = ''
  state.opt.account = ''
  state.opt.roleId = ''
  state.opt.roleIds = []
  state.opt.orgId = null
  state.opt.avatar = ''
  state.opt.status = 0
  state.opt.sex = 1
  state.opt.phone = ''
  state.opt.addr = ''
}

const btnEdit = (id:string) => {  // 编辑
  state.dlgTitle = '编辑用户'
  state.dlgVisible = true
  // console.log(id)
  state.opt.id = id
  systemReq.axiosIns.get(`api/sys/SysUser/${id}`)
  .then((res:any) => {
    // console.log(res.Data)
    state.opt.name = res.Data.realname
    state.opt.account = res.Data.account
    state.opt.roleIds = res.Data.roleIds
    state.opt.orgId = res.Data.orgId
    state.opt.avatar = res.Data.avatar
    state.opt.status = res.Data.status
    state.opt.sex = res.Data.sex?res.Data.sex:1
    state.opt.phone = res.Data.phone
    state.opt.addr = res.Data.addr
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
      systemReq.axiosIns.delete(`api/sys/SysUser/${id}`)
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
  if(!state.opt.name || !state.opt.account) {
    ElMessage({
      type: 'warning',
      message: '请填写完整信息',
    })
    return
  }

  if (!state.opt.roleIds || state.opt.roleIds.length <= 0) {
    ElMessage({
      type: 'warning',
      message: '请选择角色',
    })
    return
  }

  state.opt.roleId = state.opt.roleIds.join(',')

  if (!state.opt.id) {
    // state.opt.id = '0'
    systemReq.axiosIns.post('api/sys/SysUser', state.opt)
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
  else {
    systemReq.axiosIns.put(`api/sys/SysUser`, state.opt)
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
}

onMounted(async () => {
  getList()
  await getRoleKv()
  await getOrgKv()
})

</script>

<style scoped>

</style>
