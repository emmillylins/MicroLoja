/**
 * Utilitários para formatação de dados
 */

/**
 * Formata um valor numérico como preço em Real brasileiro
 * @param {number} preco - O valor a ser formatado
 * @returns {string} Preço formatado (ex: "1.299,99")
 */
export const formatarPreco = (preco) => {
  return new Intl.NumberFormat('pt-BR', {
    minimumFractionDigits: 2,
    maximumFractionDigits: 2
  }).format(preco)
}

/**
 * Formata um número como moeda brasileira
 * @param {number} valor - O valor a ser formatado
 * @returns {string} Valor formatado como moeda (ex: "R$ 1.299,99")
 */
export const formatarMoeda = (valor) => {
  return new Intl.NumberFormat('pt-BR', {
    style: 'currency',
    currency: 'BRL'
  }).format(valor)
}

/**
 * Formata uma data para o padrão brasileiro
 * @param {Date|string} data - A data a ser formatada
 * @returns {string} Data formatada (ex: "25/12/2023")
 */
export const formatarData = (data) => {
  const dataObj = typeof data === 'string' ? new Date(data) : data
  return dataObj.toLocaleDateString('pt-BR')
}

/**
 * Formata data e hora para o padrão brasileiro
 * @param {Date|string} data - A data a ser formatada
 * @returns {string} Data e hora formatada (ex: "25/12/2023 14:30")
 */
export const formatarDataHora = (data) => {
  const dataObj = typeof data === 'string' ? new Date(data) : data
  return dataObj.toLocaleString('pt-BR')
}

/**
 * Trunca um texto se ele for maior que o limite especificado
 * @param {string} texto - O texto a ser truncado
 * @param {number} limite - O número máximo de caracteres
 * @returns {string} Texto truncado com "..." se necessário
 */
export const truncarTexto = (texto, limite = 100) => {
  if (texto.length <= limite) return texto
  return texto.substring(0, limite) + '...'
}