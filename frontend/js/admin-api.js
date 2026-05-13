var ADMIN_API_BASE_URL = 'http://localhost:5162/api';

function getAdminToken() {
  return localStorage.getItem('adminToken');
}

function hasAdminSession() {
  return Boolean(getAdminToken());
}

function clearAdminSession() {
  localStorage.removeItem('adminToken');
  localStorage.removeItem('adminNome');
  localStorage.removeItem('adminPerfil');
  localStorage.removeItem('adminLogado');
}

function redirectToAdminLogin() {
  window.location.href = './admin-login.html';
}

function requireAdminSession() {
  if (!hasAdminSession()) {
    redirectToAdminLogin();
    return false;
  }

  return true;
}

function handleUnauthorized(response) {
  if (response.status === 401) {
    clearAdminSession();
    redirectToAdminLogin();
    return true;
  }

  return false;
}

function normalizeApiStatus(status) {
  return String(status || '').trim().toLowerCase().replace(/\s+/g, '_');
}

function normalizeApiOrder(apiOrder) {
  var rua = apiOrder.ruaEntrega || apiOrder.RuaEntrega;
  var numero = apiOrder.numeroEntrega || apiOrder.NumeroEntrega;
  var bairro = apiOrder.bairroEntrega || apiOrder.BairroEntrega;
  var complemento = apiOrder.complementoEntrega || apiOrder.ComplementoEntrega;
  var itens = apiOrder.itens || apiOrder.Itens || [];

  return {
    id: apiOrder.id || apiOrder.Id,
    codigo: apiOrder.codigo || apiOrder.Codigo,
    cliente: {
      nome: apiOrder.nomeCliente || apiOrder.NomeCliente,
      telefone: apiOrder.telefoneCliente || apiOrder.TelefoneCliente
    },
    observacoes: apiOrder.observacoes || apiOrder.Observacoes,
    dataHora: apiOrder.dataHora || apiOrder.DataHora,
    status: normalizeApiStatus(apiOrder.status || apiOrder.Status),
    total: Number(apiOrder.valorTotal || apiOrder.ValorTotal || 0),
    tipoEntrega: apiOrder.tipoEntrega || apiOrder.TipoEntrega,
    endereco: rua || numero || bairro || complemento ? {
      rua: rua,
      numero: numero,
      bairro: bairro,
      complemento: complemento
    } : null,
    pagamento: {
      formaPagamento: apiOrder.formaPagamento || apiOrder.FormaPagamento,
      statusPagamento: apiOrder.statusPagamento || apiOrder.StatusPagamento
    },
    itens: itens.map(function (item) {
      return {
        id: item.id || item.Id,
        codProd: item.codProd || item.CodProd,
        nome: item.produto || item.Produto || 'Produto não informado',
        quantidade: item.quantidade || item.Quantidade || 0,
        precoUnitario: Number(item.precoUnitario || item.PrecoUnitario || 0),
        subtotal: Number(item.subtotal || item.Subtotal || 0)
      };
    })
  };
}

async function fetchAdminOrders() {
  var response = await fetch(ADMIN_API_BASE_URL + '/Pedido', {
    headers: {
      Authorization: 'Bearer ' + getAdminToken()
    }
  });

  if (handleUnauthorized(response)) {
    return [];
  }

  if (!response.ok) {
    throw new Error('Não foi possível carregar os pedidos.');
  }

  var orders = await response.json();
  return orders.map(normalizeApiOrder);
}

async function updateAdminOrderStatus(orderId, status) {
  var response = await fetch(ADMIN_API_BASE_URL + '/Pedido/' + orderId + '/status', {
    method: 'PATCH',
    headers: {
      Authorization: 'Bearer ' + getAdminToken(),
      'Content-Type': 'application/json'
    },
    body: JSON.stringify({ status: status })
  });

  if (handleUnauthorized(response)) {
    return false;
  }

  if (!response.ok) {
    throw new Error('Não foi possível atualizar o status do pedido.');
  }

  return true;
}
