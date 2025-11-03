# 🛍️ MicroLoja - Produto API

Uma API robusta e completa para gerenciamento de produtos e categorias, desenvolvida com .NET 9 seguindo as melhores práticas de arquitetura de software.

## 🎯 Sobre o Projeto

A **MicroLoja Produto API** é um microserviço especializado no gerenciamento de produtos e categorias para um sistema de e-commerce. Foi desenvolvida com foco em performance, escalabilidade e manutenibilidade, implementando padrões consolidados da indústria.

### Características Principais

- ✅ **Arquitetura Limpa (Clean Architecture)**
- ✅ **Padrão Repository + Unit of Work**
- ✅ **Sistema de Notificações**
- ✅ **Controle Transacional Completo**
- ✅ **Validações Robustas com FluentValidation**
- ✅ **Mapeamento Automático com AutoMapper**
- ✅ **Documentação Interativa com Swagger**
- ✅ **Entity Framework Core com SQL Server**
- ✅ **Suporte a JWT para Autenticação**

## 🚀 Tecnologias Utilizadas

### Core
- **.NET 9** - Framework principal
- **ASP.NET Core** - Framework web
- **C# 13** - Linguagem de programação

### Banco de Dados
- **Entity Framework Core 9.0** - ORM
- **SQL Server** - Banco de dados
- **LocalDB** - Para desenvolvimento

### Bibliotecas e Ferramentas
- **AutoMapper 14.0** - Mapeamento objeto-objeto
- **FluentValidation 12.0** - Validações fluentes
- **Swashbuckle (Swagger)** - Documentação da API
- **Microsoft.AspNetCore.Authentication.JwtBearer** - Autenticação JWT

## 🏗️ Arquitetura

A solução segue os princípios da **Clean Architecture**, organizada em camadas bem definidas:

MicroLoja.ProdutoAPI/ 

├── Dominio/           
│   ├── Modelos/               
│   ├── Interfaces/         
│   └── Notificacoes/      
├── Aplicacao/           
│   ├── DTOs/               
│   ├── Interfaces/        
│   ├── Servicos/   
│   ├── Validacoes/           
│   └── AutoMapper/    
├── Infraestrutura/        
│   ├── Contexto/    
│   ├── Repositorios/         
│   ├── UnitOfWork/     
│   └── Seed/    
└── Controllers/         
│   ├── Main/         

## 📋 Pré-requisitos

- **.NET 9 SDK** instalado
- **SQL Server** ou **SQL Server LocalDB**
- **Visual Studio 2022** ou **VS Code** (recomendado)
- **Git** para controle de versão

## 📚 Endpoints da API

### 🛍️ Produtos

| Método | Endpoint | Descrição |
|--------|----------|-----------|
| `GET` | `/api/produtos` | Lista todos os produtos |
| `GET` | `/api/produtos/{id}` | Obtém produto por ID |
| `POST` | `/api/produtos` | Cria novo produto |
| `PUT` | `/api/produtos/{id}` | Atualiza produto |
| `DELETE` | `/api/produtos/{id}` | Exclui produto |
| `GET` | `/api/produtos/categoria/{categoriaId}` | Produtos por categoria |
| `GET` | `/api/produtos/buscar?nome={nome}` | Busca por nome |
| `GET` | `/api/produtos/preco?precoMinimo={min}&precoMaximo={max}` | Por faixa de preço |
| `GET` | `/api/produtos/mais-caros?quantidade={qtd}` | Mais caros |
| `GET` | `/api/produtos/mais-baratos?quantidade={qtd}` | Mais baratos |
| `POST` | `/api/produtos/lote` | Criação em lote |
| `DELETE` | `/api/produtos/lote` | Exclusão em lote |

### 📂 Categorias

| Método | Endpoint | Descrição |
|--------|----------|-----------|
| `GET` | `/api/categorias` | Lista todas as categorias |
| `GET` | `/api/categorias/{id}` | Obtém categoria por ID |
| `POST` | `/api/categorias` | Cria nova categoria |
| `PUT` | `/api/categorias/{id}` | Atualiza categoria |
| `DELETE` | `/api/categorias/{id}` | Exclui categoria |

### 📝 Exemplos de Payloads

#### Criar Produto
```json
{
  "nome": "Produto Exemplo",
  "descricao": "Descrição do Produto",
  "preco": 99.99,
  "categoriaId": 1
}
```

#### Resposta Produto
```json
{
  "id": 1,
  "nome": "Produto Exemplo",
  "descricao": "Descrição do Produto",
  "preco": 99.99,
  "categoriaId": 1
}
```

#### Criar Categoria
```json
{ "nome": "Eletrônicos" }
```

#### Resposta Categoria
```json
{
  "id": 1,
  "nome": "Eletrônicos"
}
```

## 🎨 Padrões Implementados

### 1. **Repository Pattern**
- Abstração do acesso aos dados
- Interface `IRepositorioBase<T>` genérica
- Implementações específicas para cada entidade

### 2. **Unit of Work Pattern**
- Controle transacional centralizado
- Garante consistência em operações complexas
- Interface `IUnitOfWork` com commit/rollback

### 3. **Notification Pattern**
- Sistema de notificações para validações
- Centralização de mensagens de erro
- Interface `INotificador`

### 4. **Dependency Injection**
- Todas as dependências registradas no container
- Facilita testes unitários e manutenção

### 5. **AutoMapper**
- Mapeamento automático entre entidades e DTOs
- Configuração centralizada em `MappingProfile`

### 6. **FluentValidation**
- Validações expressivas e reutilizáveis
- Separação clara entre validações e lógica de negócio

## 💾 Banco de Dados

### Configuração
- **Provider**: SQL Server
- **LocalDB** para desenvolvimento
- **Migrations** do Entity Framework Core

### Tabelas Principais
- **Produtos** - Armazena informações dos produtos
- **Categorias** - Categorias de produtos
- **Relacionamento**: Produto N:1 Categoria

### Seed de Dados
O projeto inclui dados iniciais (seed) com:
- 4 categorias pré-definidas
- 7 produtos de exemplo
- Configuração automática na inicialização

## ✅ Validações

### Produto
- **Nome**: Obrigatório, 2-150 caracteres
- **Preço**: Obrigatório, maior que zero
- **Descrição**: Opcional, máximo 500 caracteres
- **Categoria**: Obrigatória
- **URL da Imagem**: Opcional, máximo 500 caracteres

### Categoria
- **Nome**: Obrigatório, 2-100 caracteres
- **Unicidade**: Não permite nomes duplicados

## 📖 Documentação

### Swagger/OpenAPI
- **URL**: https://localhost:5001
- Documentação interativa completa
- Schemas detalhados
- Exemplos de requests/responses

### Comentários XML
- Documentação inline no código
- Geração automática de documentação
- Integração com Swagger

## 🚀 Deploy e Produção

### Configurações de Ambiente
- `appsettings.json` - Configurações base
- `appsettings.Development.json` - Desenvolvimento
- Variáveis de ambiente para produção

## 👥 Equipe

**Desenvolvido por**: Emmilly Lins  
**Email**: emycmlins@gmail.com

---

**🚀 Happy Coding!** 🛍️