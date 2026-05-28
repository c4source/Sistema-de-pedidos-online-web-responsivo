var defaultProducts = [
  {
    id: 1,
    nome: 'Pizza Calabresa',
    categoria: 'pizza',
    preco: 39.9,
    imagem: '/img/calabresa800.jpg',
    descricao: 'Pizza de calabresa com cebola e queijo'
  },
  {
    id: 2,
    nome: 'Pizza Frango',
    categoria: 'pizza',
    preco: 39.9,
    imagem: '/img/frango800.jpg',
    descricao: 'Pizza com frango, queijo e molho especial'
  },
  {
    id: 3,
    nome: 'Pizza 4 Queijos',
    categoria: 'pizza',
    preco: 42.9,
    imagem: '/img/4queijos800.jpg',
    descricao: 'Pizza com mistura de quatro queijos'
  },
  {
    id: 10,
    nome: 'Pizza Chocolate',
    categoria: 'doce',
    preco: 44.9,
    imagem: '/img/4queijos800.jpg',
    descricao: 'Pizza doce com chocolate cremoso e granulado'
  },
  {
    id: 11,
    nome: 'Pizza Banana com Canela',
    categoria: 'doce',
    preco: 41.9,
    imagem: '/img/frango800.jpg',
    descricao: 'Pizza doce com banana, canela e toque de açúcar'
  },
  {
    id: 12,
    nome: 'Pizza Romeu e Julieta',
    categoria: 'doce',
    preco: 43.9,
    imagem: '/img/calabresa800.jpg',
    descricao: 'Pizza doce com queijo cremoso e goiabada'
  },
  {
    id: 4,
    nome: 'Coca-Cola Lata',
    categoria: 'bebida',
    preco: 6.5,
    imagem: '/img/cocalata800.jpg',
    descricao: 'Refrigerante gelado em lata'
  },
  {
    id: 5,
    nome: 'Coca-Cola 600ml',
    categoria: 'bebida',
    preco: 8.9,
    imagem: '/img/agua600-800.jpg',
    descricao: 'Refrigerante gelado 600ml'
  },
  {
    id: 6,
    nome: 'Coca-Cola 2L',
    categoria: 'bebida',
    preco: 12.9,
    imagem: '/img/coca2l800.jpg',
    descricao: 'Refrigerante gelado 2 litros'
  },
  {
    id: 7,
    nome: 'Guaraná 2L',
    categoria: 'bebida',
    preco: 10.9,
    imagem: '/img/guarana2l800.jpg',
    descricao: 'Refrigerante guaraná 2 litros'
  },
  {
    id: 8,
    nome: 'Água sem gás',
    categoria: 'bebida',
    preco: 4.5,
    imagem: '/img/aguasemgass800.jpg',
    descricao: 'Água mineral sem gás'
  },
  {
    id: 9,
    nome: 'Água com gás',
    categoria: 'bebida',
    preco: 4.9,
    imagem: '/img/aguacmgas800.jpg',
    descricao: 'Água mineral com gás'
  },
  {
    id: 101,
    nome: 'Combo Individual',
    categoria: 'combo',
    preco: 29.9,
    imagem: '../assets/combo-individual.jpg',
    descricao: '- Pizza broto salgada\n- Refrigerante 600ml'
  },
  {
    id: 102,
    nome: 'Combo Casal',
    categoria: 'combo',
    preco: 59.9,
    imagem: '../assets/combo-casal.jpg',
    descricao: '- Pizza grande de frango\n- Refrigerante 2L'
  },
  {
    id: 103,
    nome: 'Combo Família',
    categoria: 'combo',
    preco: 89.9,
    imagem: '../assets/combo-familia.jpg',
    descricao: '- Pizza grande de frango\n- Pizza broto doce\n- Refrigerante 2L'
  }
];

function normalizeProducts(productsList) {
  return productsList.map(function (product) {
    return {
      id: product.id,
      nome: product.nome,
      categoria: product.categoria,
      preco: product.preco,
      imagem: product.imagem,
      descricao: product.descricao,
      disponivel: product.disponivel !== false
    };
  });
}

function loadProducts() {
  var storedProducts = localStorage.getItem('produtos');

  if (storedProducts) {
    try {
      return normalizeProducts(JSON.parse(storedProducts));
    } catch (error) {
      return normalizeProducts(defaultProducts);
    }
  }

  var initialProducts = normalizeProducts(defaultProducts);
  localStorage.setItem('produtos', JSON.stringify(initialProducts));
  return initialProducts;
}

var products = loadProducts();

