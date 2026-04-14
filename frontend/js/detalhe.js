var quantityValueElement = document.getElementById('quantity-value');
var decreaseButton = document.querySelector('[data-action="decrease"]');
var increaseButton = document.querySelector('[data-action="increase"]');
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
