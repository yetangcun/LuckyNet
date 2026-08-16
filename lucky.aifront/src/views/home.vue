<template>
  <div class="pg_top">
    <div style="display: flex; flex:1; justify-content: center; align-items: center; color: #3964fe;">
    <h1>Home</h1>
    </div>
  </div>
</template>

<script setup lang="ts">
import { reactive } from 'vue'

const state = reactive<{count: number}>({
  count: 0
})

console.log(state.count)

</script>

<style scoped>
</style>