async function loadProductsFromApi() {
  if (typeof buscarProdutos !== 'function') {
    return;
  }

  try {
    var apiProducts = await buscarProdutos();
    products = normalizeProducts(apiProducts);
    localStorage.setItem('produtos', JSON.stringify(products));
    renderProducts();
  } catch (error) {
    console.error('Nao foi possivel carregar produtos da API. Usando fallback local.', error);
  }
}

var productSection = document.getElementById('product-section');
var searchInput = document.getElementById('search-input');
var stickyHeader = document.querySelector('.sticky-header');
var stickySearchInput = document.getElementById('sticky-search-input');
var stickySearchForm = document.querySelector('.sticky-header__search');
var stickyMenuButton = document.querySelector('.sticky-header__menu');
var stickyMenuDropdown = document.getElementById('sticky-menu-dropdown');
var stickyMenuLinks = document.querySelectorAll('.sticky-header__dropdown-link');
var modalTriggers = document.querySelectorAll('[data-modal-open]');
var modalOverlays = document.querySelectorAll('.modal-overlay');
var modalCloseButtons = document.querySelectorAll('[data-modal-close]');
var activeModal = null;
var lastFocusedElement = null;
var filterChips = document.querySelectorAll('.filter-chip');
var cartBadgeElement = document.getElementById('cart-badge');
var stickyCartBadgeElement = document.getElementById('sticky-cart-badge');
var selectedCategory = '';

function formatPrice(value) {
  return 'R$ ' + value.toFixed(2).replace('.', ',');
}

function getFilteredProducts() {
  var searchTerm = searchInput.value.trim().toLowerCase();

  return products.filter(function (product) {
    var isAvailable = product.disponivel !== false;
    var matchesCategory = !selectedCategory || product.categoria === selectedCategory;
    var matchesSearch = product.nome.toLowerCase().includes(searchTerm);
    return isAvailable && matchesCategory && matchesSearch;
  });
}

function createProductCard(product) {
  return [
    '<article class="product-card" data-id="' + product.id + '">',
    '   <img class="product-image" src="' + product.imagem + '" alt="' + product.nome + '">',
    '  <h2 class="product-name">' + product.nome + '</h2>',
    '  <p class="product-description">' + product.descricao + '</p>',
    '  <p class="product-price">' + formatPrice(product.preco) + '</p>',
    '  <button class="add-button" type="button">Adicionar</button>',
    '</article>'
  ].join('\n');
}

function saveSelectedProduct(productId) {
  var selectedProduct = products.find(function (product) {
    return product.id === productId;
  });

  if (selectedProduct) {
    localStorage.setItem('produtoSelecionado', JSON.stringify(selectedProduct));
  }
}

function bindProductCardEvents() {
  var productCards = document.querySelectorAll('.product-card');

  productCards.forEach(function (card) {
    card.addEventListener('click', function () {
      var productId = Number(card.getAttribute('data-id'));
      saveSelectedProduct(productId);
      window.location.href = './detalhe.html';
    });
  });
}

function updateActiveFilter() {
  filterChips.forEach(function (chip) {
    chip.classList.toggle('is-active', chip.getAttribute('data-category') === selectedCategory);
  });
}

function renderProducts() {
  var filteredProducts = getFilteredProducts();

  productSection.innerHTML = '';

  filteredProducts.forEach(function (product) {
    productSection.innerHTML += createProductCard(product);
  });

  bindProductCardEvents();
  updateActiveFilter();
}

function getCartItems() {
  try {
    return JSON.parse(localStorage.getItem('carrinho')) || [];
  } catch (error) {
    return [];
  }
}

function updateCartBadge() {
  var cart = getCartItems();
  var totalItems = 0;

  cart.forEach(function (item) {
    totalItems += Number(item.quantidade) || 0;
  });

  if (totalItems > 0) {
    cartBadgeElement.textContent = totalItems;
    cartBadgeElement.hidden = false;
    if (stickyCartBadgeElement) {
      stickyCartBadgeElement.textContent = totalItems;
      stickyCartBadgeElement.hidden = false;
    }
    return;
  }

  cartBadgeElement.hidden = true;
  if (stickyCartBadgeElement) {
    stickyCartBadgeElement.hidden = true;
  }
}

function updateStickyHeaderVisibility() {
  if (!stickyHeader) {
    return;
  }

  var shouldShow = window.scrollY > 220;
  stickyHeader.classList.toggle('is-visible', shouldShow);
  stickyHeader.setAttribute('aria-hidden', shouldShow ? 'false' : 'true');
  stickyHeader.toggleAttribute('inert', !shouldShow);

  if (!shouldShow) {
    closeStickyMenu();
  }
}

