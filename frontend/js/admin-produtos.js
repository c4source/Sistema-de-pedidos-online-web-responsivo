if (requireAdminSession()) {
  initializeAdminProducts();
}

function initializeAdminProducts() {
  var allowedCategories = ['pizza', 'doce', 'bebida', 'combo'];
  var productsTotalElement = document.getElementById('admin-products-total');
  var productsAvailableElement = document.getElementById('admin-products-available');
  var productsUnavailableElement = document.getElementById('admin-products-unavailable');
  var productsStockTotalElement = document.getElementById('admin-products-stock-total');
  var productsBackDashboardButton = document.getElementById('admin-products-back-dashboard');
  var productsLogoutButton = document.getElementById('admin-products-logout');
  var productsForm = document.getElementById('admin-products-form');
  var productFormTitle = document.getElementById('product-form-title');
  var productIdInput = document.getElementById('product-id');
  var productNameInput = document.getElementById('product-name');
  var productCategoryInput = document.getElementById('product-category');
  var productPriceInput = document.getElementById('product-price');
  var productDescriptionInput = document.getElementById('product-description');
  var productImageInput = document.getElementById('product-image');
  var productStockInput = document.getElementById('product-stock');
  var productAvailableInput = document.getElementById('product-available');
  var productSubmitButton = document.getElementById('product-submit-button');
  var productCancelEditButton = document.getElementById('product-cancel-edit-button');
  var productFormError = document.getElementById('product-form-error');
  var productsListElement = document.getElementById('admin-products-list');
  var productsEmptyElement = document.getElementById('admin-products-empty');
  var categoryFilterButtons = document.querySelectorAll('[data-category]');
  var statusFilterButtons = document.querySelectorAll('[data-status]');
  var selectedCategory = 'todos';
  var selectedStatus = 'todos';
  var produtos = [];

  function loadLocalProducts() {
    try {
      return normalizeProducts(JSON.parse(localStorage.getItem('produtos')) || []);
    } catch (error) {
      return [];
    }
  }

  function normalizeProducts(productsList) {
    if (!Array.isArray(productsList)) {
      return [];
    }

    return productsList.map(function (product) {
      return {
        id: product.id || product.codprod,
        nome: product.nome || product.nome_produto || '',
        categoria: product.categoria || '',
        preco: Number(product.preco) || 0,
        imagem: product.imagem || product.imagemUrl || product.imagem_url || '',
        descricao: product.descricao || '',
        estoque: Number.isInteger(Number(product.estoque)) && Number(product.estoque) >= 0 ? Number(product.estoque) : 0,
        disponivel: normalizeAvailability(product)
      };
    });
  }

  function normalizeAvailability(product) {
    var status = String(product.status || product.status_disponibilidade || '').toLowerCase();

    if (product.disponivel === false || status === 'false' || status === 'indisponivel' || status === 'indisponível') {
      return false;
    }

    return true;
  }

  function formatPrice(value) {
    return 'R$ ' + (Number(value) || 0).toFixed(2).replace('.', ',');
  }

  function formatCategory(category) {
    var categoryMap = {
      pizza: 'Pizza',
      doce: 'Doce',
      bebida: 'Bebida',
      combo: 'Combo'
    };

    return categoryMap[category] || 'Sem categoria';
  }

  function formatAvailability(product) {
    return product.disponivel === false ? 'Indisponível' : 'Disponível';
  }

  function getFormData() {
    return {
      id: productIdInput.value,
      nome: productNameInput.value.trim(),
      categoria: productCategoryInput.value,
      preco: Number(String(productPriceInput.value).replace(',', '.')),
      imagem: productImageInput.value.trim(),
      descricao: productDescriptionInput.value.trim(),
      estoque: Number(productStockInput.value),
      disponivel: productAvailableInput.value === 'true'
    };
  }

  function showFormError(message) {
    productFormError.textContent = message;
  }

  function clearFormError() {
    productFormError.textContent = '';
  }

  function validateProduct(productData) {
    if (productData.nome.length < 3) {
      return 'Informe um nome válido.';
    }

    if (allowedCategories.indexOf(productData.categoria) === -1) {
      return 'Selecione uma categoria válida.';
    }

    if (!productData.preco || productData.preco <= 0) {
      return 'Informe um preço válido.';
    }

    if (productData.descricao.length < 5) {
      return 'Informe uma descrição válida.';
    }

    if (!productData.imagem) {
      return 'Informe a imagem do produto.';
    }

    if (!Number.isInteger(productData.estoque) || productData.estoque < 0) {
      return 'Informe um estoque válido.';
    }

    if (productAvailableInput.value !== 'true' && productAvailableInput.value !== 'false') {
      return 'Selecione uma disponibilidade válida.';
    }

    return '';
  }

  function resetForm() {
    productsForm.reset();
    productIdInput.value = '';
    productAvailableInput.value = 'true';
    productFormTitle.textContent = 'Novo produto';
    productSubmitButton.textContent = 'Salvar produto';
    productSubmitButton.disabled = false;
    productCancelEditButton.hidden = true;
    clearFormError();
  }

  async function reloadProducts() {
    produtos = await fetchAdminProducts();
    renderPage();
  }

  async function handleCreateProduct(productData) {
    try {
      productSubmitButton.disabled = true;
      await createAdminProduct(productData);
      await reloadProducts();
      resetForm();
    } catch (error) {
      showFormError('Não foi possível cadastrar o produto.');
    } finally {
      productSubmitButton.disabled = false;
    }
  }

  async function handleEditProduct(productData) {
    var productId = Number(productData.id);
    var productExists = produtos.some(function (product) {
      return Number(product.id) === productId;
    });

    if (!productExists) {
      showFormError('Produto não encontrado.');
      return;
    }

    try {
      productSubmitButton.disabled = true;
      await updateAdminProduct(productId, productData);
      await reloadProducts();
      resetForm();
    } catch (error) {
      showFormError('Não foi possível salvar as alterações.');
    } finally {
      productSubmitButton.disabled = false;
    }
  }

  function editProduct(productId) {
    var product = produtos.find(function (item) {
      return Number(item.id) === Number(productId);
    });

    if (!product) {
      return;
    }

    productIdInput.value = product.id;
    productNameInput.value = product.nome;
    productCategoryInput.value = product.categoria;
    productPriceInput.value = product.preco;
    productDescriptionInput.value = product.descricao;
    productImageInput.value = product.imagem;
    productStockInput.value = product.estoque;
    productAvailableInput.value = product.disponivel === false ? 'false' : 'true';
    productFormTitle.textContent = 'Editar produto';
    productSubmitButton.textContent = 'Salvar alterações';
    productCancelEditButton.hidden = false;
    clearFormError();
    productsForm.scrollIntoView({ behavior: 'smooth', block: 'start' });
  }

  async function deleteProduct(productId) {
    var product = produtos.find(function (item) {
      return Number(item.id) === Number(productId);
    });

    if (!product) {
      return;
    }

    if (!window.confirm('Deseja excluir este produto?')) {
      return;
    }

    try {
      await deleteAdminProduct(product.id);
      await reloadProducts();
    } catch (error) {
      showFormError('Não foi possível excluir o produto.');
    }
  }

  function getFilteredProducts() {
    var filteredProducts = produtos.filter(function (product) {
      var matchesCategory = selectedCategory === 'todos' || product.categoria === selectedCategory;
      var matchesStatus = selectedStatus === 'todos' ||
        (selectedStatus === 'disponivel' && product.disponivel !== false) ||
        (selectedStatus === 'indisponivel' && product.disponivel === false);

      return matchesCategory && matchesStatus;
    });

    if (selectedStatus !== 'todos') {
      return filteredProducts;
    }

    return filteredProducts.slice().sort(function (firstProduct, secondProduct) {
      if (firstProduct.disponivel === false && secondProduct.disponivel !== false) {
        return 1;
      }

      if (firstProduct.disponivel !== false && secondProduct.disponivel === false) {
        return -1;
      }

      return 0;
    });
  }

  function updateSummary() {
    var availableProducts = produtos.filter(function (product) {
      return product.disponivel !== false;
    }).length;
    var unavailableProducts = produtos.filter(function (product) {
      return product.disponivel === false;
    }).length;
    var stockTotal = produtos.reduce(function (total, product) {
      return total + (Number(product.estoque) || 0);
    }, 0);

    productsTotalElement.textContent = produtos.length;
    productsAvailableElement.textContent = availableProducts;
    productsUnavailableElement.textContent = unavailableProducts;
    productsStockTotalElement.textContent = stockTotal;
  }

  function updateActiveFilters() {
    categoryFilterButtons.forEach(function (button) {
      button.classList.toggle('is-active', button.dataset.category === selectedCategory);
    });

    statusFilterButtons.forEach(function (button) {
      button.classList.toggle('is-active', button.dataset.status === selectedStatus);
    });
  }

  function createProductInfo(label, value) {
    var row = document.createElement('p');
    var strong = document.createElement('strong');
    var span = document.createElement('span');

    row.className = 'admin-product-info-row';
    strong.textContent = label + ': ';
    span.textContent = value;

    row.appendChild(strong);
    row.appendChild(span);

    return row;
  }

  function createProductCard(product) {
    var card = document.createElement('article');
    var image = document.createElement('img');
    var content = document.createElement('div');
    var top = document.createElement('div');
    var titleGroup = document.createElement('div');
    var title = document.createElement('h2');
    var status = document.createElement('span');
    var details = document.createElement('div');
    var description = document.createElement('p');
    var actions = document.createElement('div');
    var editButton = document.createElement('button');
    var deleteButton = document.createElement('button');

    card.className = product.disponivel === false ? 'admin-product-card is-unavailable' : 'admin-product-card is-available';
    image.className = 'admin-product-image';
    content.className = 'admin-product-content';
    top.className = 'admin-product-card-top';
    titleGroup.className = 'admin-product-title-group';
    title.className = 'admin-product-title';
    status.className = product.disponivel === false ? 'admin-product-status admin-product-status-unavailable is-unavailable' : 'admin-product-status admin-product-status-available is-available';
    details.className = 'admin-product-details';
    description.className = 'admin-product-description';
    actions.className = 'admin-product-actions';
    editButton.className = 'admin-product-action-button admin-product-action-secondary';
    deleteButton.className = 'admin-product-action-button admin-product-action-secondary';

    image.src = product.imagem;
    image.alt = product.nome;
    title.textContent = product.nome;
    status.textContent = formatAvailability(product);
    description.textContent = product.descricao;
    editButton.type = 'button';
    editButton.textContent = 'Editar';
    deleteButton.type = 'button';
    deleteButton.textContent = 'Excluir';

    editButton.addEventListener('click', function () {
      editProduct(product.id);
    });

    deleteButton.addEventListener('click', function () {
      deleteProduct(product.id);
    });

    titleGroup.appendChild(title);
    titleGroup.appendChild(status);
    top.appendChild(titleGroup);
    details.appendChild(createProductInfo('Categoria', formatCategory(product.categoria)));
    details.appendChild(createProductInfo('Preço', formatPrice(product.preco)));
    details.appendChild(createProductInfo('Estoque', String(product.estoque)));
    details.appendChild(createProductInfo('Status', formatAvailability(product)));
    actions.appendChild(editButton);
    actions.appendChild(deleteButton);
    content.appendChild(top);
    content.appendChild(details);
    content.appendChild(description);
    content.appendChild(actions);
    card.appendChild(image);
    card.appendChild(content);

    return card;
  }

  function updateEmptyState(filteredProducts) {
    if (filteredProducts.length > 0) {
      productsListElement.hidden = false;
      productsEmptyElement.hidden = true;
      return;
    }

    productsListElement.hidden = true;
    productsEmptyElement.hidden = false;

    if (produtos.length === 0) {
      productsEmptyElement.querySelector('.admin-products-empty-title').textContent = 'Nenhum produto cadastrado.';
      productsEmptyElement.querySelector('.admin-products-empty-text').textContent = 'Use o formulário acima para adicionar um produto ao cardápio.';
      return;
    }

    productsEmptyElement.querySelector('.admin-products-empty-title').textContent = 'Nenhum produto encontrado para este filtro.';
    productsEmptyElement.querySelector('.admin-products-empty-text').textContent = '';
  }

  function renderProducts() {
    var filteredProducts = getFilteredProducts();

    productsListElement.innerHTML = '';
    updateEmptyState(filteredProducts);

    filteredProducts.forEach(function (product) {
      productsListElement.appendChild(createProductCard(product));
    });
  }

  function renderPage() {
    updateSummary();
    updateActiveFilters();
    renderProducts();
  }

  function showLoadError() {
    if (produtos.length > 0) {
      showFormError('Não foi possível carregar os produtos da API. Exibindo fallback local.');
      return;
    }

    productsEmptyElement.hidden = false;
    productsEmptyElement.querySelector('.admin-products-empty-title').textContent = 'Não foi possível carregar os produtos.';
    productsEmptyElement.querySelector('.admin-products-empty-text').textContent = 'Verifique se o backend está rodando e tente novamente.';
  }

  async function loadProductsFromApi() {
    try {
      produtos = await fetchAdminProducts();
      renderPage();
    } catch (error) {
      produtos = loadLocalProducts();
      renderPage();
      showLoadError();
    }
  }

  productsForm.addEventListener('submit', async function (event) {
    var productData;
    var validationMessage;

    event.preventDefault();
    productData = getFormData();
    validationMessage = validateProduct(productData);

    if (validationMessage) {
      showFormError(validationMessage);
      return;
    }

    if (productData.id) {
      await handleEditProduct(productData);
      return;
    }

    await handleCreateProduct(productData);
  });

  productCancelEditButton.addEventListener('click', resetForm);

  categoryFilterButtons.forEach(function (button) {
    button.addEventListener('click', function () {
      selectedCategory = button.dataset.category;
      renderPage();
    });
  });

  statusFilterButtons.forEach(function (button) {
    button.addEventListener('click', function () {
      selectedStatus = button.dataset.status;
      renderPage();
    });
  });

  productsBackDashboardButton.addEventListener('click', function () {
    window.location.href = './admin-dashboard.html';
  });

  productsLogoutButton.addEventListener('click', function () {
    clearAdminSession();
    window.location.href = './admin-login.html';
  });

  resetForm();
  loadProductsFromApi();
}
