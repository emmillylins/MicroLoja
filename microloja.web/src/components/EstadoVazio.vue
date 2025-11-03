<template>
  <div class="empty-state text-center py-5">
    <div class="empty-icon mb-4">
      <i :class="icone" class="display-1 text-muted"></i>
    </div>
    <h3 class="h4 text-dark mb-3">{{ titulo }}</h3>
    <p class="text-muted mb-4">{{ mensagem }}</p>
    <div class="empty-actions">
      <slot name="acoes">
        <router-link to="/" class="btn btn-primary">
          <i class="bi bi-house me-2"></i>
          Voltar ao Início
        </router-link>
      </slot>
    </div>
  </div>
</template>

<script>
export default {
  name: 'EstadoVazio',
  props: {
    icone: {
      type: String,
      default: 'bi bi-search'
    },
    titulo: {
      type: String,
      default: 'Nenhum item encontrado'
    },
    mensagem: {
      type: String,
      default: 'Tente ajustar seus filtros ou fazer uma nova busca.'
    }
  }
}
</script>

<style scoped>
.empty-state {
  min-height: 300px;
  display: flex;
  flex-direction: column;
  justify-content: center;
  align-items: center;
}

.empty-icon {
  opacity: 0.7;
  animation: pulse 2s infinite;
}

@keyframes pulse {
  0% {
    opacity: 0.7;
  }
  50% {
    opacity: 0.4;
  }
  100% {
    opacity: 0.7;
  }
}

.empty-actions .btn {
  margin: 0 0.5rem;
}

@media (max-width: 768px) {
  .empty-state {
    padding: 2rem 1rem;
  }
  
  .display-1 {
    font-size: 3rem;
  }
}
</style>