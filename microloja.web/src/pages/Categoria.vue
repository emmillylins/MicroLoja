<template>
  <div class="pagina-categoria">
    <!-- Breadcrumb -->
    <div class="container py-3">
      <nav aria-label="breadcrumb">
        <ol class="breadcrumb">
          <li class="breadcrumb-item">
            <router-link to="/" class="text-decoration-none">
              <i class="bi bi-house me-1"></i>
              Home
            </router-link>
          </li>
          <li class="breadcrumb-item active" aria-current="page">
            {{ categoriaAtual?.nome || 'Categoria' }}
          </li>
        </ol>
      </nav>
    </div>

    <!-- Cabeçalho da Categoria -->
    <div class="container mb-4">
      <div class="row align-items-center">
        <div class="col-md-8">
          <h1 class="h2 fw-bold mb-2">{{ categoriaAtual?.nome || 'Categoria' }}</h1>
          <p class="text-muted mb-0" v-if="categoriaAtual?.descricao">
            {{ categoriaAtual.descricao }}
          </p>
        </div>
        <div class="col-md-4 text-md-end">
          <span class="badge bg-primary fs-6">
            {{ produtos.length }} produtos encontrados
          </span>
        </div>
      </div>
    </div>

    <!-- Filtros e Ordenação -->
    <div class="container mb-4">
      <div class="row">
        <div class="col-md-6">
          <div class="d-flex align-items-center">
            <label class="form-label me-3 mb-0">Filtrar por preço:</label>
            <div class="input-group" style="max-width: 300px;">
              <span class="input-group-text">R$</span>
              <input 
                type="number" 
                class="form-control" 
                placeholder="Min"
                v-model="filtroPrecoMin"
                @change="aplicarFiltros"
              >
              <span class="input-group-text">-</span>
              <input 
                type="number" 
                class="form-control" 
                placeholder="Max"
                v-model="filtroPrecoMax"
                @change="aplicarFiltros"
              >
            </div>
          </div>
        </div>
        <div class="col-md-6">
          <div class="d-flex align-items-center justify-content-md-end">
            <label class="form-label me-3 mb-0">Ordenar por:</label>
            <select 
              class="form-select" 
              style="max-width: 200px;"
              v-model="ordenacao"
              @change="aplicarOrdenacao"
            >
              <option value="relevancia">Relevância</option>
              <option value="preco-menor">Menor Preço</option>
              <option value="preco-maior">Maior Preço</option>
              <option value="nome">Nome A-Z</option>
              <option value="mais-vendidos">Mais Vendidos</option>
            </select>
          </div>
        </div>
      </div>
    </div>

    <!-- Loading -->
    <div v-if="carregando" class="container text-center py-5">
      <div class="spinner-border text-primary" role="status">
        <span class="visually-hidden">Carregando...</span>
      </div>
      <p class="mt-3">Carregando produtos...</p>
    </div>

    <!-- Conteúdo da categoria -->
    <div v-else class="container">
      <!-- Estado vazio -->
      <EstadoVazio 
        v-if="produtos.length === 0"
        icone="bi bi-box"
        titulo="Nenhum produto nesta categoria"
        mensagem="Esta categoria ainda não possui produtos disponíveis."
      >
        <template #acoes>
          <router-link to="/" class="btn btn-primary me-2">
            <i class="bi bi-house me-2"></i>
            Voltar ao Início
          </router-link>
          <button class="btn btn-outline-secondary" @click="limparFiltros">
            <i class="bi bi-arrow-clockwise me-2"></i>
            Limpar Filtros
          </button>
        </template>
      </EstadoVazio>

      <!-- Grid de produtos -->
      <div v-else class="row g-4">
        <div 
          v-for="produto in produtos" 
          :key="produto.id" 
          class="col-lg-3 col-md-4 col-sm-6"
        >
          <div class="card h-100 produto-card">
            <!-- Imagem do produto -->
            <div class="card-img-top produto-imagem">
              <img 
                :src="produto.imagemUrl || '/placeholder-produto.jpg'" 
                :alt="produto.nome"
                class="img-fluid"
              >
              <!-- Badge de desconto -->
              <div v-if="produto.desconto" class="badge bg-danger position-absolute top-0 start-0 m-2">
                -{{ produto.desconto }}%
              </div>
              <!-- Botões de ação -->
              <div class="produto-acoes">
                <button 
                  class="btn btn-outline-light btn-sm"
                  @click="adicionarFavoritos(produto)"
                  :class="{ 'active': produto.favorito }"
                >
                  <i class="bi bi-heart-fill"></i>
                </button>
                <button 
                  class="btn btn-outline-light btn-sm"
                  @click="visualizarProduto(produto)"
                >
                  <i class="bi bi-eye"></i>
                </button>
              </div>
            </div>

            <!-- Corpo do card -->
            <div class="card-body d-flex flex-column">
              <h6 class="card-title text-truncate" :title="produto.nome">{{ produto.nome }}</h6>
              <p class="card-text text-muted small text-truncate" :title="produto.descricao">
                {{ produto.descricao }}
              </p>
              
              <!-- Preços -->
              <div class="mt-auto">
                <div v-if="produto.precoOriginal && produto.precoOriginal !== produto.preco" class="small">
                  <span class="text-decoration-line-through text-muted">
                    R$ {{ formatarPreco(produto.precoOriginal) }}
                  </span>
                </div>
                <div class="fw-bold text-primary fs-5">
                  R$ {{ formatarPreco(produto.preco) }}
                </div>
                
                <!-- Avaliação -->
                <div class="d-flex align-items-center mt-2">
                  <div class="estrelas me-2">
                    <i 
                      v-for="n in 5" 
                      :key="n"
                      class="bi"
                      :class="n <= (produto.avaliacao || 0) ? 'bi-star-fill text-warning' : 'bi-star text-muted'"
                    ></i>
                  </div>
                  <small class="text-muted">({{ produto.numeroAvaliacoes || 0 }})</small>
                </div>

                <!-- Botão adicionar ao carrinho -->
                <button 
                  class="btn btn-primary w-100 mt-3"
                  @click="adicionarCarrinho(produto)"
                  :disabled="!produto.estoque || produto.estoque === 0"
                >
                  <i class="bi bi-cart-plus me-2"></i>
                  {{ produto.estoque > 0 ? 'Adicionar ao Carrinho' : 'Sem Estoque' }}
                </button>
              </div>
            </div>
          </div>
        </div>
      </div>

      <!-- Paginação -->
      <nav aria-label="Navegação de páginas" class="mt-5" v-if="totalPaginas > 1">
        <ul class="pagination justify-content-center">
          <li class="page-item" :class="{ disabled: paginaAtual === 1 }">
            <button class="page-link" @click="irParaPagina(paginaAtual - 1)">
              <i class="bi bi-chevron-left"></i>
            </button>
          </li>
          
          <li 
            v-for="pagina in paginasVisiveis" 
            :key="pagina"
            class="page-item" 
            :class="{ active: pagina === paginaAtual }"
          >
            <button class="page-link" @click="irParaPagina(pagina)">{{ pagina }}</button>
          </li>
          
          <li class="page-item" :class="{ disabled: paginaAtual === totalPaginas }">
            <button class="page-link" @click="irParaPagina(paginaAtual + 1)">
              <i class="bi bi-chevron-right"></i>
            </button>
          </li>
        </ul>
      </nav>
    </div>
  </div>
