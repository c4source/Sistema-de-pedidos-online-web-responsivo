var cartItems = document.querySelectorAll('[data-item]');
var totalElement = document.getElementById('cart-total');
var cartListElement = document.getElementById('cart-list');
var cartTotalSection = document.getElementById('cart-total-section');
var cartFooter = document.getElementById('cart-footer');
var emptyCartElement = document.getElementById('empty-cart');
var cartBackButton = document.getElementById('cart-back-button');
var cartCheckoutButton = document.getElementById('cart-checkout-button');
var emptyCartButton = document.getElementById('empty-cart-button');

function formatPrice(value) {
  return 'R$ ' + value.toFixed(2).replace('.', ',');
}

function hasItemsInCart() {
  return document.querySelectorAll('[data-item]').length > 0;
}

function updateCartState() {
  var hasItems = hasItemsInCart();

  cartListElement.hidden = !hasItems;
  cartTotalSection.hidden = !hasItems;
  cartFooter.hidden = !hasItems;
  emptyCartElement.hidden = hasItems;

  cartListElement.style.display = hasItems ? '' : 'none';
  cartTotalSection.style.display = hasItems ? '' : 'none';
  cartFooter.style.display = hasItems ? 'flex' : 'none';
  emptyCartElement.style.display = hasItems ? 'none' : 'flex';
}

function updateTotal() {
  if (!hasItemsInCart()) {
    return;
  }

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
    updateCartState();
    updateTotal();
  });
});

updateCartState();
updateTotal();

cartBackButton.addEventListener('click', function () {
  window.location.href = './detalhe.html';
});

cartCheckoutButton.addEventListener('click', function () {
  window.location.href = './checkout.html';
});

emptyCartButton.addEventListener('click', function () {
  window.location.href = './index.html';
});
