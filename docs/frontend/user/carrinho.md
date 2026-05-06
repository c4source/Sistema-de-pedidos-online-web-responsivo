# Tela: Carrinho

## Objetivo

A tela de carrinho permite que o cliente revise os itens adicionados antes de prosseguir para a finalizacao do pedido. Ela funciona como uma etapa de conferencia e ajuste do pedido.

## Funcionalidades

- Listagem dos itens armazenados no carrinho.
- Exibicao da imagem real, nome, preco e quantidade de cada item.
- Controle para aumentar ou diminuir a quantidade.
- Remocao de itens individuais.
- Calculo automatico do total do pedido.
- Estado visual para carrinho vazio.
- Navegacao para continuar comprando.
- Navegacao para a tela de checkout.

## Regras e comportamento

Os itens sao carregados a partir da chave `carrinho` no `localStorage`. O carrinho ainda utiliza essa persistencia local para manter os produtos durante o fluxo do pedido. Ao alterar a quantidade ou remover um item, o carrinho e atualizado novamente no `localStorage`.

Produtos repetidos sao agrupados pela logica do carrinho. Quando dois itens representam o mesmo produto, suas quantidades sao somadas, evitando duplicidade visual na lista.

O total do pedido e calculado multiplicando o preco de cada item pela sua quantidade e somando todos os resultados. Quando o carrinho nao possui itens, a lista, o total e as acoes principais sao ocultados, e a tela exibe uma mensagem de carrinho vazio com opcao de retorno ao cardapio.

A quantidade minima de um item no carrinho e 1. Para remover completamente um produto, o usuario deve utilizar o botao "Remover".

## Fluxo do usuario

O usuario chega ao carrinho apos adicionar um produto pela tela de detalhe. Nessa etapa, ele pode revisar o pedido, alterar quantidades, remover itens, voltar ao cardapio para continuar comprando ou avancar para o checkout.

## Implementacao relacionada

- HTML: `frontend/html/carrinho.html`
- CSS: `frontend/css/style.css`
- JS: `frontend/js/carrinho.js`
