var confirmationBackButton = document.getElementById('confirmation-back-button');
var confirmationHomeButton = document.getElementById('confirmation-home-button');

confirmationBackButton.addEventListener('click', function () {
  window.location.href = './checkout.html';
});

confirmationHomeButton.addEventListener('click', function () {
  window.location.href = './index.html';
});
