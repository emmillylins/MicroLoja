import { createRouter, createWebHistory } from 'vue-router';
import PaginaPrincipal from './pages/PaginaPrincipal.vue';

const routes = [
  { 
    path: '/', 
    name: 'Inicio',
    component: PaginaPrincipal 
  },
  { 
    path: '/categoria/:categoriaId', 
    name: 'ProdutosPorCategoria',
    component: PaginaPrincipal, 
    props: true 
  },
  { 
    path: '/pesquisa', 
    name: 'ResultadosPesquisa',
    component: PaginaPrincipal, 
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
