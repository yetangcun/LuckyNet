<template>
  <div style="display: flex; flex: 1; width: 100%; height: 100%; flex-direction: column;">
    <div v-for="menu in menus" :key="menu.code">
      <div v-if="menu.menu_type==1 || menu.menu_type==2"> <!-- 模块、分组 -->
        <div v-if="menu.childs && menu.childs.length>0">
          <div class="menuStl" @click="menu.isExpand=!menu.isExpand">
            <div class="menuPartStl">
              <span :class="'iconfont '+ menu.icon" :style="{fontSize:menu.icon_size+'px',marginTop:'2px', marginRight:'6px'}"></span>
              <span>{{ menu.name }}</span>
            </div>
            <span :class="menu.isExpand?'iconfont icon-arrow-down':'iconfont icon-arrow-right'" :style="{fontSize:menu.icon_size+'px',marginTop:'2px',display:'flex'}"></span>
          </div>
          <div v-show="menu.isExpand">
            <div v-for="child in menu.childs" :key="child.code">
              <div v-if="child.menu_type==1 || child.menu_type==2"> <!-- 模块、分组 -->
                <div v-if="child.childs && child.childs.length>0">
                  <div class="menuStl">
                    <div class="menuPartStl">
                      <span :class="'iconfont '+ child.icon" :style="{fontSize:child.icon_size+'px',marginTop:'2px', marginRight:'6px'}"></span>
                      <span>{{ child.name }}</span>
                    </div>
                    <span :class="child.isExpand?'iconfont icon-arrow-down':'iconfont icon-arrow-right'" :style="{fontSize:child.icon_size+'px',marginTop:'2px',display:'flex'}"></span>
                  </div>
                  <div v-show="child.isExpand">
                    <div v-for="chr in child.childs" :key="chr.code">
                      <div class="menuPartStl">
                        <span :class="'iconfont '+ chr.icon" :style="{fontSize:chr.icon_size+'px',marginTop:'2px'}"></span>
                        <span>{{ chr.name }}</span>
                      </div>
                    </div>
                  </div>
                </div>
                <div v-else>
                  <div class="menuPartStl">
                    <span :class="'iconfont '+ child.icon" :style="{fontSize:child.iconSize+'px'}"></span>
                    <span>{{ child.name }}</span>
                  </div>
                </div>
              </div>
              <div v-else-if="child.menu_type==3">
                <div class="menuPartStl">
                  <span :class="'iconfont '+ child.icon" :style="{fontSize:child.icon_size+'px',marginTop:'2px'}"></span>
                  <span>{{ child.name }}</span>
                </div>
              </div>
            </div>
          </div>
        </div>
        <div v-else>
          <div class="menuPartStl">
            <span :class="'iconfont '+ menu.icon" :style="{fontSize:menu.icon_size+'px',marginTop:'2px'}"></span>
            <span>{{ menu.name }}</span>
          </div>
        </div>
      </div>
      <div v-else-if="menu.menu_type==3">
          <div class="menuPartStl">
            <span :class="'iconfont '+ menu.icon" :style="{fontSize:menu.icon_size+'px',marginTop:'2px'}"></span>
            <span>{{ menu.name }}</span>
          </div>
      </div>
    </div>
  </div>
</template>

<script lang="ts" setup>
const props = defineProps({
  menus: Object,
  isExpand: Boolean
})

console.log(props.isExpand)

</script>

<style scoped>

 .menuStl {
  display: flex;
  width: 100%;
  justify-content: space-between;
  align-items: center;
  color: white;
  font-size: 17px;
  cursor: pointer;
  margin-right: 10px;
 }

 .menuPartStl {
  display: flex;
  justify-content: center;
  align-items: center;
  color: white;
  font-size: 18px;
  padding: 10px 0px;
 }

</style>
