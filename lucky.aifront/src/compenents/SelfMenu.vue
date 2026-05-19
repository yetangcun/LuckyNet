<template>
  <div v-for="menu in menus" :key="menu.code">
    <div v-if="menu.menu_type==1 || menu.menu_type==2">
      <div v-if="menu.childs && menu.childs.length>0">
        <div v-for="child in menu.childs" :key="child.code">

        </div>
      </div>
      <div v-else></div>
    </div>
    <div v-else-if="menu.menu_type==3">

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
