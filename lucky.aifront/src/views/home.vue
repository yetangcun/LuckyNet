<template>
  <div class="pg_top">
    <h1>Home</h1>
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