function closeStickyMenu() {
  if (!stickyMenuButton || !stickyMenuDropdown) {
    return;
  }

  stickyMenuButton.setAttribute('aria-expanded', 'false');
  stickyMenuDropdown.classList.remove('is-open');
  stickyMenuDropdown.setAttribute('aria-hidden', 'true');
}

function toggleStickyMenu() {
  if (!stickyMenuButton || !stickyMenuDropdown) {
    return;
  }

  var isOpen = stickyMenuButton.getAttribute('aria-expanded') === 'true';
  stickyMenuButton.setAttribute('aria-expanded', isOpen ? 'false' : 'true');
  stickyMenuDropdown.classList.toggle('is-open', !isOpen);
  stickyMenuDropdown.setAttribute('aria-hidden', isOpen ? 'true' : 'false');
}

function syncSearchFromSticky() {
  if (!stickySearchInput) {
    return;
  }

  searchInput.value = stickySearchInput.value;
  renderProducts();
}

function openMenuModal(modalId, triggerElement) {
  var modalOverlay = document.getElementById(modalId);

  if (!modalOverlay) {
    return;
  }

  var modalDialog = modalOverlay.querySelector('.menu-modal');
  var closeButton = modalOverlay.querySelector('[data-modal-close]');

  closeMenuModal();
  lastFocusedElement = triggerElement || document.activeElement;
  activeModal = modalOverlay;
  modalOverlay.classList.add('is-open');
  modalOverlay.setAttribute('aria-hidden', 'false');

  if (modalDialog) {
    modalDialog.classList.add('is-open');
  }

  document.body.classList.add('menu-modal-open');

  if (closeButton) {
    closeButton.focus();
  }
}

function closeMenuModal() {
  if (!activeModal) {
    return;
  }

  var modalDialog = activeModal.querySelector('.menu-modal');
  activeModal.classList.remove('is-open');
  activeModal.setAttribute('aria-hidden', 'true');

  if (modalDialog) {
    modalDialog.classList.remove('is-open');
  }

  document.body.classList.remove('menu-modal-open');

  if (lastFocusedElement && typeof lastFocusedElement.focus === 'function') {
    lastFocusedElement.focus();
  }

  activeModal = null;
  lastFocusedElement = null;
}

searchInput.addEventListener('input', function () {
  if (stickySearchInput && stickySearchInput.value !== searchInput.value) {
    stickySearchInput.value = searchInput.value;
  }

  renderProducts();
});

if (stickySearchInput) {
  stickySearchInput.addEventListener('input', syncSearchFromSticky);
}

if (stickySearchForm) {
  stickySearchForm.addEventListener('submit', function (event) {
    event.preventDefault();
    syncSearchFromSticky();
    productSection.scrollIntoView({ behavior: 'smooth', block: 'start' });
  });
}

if (stickyMenuButton) {
  stickyMenuButton.addEventListener('click', function (event) {
    event.stopPropagation();
    toggleStickyMenu();
  });
}

modalTriggers.forEach(function (trigger) {
  trigger.addEventListener('click', function (event) {
    event.preventDefault();
    openMenuModal(trigger.getAttribute('data-modal-open'), trigger);
  });
});

modalCloseButtons.forEach(function (button) {
  button.addEventListener('click', closeMenuModal);
});

modalOverlays.forEach(function (overlay) {
  overlay.addEventListener('click', function (event) {
    if (event.target === overlay) {
      closeMenuModal();
    }
  });
});

stickyMenuLinks.forEach(function (link) {
  link.addEventListener('click', function () {
    closeStickyMenu();
  });
});

document.addEventListener('click', function (event) {
  if (!stickyHeader || !stickyMenuDropdown || !stickyMenuButton) {
    return;
  }

  if (!stickyHeader.contains(event.target)) {
    closeStickyMenu();
  }
});

document.addEventListener('keydown', function (event) {
  if (event.key === 'Escape') {
    closeMenuModal();
    closeStickyMenu();
  }
});

window.addEventListener('scroll', updateStickyHeaderVisibility, { passive: true });

filterChips.forEach(function (chip) {
  chip.addEventListener('click', function () {
    var category = chip.getAttribute('data-category');
    selectedCategory = selectedCategory === category ? '' : category;
    renderProducts();
  });
});

renderProducts();
updateCartBadge();
updateStickyHeaderVisibility();
loadProductsFromApi();
