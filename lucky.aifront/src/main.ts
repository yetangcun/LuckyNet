import { createApp } from 'vue'
import ElementPlus from 'element-plus'
import { createPinia } from 'pinia'
import router from './router'
import App from './App.vue'
import zhCn from 'element-plus/es/locale/lang/zh-cn'

import 'element-plus/dist/index.css'
import '@/assets/iconfonts/iconfont.css'
import './assets/main.css'

// import './assets/iconfonts/iconfont.js'

const app = createApp(App)

app.use(ElementPlus, {
  locale: zhCn,
})

// app.use(ElementPlus)
app.use(createPinia())
app.use(router)

app.mount('#app')