</template>

<script>
import { ref, onMounted, computed, watch } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import produtoService from '@/services/produtoService'
import categoriaService from '@/services/categoriaService'
import { formatarPreco } from '@/utils/formatacao'
import EstadoVazio from '@/components/EstadoVazio.vue'

export default {
  name: 'PaginaCategoria',
  components: {
    EstadoVazio
  },
  setup() {
    const route = useRoute()
    const router = useRouter()
    
    const produtos = ref([])
    const carregando = ref(true)
    const categoriaAtual = ref(null)
    const filtroPrecoMin = ref('')
    const filtroPrecoMax = ref('')
    const ordenacao = ref('relevancia')
    const paginaAtual = ref(1)
    const totalPaginas = ref(1)
    const itensPorPagina = 12

    const paginasVisiveis = computed(() => {
      const paginas = []
      const inicio = Math.max(1, paginaAtual.value - 2)
      const fim = Math.min(totalPaginas.value, paginaAtual.value + 2)
      
      for (let i = inicio; i <= fim; i++) {
        paginas.push(i)
      }
      return paginas
    })

    const carregarCategoria = async () => {
      try {
        const categoriaId = route.params.categoriaId
        const response = await categoriaService.obterPorId(categoriaId)
        categoriaAtual.value = response.data
      } catch (error) {
        console.error('Erro ao carregar categoria:', error)
        categoriaAtual.value = null
      }
    }

    const carregarProdutos = async () => {
      carregando.value = true
      try {
        const categoriaId = route.params.categoriaId
        let response = await produtoService.obterPorCategoria(categoriaId)
        
        let produtosFiltrados = response.data

        // Aplicar filtros de preço se especificados
        if (filtroPrecoMin.value || filtroPrecoMax.value) {
          const min = parseFloat(filtroPrecoMin.value) || 0
          const max = parseFloat(filtroPrecoMax.value) || Infinity
          produtosFiltrados = produtosFiltrados.filter(produto => 
            produto.preco >= min && produto.preco <= max
          )
        }

        produtos.value = produtosFiltrados.map(produto => ({
          ...produto,
          favorito: false,
          imagemUrl: produto.imagemUrl ? 
            `https://localhost:5000/${produto.imagemUrl}` : 
            `https://via.placeholder.com/300x300/007bff/ffffff?text=${encodeURIComponent(produto.nome)}`,
          precoOriginal: produto.precoOriginal || null,
          desconto: produto.precoOriginal ? 
            Math.round(((produto.precoOriginal - produto.preco) / produto.precoOriginal) * 100) : 
            null,
          estoque: produto.estoque || 1,
          avaliacao: produto.avaliacao || 0,
          numeroAvaliacoes: produto.numeroAvaliacoes || 0
        }))
        
        // Aplicar ordenação
        aplicarOrdenacao()
        
        // Calcular paginação
        totalPaginas.value = Math.ceil(produtos.value.length / itensPorPagina)
        
      } catch (error) {
        console.error('Erro ao carregar produtos da categoria:', error)
        produtos.value = []
        totalPaginas.value = 0
      } finally {
        carregando.value = false
      }
    }

    const aplicarFiltros = () => {
      carregarProdutos()
    }

    const limparFiltros = () => {
      filtroPrecoMin.value = ''
      filtroPrecoMax.value = ''
      ordenacao.value = 'relevancia'
      carregarProdutos()
    }

    const aplicarOrdenacao = () => {
      switch (ordenacao.value) {
        case 'preco-menor':
          produtos.value.sort((a, b) => a.preco - b.preco)
          break
        case 'preco-maior':
          produtos.value.sort((a, b) => b.preco - a.preco)
          break
        case 'nome':
          produtos.value.sort((a, b) => a.nome.localeCompare(b.nome))
          break
        case 'mais-vendidos':
          produtos.value.sort((a, b) => (b.numeroAvaliacoes || 0) - (a.numeroAvaliacoes || 0))
          break
        default:
          // Relevância - manter ordem original
          break
      }
    }

    const adicionarFavoritos = (produto) => {
      produto.favorito = !produto.favorito
      console.log(`Produto ${produto.favorito ? 'adicionado aos' : 'removido dos'} favoritos:`, produto.nome)
    }

    const visualizarProduto = (produto) => {
      router.push(`/produto/${produto.id}`)
    }

    const adicionarCarrinho = (produto) => {
      console.log('Produto adicionado ao carrinho:', produto.nome)
      // TODO: Implementar lógica do carrinho
    }

    const irParaPagina = (pagina) => {
      if (pagina >= 1 && pagina <= totalPaginas.value) {
        paginaAtual.value = pagina
        window.scrollTo({ top: 0, behavior: 'smooth' })
      }
    }

    // Watchers
    watch(() => route.params.categoriaId, () => {
      carregarCategoria()
      carregarProdutos()
    })

    onMounted(() => {
      carregarCategoria()
      carregarProdutos()
    })

    return {
      produtos,
      carregando,
      categoriaAtual,
      filtroPrecoMin,
      filtroPrecoMax,
      ordenacao,
      paginaAtual,
      totalPaginas,
      paginasVisiveis,
      formatarPreco,
      aplicarFiltros,
      limparFiltros,
      aplicarOrdenacao,
      adicionarFavoritos,
      visualizarProduto,
      adicionarCarrinho,
      irParaPagina
    }
  }
}
</script>

