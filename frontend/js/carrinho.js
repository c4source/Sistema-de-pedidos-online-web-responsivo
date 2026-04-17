var totalElement = document.getElementById('cart-total');
var cartListElement = document.getElementById('cart-list');
var cartTotalSection = document.getElementById('cart-total-section');
var cartFooter = document.getElementById('cart-footer');
var emptyCartElement = document.getElementById('empty-cart');
var cartBackButton = document.getElementById('cart-back-button');
var cartCheckoutButton = document.getElementById('cart-checkout-button');
var emptyCartButton = document.getElementById('empty-cart-button');
var cart = JSON.parse(localStorage.getItem('carrinho')) || [];

function formatPrice(value) {
  return 'R$ ' + value.toFixed(2).replace('.', ',');
}

function saveCart() {
  localStorage.setItem('carrinho', JSON.stringify(cart));
}

function updateCartState() {
  var hasItems = cart.length > 0;

  cartListElement.style.display = hasItems ? '' : 'none';
  cartTotalSection.style.display = hasItems ? '' : 'none';
  cartFooter.style.display = hasItems ? 'flex' : 'none';
  emptyCartElement.style.display = hasItems ? 'none' : 'flex';
}

function updateTotal() {
  var total = 0;

  cart.forEach(function (item) {
    total += item.preco * item.quantidade;
  });

  totalElement.textContent = formatPrice(total);
}

function createCartItem(item, index) {
  return [
    '<article class="cart-item" data-index="' + index + '">',
    '  <div class="cart-item-top">',
    '    <div class="cart-item-info">',
    '      <div class="cart-item-image" aria-hidden="true">IMG</div>',
    '      <div class="cart-item-text">',
    '        <h2 class="cart-item-name">' + item.nome + '</h2>',
    '        <p class="cart-item-price">' + formatPrice(item.preco) + '</p>',
    '      </div>',
    '    </div>',
    '    <button class="remove-button" type="button" data-action="remove">Remover</button>',
    '  </div>',
    '',
    '  <div class="cart-item-actions">',
    '    <div class="quantity-control">',
    '      <button class="quantity-button" type="button" data-action="decrease" aria-label="Diminuir quantidade">-</button>',
    '      <span class="quantity-value" data-quantity>' + item.quantidade + '</span>',
    '      <button class="quantity-button" type="button" data-action="increase" aria-label="Aumentar quantidade">+</button>',
    '    </div>',
    '  </div>',
    '</article>'
  ].join('\n');
}

function renderCart() {
  cartListElement.innerHTML = '';

  cart.forEach(function (item, index) {
    cartListElement.innerHTML += createCartItem(item, index);
  });

  bindCartItemEvents();
  updateCartState();
  updateTotal();
}

function bindCartItemEvents() {
  var cartItems = document.querySelectorAll('.cart-item');

  cartItems.forEach(function (itemElement) {
    var itemIndex = Number(itemElement.getAttribute('data-index'));
    var quantityElement = itemElement.querySelector('[data-quantity]');
    var decreaseButton = itemElement.querySelector('[data-action="decrease"]');
    var increaseButton = itemElement.querySelector('[data-action="increase"]');
    var removeButton = itemElement.querySelector('[data-action="remove"]');

    decreaseButton.addEventListener('click', function () {
      if (cart[itemIndex].quantidade > 1) {
        cart[itemIndex].quantidade -= 1;
        quantityElement.textContent = cart[itemIndex].quantidade;
        saveCart();
        updateTotal();
      }
    });

    increaseButton.addEventListener('click', function () {
      cart[itemIndex].quantidade += 1;
      quantityElement.textContent = cart[itemIndex].quantidade;
      saveCart();
      updateTotal();
    });

    removeButton.addEventListener('click', function () {
      cart.splice(itemIndex, 1);
      saveCart();
      renderCart();
    });
  });
}

renderCart();

cartBackButton.addEventListener('click', function () {
  window.location.href = './detalhe.html';
});

cartCheckoutButton.addEventListener('click', function () {
  window.location.href = './checkout.html';
});

emptyCartButton.addEventListener('click', function () {
  window.location.href = './index.html';
});
