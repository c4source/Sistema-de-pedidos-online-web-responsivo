# Tela: Detalhe do Produto

## Objetivo

A tela de detalhe do produto apresenta informacoes especificas do item selecionado no cardapio e permite que o cliente defina a quantidade antes de adicionar o produto ao pedido.

## Funcionalidades

- Exibicao do nome do produto.
- Exibicao do preco formatado.
- Exibicao da descricao do produto.
- Area visual reservada para imagem ou representacao do produto.
- Controle de quantidade com botoes de aumentar e diminuir.
- Botao para adicionar o item ao pedido.
- Botao de retorno ao cardapio.

## Regras e comportamento

A tela utiliza o produto salvo em `produtoSelecionado` no `localStorage`. Quando esse dado existe, as informacoes do produto sao carregadas na interface.

O controle de quantidade inicia em 1. O botao de diminuir nao permite reduzir a quantidade abaixo de 1. Ao clicar em "Adicionar ao pedido", o produto e salvo no `localStorage` dentro da chave `carrinho`, contendo nome, preco e quantidade escolhida. Depois disso, o usuario e direcionado para a tela de carrinho.

## Fluxo do usuario

O usuario chega a esta tela apos selecionar um produto no cardapio. Ele revisa as informacoes, ajusta a quantidade desejada e adiciona o produto ao carrinho. Caso desista, pode retornar ao cardapio pelo botao de voltar.

## Implementacao relacionada

- HTML: `frontend/html/detalhe.html`
- CSS: `frontend/css/style.css`
- JS: `frontend/js/detalhe.js`

