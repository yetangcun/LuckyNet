<template>
  <div style="display: flex; flex: 1; border: 1px solid #eee; width: 100%; height: 100%;">
      <div style="display: flex; flex-direction: column; border-right: 1px solid #eee;">
          <div style="display: flex; max-height: 60px; min-height: 60px; justify-content: space-between; align-items: center; padding: 0px 10px; border-bottom: 1px solid #eee; min-width: 169px;">
            <label style="margin-right: 16px;">菜单树</label>
            <el-button type="primary" :icon="Finished" class="btnStl" @click="btnRelate">保存</el-button>
          </div>
          <div style="display: flex; flex: 1; min-height: 0; overflow: auto;">
            <el-tree
              ref="treeRef"
              :data="state.pData"
              show-checkbox
              node-key="value"
              default-expand-all
              :props="dftProps"
            />
          </div>
      </div>
      <div style="display: flex; flex:1;flex-direction: column; min-width: 0;">
        <div  style="display: flex;
        max-height: 60px;
        min-height: 60px;
        flex-wrap: wrap;
        align-items: center;
        justify-content: flex-start;">
          <el-text class="lbStl">关键字:</el-text>
          <el-input class="commonInput" v-model="state.query.name" placeholder="名称|标识"></el-input>
          <el-button type="primary" :icon="Search" class="btnStl" @click="getList">查询</el-button>
          <el-button type="danger" :icon="Plus" @click="btnAdd" class="btnStl">新增</el-button>
        </div>
        <div style="display: flex; flex: 1; border-top: 1px solid #eee; flex-direction: column; min-height: 0;">
          <div class="grid_div">
            <el-table :data="state.tbData" row-key="id" @row-click="handleSelectionChange" height="100%">
                <el-table-column type="selection" width="55" />
                <!-- <el-table-column label="Date" width="120">
                  <template #default="scope">{{ scope.row.date }}</template>
                </el-table-column> -->
                <el-table-column property="name" label="名称" width="120" />
                <!-- <el-table-column property="sex" label="英文名" width="88">
                  <template #default="scope">
                    <div style="display: flex; align-items: center">
                      <span v-if="scope.row.sex==1">男</span>
                      <span v-else>女</span>
                    </div>
                  </template>
                </el-table-column> -->
                <el-table-column property="word" label="标识" width="120" show-overflow-tooltip />
                <el-table-column property="roleType" label="类型" width="120">
                  <template #default="scope">
                    <div style="display: flex; align-items: center">
                      <span v-if="scope.row.roleType==1">管理员</span>
                      <span v-else-if="scope.row.roleType==2">普通用户</span>
                      <span v-else-if="scope.row.roleType==3">超级管理员</span>
                    </div>
                  </template>
                </el-table-column>
                <el-table-column property="status" label="状态" width="120">
                  <template #default="scope">
                    <div style="display: flex; align-items: center">
                      <span v-if="scope.row.status==1">正常</span>
                      <span v-else-if="scope.row.status==0">禁用</span>
                    </div>
                  </template>
                </el-table-column>
                <el-table-column property="sort" label="排序" width="86"/>
                <el-table-column property="remark" label="备注" width="120" show-overflow-tooltip />

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
          :size="'default'"
          :background="true"
          :total="state.query.total"
          @size-change="hdlSizeChange"
          v-model:current-page="state.query.pageIndex"
          v-model:page-size="state.query.pageSize"
          :page-sizes="[20, 50, 100, 300, 500, 1000]"
          style="display: flex; justify-content: flex-end; margin: 10px 10px 10px 1px;"
          layout="total, sizes, prev, pager, next"
          @current-change="hdlCurrentChange"
        />
        </div>
      </div>
  </div>
  <el-dialog
    v-model="state.dlgVisible"
    :title="state.dlgTitle"
    width="500"
    draggable
    overflow
  >
      <el-form ref="formRef" :model="state.opt" label-width="80px">
        <el-form-item label="名称">
          <el-input v-model="state.opt.name" placeholder="请输入名称"></el-input>
        </el-form-item>
        <el-form-item label="昵称">
          <el-input v-model="state.opt.word" placeholder="请输入昵称"></el-input>
        </el-form-item>
        <el-form-item label="类型">
          <el-select v-model="state.opt.roleType" placeholder="请选择类型">
            <el-option v-for="item in state.roleType" :key="item.value" :label="item.label" :value="item.value"></el-option>
          </el-select>
        </el-form-item>
        <el-form-item label="状态">
          <el-select v-model="state.opt.status" placeholder="请选择状态">
            <el-option v-for="item in state.status" :key="item.value" :label="item.label" :value="item.value"></el-option>
          </el-select>
        </el-form-item>
        <el-form-item label="排序">
          <el-input v-model="state.opt.sort" placeholder="请输入排序"></el-input>
        </el-form-item>
        <el-form-item label="备注">
          <el-input v-model="state.opt.remark" placeholder="请输入备注"></el-input>
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
import { onMounted, reactive, ref } from 'vue';
import { Search, Plus, Finished } from '@element-plus/icons-vue'
import { ElButton, ElMessage, ElMessageBox } from 'element-plus'
import type { TreeInstance} from 'element-plus'
import type { selNumKV } from '@/models/common/selectKV'
import type { PermissionQueryModel, PermissionModel, PermissionOptModel } from '@/models/sys/permissionModel'

