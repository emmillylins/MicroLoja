<template>
  <div id="app">
    <!-- Navegação Principal -->
    <Nav />
    
    <!-- Conteúdo Principal -->
    <main>
      <router-view />
    </main>
    
    <!-- Footer -->
    <footer class="bg-dark text-white py-5 mt-5">
      <div class="container">
        <div class="row">
          <div class="col-md-4 mb-4">
            <h5 class="fw-bold mb-3">
              <span class="fs-4 me-2">🛍️</span>
              MicroLoja
            </h5>
            <p class="text-light">
              Sua loja online de confiança. Os melhores produtos com qualidade garantida e entrega rápida.
            </p>
            <div class="d-flex">
              <a href="#" class="text-white me-3"><i class="bi bi-facebook fs-4"></i></a>
              <a href="#" class="text-white me-3"><i class="bi bi-instagram fs-4"></i></a>
              <a href="#" class="text-white me-3"><i class="bi bi-twitter fs-4"></i></a>
              <a href="#" class="text-white"><i class="bi bi-whatsapp fs-4"></i></a>
            </div>
          </div>
          
          <div class="col-md-2 mb-4">
            <h6 class="fw-bold mb-3">Institucional</h6>
            <ul class="list-unstyled">
              <li><a href="#" class="text-light text-decoration-none">Sobre Nós</a></li>
              <li><a href="#" class="text-light text-decoration-none">Carreiras</a></li>
              <li><a href="#" class="text-light text-decoration-none">Imprensa</a></li>
              <li><a href="#" class="text-light text-decoration-none">Investidores</a></li>
            </ul>
          </div>
          
          <div class="col-md-2 mb-4">
            <h6 class="fw-bold mb-3">Ajuda</h6>
            <ul class="list-unstyled">
              <li><a href="#" class="text-light text-decoration-none">Central de Ajuda</a></li>
              <li><a href="#" class="text-light text-decoration-none">Como Comprar</a></li>
              <li><a href="#" class="text-light text-decoration-none">Trocas e Devoluções</a></li>
              <li><a href="#" class="text-light text-decoration-none">Fale Conosco</a></li>
            </ul>
          </div>
          
          <div class="col-md-2 mb-4">
            <h6 class="fw-bold mb-3">Políticas</h6>
            <ul class="list-unstyled">
              <li><a href="#" class="text-light text-decoration-none">Termos de Uso</a></li>
              <li><a href="#" class="text-light text-decoration-none">Privacidade</a></li>
              <li><a href="#" class="text-light text-decoration-none">Cookies</a></li>
              <li><a href="#" class="text-light text-decoration-none">Segurança</a></li>
            </ul>
          </div>
          
          <div class="col-md-2 mb-4">
            <h6 class="fw-bold mb-3">Pagamento</h6>
            <div class="d-flex flex-wrap">
              <i class="bi bi-credit-card text-white me-2 mb-2 fs-4"></i>
              <i class="bi bi-paypal text-white me-2 mb-2 fs-4"></i>
              <i class="bi bi-currency-bitcoin text-white me-2 mb-2 fs-4"></i>
            </div>
            <h6 class="fw-bold mb-3 mt-3">Segurança</h6>
            <div class="d-flex">
              <i class="bi bi-shield-check text-success fs-4 me-2"></i>
              <small class="text-light">Site Seguro</small>
            </div>
          </div>
        </div>
        
        <hr class="my-4">
        
        <div class="row align-items-center">
          <div class="col-md-6">
            <p class="mb-0 text-light">
              © 2025 MicroLoja. Todos os direitos reservados.
            </p>
          </div>
          <div class="col-md-6 text-md-end">
            <p class="mb-0 text-light">
              CNPJ: 00.000.000/0001-00 | Desenvolvido com ❤️
            </p>
          </div>
        </div>
      </div>
    </footer>
  </div>
</template>

<script>
import Nav from './components/Nav.vue'

export default {
  name: 'App',
  components: {
    Nav
  }
}
</script>

<style>
#app {
  font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  color: #2c3e50;
  min-height: 100vh;
  display: flex;
  flex-direction: column;
}

main {
  flex: 1;
}

/* Scrollbar personalizada */
::-webkit-scrollbar {
  width: 8px;
}

::-webkit-scrollbar-track {
  background: #f1f1f1;
}

::-webkit-scrollbar-thumb {
  background: #888;
  border-radius: 4px;
}

::-webkit-scrollbar-thumb:hover {
  background: #555;
}

/* Links globais */
a {
  transition: color 0.2s ease;
}

/* Botões globais */
.btn {
  transition: all 0.2s ease;
}

.btn:hover {
  transform: translateY(-1px);
}

/* Cards globais */
.card {
  border: none;
  box-shadow: 0 2px 4px rgba(0,0,0,0.1);
  transition: box-shadow 0.2s ease;
}

.card:hover {
  box-shadow: 0 4px 8px rgba(0,0,0,0.15);
}

/* Animações de entrada */
@keyframes fadeInUp {
  from {
    opacity: 0;
    transform: translateY(30px);
  }
  to {
    opacity: 1;
    transform: translateY(0);
  }
}

.fade-in-up {
  animation: fadeInUp 0.6s ease-out;
}

/* Responsividade para footer */
@media (max-width: 768px) {
  footer .col-md-2,
  footer .col-md-4 {
    text-align: center;
    margin-bottom: 2rem;
  }
  
  footer .d-flex {
    justify-content: center;
  }
}

/* Loading spinner global */
.spinner-border-sm {
  width: 1rem;
  height: 1rem;
}

/* Badges personalizados */
.badge {
  font-size: 0.75em;
  padding: 0.5em 0.75em;
}

/* Inputs com foco melhorado */
.form-control:focus,
.form-select:focus {
  border-color: #0d6efd;
  box-shadow: 0 0 0 0.2rem rgba(13, 110, 253, 0.25);
}

/* Tooltips personalizados */
.tooltip-inner {
  background-color: #212529;
  font-size: 0.875rem;
}

/* Alertas personalizados */
.alert {
  border: none;
  border-radius: 8px;
}

/* Progress bars */
.progress {
  height: 8px;
  border-radius: 4px;
}

/* Breadcrumbs personalizados */
.breadcrumb {
  background-color: transparent;
  padding: 0;
  margin: 0;
}

.breadcrumb-item + .breadcrumb-item::before {
  content: ">";
  color: #6c757d;
}
</style>
