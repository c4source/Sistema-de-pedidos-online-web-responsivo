# Tela: Checkout

## Objetivo

A tela de checkout coleta os dados basicos do cliente e apresenta o resumo do pedido antes da confirmacao. Ela representa a etapa final de preenchimento antes de concluir o pedido no MVP.

## Funcionalidades

- Campo para preenchimento do nome do cliente.
- Campo para preenchimento do telefone.
- Mascara simples de telefone durante a digitacao.
- Escolha entre entrega e retirada.
- Exibicao condicional dos campos de endereco quando a opcao "Entrega" esta selecionada.
- Resumo dos itens do pedido.
- Exibicao do total do pedido.
- Botao para confirmar o pedido.
- Botao de retorno ao carrinho.

## Regras e comportamento

O resumo do pedido e carregado a partir da chave `carrinho` no `localStorage`. Se nao houver itens, a tela informa que nao ha itens no carrinho e exibe total zerado.

O tipo de entrega inicia como "Entrega", exibindo os campos de endereco. Quando o usuario seleciona "Retirada", a secao de endereco e ocultada. A tela possui campos para nome, telefone, rua, numero e bairro, mas no estado atual do frontend nao ha validacao bloqueando a confirmacao do pedido.

Ao confirmar o pedido, o usuario e direcionado para a tela de confirmacao. A integracao com backend para registro definitivo do pedido ainda nao esta implementada nesta etapa.

## Fluxo do usuario

O usuario acessa o checkout a partir do carrinho, revisa o resumo do pedido, informa seus dados e escolhe o tipo de entrega. Em seguida, confirma o pedido e segue para a tela de confirmacao.

## Implementacao relacionada

- HTML: `frontend/html/checkout.html`
- CSS: `frontend/css/style.css`
- JS: `frontend/js/checkout.js`

