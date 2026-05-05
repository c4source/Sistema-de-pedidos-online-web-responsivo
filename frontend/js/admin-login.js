var adminUsernameInput = document.getElementById('admin-username');
var adminPasswordInput = document.getElementById('admin-password');
var adminLoginButton = document.getElementById('admin-login-button');
var adminLoginError = document.getElementById('admin-login-error');
var adminBackButton = document.getElementById('admin-back-button');

if (localStorage.getItem('adminLogado') === 'true') {
  window.location.href = './admin-dashboard.html';
}

function showAdminLoginError(message) {
  adminLoginError.textContent = message;
}

function clearAdminLoginError() {
  adminLoginError.textContent = '';
}

function validateAdminLogin() {
  var username = adminUsernameInput.value.trim();
  var password = adminPasswordInput.value.trim();

  if (!username || !password) {
    showAdminLoginError('Informe usuário e senha.');
    return false;
  }

  if (username !== 'admin' || password !== '123456') {
    showAdminLoginError('Usuário ou senha inválidos.');
    return false;
  }

  localStorage.setItem('adminLogado', 'true');
  window.location.href = './admin-dashboard.html';
  return true;
}

adminLoginButton.addEventListener('click', validateAdminLogin);

adminPasswordInput.addEventListener('keydown', function (event) {
  if (event.key === 'Enter') {
    validateAdminLogin();
  }
});

adminUsernameInput.addEventListener('input', clearAdminLoginError);
adminPasswordInput.addEventListener('input', clearAdminLoginError);

adminBackButton.addEventListener('click', function () {
  window.location.href = './index.html';
});
