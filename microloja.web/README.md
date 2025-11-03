# MicroLoja - E-commerce Frontend

Uma aplicação de e-commerce moderna desenvolvida com Vue.js 3 e Bootstrap 5.

## 🚀 Funcionalidades

### 🏠 Página Principal
- **Hero Section** com apresentação da loja
- **Grid de produtos** responsivo
- **Filtros por preço** (mínimo e máximo)
- **Ordenação** por relevância, preço, nome e mais vendidos
- **Paginação** para navegação entre produtos

### 🗂️ Navegação por Categorias
- **Menu dropdown** com todas as categorias
- **Página dedicada** para cada categoria (sem hero section)
- **Breadcrumb** para navegação contextual
- **Filtros específicos** por categoria

### 🔍 Sistema de Busca
- **Barra de pesquisa** na navegação
- **Resultados em tempo real** por nome do produto
- **Página de resultados** com filtros aplicáveis

### 🛒 Recursos do Produto
- **Cards visuais** com imagem, preço e avaliações
- **Badges de desconto** quando aplicável
- **Botões de ação**: favoritar, visualizar, adicionar ao carrinho
- **Estados de estoque** (disponível/esgotado)
- **Avaliações em estrelas** e número de reviews

### 📱 Interface Responsiva
- **Design mobile-first** com Bootstrap 5
- **Navegação adaptável** para diferentes tamanhos de tela
- **Ícones do Bootstrap Icons** para melhor UX

### 🔄 Estados da Aplicação
- **Loading states** durante carregamento
- **Estados vazios** personalizados quando não há produtos
- **Tratamento de erros** com mensagens amigáveis

## 🛠️ Tecnologias

- **Vue.js 3** - Framework principal
- **Vue Router 4** - Roteamento
- **Bootstrap 5** - Estilização e layout
- **Bootstrap Icons** - Ícones
- **Axios** - Requisições HTTP
- **Composition API** - Padrão Vue 3

## 📦 Estrutura do Projeto

```
src/
├── components/         # Componentes reutilizáveis
│   ├── Cabecalho.vue  # Navegação principal
│   └── EstadoVazio.vue # Estado vazio genérico
├── pages/             # Páginas da aplicação
│   ├── Inicio.vue     # Página principal com hero
│   ├── Categoria.vue  # Página de categoria específica
│   ├── Carrinho.vue   # Carrinho de compras
│   └── ...
├── services/          # Integração com API
│   ├── api.js         # Cliente HTTP base
│   ├── produtoService.js
│   └── categoriaService.js
└── utils/             # Utilitários
    ├── formatacao.js  # Formatação de dados
    └── errorHandler.js # Tratamento de erros
```

## 🌐 Integração com Backend

A aplicação está preparada para integração com API .NET 9, com:
- **Interceptors de autenticação**
- **Tratamento de respostas** padronizadas
- **Fallbacks** para imagens e dados

## 🎨 Recursos Visuais

- **Animações suaves** nos cards de produto
- **Hover effects** para melhor interatividade
- **Gradientes** na seção hero
- **Layout consistente** em todas as páginas

## 🚀 Como executar

### Instalação
```bash
npm install
```

### Desenvolvimento
```bash
npm run serve
```

### Build para produção
```bash
npm run build
```

### Linting
```bash
npm run lint
```

---

Desenvolvido com ❤️ usando Vue.js 3
