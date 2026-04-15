var quantityValueElement = document.getElementById('quantity-value');
var decreaseButton = document.querySelector('[data-action="decrease"]');
var increaseButton = document.querySelector('[data-action="increase"]');
var detailBackButton = document.getElementById('detail-back-button');
var detailAddButton = document.getElementById('detail-add-button');
var quantity = 1;

function updateQuantity() {
  quantityValueElement.textContent = quantity;
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
  window.location.href = './carrinho.html';
});
