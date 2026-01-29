<script setup lang="ts">
import { RouterView } from 'vue-router'
// import { ref } from 'vue'
// import {
//   getMd5,
//   str2Hex,
//   hex2Str,
//   getBase64,
//   base642Str,
//   encryptAes,
//   decryptAes,
//   sha256,
// } from './utils/pwdutil'

// const pwd1 = ref(getMd5('666pwd666'))

// const pwd2 = ref('')
// pwd2.value = str2Hex('888pwd888')

// const pwd3 = ref('')
// pwd3.value = hex2Str(pwd2.value)

// const pwd4 = ref('')
// pwd4.value = getBase64('999pwd999')

// const pwd5 = ref('')
// pwd5.value = base642Str(pwd4.value)

// const pwd6 = ref('')
// pwd6.value = encryptAes('123pwd123', '666666')

// const pwd7 = ref('')
// pwd7.value = decryptAes(pwd6.value, '666666')

// const pwd8 = ref('')
// pwd8.value = sha256('666pwd888')
</script>

<template>
  <RouterView />
  <!-- <h1>You did it!</h1>
  <p>
    Visit <a href="https://vuejs.org/" target="_blank" rel="noopener">vuejs.org</a> to read the
    <br />
    {{ pwd1 }}
    <br />
    {{ pwd2 }}
    <br />
    {{ pwd3 }}
    <br />
    {{ pwd4 }}
    <br />
    {{ pwd5 }}
    <br />
    {{ pwd6 }}
    <br />
    {{ pwd7 }}
    <br />
    {{ pwd8 }}
  </p> -->
</template>

<style scoped></style>
