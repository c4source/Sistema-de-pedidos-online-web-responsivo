var quantityValueElement = document.getElementById('quantity-value');
var decreaseButton = document.querySelector('[data-action="decrease"]');
var increaseButton = document.querySelector('[data-action="increase"]');
var detailBackButton = document.getElementById('detail-back-button');
var detailAddButton = document.getElementById('detail-add-button');
var detailTitleElement = document.querySelector('.detail-title');
var detailImageElement = document.querySelector('.detail-image');
var productNameElement = document.querySelector('.detail-product-name');
var productPriceElement = document.querySelector('.detail-product-price');
var productDescriptionElement = document.querySelector('.detail-description-text');
var selectedProduct = JSON.parse(localStorage.getItem('produtoSelecionado'));
var quantity = 1;

function updateQuantity() {
  quantityValueElement.textContent = quantity;
}

function getProductPrice() {
  var priceText = productPriceElement.textContent;
  return Number(priceText.replace('R$', '').replace(/\s/g, '').replace('.', '').replace(',', '.'));
}

function saveProductToCart() {
  var cart = JSON.parse(localStorage.getItem('carrinho')) || [];
  var product = {
    nome: productNameElement.textContent,
    preco: getProductPrice(),
    quantidade: quantity
  };

  cart.push(product);
  localStorage.setItem('carrinho', JSON.stringify(cart));
}

function loadSelectedProduct() {
  if (!selectedProduct) {
    return;
  }

  detailTitleElement.textContent = selectedProduct.nome;
  detailImageElement.textContent = 'IMG';
  productNameElement.textContent = selectedProduct.nome;
  productPriceElement.textContent = getFormattedPrice(selectedProduct.preco);
  productDescriptionElement.textContent = selectedProduct.descricao;
}

function getFormattedPrice(price) {
  return 'R$ ' + price.toFixed(2).replace('.', ',');
}

increaseButton.addEventListener('click', function () {
  quantity += 1;
  updateQuantity();
});

decreaseButton.addEventListener('click', function () {
  if (quantity > 1) {
    quantity -= 1;
    updateQuantity();
  }
});

detailBackButton.addEventListener('click', function () {
  window.location.href = './index.html';
});

detailAddButton.addEventListener('click', function () {
  saveProductToCart();
  window.location.href = './carrinho.html';
});

loadSelectedProduct();
