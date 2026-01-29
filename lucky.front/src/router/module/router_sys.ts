const sysrouter = [
  {
    path: '/sys/home',
    name: 'sys_home',
    component: () => import('@/views/home.vue'),
    meta: {
      title: '首页',
      keepAlive: false,
      icon: 'el-icon-s-platform',
    },
  },
  {
    path: '/sys/user',
    name: 'sys_user',
    component: () => import('../../views/sys/user.vue'),
    meta: {
      keepAlive: false,
      title: '主页',
      icon: 'el-icon-s-platform',
    },
  },
  // {
  //   path: '/sys/employee',
  //   name: 'sys_employee',
  //   component: () => import('../../views/sys/employee.vue'),
  //   meta: {
  //     keepAlive: false,
  //   },
  // },
  // {
  //   path: '/sys/menu',
  //   name: 'sys_menu',
  //   component: () => import('../../views/sys/menu.vue'),
  //   meta: {
  //     keepAlive: true,
  //   },
  // },
  // {
  //   path: '/sys/permission',
  //   name: 'sys_permission',
  //   component: () => import('../../views/sys/permission.vue'),
  //   meta: {
  //     keepAlive: true,
  //   },
  // },
  // {
  //   path: '/sys/setting',
  //   name: 'sys_setting',
  //   component: () => import('../../views/sys/setting.vue'),
  //   meta: {
  //     keepAlive: true,
  //   },
  // },
  // {
  //   path: '/sys/organization',
  //   name: 'sys_organization',
  //   component: () => import('../../views/sys/organization.vue'),
  //   meta: {
  //     keepAlive: true,
  //   },
  // },
  // {
  //   path: '/sys/loginlog',
  //   name: 'sys_loginlog',
  //   component: () => import('../../views/sys/loginlog.vue'),
  //   meta: {
  //     keepAlive: true,
  //   },
  // },
]

export default sysrouter
