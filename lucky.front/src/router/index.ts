import { createRouter, createWebHistory } from 'vue-router'
import devicerouter from './module/router_device'
import sysrouter from './module/router_sys'

const childs = sysrouter.concat(devicerouter) // 组合路由集合

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      name: 'login',
      component: () => import('@/views/login.vue'),
    },
    {
      path: '/index',
      name: 'index',
      component: () => import('@/views/main.vue'),
      children: childs,
    },
  ],
})

export default router
