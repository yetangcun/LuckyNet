<template>
  <div class="pg_top">
    <div class="pg_query">
      <el-text class="lbStl">关键字:</el-text>
      <el-input class="commonInput" v-model="state.query.txt" placeholder="名称|账号"></el-input>
      <el-text class="lbStl">组织机构:</el-text>
      <el-tree-select
        v-model="state.query.orgId"
        :data="state.selKv"
        check-strictly
        placeholder="请选择"
        class="commonInput"
        :render-after-expand="false"/>
      <!-- <el-select class="commonInput" v-model="state.query.orgId"
      :options="state.selKv" :props="props" placeholder="请选择"/> -->
      <el-button type="primary" :icon="Search" class="btnStl">查询</el-button>
    </div>
    <div class="pg_grid">
      <el-table :data="state.tbData" style="display: flex; flex: 1;width: auto; height: 100%; flex-wrap: wrap; flex-shrink: 1;">
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
          <el-table-column property="roleName" label="角色" width="120" />
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
          <el-table-column label="操作">
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
  selKv: selKV[],
  opt: usrOptModel,
  tbData: usrInfoModel[]
}>({
  loading: false,
  dlgTitle: '',
  dlgVisible:false,
  query: {
    txt: '',
    orgId: ''
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
    name: ''
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

/*
.pg_top {
  display: flex;
  flex: 1;
  width: 100%;
  height: 100%;
}
*/

</style>
