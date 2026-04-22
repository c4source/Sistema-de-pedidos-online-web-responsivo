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

Na tela de cardapio, o usuario consulta os produtos disponiveis, utiliza busca e filtros por categoria e escolhe um item. Em seguida, a tela de detalhe permite revisar informacoes e definir a quantidade antes de adicionar ao carrinho. No carrinho, o pedido pode ser revisado, com alteracao de quantidades ou remocao de itens. O checkout coleta dados do cliente e tipo de entrega. Por fim, a tela de confirmacao apresenta o resumo final do pedido.

## Persistencia no frontend

No estado atual do MVP, a persistencia temporaria do fluxo no navegador e feita por meio de `localStorage`.

Principais informacoes armazenadas:

- `produtoSelecionado`: produto escolhido no cardapio para exibicao na tela de detalhe.
- `carrinho`: lista de itens adicionados ao pedido, com nome, preco e quantidade.

## Integracao futura

A integracao com API, backend e banco de dados sera tratada em outra etapa do projeto. Portanto, esta documentacao descreve o comportamento atual do frontend estatico e interativo, sem assumir persistencia definitiva em servidor.

