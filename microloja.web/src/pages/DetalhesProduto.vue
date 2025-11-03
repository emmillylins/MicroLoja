<template>
  <div class="container py-5">
    <div class="row justify-content-center">
      <div class="col-lg-8">
        <div class="text-center mb-5">
          <i class="bi bi-gear display-1 text-primary"></i>
          <h1 class="h3 mt-3">Página em Desenvolvimento</h1>
          <p class="text-muted">
            Esta página está sendo desenvolvida. Em breve você poderá visualizar os detalhes do produto aqui.
          </p>
          <router-link to="/" class="btn btn-primary">
            <i class="bi bi-house me-2"></i>
            Voltar ao Início
          </router-link>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  name: 'DetalhesProduto',
  props: {
    id: {
      type: String,
      required: true
    }
  },
  mounted() {
    console.log('Detalhes do produto ID:', this.id)
  }
}
</script>