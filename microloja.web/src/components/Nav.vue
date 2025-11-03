<template>
  <nav class="navbar navbar-expand-lg navbar-light bg-white shadow-sm sticky-top">
    <!-- Seção principal da navegação -->
    <div class="container-fluid px-4 d-flex justify-content-between align-items-center">
      <!-- Logo na esquerda -->
      <router-link to="/" class="navbar-brand d-flex align-items-center">
        <span class="fs-1 me-2">🛍️</span>
        <span class="fw-bold text-primary fs-3">MicroLoja</span>
      </router-link>

      <!-- Barra de pesquisa no centro -->
      <div class="flex-grow-1 d-flex justify-content-center mx-5">
        <div class="d-flex align-items-center gap-3" style="width: 100%; max-width: 700px;">
          <div class="input-group flex-grow-1">
            <input 
              type="text" 
              class="form-control" 
              placeholder="Pesquisar produtos..."
              v-model="termoPesquisa"
              @keyup.enter="pesquisar"
            >
            <button 
              class="btn btn-outline-primary" 
              type="button"
              @click="pesquisar"
            >
              <i class="bi bi-search"></i>
            </button>
          </div>
          
          <!-- Dropdown de Categorias -->
          <div class="dropdown">
            <button 
              class="btn btn-outline-primary dropdown-toggle d-flex align-items-center" 
              type="button" 
              id="dropdownCategorias" 
              data-bs-toggle="dropdown" 
              aria-expanded="false"
              style="min-width: 120px;"
            >
              <i class="bi bi-grid-3x3-gap me-2"></i>
              <span class="d-none d-lg-inline">Categorias</span>
            </button>
            <ul class="dropdown-menu shadow-lg" aria-labelledby="dropdownCategorias" style="min-width: 250px;">
              <li>
                <router-link to="/" class="dropdown-item py-2">
                  <i class="bi bi-house me-3 text-primary"></i>
                  <span class="fw-medium">Todos os Produtos</span>
                </router-link>
              </li>
              <li><hr class="dropdown-divider my-1"></li>
              <li v-for="categoria in categorias" :key="categoria.id">
                <router-link 
                  :to="`/categoria/${categoria.id}`" 
                  class="dropdown-item py-2"
                  @click="selecionarCategoria(categoria)"
                >
                  <i :class="categoria.icone + ' me-3 text-primary'"></i>
                  <span class="fw-medium">{{ categoria.nome }}</span>
                </router-link>
              </li>
            </ul>
          </div>
        </div>
      </div>

      <!-- Ícones na direita -->
      <div class="d-flex align-items-center gap-3">
        <!-- Favoritos -->
        <router-link to="/favoritos" class="btn btn-outline-light text-dark position-relative">
          <i class="bi bi-heart fs-5"></i>
          <span class="position-absolute top-0 start-100 translate-middle badge rounded-pill bg-danger" v-if="favoritosCount > 0">
            {{ favoritosCount }}
          </span>
        </router-link>

        <!-- Carrinho -->
        <router-link to="/carrinho" class="btn btn-outline-light text-dark position-relative">
          <i class="bi bi-cart3 fs-5"></i>
          <span class="position-absolute top-0 start-100 translate-middle badge rounded-pill bg-primary" v-if="carrinhoCount > 0">
            {{ carrinhoCount }}
          </span>
        </router-link>

        <!-- Login/Cadastro -->
        <div class="dropdown">
          <button 
            class="btn btn-outline-primary d-flex align-items-center" 
            type="button" 
            id="dropdownUsuario" 
            data-bs-toggle="dropdown" 
            aria-expanded="false"
          >
            <i class="bi bi-person-circle fs-5 me-2"></i>
            <span class="d-none d-md-inline">{{ usuarioLogado ? usuarioLogado.nome : 'Entrar' }}</span>
          </button>
          <ul class="dropdown-menu" aria-labelledby="dropdownUsuario">
            <li v-if="!usuarioLogado">
              <a class="dropdown-item" href="#" @click="abrirLogin">
                <i class="bi bi-box-arrow-in-right me-2"></i>
                Fazer Login
              </a>
            </li>
            <li v-if="!usuarioLogado">
              <a class="dropdown-item" href="#" @click="abrirCadastro">
                <i class="bi bi-person-plus me-2"></i>
                Criar Conta
              </a>
            </li>
            <li v-if="usuarioLogado">
              <a class="dropdown-item" href="#" @click="verPerfil">
                <i class="bi bi-person me-2"></i>
                Meu Perfil
              </a>
            </li>
            <li v-if="usuarioLogado">
              <a class="dropdown-item" href="#" @click="verPedidos">
                <i class="bi bi-bag me-2"></i>
                Meus Pedidos
              </a>
            </li>
            <li v-if="usuarioLogado"><hr class="dropdown-divider"></li>
            <li v-if="usuarioLogado">
              <a class="dropdown-item" href="#" @click="logout">
                <i class="bi bi-box-arrow-right me-2"></i>
                Sair
              </a>
            </li>
          </ul>
        </div>
      </div>
    </div>
  </nav>
</template>

<script>
import { ref, onMounted } from 'vue'
import { useRouter } from 'vue-router'
import categoriaService from '@/services/categoriaService'

