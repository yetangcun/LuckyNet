// import { createRouter, createWebHistory } from 'vue-router'

// const router = createRouter({
//   history: createWebHistory(import.meta.env.BASE_URL),
//   routes: [],
// })

// export default router

import { createRouter, createWebHistory } from 'vue-router'
import sysrouter from './module/sysrouter'
import airouter from './module/airouter'

const routers = sysrouter.concat(airouter) // 组合路由集合

// console.log(routers)

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      name: 'login',
      component: () => import('../views/login.vue'),
    },
    {
      path: '/index',
      name: 'index',
      component: () => import('../views/main.vue'),
      children: routers,
    },
  ],
})

export default router
