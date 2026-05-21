<template>
  <div style="display: flex; flex: 1; width: 100%; height: 100%; flex-direction: column;">
    <div v-for="menu in menus" :key="menu.code">
      <div v-if="menu.menu_type==1 || menu.menu_type==2"> <!-- 模块、分组 -->
        <div v-if="menu.childs && menu.childs.length>0">
          <div class="menuStl" :style="{justifyContent:isExpand?'space-between':'center'}" @click="menu.isExpand=!menu.isExpand">
            <div class="menuPartStl">
              <span :class="'iconfont '+ menu.icon" :style="{fontSize:menu.icon_size+'px',margin:'2px 6px 0px 8px'}"></span>
              <span v-show="isExpand">{{ menu.name }}</span>
            </div>
            <span v-show="isExpand" :class="menu.isExpand?'iconfont icon-arrow-down':'iconfont icon-arrow-right'" :style="{fontSize:menu.icon_size+'px',marginTop:'2px',display:'flex', opacity:'0.7'}"></span>
          </div>
          <div v-show="menu.isExpand" class="menuChildPanel">
            <div v-for="child in menu.childs" :key="child.code">
              <div v-if="child.menu_type==1 || child.menu_type==2"> <!-- 模块、分组 -->
                <div v-if="child.childs && child.childs.length>0">
                  <div class="menuChildStl" @click="child.isExpand=!child.isExpand">
                    <div class="menuChildPartStl">
                      <span :class="'iconfont '+ child.icon" :style="{fontSize:child.icon_size+'px',margin:'2px 6px 0px 8px'}"></span>
                      <span v-show="isExpand">{{ child.name }}</span>
                    </div>
                    <span v-show="isExpand" :class="child.isExpand?'iconfont icon-arrow-down':'iconfont icon-arrow-right'" :style="{fontSize:child.icon_size+'px',marginTop:'2px',display:'flex', opacity:'0.7'}"></span>
                  </div>
                  <div v-show="child.isExpand" class="menuChildPanel">
                    <div v-for="chr in child.childs" :key="chr.code">
                      <div class="menuChildPartStl" @click="to_pg(chr)">
                        <span :class="'iconfont '+ chr.icon" :style="{fontSize:chr.icon_size+'px',margin:'2px 6px 0px 20px'}"></span>
                        <span v-show="isExpand">{{ chr.name }}</span>
                      </div>
                    </div>
                  </div>
                </div>
                <div v-else>
                  <div class="menuChildPartStl">
                    <span :class="'iconfont '+ child.icon" :style="{fontSize:child.iconSize+'px',margin:'2px 6px 0px 8px'}"></span>
                    <span v-show="isExpand">{{ child.name }}</span>
                  </div>
                </div>
              </div>
              <div v-else-if="child.menu_type==3">
                <div class="menuChildPartStl" @click="to_pg(child)">
                  <span :class="'iconfont '+ child.icon" :style="{fontSize:child.icon_size+'px',margin:'2px 6px 0px 8px'}"></span>
                  <span v-show="isExpand">{{ child.name }}</span>
                </div>
              </div>
            </div>
          </div>
        </div>
        <div v-else>
          <div class="menuPartStl">
            <span :class="'iconfont '+ menu.icon" :style="{fontSize:menu.icon_size+'px',margin:'2px 6px 0px 8px'}"></span>
            <span v-show="isExpand">{{ menu.name }}</span>
          </div>
        </div>
      </div>
      <div v-else-if="menu.menu_type==3">
          <div class="menuPartStl" @click="to_pg(menu)">
            <span :class="'iconfont '+ menu.icon" :style="{fontSize:menu.icon_size+'px',margin:'2px 6px 0px 8px'}"></span>
            <span v-show="isExpand">{{ menu.name }}</span>
          </div>
      </div>
    </div>
  </div>
</template>

<script lang="ts" setup>
import { useRouter } from 'vue-router';
import type { menuModel } from '@/models/sys/menuModel'
const router = useRouter()

const props = defineProps({
  menus: Object,
  isExpand: Boolean
})

const to_pg = (obj: menuModel) => {
  router.push(obj.path)
}

console.log(props.isExpand)

</script>

<style scoped>

 .menuStl {
  display: flex;
  width: 100%;
  align-items: center;
  color: white;
  font-size: 18px;
  cursor: pointer;
 }

 .menuPartStl {
  display: flex;
  justify-content: flex-start;
  align-items: center;
  color: white;
  font-size: 18px;
  padding: 10px 0px;
 }

 .menuChildStl {
  display: flex;
  width: 100%;
  justify-content: space-between;
  align-items: center;
  color: white;
  font-size: 16px;
  cursor: pointer;
 }

 .menuChildPartStl {
  display: flex;
  justify-content: flex-start;
  align-items: center;
  color: white;
  font-size: 16px;
  cursor: pointer;
  padding: 10px 0px 8px 18px;
 }

.menuChildPanel {
  display: flex;
  flex-direction: column;
  background-color: cornflowerblue;
}
</style>
