<template>
  <div style="display: flex; flex: 1; width: 100%; height: 100%; flex-direction: column;">
    <div v-for="menu in menus" :key="menu.code">
      <div v-if="menu.menuType==1 || menu.menuType==2"> <!-- 模块、分组 -->
        <div v-if="menu.children && menu.children.length>0">
          <div class="menuStl" :style="{justifyContent:isExpand?'space-between':'center'}" @click="expandClose(menu)">
            <div class="menuPartStl">
              <span :class="'iconfont '+ menu.icon" :style="{fontSize:menu.iconSize+'px',margin:'2px 6px 0px 8px'}"></span>
              <span v-show="isExpand">{{ menu.name }}</span>
            </div>
            <span v-show="isExpand" :class="menu.isExpand?'iconfont icon-arrow-down':'iconfont icon-arrow-right'" :style="{fontSize:menu.iconSize+'px',marginTop:'2px',display:'flex', opacity:'0.7'}"></span>
          </div>
          <div v-show="menu.isExpand" class="menuChildPanel">
            <div v-for="child in menu.children" :key="child.code">
              <div v-if="child.menuType==1 || child.menuType==2"> <!-- 模块、分组 -->
                <div v-if="child.children && child.children.length>0">
                  <div class="menuChildStl" @click="expandClose(child)">
                    <div class="menuChildPartStl">
                      <span :class="'iconfont '+ child.icon" :style="{fontSize:child.iconSize+'px',margin:'2px 6px 0px 8px'}"></span>
                      <span v-show="isExpand">{{ child.name }}</span>
                    </div>
                    <span v-show="isExpand" :class="child.isExpand?'iconfont icon-arrow-down':'iconfont icon-arrow-right'" :style="{fontSize:child.iconSize+'px',marginTop:'2px',display:'flex', opacity:'0.7'}"></span>
                  </div>
                  <div v-show="child.isExpand" class="menuChildPanel">
                    <div v-for="chr in child.children" :key="chr.code">
                      <div :class="chr.isSelect?'menuChildPartStlSel':'menuChildPartStl'" @click="$emit('toPg', chr)">
                        <span :class="'iconfont '+ chr.icon" :style="{fontSize:chr.iconSize+'px',margin:'2px 6px 0px 20px'}"></span>
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
              <div v-else-if="child.menuType==3">
                <div :class="child.isSelect?'menuChildPartStlSel':'menuChildPartStl'" @click="$emit('toPg', child)">
                  <span :class="'iconfont '+ child.icon" :style="{fontSize:child.iconSize+'px',margin:'2px 6px 0px 8px'}"></span>
                  <span v-show="isExpand">{{ child.name }}</span>
                </div>
              </div>
            </div>
          </div>
        </div>
        <div v-else>
          <div class="menuPartStl">
            <span :class="'iconfont '+ menu.icon" :style="{fontSize:menu.iconSize+'px',margin:'2px 6px 0px 8px'}"></span>
            <span v-show="isExpand">{{ menu.name }}</span>
          </div>
        </div>
      </div>
      <div v-else-if="menu.menuType==3">
          <div :class="menu.isSelect?'menuPartStlSel':'menuPartStl'" @click="$emit('toPg', menu)">
            <span :class="'iconfont '+ menu.icon" :style="{fontSize:menu.iconSize+'px',margin:'2px 6px 0px 8px'}"></span>
            <span v-show="isExpand">{{ menu.name }}</span>
          </div>
      </div>
    </div>
  </div>
</template>

<script lang="ts" setup>
import type { menuModel } from '@/models/sys/menuModel'

const prps = defineProps({
  menus: Object,
  isExpand: Boolean
})

const expandClose = (obj: menuModel) => {
  const sourceState = obj.isExpand
  if (prps.menus) {
    if (obj.parentId=='0') {
      prps.menus.forEach((e:menuModel)=>{
        e.isExpand = false
      });
      if (sourceState) return
    }
    else {
      const pid = obj.parentId
      let tmp:menuModel = undefined as any
      prps.menus.forEach((e:menuModel)=>{
        if (pid == e.id) {
          tmp = e
          return
        }
      })

      if (tmp && tmp.children && tmp.children.length>0) {
        tmp.children.forEach((e:menuModel)=>{
          e.isExpand = false
        })

        if (sourceState) return
      }
    }
  }
  obj.isExpand = !obj.isExpand
}

</script>

<style scoped>

 .menuStl {
  display: flex;
  width: 100%;
  align-items: center;
  color: white;
  font-size: 18px;
  cursor: pointer;
  margin-top: 4px;
  box-shadow: 0px 0px 14px 0px cornflowerblue inset;
  /* border-bottom: 1px solid cornflowerblue; */
 }

 .menuPartStl {
  display: flex;
  justify-content: flex-start;
  align-items: center;
  color: white;
  font-size: 18px;
  padding: 10px 0px;
 }

 .menuPartStlSel {
  display: flex;
  justify-content: flex-start;
  align-items: center;
  color: #3964fe;
  font-size: 18px;
  padding: 10px 0px;
  background-color: white;
 }

 .menuChildStl {
  display: flex;
  width: 100%;
  justify-content: space-between;
  align-items: center;
  color: white;
  font-size: 16px;
  cursor: pointer;
  margin-top: 4px;
  box-shadow: 0px 0px 14px 0px cornflowerblue inset;
  /* border-bottom: 1px solid cornflowerblue; */
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

 .menuChildPartStlSel {
  display: flex;
  justify-content: flex-start;
  align-items: center;
  color: #3964fe;
  font-size: 16px;
  cursor: pointer;
  background-color: white;
  padding: 10px 0px 8px 18px;
 }

.menuChildPanel {
  display: flex;
  flex-direction: column;
  background-color: cornflowerblue;
}
</style>
