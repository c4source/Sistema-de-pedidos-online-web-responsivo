var confirmationBackButton = document.getElementById('confirmation-back-button');
var confirmationHomeButton = document.getElementById('confirmation-home-button');
var confirmationSummaryList = document.querySelector('.confirmation-summary-list');
var confirmationTotalElement = document.querySelector('.confirmation-total');
var cart = JSON.parse(localStorage.getItem('carrinho')) || [];

function formatPrice(value) {
  return 'R$ ' + value.toFixed(2).replace('.', ',');
}

function renderConfirmationSummary() {
  var total = 0;

  confirmationSummaryList.innerHTML = '';

  if (cart.length === 0) {
    confirmationSummaryList.innerHTML = '<li class="confirmation-summary-item">Nenhum item no pedido</li>';
    confirmationTotalElement.textContent = 'Total: R$ 0,00';
    return;
  }

  cart.forEach(function (item) {
    var listItem = document.createElement('li');
    listItem.className = 'confirmation-summary-item';
    listItem.textContent = '- ' + item.nome + ' x' + item.quantidade;
    confirmationSummaryList.appendChild(listItem);

    total += item.preco * item.quantidade;
  });

  confirmationTotalElement.textContent = 'Total: ' + formatPrice(total);
}

renderConfirmationSummary();

confirmationBackButton.addEventListener('click', function () {
  window.location.href = './checkout.html';
});

confirmationHomeButton.addEventListener('click', function () {
  localStorage.removeItem('carrinho');
  window.location.href = './index.html';
});
