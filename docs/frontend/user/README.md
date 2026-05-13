# Documentacao do Frontend

## Visao geral

Este diretorio documenta o modulo frontend do sistema web de pizzaria desenvolvido no PIM 3. A documentacao esta organizada por tela, com foco funcional e na experiencia do usuario, seguindo o fluxo ja previsto nos materiais de UX e wireframes do projeto.

O frontend atual utiliza HTML, CSS e JavaScript puro, sem frameworks. As telas implementadas representam o fluxo principal do cliente no MVP: visualizacao do cardapio, consulta do produto, montagem do carrinho, preenchimento dos dados do pedido e confirmacao final.

## Telas documentadas

- [Cardapio](./cardapio.md)
- [Detalhe do Produto](./detalhe-produto.md)
- [Carrinho](./carrinho.md)
- [Checkout](./checkout.md)
- [Confirmacao](./confirmacao.md)

## Fluxo do usuario

O fluxo principal do cliente segue a sequencia:

Cardapio -> Detalhe do Produto -> Carrinho -> Checkout -> Confirmacao

Na tela de cardapio, o usuario consulta os produtos disponiveis, utiliza busca e filtros por categoria e escolhe um item. Em seguida, a tela de detalhe permite revisar informacoes e definir a quantidade antes de adicionar ao carrinho. No carrinho, o pedido pode ser revisado, com alteracao de quantidades ou remocao de itens. O checkout coleta dados do cliente e tipo de entrega, e envia o pedido para a API publica `POST /api/Pedido/checkout-mvp`. Por fim, a tela de confirmacao apresenta o resumo final do pedido.

## Persistencia no frontend

No estado atual do MVP, a persistencia temporaria do fluxo no navegador e feita por meio de `localStorage`.

Principais informacoes armazenadas:

- `produtoSelecionado`: produto escolhido no cardapio para exibicao na tela de detalhe.
- `carrinho`: lista de itens adicionados ao pedido, com nome, preco, quantidade e, quando disponivel, imagem do produto.
- `pedidoAtual`: pedido confirmado no checkout. Essa chave armazena os dados do cliente, tipo de entrega, endereco quando necessario, itens, total, status e codigo do pedido. Ela e utilizada pela tela de confirmacao para apresentar o resumo final do pedido confirmado.

Essas informacoes permitem que o usuario avance entre as telas sem perder o estado do fluxo. O carrinho continua local e temporario no MVP, mas o checkout salva o pedido definitivo no PostgreSQL por meio da API.

## Integracao com API

O cardapio consulta produtos pela API publica `GET /api/Produto`, e o checkout cria o pedido pela rota publica `POST /api/Pedido/checkout-mvp`. O cliente comum nao usa login nem JWT no MVP.
