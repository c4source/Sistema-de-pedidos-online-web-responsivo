var productCards = document.querySelectorAll('.product-card');

productCards.forEach(function (card) {
  card.addEventListener('click', function () {
    var targetPage = card.getAttribute('data-link');

    if (targetPage) {
      window.location.href = './' + targetPage;
    }
  });
});
