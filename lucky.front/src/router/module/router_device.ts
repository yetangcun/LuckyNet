const devicerouter = [
  {
    path: '/device',
    name: 'device',
    component: () => import('@/views/device/index.vue'),
    meta: {
      title: '设备管理',
      keepAlive: false,
      icon: 'el-icon-s-platform',
      // roles: ['admin', 'editor'],
    },
  },
]

export default devicerouter
