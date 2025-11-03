import api from './api'

const categoriaService = {
  // Obter todas as categorias
  async obterTodas() {
    try {
      const response = await api.get('/categorias')
      
      if (response.data.success) {
        return {
          ...response,
          data: response.data.data
        }
      } else {
        throw new Error(response.data.errors?.join(', ') || 'Erro ao buscar categorias')
      }
    } catch (error) {
      console.error('Erro ao buscar categorias:', error)
      throw error
    }
  },

  // Obter categoria por ID
  async obterPorId(id) {
    try {
      const response = await api.get(`/categorias/${id}`)
      
      if (response.data.success) {
        return {
          ...response,
          data: response.data.data
        }
      } else {
        throw new Error(response.data.errors?.join(', ') || `Erro ao buscar categoria ${id}`)
      }
    } catch (error) {
      console.error(`Erro ao buscar categoria ${id}:`, error)
      throw error
    }
  }
}

export default categoriaService