// Utilitário para tratamento de erros da API
export const tratarErroApi = (error) => {
  if (error.response) {
    // Erro de resposta da API
    const { data, status } = error.response
    
    if (data && !data.success && data.errors) {
      // Formato padrão da API: { success: false, errors: [...] }
      return {
        mensagem: data.errors.join(', '),
        codigo: status,
        tipo: 'api'
      }
    } else if (data && data.message) {
      return {
        mensagem: data.message,
        codigo: status,
        tipo: 'api'
      }
    } else {
      return {
        mensagem: `Erro ${status}: ${error.response.statusText}`,
        codigo: status,
        tipo: 'http'
      }
    }
  } else if (error.request) {
    // Erro de rede/conexão
    return {
      mensagem: 'Erro de conexão. Verifique sua internet e tente novamente.',
      codigo: 0,
      tipo: 'rede'
    }
  } else {
    // Erro genérico
    return {
      mensagem: error.message || 'Erro inesperado.',
      codigo: -1,
      tipo: 'generico'
    }
  }
}

// Utilitário para exibir mensagens de erro ao usuário
export const exibirErro = (error) => {
  const erro = tratarErroApi(error)
  
  // Aqui você pode integrar com uma biblioteca de notificações
  // Por enquanto, apenas log no console
  console.error('Erro tratado:', erro)
  
  // Retorna a mensagem para exibição na UI
  return erro.mensagem
}

// Utilitário para verificar se a API está respondendo com o formato esperado
export const validarRespostaApi = (response) => {
  if (!response || !response.data) {
    throw new Error('Resposta inválida da API')
  }
  
  const { data } = response
  
  if (typeof data.success !== 'boolean') {
    throw new Error('Formato de resposta da API inválido')
  }
  
  return true
}