var quantityValueElement = document.getElementById('quantity-value');
var decreaseButton = document.querySelector('[data-action="decrease"]');
var increaseButton = document.querySelector('[data-action="increase"]');
var detailBackButton = document.getElementById('detail-back-button');
var detailAddButton = document.getElementById('detail-add-button');
var productNameElement = document.querySelector('.detail-product-name');
var productPriceElement = document.querySelector('.detail-product-price');
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
  
  //redireciona para o carrinho
  window.location.href = './carrinho.html';
});
