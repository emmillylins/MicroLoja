<template>
  <div class="container py-5">
    <div class="row justify-content-center">
      <div class="col-lg-8">
        <div class="text-center mb-5">
          <i class="bi bi-heart display-1 text-primary"></i>
          <h1 class="h3 mt-3">Produtos Favoritos</h1>
          <p class="text-muted">
            Esta página está sendo desenvolvida. Em breve você poderá gerenciar seus favoritos aqui.
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
  name: 'PaginaFavoritos'
}
</script>