<style scoped>
.produto-card {
  transition: transform 0.2s ease, box-shadow 0.2s ease;
  border: none;
  box-shadow: 0 2px 4px rgba(0,0,0,0.1);
}

.produto-card:hover {
  transform: translateY(-5px);
  box-shadow: 0 8px 25px rgba(0,0,0,0.15);
}

.produto-imagem {
  position: relative;
  height: 250px;
  overflow: hidden;
  background-color: #f8f9fa;
  display: flex;
  align-items: center;
  justify-content: center;
}

.produto-imagem img {
  width: 100%;
  height: 100%;
  object-fit: cover;
  transition: transform 0.3s ease;
}

.produto-card:hover .produto-imagem img {
  transform: scale(1.05);
}

.produto-acoes {
  position: absolute;
  top: 50%;
  left: 50%;
  transform: translate(-50%, -50%);
  opacity: 0;
  transition: opacity 0.3s ease;
}

.produto-card:hover .produto-acoes {
  opacity: 1;
}

.produto-acoes .btn {
  margin: 0 5px;
  border-color: rgba(255,255,255,0.5);
  background-color: rgba(0,0,0,0.5);
  color: white;
}

.produto-acoes .btn:hover {
  background-color: rgba(0,0,0,0.8);
  border-color: white;
}

.produto-acoes .btn.active {
  background-color: #dc3545;
  border-color: #dc3545;
  color: white;
}

.estrelas i {
  font-size: 0.8rem;
}

.btn:disabled {
  cursor: not-allowed;
}

.breadcrumb {
  background-color: transparent;
  padding: 0;
  margin: 0;
}

.breadcrumb-item + .breadcrumb-item::before {
  content: ">";
  color: #6c757d;
}

/* Responsividade */
@media (max-width: 768px) {
  .produto-imagem {
    height: 200px;
  }
  
  .col-md-8, .col-md-4 {
    text-align: center !important;
  }
  
  .col-md-4 {
    margin-top: 1rem;
  }
}

@media (max-width: 576px) {
  .container-fluid {
    padding-left: 15px;
    padding-right: 15px;
  }
  
  .produto-acoes {
    opacity: 1;
    position: static;
    transform: none;
    margin-top: 10px;
  }
}
</style>