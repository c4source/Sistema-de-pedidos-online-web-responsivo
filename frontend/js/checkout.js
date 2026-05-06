var deliveryOptions = document.querySelectorAll('input[name="tipo-entrega"]');
var paymentOptions = document.querySelectorAll('input[name="forma-pagamento"]');
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

function getSelectedPaymentMethod() {
  var selectedPayment = document.querySelector('input[name="forma-pagamento"]:checked');

  return selectedPayment ? selectedPayment.value : '';
}

function isValidPaymentMethod(paymentMethod) {
  return paymentMethod === 'dinheiro' || paymentMethod === 'cartao' || paymentMethod === 'pix';
}

function updateAddressSection() {
  var isDelivery = getSelectedDeliveryType() === 'entrega';

  addressSection.hidden = !isDelivery;

  if (!isDelivery) {
    clearFieldError(ruaInput, 'rua-error');
    clearFieldError(numeroInput, 'numero-error');
    clearFieldError(bairroInput, 'bairro-error');
  }
}

function getPhoneDigits() {
  return telefoneInput.value.replace(/\D/g, '');
}

function isValidName(name) {
  var cleanedName = name.trim().replace(/\s+/g, ' ');
  var letters = cleanedName.match(/[A-Za-zÀ-ÖØ-öø-ÿ]/g) || [];
  var hasOnlyLettersAndSpaces = /^[A-Za-zÀ-ÖØ-öø-ÿ\s]+$/.test(cleanedName);
  var hasAtLeastTwoWords = cleanedName.split(' ').length >= 2;

  return cleanedName.length >= 3 &&
         letters.length >= 3 &&
         hasOnlyLettersAndSpaces &&
         hasAtLeastTwoWords;
}

function isValidPhone(phoneDigits) {
  var hasValidLength = phoneDigits.length === 10 || phoneDigits.length === 11;
  var allSameDigits = /^(\d)\1+$/.test(phoneDigits);

  return hasValidLength && !allSameDigits;
}

function showFieldError(input, errorElementId, message) {
  var errorElement = document.getElementById(errorElementId);

  input.classList.add('input-error');

  if (errorElement) {
    errorElement.textContent = message;
  }
}

function clearFieldError(input, errorElementId) {
  var errorElement = document.getElementById(errorElementId);

  input.classList.remove('input-error');

  if (errorElement) {
    errorElement.textContent = '';
  }
}

function showPaymentError(message) {
  var errorElement = document.getElementById('payment-error');

  if (errorElement) {
    errorElement.textContent = message;
  }
}

function clearPaymentError() {
  showPaymentError('');
}

function clearCheckoutErrors() {
  clearFieldError(nomeInput, 'nome-error');
  clearFieldError(telefoneInput, 'telefone-error');
  clearFieldError(ruaInput, 'rua-error');
  clearFieldError(numeroInput, 'numero-error');
  clearFieldError(bairroInput, 'bairro-error');
  clearPaymentError();
}

function validateCheckout() {
  var tipoEntrega = getSelectedDeliveryType();
  var formaPagamento = getSelectedPaymentMethod();
  var nome = nomeInput.value.trim();
  var telefoneDigits = getPhoneDigits();
  var firstInvalidField = null;

  clearCheckoutErrors();

  if (!isValidName(nome)) {
    showFieldError(nomeInput, 'nome-error', 'Informe um nome válido.');
    firstInvalidField = firstInvalidField || nomeInput;
  }

  if (!isValidPhone(telefoneDigits)) {
    showFieldError(telefoneInput, 'telefone-error', 'Informe um telefone válido com DDD.');
    firstInvalidField = firstInvalidField || telefoneInput;
  }

  if (!tipoEntrega) {
    return false;
  }

  if (!isValidPaymentMethod(formaPagamento)) {
    showPaymentError('Selecione uma forma de pagamento.');
    return false;
  }

  if (tipoEntrega === 'entrega') {
    if (!ruaInput.value.trim()) {
      showFieldError(ruaInput, 'rua-error', 'Informe a rua.');
      firstInvalidField = firstInvalidField || ruaInput;
    }

    if (!numeroInput.value.trim()) {
      showFieldError(numeroInput, 'numero-error', 'Informe o número.');
      firstInvalidField = firstInvalidField || numeroInput;
    }

    if (!bairroInput.value.trim()) {
      showFieldError(bairroInput, 'bairro-error', 'Informe o bairro.');
      firstInvalidField = firstInvalidField || bairroInput;
    }
  } else {
    clearFieldError(ruaInput, 'rua-error');
    clearFieldError(numeroInput, 'numero-error');
    clearFieldError(bairroInput, 'bairro-error');
  }

  if (firstInvalidField) {
    firstInvalidField.focus();
    return false;
  }

  return true;
}

function createOrder() {
  var tipoEntrega = getSelectedDeliveryType();
  var formaPagamento = getSelectedPaymentMethod();
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
      telefone: getPhoneDigits()
    },
    tipoEntrega: tipoEntrega,
    endereco: endereco,
    itens: cart,
    total: getCartTotal(),
    status: 'recebido',
    dataHora: new Date().toISOString(),
    pagamento: {
      formaPagamento: formaPagamento,
      statusPagamento: 'pendente',
      valorPago: getCartTotal()
    }
  };
}

function saveOrderToHistory(order) {
  var pedidos = JSON.parse(localStorage.getItem('pedidos')) || [];

  var alreadyExists = pedidos.some(function (pedido) {
    return pedido.codigo === order.codigo;
  });

  if (!alreadyExists) {
    pedidos.push(order);
    localStorage.setItem('pedidos', JSON.stringify(pedidos));
  }
}

deliveryOptions.forEach(function (option) {
  option.addEventListener('change', updateAddressSection);
});

paymentOptions.forEach(function (option) {
  option.addEventListener('change', clearPaymentError);
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
    clearFieldError(telefoneInput, 'telefone-error');
  });
}

[
  { input: nomeInput, errorId: 'nome-error' },
  { input: ruaInput, errorId: 'rua-error' },
  { input: numeroInput, errorId: 'numero-error' },
  { input: bairroInput, errorId: 'bairro-error' }
].forEach(function (field) {
  field.input.addEventListener('input', function () {
    clearFieldError(field.input, field.errorId);
  });
});

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
  saveOrderToHistory(pedidoAtual);

  window.location.href = './confirmacao.html';
});
