<template>
  <div class="inicio">
    <!-- Seção Hero/Banner -->
    <section class="hero-section bg-primary text-white py-5 mb-4">
      <div class="container">
        <div class="row align-items-center">
          <div class="col-lg-6">
            <h1 class="display-4 fw-bold mb-3">Bem-vindo à MicroLoja</h1>
            <p class="lead mb-4">Encontre os melhores produtos com os melhores preços. Qualidade garantida e entrega rápida.</p>
            <button class="btn btn-light btn-lg">
              <i class="bi bi-arrow-down-circle me-2"></i>
              Ver Produtos
            </button>
          </div>
          <div class="col-lg-6 text-center">
            <i class="bi bi-bag-heart display-1"></i>
          </div>
        </div>
      </div>
    </section>

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

    <!-- Grid de Produtos -->
    <div v-else class="container">
      <!-- Título da seção -->
      <div class="row mb-4">
        <div class="col-12">
          <h2 class="h3 fw-bold text-center mb-0">
            {{ tituloSecao }}
          </h2>
          <p class="text-muted text-center">{{ produtos.length }} produtos encontrados</p>
        </div>
      </div>

      <!-- Produtos não encontrados -->
      <EstadoVazio 
        v-if="produtos.length === 0"
        icone="bi bi-search"
        titulo="Nenhum produto encontrado"
        mensagem="Tente ajustar seus filtros ou fazer uma nova busca."
      >
        <template #acoes>
          <router-link to="/" class="btn btn-primary me-2">
            <i class="bi bi-house me-2"></i>
            Todos os Produtos
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
import EstadoVazio from '@/components/EstadoVazio.vue'