import { systemReq } from '@/utils/reqUtil';

const dftProps = {
  children: 'children',
  label: 'label'
}

const treeRef = ref<TreeInstance>()

const state = reactive<{
  loading:boolean,
  dlgTitle:string,
  dlgVisible:boolean,
  query:PermissionQueryModel,
  opt: PermissionOptModel,
  pData: any[],
  selRoleId: string,
  tbData: PermissionModel[],
  roleType: selNumKV[],
  status: selNumKV[]
}>({
  loading: false,
  dlgTitle: '',
  dlgVisible:false,
  selRoleId: '',
  query: {
    name: '',
    pageIndex: 1,
    pageSize: 20,
    total: 0
  },
  status: [
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
  roleType: [
    {
      label: '请选择',
      value: -1
    },
    {
      label: '管理员',
      value: 1
    },
    {
      label: '普通用户',
      value: 2
    },
    {
      label: '超级管理员',
      value: 3
    }
  ],
  pData:[
    {
      value: 1,
      label: '系统管理',
      children: [
        {
          value: 2,
          label: '用户管理'
        },
        {
          value: 3,
          label: '角色管理'
        }
      ]
    },
    {
      value: 4,
      label: '业务管理',
      children: [
        {
          value: 5,
          label: '订单管理'
        },
        {
          value: 6,
          label: '商品管理'
        }
      ]
    }
  ],
  tbData: [],
  opt: {
    id: '',
    name: '',
    word: '',
    roleType: -1,
    status: -1,
    sort: 0,
    remark: ''
  }
})

const getList = () => {
  systemReq.axiosIns.get('api/sys/SysRole/pages', { params:state.query })
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
  state.dlgTitle = '新增角色'
  state.dlgVisible = true
  state.opt = {
    id: '',
    name: '',
    word: '',
    roleType: -1,
    status: 1,
    sort: 0,
    remark: ''
  }
}

const handleSelectionChange = (val:PermissionModel) => {
  state.selRoleId = val.id
  systemReq.axiosIns.get(`api/sys/SysRole/getRoleMenus/${state.selRoleId}`)
  .then((res:any) => {
    // console.log(res.Data)
    treeRef.value?.setCheckedKeys(res.Data)
  })
  .catch((err:any)=>{
    console.log(err)
  })
}

const btnEdit = (id:string) => {
  state.dlgTitle = '编辑角色'
  state.dlgVisible = true
  // console.log(id)
  systemReq.axiosIns.get(`api/sys/SysRole/${id}`)
  .then((res:any) => {
    console.log(res.Data)
    state.opt = res.Data
  })
  .catch((err:any)=>{
    console.log(err)
  })
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
      systemReq.axiosIns.delete(`api/sys/SysRole/${id}`)
      .then((res:any) => {
        console.log(res.Data)
        getList()
      })
      .catch((err:any)=>{
        console.log(err)
      })
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

const btnCancel = () => {
  state.dlgVisible = false
}

const hdlSizeChange = (val:number) => {
  state.query.pageSize = val
  getList()
}

const hdlCurrentChange = (val:number) => {
  state.query.pageIndex = val
  getList()
}

const btnSave = () => {
  state.dlgVisible = false
  if (!state.opt.id) {
    state.opt.id = '0'
    systemReq.axiosIns.post('api/sys/SysRole', state.opt)
    .then((res:any) => {
      console.log(res.Data)
      getList()
    })
    .catch((err:any)=>{
      console.log(err)
    })
  }
  else {
    systemReq.axiosIns.put('api/sys/SysRole', state.opt)
    .then((res:any) => {
      console.log(res.Data)
      getList()
    })
    .catch((err:any)=>{
      console.log(err)
    })
  }
}

const btnRelate = () => {
  const selKeys = treeRef.value?.getCheckedKeys()
  console.log(selKeys)
  if (selKeys && state.selRoleId) {
    systemReq.axiosIns.post(`api/sys/SysRole/setRoleMenus`, {roleId: state.selRoleId, menuIds: selKeys})
    .then((res:any) => {
      console.log(res.Data)
      ElMessage({
        type: 'success',
        message: '设置成功',
      })
    })
    .catch((err:any)=>{
      console.log(err)
    })
  }
}

const getMenuTree = () => {
  systemReq.axiosIns.get('api/sys/SysMenu/getMenuSelTree')
  .then((res:any) => {
    console.log(res.Data)
    state.pData = res.Data
  })
  .catch((err:any)=>{
    console.log(err)
  })
}

onMounted(() => {
  getMenuTree()
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

.pTreeStl {
  display: flex;
  flex: 1;
  height: 100%;
  background-color: red;
}

</style>
