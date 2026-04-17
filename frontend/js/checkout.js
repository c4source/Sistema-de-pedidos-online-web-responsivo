var deliveryOptions = document.querySelectorAll('input[name="tipo-entrega"]');
var addressSection = document.getElementById('checkout-address-section');
var checkoutBackButton = document.getElementById('checkout-back-button');
var checkoutConfirmButton = document.getElementById('checkout-confirm-button');
var checkoutSummaryList = document.querySelector('.checkout-summary-list');
var checkoutTotalElement = document.querySelector('.checkout-total');
var cart = JSON.parse(localStorage.getItem('carrinho')) || [];

function formatPrice(value) {
  return 'R$ ' + value.toFixed(2).replace('.', ',');
}

function renderCheckoutSummary() {
  var total = 0;

  checkoutSummaryList.innerHTML = '';

  if (cart.length === 0) {
    checkoutSummaryList.innerHTML = '<li class="checkout-summary-item">Nenhum item no carrinho</li>';
    checkoutTotalElement.textContent = 'Total: R$ 0,00';
    return;
  }

  cart.forEach(function (item) {
    var listItem = document.createElement('li');
    listItem.className = 'checkout-summary-item';
    listItem.textContent = '- ' + item.nome + ' x' + item.quantidade;
    checkoutSummaryList.appendChild(listItem);

    total += item.preco * item.quantidade;
  });

  checkoutTotalElement.textContent = 'Total: ' + formatPrice(total);
}

function updateAddressSection() {
  var selectedOption = document.querySelector('input[name="tipo-entrega"]:checked');
  var isDelivery = selectedOption && selectedOption.value === 'entrega';

  addressSection.hidden = !isDelivery;
}

deliveryOptions.forEach(function (option) {
  option.addEventListener('change', updateAddressSection);
});

updateAddressSection();
renderCheckoutSummary();

var telefoneInput = document.getElementById('telefone');

if (telefoneInput) {
  telefoneInput.addEventListener('input', function (e) {
    var value = e.target.value.replace(/\D/g, '');

    if (value.length > 11) {
      value = value.slice(0, 11);
    }

    if (value.length > 6) {
      value = value.replace(/(\d{2})(\d{5})(\d+)/, '($1) $2-$3');
    } else if (value.length > 2) {
      value = value.replace(/(\d{2})(\d+)/, '($1) $2');
    } else if (value.length > 0) {
      value = value.replace(/(\d+)/, '($1');
    }

    e.target.value = value;
  });
}

checkoutBackButton.addEventListener('click', function () {
  window.location.href = './carrinho.html';
});

checkoutConfirmButton.addEventListener('click', function () {
  window.location.href = './confirmacao.html';
});