export default {
  name: 'PaginaInicio',
  components: {
    EstadoVazio
  },
  setup() {
    const route = useRoute()
    const router = useRouter()
    
    const produtos = ref([])
    const carregando = ref(true)
    const termoPesquisa = ref('')
    const filtroPrecoMin = ref('')
    const filtroPrecoMax = ref('')
    const ordenacao = ref('relevancia')
    const paginaAtual = ref(1)
    const totalPaginas = ref(1)
    const itensPorPagina = 12

    const tituloSecao = computed(() => {
      if (termoPesquisa.value) {
        return `Resultados para "${termoPesquisa.value}"`
      }
      return 'Todos os Produtos'
    })

    const paginasVisiveis = computed(() => {
      const paginas = []
      const inicio = Math.max(1, paginaAtual.value - 2)
      const fim = Math.min(totalPaginas.value, paginaAtual.value + 2)
      
      for (let i = inicio; i <= fim; i++) {
        paginas.push(i)
      }
      return paginas
    })

    const carregarProdutos = async () => {
      carregando.value = true
      try {
        let response

        // Determinar qual endpoint usar baseado na rota
        if (route.query.q) {
          termoPesquisa.value = route.query.q
          response = await produtoService.buscarPorNome(route.query.q)
        } else if (filtroPrecoMin.value || filtroPrecoMax.value) {
          response = await produtoService.obterPorFaixaPreco(
            filtroPrecoMin.value || 0,
            filtroPrecoMax.value || 999999
          )
        } else {
          response = await produtoService.obterTodos()
        }

        produtos.value = response.data.map(produto => ({
          ...produto,
          favorito: false, // TODO: Implementar lógica de favoritos
          imagemUrl: produto.imagemUrl ? 
            `https://localhost:5000/${produto.imagemUrl}` : 
            `https://via.placeholder.com/300x300/007bff/ffffff?text=${encodeURIComponent(produto.nome)}`,
          // Adicionar campos que podem estar faltando
          precoOriginal: produto.precoOriginal || null,
          desconto: produto.precoOriginal ? 
            Math.round(((produto.precoOriginal - produto.preco) / produto.precoOriginal) * 100) : 
            null,
          estoque: produto.estoque || 1, // Assumir que tem estoque se não especificado
          avaliacao: produto.avaliacao || 0,
          numeroAvaliacoes: produto.numeroAvaliacoes || 0
        }))
        
        // Aplicar ordenação
        aplicarOrdenacao()
        
        // Calcular paginação
        totalPaginas.value = Math.ceil(produtos.value.length / itensPorPagina)
        
      } catch (error) {
        console.error('Erro ao carregar produtos:', error)
        // Em caso de erro, exibir array vazio e mostrar mensagem de erro
        produtos.value = []
        totalPaginas.value = 0
        console.warn('Não foi possível carregar os produtos. Verifique a conexão com o servidor.')
      } finally {
        carregando.value = false
      }
    }

    // const gerarProdutosMock = () => {
    //   const produtosMock = [
    //     {
    //       id: 1,
    //       nome: 'Smartphone Samsung Galaxy A54',
    //       descricao: 'Smartphone com tela de 6.4 polegadas, 128GB, câmera tripla',
    //       preco: 1299.99,
    //       precoOriginal: 1499.99,
    //       desconto: 13,
    //       estoque: 15,
    //       avaliacao: 4,
    //       numeroAvaliacoes: 127,
    //       imagemUrl: 'https://via.placeholder.com/300x300/007bff/ffffff?text=Samsung+A54'
    //     },
    //     {
    //       id: 2,
    //       nome: 'Notebook Dell Inspiron 15',
    //       descricao: 'Notebook Intel i5, 8GB RAM, SSD 256GB, tela 15.6"',
    //       preco: 2599.99,
    //       estoque: 8,
    //       avaliacao: 5,
    //       numeroAvaliacoes: 89,
    //       imagemUrl: 'https://via.placeholder.com/300x300/28a745/ffffff?text=Dell+Inspiron'
    //     },
    //     {
    //       id: 3,
    //       nome: 'Camiseta Básica Premium',
    //       descricao: 'Camiseta 100% algodão, corte moderno, diversas cores',
    //       preco: 49.99,
    //       precoOriginal: 79.99,
    //       desconto: 37,
    //       estoque: 42,
    //       avaliacao: 4,
    //       numeroAvaliacoes: 234,
    //       imagemUrl: 'https://via.placeholder.com/300x300/6f42c1/ffffff?text=Camiseta'
    //     },
    //     {
    //       id: 4,
    //       nome: 'Smart TV LG 50" 4K',
    //       descricao: 'Smart TV LED 50 polegadas, resolução 4K, HDR',
    //       preco: 1899.99,
    //       estoque: 6,
    //       avaliacao: 5,
    //       numeroAvaliacoes: 156,
    //       imagemUrl: 'https://via.placeholder.com/300x300/fd7e14/ffffff?text=LG+TV'
    //     },
    //     {
    //       id: 5,
    //       nome: 'Fone Bluetooth JBL',
    //       descricao: 'Fone de ouvido wireless com cancelamento de ruído',
    //       preco: 299.99,
    //       precoOriginal: 399.99,
    //       desconto: 25,
    //       estoque: 23,
    //       avaliacao: 4,
    //       numeroAvaliacoes: 312,
    //       imagemUrl: 'https://via.placeholder.com/300x300/dc3545/ffffff?text=JBL+Fone'
    //     },
    //     {
    //       id: 6,
    //       nome: 'Tênis Nike Air Max',
    //       descricao: 'Tênis esportivo com tecnologia Air Max, conforto garantido',
    //       preco: 399.99,
    //       estoque: 18,
    //       avaliacao: 5,
    //       numeroAvaliacoes: 89,
    //       imagemUrl: 'https://via.placeholder.com/300x300/20c997/ffffff?text=Nike+Air'
    //     }
    //   ]
      
    //   // Duplicar produtos para simular mais itens
    //   return [...produtosMock, ...produtosMock.map(p => ({ ...p, id: p.id + 10 }))]
    // }

    const formatarPreco = (preco) => {
      return new Intl.NumberFormat('pt-BR', {
        minimumFractionDigits: 2,
        maximumFractionDigits: 2
      }).format(preco)
    }

    const aplicarFiltros = () => {
      carregarProdutos()
    }

    const limparFiltros = () => {
      filtroPrecoMin.value = ''
      filtroPrecoMax.value = ''
      ordenacao.value = 'mais-vendidos'
      router.push({ path: '/' })
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
    watch(() => route.query.q, () => {
      carregarProdutos()
    })

    onMounted(() => {
      carregarProdutos()
    })

    return {
      produtos,
      carregando,
      tituloSecao,
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
.hero-section {
  background: linear-gradient(135deg, #007bff 0%, #0056b3 100%);
}

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

/* Responsividade */
@media (max-width: 768px) {
  .hero-section {
    text-align: center;
  }
  
  .produto-imagem {
    height: 200px;
  }
  
  .display-4 {
    font-size: 2rem;
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