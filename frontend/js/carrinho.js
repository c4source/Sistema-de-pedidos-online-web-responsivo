var cartItems = document.querySelectorAll('[data-item]');
var totalElement = document.getElementById('cart-total');

function formatPrice(value) {
  return 'R$ ' + value.toFixed(2).replace('.', ',');
}

function updateTotal() {
  var total = 0;

  cartItems.forEach(function (item) {
    if (!item.isConnected) {
      return;
    }

    var price = Number(item.getAttribute('data-price'));
    var quantity = Number(item.querySelector('[data-quantity]').textContent);
    total += price * quantity;
  });

  totalElement.textContent = formatPrice(total);
}

cartItems.forEach(function (item) {
  var quantityElement = item.querySelector('[data-quantity]');
  var decreaseButton = item.querySelector('[data-action="decrease"]');
  var increaseButton = item.querySelector('[data-action="increase"]');
  var removeButton = item.querySelector('[data-action="remove"]');

  decreaseButton.addEventListener('click', function () {
    var quantity = Number(quantityElement.textContent);

    if (quantity > 1) {
      quantityElement.textContent = quantity - 1;
      updateTotal();
    }
  });

  increaseButton.addEventListener('click', function () {
    var quantity = Number(quantityElement.textContent);
    quantityElement.textContent = quantity + 1;
    updateTotal();
  });

  removeButton.addEventListener('click', function () {
    item.remove();
    updateTotal();
  });
});

updateTotal();
