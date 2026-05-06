# Tela: Detalhe do Produto

## Objetivo

A tela de detalhe do produto apresenta informacoes especificas do item selecionado no cardapio e permite que o cliente defina a quantidade antes de adicionar o produto ao pedido.

## Funcionalidades

- Exibicao do nome do produto.
- Exibicao do preco formatado.
- Exibicao da descricao do produto.
- Exibicao da imagem real do produto selecionado.
- Controle de quantidade com botoes de aumentar e diminuir.
- Botao para adicionar o item ao pedido.
- Botao de retorno ao cardapio.

## Regras e comportamento

A tela utiliza o produto salvo em `produtoSelecionado` no `localStorage`. Quando esse dado existe, as informacoes do produto sao carregadas na interface, incluindo nome, preco, descricao e imagem.

O controle de quantidade inicia em 1. O botao de diminuir nao permite reduzir a quantidade abaixo de 1. Ao clicar em "Adicionar ao pedido", o produto e salvo no `localStorage` dentro da chave `carrinho`, contendo identificador, nome, preco, quantidade escolhida e imagem quando disponivel. Depois disso, o usuario e direcionado para a tela de carrinho.

Quando o produto adicionado ja existe no carrinho, a logica soma a nova quantidade ao item existente em vez de criar uma entrada duplicada. Esse agrupamento e tratado pela logica do carrinho a partir do identificador do produto ou, quando necessario, pelo nome.

## Fluxo do usuario

O usuario chega a esta tela apos selecionar um produto no cardapio. Ele revisa as informacoes, ajusta a quantidade desejada e adiciona o produto ao carrinho. Caso desista, pode retornar ao cardapio pelo botao de voltar.

## Implementacao relacionada

- HTML: `frontend/html/detalhe.html`
- CSS: `frontend/css/style.css`
- JS: `frontend/js/detalhe.js`
