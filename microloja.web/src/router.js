import { createRouter, createWebHistory } from 'vue-router';
import Inicio from './pages/Inicio.vue';
import Categoria from './pages/Categoria.vue';

const routes = [
  { 
    path: '/', 
    name: 'Inicio',
    component: Inicio 
  },
  { 
    path: '/categoria/:categoriaId', 
    name: 'Categoria',
    component: Categoria, 
    props: true 
  },
  { 
    path: '/pesquisa', 
    name: 'ResultadosPesquisa',
    component: Inicio, 
    props: route => ({ busca: route.query.q })
  },
  { 
    path: '/produto/:id', 
    name: 'DetalhesProduto',
    component: () => import('./pages/DetalhesProduto.vue'),
    props: true 
  },
  { 
    path: '/carrinho', 
    name: 'Carrinho',
    component: () => import('./pages/Carrinho.vue')
  },
  { 
    path: '/favoritos', 
    name: 'Favoritos',
    component: () => import('./pages/Favoritos.vue')
  },
  { 
    path: '/perfil', 
    name: 'Perfil',
    component: () => import('./pages/Perfil.vue')
  },
  { 
    path: '/pedidos', 
    name: 'Pedidos',
    component: () => import('./pages/Pedidos.vue')
  }
];

export default createRouter({
  history: createWebHistory(),
  routes,
});
