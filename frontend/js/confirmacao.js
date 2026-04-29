var confirmationBackButton = document.getElementById('confirmation-back-button');
var confirmationHomeButton = document.getElementById('confirmation-home-button');
var confirmationMessage = document.querySelector('.confirmation-message');
var confirmationSummaryList = document.querySelector('.confirmation-summary-list');
var confirmationTotalElement = document.querySelector('.confirmation-total');
var pedidoAtual = JSON.parse(localStorage.getItem('pedidoAtual'));

if (!pedidoAtual) {
  window.location.href = './index.html';
}

function formatPrice(value) {
  return 'R$ ' + value.toFixed(2).replace('.', ',');
}

function renderConfirmationSummary() {
  var tipoEntregaLabel = pedidoAtual.tipoEntrega === 'entrega' ? 'Entrega' : 'Retirada';

  confirmationMessage.textContent = 'Pedido ' + pedidoAtual.codigo + ' - Status: Recebido - ' + tipoEntregaLabel;
  confirmationSummaryList.innerHTML = '';

  if (!pedidoAtual.itens || pedidoAtual.itens.length === 0) {
    confirmationSummaryList.innerHTML = '<li class="confirmation-summary-item">Nenhum item no pedido</li>';
    confirmationTotalElement.textContent = 'Total: R$ 0,00';
    return;
  }

  pedidoAtual.itens.forEach(function (item) {
    var listItem = document.createElement('li');
    listItem.className = 'confirmation-summary-item';
    listItem.textContent = '- ' + item.nome + ' x' + item.quantidade;
    confirmationSummaryList.appendChild(listItem);
  });

  confirmationTotalElement.textContent = 'Total: ' + formatPrice(pedidoAtual.total);
}

if (pedidoAtual) {
  renderConfirmationSummary();
  localStorage.removeItem('carrinho');
}

confirmationBackButton.addEventListener('click', function () {
  window.location.href = './checkout.html';
});

confirmationHomeButton.addEventListener('click', function () {
  localStorage.removeItem('carrinho');
  localStorage.removeItem('pedidoAtual');
  window.location.href = './index.html';
});