export default {
  name: 'NavegacaoPrincipal',
  setup() {
    const router = useRouter()
    const termoPesquisa = ref('')
    const categorias = ref([])
    const favoritosCount = ref(0)
    const carrinhoCount = ref(0)
    const usuarioLogado = ref(null)

    const carregarCategorias = async () => {
      try {
        const response = await categoriaService.obterTodas()
        categorias.value = response.data.map(cat => ({
          ...cat,
          icone: cat.icone || obterIconePadrao(cat.nome)
        }))
      } catch (error) {
        console.error('Erro ao carregar categorias:', error)
        // Em caso de erro, mostrar mensagem ao usuário
        console.warn('Não foi possível carregar as categorias. Verifique a conexão com o servidor.')
      }
    }

    const obterIconePadrao = (nomeCategoria) => {
      const iconesPadrao = {
        'Smartphones': 'bi bi-phone',
        'Notebooks': 'bi bi-laptop',
        'Acessórios': 'bi bi-headphones',
        'TVs e Áudio': 'bi bi-tv',
        'Eletrônicos': 'bi bi-laptop'
      }
      return iconesPadrao[nomeCategoria] || 'bi bi-tag'
    }

    const pesquisar = () => {
      if (termoPesquisa.value.trim()) {
        router.push({
          path: '/pesquisa',
          query: { q: termoPesquisa.value }
        })
      }
    }

    const selecionarCategoria = (categoria) => {
      console.log('Categoria selecionada:', categoria.nome)
    }

    const abrirLogin = () => {
      console.log('Abrir modal de login')
    }

    const abrirCadastro = () => {
      console.log('Abrir modal de cadastro')
    }

    const verPerfil = () => {
      router.push('/perfil')
    }

    const verPedidos = () => {
      router.push('/pedidos')
    }

    const logout = () => {
      usuarioLogado.value = null
      console.log('Usuário deslogado')
    }

    onMounted(() => {
      carregarCategorias()
      // Simular dados do carrinho e favoritos
      carrinhoCount.value = 3
      favoritosCount.value = 1
    })

    return {
      termoPesquisa,
      categorias,
      favoritosCount,
      carrinhoCount,
      usuarioLogado,
      pesquisar,
      selecionarCategoria,
      abrirLogin,
      abrirCadastro,
      verPerfil,
      verPedidos,
      logout
    }
  }
}
</script>

<style scoped>
.navbar {
  border-bottom: 1px solid #dee2e6;
  display: block;
}

.navbar-brand {
  text-decoration: none;
  font-size: 2rem;
  font-weight: bold;
}

.navbar-brand:hover {
  text-decoration: none;
}

.btn-outline-light.text-dark {
  border: none;
  color: #495057 !important;
}

.btn-outline-light.text-dark:hover {
  background-color: #f8f9fa;
  color: #495057 !important;
}

.dropdown-item {
  transition: all 0.2s ease;
  border-radius: 4px;
  margin: 2px 8px;
  padding: 8px 12px;
}

.dropdown-item:hover {
  background-color: #e3f2fd;
  color: #1976d2;
  transform: translateX(5px);
}

.dropdown-item i {
  width: 20px;
  text-align: center;
}

.dropdown-menu {
  border: none;
  border-radius: 8px;
  padding: 8px 0;
}

.input-group .form-control:focus {
  border-color: #0d6efd;
  box-shadow: 0 0 0 0.2rem rgba(13, 110, 253, 0.25);
}

.badge {
  font-size: 0.6rem;
}

/* Botão de categorias personalizado */
.btn-light {
  background-color: #ffffff;
  border: 1px solid #dee2e6;
  color: #212529;
  font-weight: 600;
}

.btn-light:hover {
  background-color: #f8f9fa;
  border-color: #dee2e6;
  color: #212529;
}

/* Efeito hover nos ícones da navegação */
.btn-outline-light.text-dark {
  transition: all 0.2s ease;
}

.btn-outline-light.text-dark:hover {
  transform: translateY(-2px);
  box-shadow: 0 4px 8px rgba(0,0,0,0.1);
}

/* Responsividade */
@media (max-width: 768px) {
  .container-fluid {
    padding-left: 1rem;
    padding-right: 1rem;
  }
  
  .flex-grow-1 {
    margin-left: 2rem !important;
    margin-right: 2rem !important;
  }
  
  .gap-3 {
    gap: 1rem !important;
  }
  
  .d-flex.align-items-center.gap-3[style*="max-width"] {
    max-width: 500px !important;
  }
  
  .navbar-brand {
    font-size: 1.5rem;
  }
  
  .navbar-brand span:first-child {
    font-size: 2rem;
  }
}

@media (max-width: 576px) {
  .d-flex.justify-content-between {
    flex-direction: column;
    gap: 1rem;
  }
  
  .flex-grow-1 {
    order: 2;
    margin: 0 !important;
  }
  
  .navbar-brand {
    order: 1;
  }
  
  .d-flex.align-items-center.gap-3:last-child {
    order: 3;
    justify-content: center;
  }
  
  .d-flex.align-items-center.gap-3[style*="max-width"] {
    max-width: 100% !important;
    flex-direction: column;
    gap: 0.5rem !important;
  }
  
  .input-group {
    width: 100%;
  }
}
</style>