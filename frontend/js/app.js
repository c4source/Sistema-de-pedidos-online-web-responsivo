var products = [
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
    imagem: '../assets/coca-cola-lata.jpg',
    descricao: 'Refrigerante gelado em lata'
  },
  {
    id: 5,
    nome: 'Coca-Cola 600ml',
    categoria: 'bebida',
    preco: 8.9,
    imagem: '../assets/coca-cola-600ml.jpg',
    descricao: 'Refrigerante gelado 600ml'
  },
  {
    id: 6,
    nome: 'Coca-Cola 2L',
    categoria: 'bebida',
    preco: 12.9,
    imagem: '../assets/coca-cola-2l.jpg',
    descricao: 'Refrigerante gelado 2 litros'
  },
  {
    id: 7,
    nome: 'Guaraná 2L',
    categoria: 'bebida',
    preco: 10.9,
    imagem: '../assets/guarana-2l.jpg',
    descricao: 'Refrigerante guaraná 2 litros'
  },
  {
    id: 8,
    nome: 'Água sem gás',
    categoria: 'bebida',
    preco: 4.5,
    imagem: '../assets/agua-sem-gas.jpg',
    descricao: 'Água mineral sem gás'
  },
  {
    id: 9,
    nome: 'Água com gás',
    categoria: 'bebida',
    preco: 4.9,
    imagem: '../assets/agua-com-gas.jpg',
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

var productSection = document.getElementById('product-section');
var searchInput = document.getElementById('search-input');
var filterChips = document.querySelectorAll('.filter-chip');
var cartBadgeElement = document.getElementById('cart-badge');
var selectedCategory = '';

function formatPrice(value) {
  return 'R$ ' + value.toFixed(2).replace('.', ',');
}

function getFilteredProducts() {
  var searchTerm = searchInput.value.trim().toLowerCase();

  return products.filter(function (product) {
    var matchesCategory = !selectedCategory || product.categoria === selectedCategory;
    var matchesSearch = product.nome.toLowerCase().includes(searchTerm);
    return matchesCategory && matchesSearch;
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
    return;
  }

  cartBadgeElement.hidden = true;
}

searchInput.addEventListener('input', renderProducts);

filterChips.forEach(function (chip) {
  chip.addEventListener('click', function () {
    var category = chip.getAttribute('data-category');
    selectedCategory = selectedCategory === category ? '' : category;
    renderProducts();
  });
});

renderProducts();
updateCartBadge();
