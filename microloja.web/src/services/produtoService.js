import api from './api'
import { validarRespostaApi, tratarErroApi } from '@/utils/errorHandler'

const produtoService = {
  async obterTodos() {
    try {
      const response = await api.get('/produtos')
      validarRespostaApi(response)
      
      if (response.data.success) {
        return {
          ...response,
          data: response.data.data
        }
      } else {
        const erro = tratarErroApi({ response })
        throw new Error(erro.mensagem)
      }
    } catch (error) {
      const erro = tratarErroApi(error)
      console.error('Erro ao buscar produtos:', erro)
      throw new Error(erro.mensagem)
    }
  },

  async obterPorId(id) {
    try {
      const response = await api.get(`/produtos/${id}`)
      
      if (response.data.success) {
        return {
          ...response,
          data: response.data.data
        }
      } else {
        throw new Error(response.data.errors?.join(', ') || `Erro ao buscar produto ${id}`)
      }
    } catch (error) {
      console.error(`Erro ao buscar produto ${id}:`, error)
      throw error
    }
  },

  async obterPorCategoria(categoriaId) {
    try {
      const response = await api.get(`/produtos/categoria/${categoriaId}`)
      
      if (response.data.success) {
        return {
          ...response,
          data: response.data.data
        }
      } else {
        throw new Error(response.data.errors?.join(', ') || `Erro ao buscar produtos da categoria ${categoriaId}`)
      }
    } catch (error) {
      console.error(`Erro ao buscar produtos da categoria ${categoriaId}:`, error)
      throw error
    }
  },

  async buscarPorNome(nome) {
    try {
      const response = await api.get(`/produtos/buscar?nome=${encodeURIComponent(nome)}`)
      
      if (response.data.success) {
        return {
          ...response,
          data: response.data.data
        }
      } else {
        throw new Error(response.data.errors?.join(', ') || `Erro ao buscar produtos com nome "${nome}"`)
      }
    } catch (error) {
      console.error(`Erro ao buscar produtos com nome "${nome}":`, error)
      throw error
    }
  },

  async obterPorFaixaPreco(precoMinimo, precoMaximo) {
    try {
      const response = await api.get(`/produtos/preco?precoMinimo=${precoMinimo}&precoMaximo=${precoMaximo}`)
      
      if (response.data.success) {
        return {
          ...response,
          data: response.data.data
        }
      } else {
        throw new Error(response.data.errors?.join(', ') || 'Erro ao buscar produtos por faixa de preço')
      }
    } catch (error) {
      console.error(`Erro ao buscar produtos por faixa de preço:`, error)
      throw error
    }
  },

  async obterMaisCaros(quantidade = 10) {
    try {
      const response = await api.get(`/produtos/mais-caros?quantidade=${quantidade}`)
      
      if (response.data.success) {
        return {
          ...response,
          data: response.data.data
        }
      } else {
        throw new Error(response.data.errors?.join(', ') || 'Erro ao buscar produtos mais caros')
      }
    } catch (error) {
      console.error('Erro ao buscar produtos mais caros:', error)
      throw error
    }
  },

  async obterMaisBaratos(quantidade = 10) {
    try {
      const response = await api.get(`/produtos/mais-baratos?quantidade=${quantidade}`)
      
      if (response.data.success) {
        return {
          ...response,
          data: response.data.data
        }
      } else {
        throw new Error(response.data.errors?.join(', ') || 'Erro ao buscar produtos mais baratos')
      }
    } catch (error) {
      console.error('Erro ao buscar produtos mais baratos:', error)
      throw error
    }
  }
}

export default produtoService