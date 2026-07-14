<template>
  <div class="pg_top">
    <div class="pg_query">
      <el-text class="lbStl">关键字:</el-text>
      <el-input class="commonInput" v-model="state.query.name" placeholder="名称|编码"></el-input>
      <!-- <el-text class="lbStl">组织机构:</el-text>
      <el-tree-select
        v-model="state.query.orgId"
        :data="state.selKv"
        check-strictly
        placeholder="请选择"
        class="commonInput"
        :render-after-expand="false"/> -->
      <!-- <el-select class="commonInput" v-model="state.query.orgId"
      :options="state.selKv" :props="props" placeholder="请选择"/> -->
      <el-button type="primary" :icon="Search" class="btnStl" @click="getList">查询</el-button>
      <el-button type="danger" :icon="Plus" class="btnStl" @click="btnAdd">新增</el-button>
    </div>
    <div class="pg_grid">
      <el-table :data="state.tbData" row-key="id" default-expand-all
                style="display: flex; flex: 1;width: auto; height: 100%; flex-wrap: wrap; flex-shrink: 1;">
          <el-table-column type="selection" width="55" />
          <!-- <el-table-column label="Date" width="120">
            <template #default="scope">{{ scope.row.date }}</template>
          </el-table-column> -->
          <!-- <el-table-column property="id" label="ID" width="120" /> -->
          <el-table-column property="name" label="名称" width="257" />
          <el-table-column property="code" label="编码" width="80" />
          <el-table-column property="menuType" label="菜单类型" width="100">
            <template #default="scope">
              <div style="display: flex; align-items: center">
                <span v-if="scope.row.menuType==1">模块</span>
                <span v-else-if="scope.row.menuType==2">分组</span>
                <span v-else-if="scope.row.menuType==3">功能</span>
                <span v-else-if="scope.row.menuType==4">按钮</span>
                <span v-else-if="scope.row.menuType==5">链接</span>
              </div>
            </template>
          </el-table-column>
            <el-table-column property="icon" label="图标" width="168" show-overflow-tooltip/>
          <el-table-column property="iconSize" label="图标大小" width="111"/>
          <el-table-column property="url" label="路由地址" width="156"/>
          <el-table-column property="sort" label="排序" width="80"/>
          <el-table-column property="status" label="状态" width="80">
            <template #default="scope">
              <div style="display: flex; align-items: center">
                <span v-if="scope.row.status==1">正常</span>
                <span v-else-if="scope.row.status==0">禁用</span>
                <span v-else-if="scope.row.status==2">隐藏</span>
              </div>
            </template>
          </el-table-column>
          <!-- <el-table-column property="sex" label="性别" width="88">
            <template #default="scope">
              <div style="display: flex; align-items: center">
                <span v-if="scope.row.sex==1">男</span>
                <span v-else>女</span>
              </div>
            </template>
          </el-table-column> -->
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
  <el-form ref="form" :model="state.opt" label-width="auto" style="width: 100%;">
    <el-form-item label="名称" placeholder="请输入名称">
      <el-input v-model="state.opt.name" />
    </el-form-item>
    <el-form-item label="编码" placeholder="请输入编码">
      <el-input v-model="state.opt.code" />
    </el-form-item>
    <el-form-item label="图标" placeholder="请输入图标">
      <el-input v-model="state.opt.icon" />
    </el-form-item>
    <el-form-item label="图标大小" placeholder="请输入图标大小">
      <el-input v-model="state.opt.iconSize" />
    </el-form-item>
    <el-form-item label="路由地址" placeholder="请输入图标路径">
      <el-input v-model="state.opt.path" />
    </el-form-item>
    <el-form-item label="路由地址" placeholder="请输入路由地址">
      <el-input v-model="state.opt.path" />
    </el-form-item>
    <el-form-item label="菜单类型" placeholder="请选择菜单类型">
      <el-select :options="state.menuTypeKv" v-model="state.opt.menuType" placeholder="请选择"/>
    </el-form-item>
    <el-form-item label="状态" placeholder="请选择状态">
      <el-select :options="state.statusKv" v-model="state.opt.status" placeholder="请选择"/>
    </el-form-item>
    <el-form-item label="排序" placeholder="请输入排序">
      <el-input v-model="state.opt.sort" />
    </el-form-item>
  </el-form>
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
import { Search, Plus } from '@element-plus/icons-vue'
import { ElMessage, ElMessageBox } from 'element-plus'
import type { selKV } from '@/models/common/selectKV';
import type { menuModel, menuQueryModel, menuOptModel } from '@/models/sys/menuModel';

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
  query:menuQueryModel,
  menuTypeKv: selKV[],
  statusKv: selKV[],
  opt: menuOptModel,
  tbData: menuModel[]
}>({
  loading: false,
  dlgTitle: '',
  dlgVisible:false,
  query: {
    name: ''
  },
  menuTypeKv: [
    {
      label: '请选择',
      value: ''
    },
    {
      label: '模块',
      value: '1'
    },
    {
      label: '分组',
      value: '2'
    },
    {
      label: '功能',
      value: '3'
    }
  ],
  statusKv: [
    {
      label: '请选择',
      value: ''
    },
    {
      label: '正常',
      value: '1'
    },
    {
      label: '禁用',
      value: '0'
    },
    {
      label: '隐藏',
      value: '2'
    }
  ],
  tbData: [],
  opt: {
    id: '',
    name: '',
    parentId: '',
    menuType: '',
    code: '',
    path: '',
    icon: '',
    iconSize: '',
    status: '',
    sort: 0
  }
})

const getList = () => {
  systemReq.axiosIns.get('api/sys/SysMenu/getMenuTree', { params:state.query })
  .then((res:any) => {
    console.log(res.Data)
    state.tbData = res.Data
  })
  .catch((err:any)=>{
    console.log(err)
  })
}

const btnAdd = () => {
  state.dlgTitle = '新增菜单'
  state.dlgVisible = true
}

const btnEdit = (id:string) => {
  state.dlgTitle = '编辑菜单'
  state.dlgVisible = true
  console.log(id)
}

const btnDel = (id:string) => {
  console.log(id)
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
