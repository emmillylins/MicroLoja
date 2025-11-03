<template>
  <div class="container py-5">
    <div class="row justify-content-center">
      <div class="col-lg-8">
        <div class="text-center mb-5">
          <i class="bi bi-cart display-1 text-primary"></i>
          <h1 class="h3 mt-3">Carrinho de Compras</h1>
          <p class="text-muted">
            Esta página está sendo desenvolvida. Em breve você poderá gerenciar seu carrinho aqui.
          </p>
          <router-link to="/" class="btn btn-primary">
            <i class="bi bi-house me-2"></i>
            Continuar Comprando
          </router-link>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  name: 'PaginaCarrinho'
}
</script>