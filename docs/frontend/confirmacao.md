# Tela: Confirmacao

## Objetivo

A tela de confirmacao informa ao cliente que o pedido foi concluido no fluxo do frontend e apresenta um resumo final dos itens. Ela encerra a jornada principal do pedido no MVP.

## Funcionalidades

- Mensagem de sucesso do pedido.
- Informacao de que o pedido esta sendo preparado.
- Resumo dos itens do pedido.
- Exibicao do total final.
- Botao para voltar ao cardapio.
- Botao de retorno para o checkout.

## Regras e comportamento

O resumo final e carregado a partir da chave `carrinho` no `localStorage`. Se o carrinho estiver vazio, a tela informa que nao ha itens no pedido e exibe total zerado.

Ao clicar em "Voltar ao Cardapio", o sistema remove a chave `carrinho` do `localStorage` e redireciona o usuario para a tela de cardapio. Esse comportamento limpa o pedido atual e prepara o fluxo para uma nova compra.

A tela representa a confirmacao visual do MVP. O envio real do pedido para API ou backend ainda nao esta implementado nesta etapa.

## Fluxo do usuario

O usuario chega a esta tela apos confirmar o pedido no checkout. Ele confere a mensagem de sucesso e o resumo final. Ao retornar ao cardapio, o carrinho e limpo e o fluxo pode ser reiniciado.

## Implementacao relacionada

- HTML: `frontend/html/confirmacao.html`
- CSS: `frontend/css/style.css`
- JS: `frontend/js/confirmacao.js`

