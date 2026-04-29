var deliveryOptions = document.querySelectorAll('input[name="tipo-entrega"]');
var addressSection = document.getElementById('checkout-address-section');
var checkoutBackButton = document.getElementById('checkout-back-button');
var checkoutConfirmButton = document.getElementById('checkout-confirm-button');
var checkoutSummaryList = document.querySelector('.checkout-summary-list');
var checkoutTotalElement = document.querySelector('.checkout-total');
var nomeInput = document.getElementById('nome');
var telefoneInput = document.getElementById('telefone');
var ruaInput = document.getElementById('rua');
var numeroInput = document.getElementById('numero');
var bairroInput = document.getElementById('bairro');
var cart = JSON.parse(localStorage.getItem('carrinho')) || [];

function formatPrice(value) {
  return 'R$ ' + value.toFixed(2).replace('.', ',');
}

function getCartTotal() {
  var total = 0;

  cart.forEach(function (item) {
    total += item.preco * item.quantidade;
  });

  return total;
}

function renderCheckoutSummary() {
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
  });

  checkoutTotalElement.textContent = 'Total: ' + formatPrice(getCartTotal());
}

function getSelectedDeliveryType() {
  var selectedOption = document.querySelector('input[name="tipo-entrega"]:checked');

  return selectedOption ? selectedOption.value : '';
}

function updateAddressSection() {
  var isDelivery = getSelectedDeliveryType() === 'entrega';

  addressSection.hidden = !isDelivery;
}

function validateCheckout() {
  var tipoEntrega = getSelectedDeliveryType();

  if (!nomeInput.value.trim()) {
    alert('Informe o nome.');
    return false;
  }

  if (!telefoneInput.value.trim()) {
    alert('Informe o telefone.');
    return false;
  }

  if (!tipoEntrega) {
    alert('Selecione o tipo de entrega.');
    return false;
  }

  if (tipoEntrega === 'entrega') {
    if (!ruaInput.value.trim()) {
      alert('Informe a rua.');
      return false;
    }

    if (!numeroInput.value.trim()) {
      alert('Informe o número.');
      return false;
    }

    if (!bairroInput.value.trim()) {
      alert('Informe o bairro.');
      return false;
    }
  }

  return true;
}

function createOrder() {
  var tipoEntrega = getSelectedDeliveryType();
  var endereco = null;

  if (tipoEntrega === 'entrega') {
    endereco = {
      rua: ruaInput.value.trim(),
      numero: numeroInput.value.trim(),
      bairro: bairroInput.value.trim()
    };
  }

  return {
    codigo: 'PED-' + Date.now(),
    cliente: {
      nome: nomeInput.value.trim(),
      telefone: telefoneInput.value.trim()
    },
    tipoEntrega: tipoEntrega,
    endereco: endereco,
    itens: cart,
    total: getCartTotal(),
    status: 'recebido',
    dataHora: new Date().toISOString()
  };
}

deliveryOptions.forEach(function (option) {
  option.addEventListener('change', updateAddressSection);
});

updateAddressSection();
renderCheckoutSummary();

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
  if (cart.length === 0) {
    alert('Seu carrinho está vazio.');
    window.location.href = './carrinho.html';
    return;
  }

  if (!validateCheckout()) {
    return;
  }

  var pedidoAtual = createOrder();
  localStorage.setItem('pedidoAtual', JSON.stringify(pedidoAtual));

  window.location.href = './confirmacao.html';
});
