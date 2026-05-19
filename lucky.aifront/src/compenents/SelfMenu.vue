<template>
  <div v-for="menu in menus" :key="menu.code">
    <div v-if="menu.menu_type==1 || menu.menu_type==2"> <!-- 模块、分组 -->
      <div v-if="menu.childs && menu.childs.length>0">
        <div class="menuStl">

        </div>
        <div v-for="child in menu.childs" :key="child.code">
          <div v-if="child.menu_type==1 || child.menu_type==2"> <!-- 模块、分组 -->
            <div v-if="child.childs && child.childs.length>0">
              <div class="menuStl">

              </div>
              <div v-for="chr in child.childs" :key="chr.code">

              </div>
            </div>
            <div v-else>

            </div>
          </div>
          <div v-else-if="child.menu_type==3">
            <div class="menuStl">

            </div>
          </div>
        </div>
      </div>
      <div v-else>

      </div>
    </div>
    <div v-else-if="menu.menu_type==3">
        <div class="menuStl">

        </div>
    </div>
  </div>
</template>

<script lang="ts" setup>
const props = defineProps({
  menus: [],
  isExpand: Boolean
})

console.log(props.isExpand)

</script>

<style scoped>

 .menuStl {
  display: flex;
  justify-content: center;
  align-items: center;
 }

</style>
