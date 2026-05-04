# Tela: Confirmacao

## Objetivo

A tela de confirmacao informa ao cliente que o pedido foi concluido no fluxo do frontend e apresenta um resumo final dos itens. Ela encerra a jornada principal do pedido no MVP.

## Funcionalidades

- Mensagem de sucesso do pedido.
- Informacao de que o pedido esta sendo preparado.
- Exibicao de codigo curto do pedido.
- Exibicao do status do pedido.
- Exibicao do tipo de entrega.
- Exibicao do tempo estimado.
- Resumo dos itens do pedido.
- Exibicao do total final.
- Botao para voltar ao cardapio.
- Botao de retorno para o checkout.

## Regras e comportamento

O resumo final e carregado a partir da chave `pedidoAtual` no `localStorage`. Essa chave e criada no checkout depois que os dados do pedido passam pela validacao. Se nao houver `pedidoAtual`, o usuario e redirecionado para o cardapio.

A tela exibe um codigo curto do pedido, o status, o tipo de entrega, o tempo estimado, o resumo dos itens e o total. Depois que a confirmacao e renderizada com sucesso, o carrinho e limpo do `localStorage`, pois o pedido ja foi consolidado em `pedidoAtual`.

Ao clicar em "Voltar ao Cardapio", o sistema remove `carrinho` e `pedidoAtual` do `localStorage` e redireciona o usuario para a tela de cardapio. Esse comportamento prepara o fluxo para uma nova compra.

A tela representa a confirmacao visual do MVP. O envio real do pedido para API ou backend ainda nao esta implementado nesta etapa.

## Fluxo do usuario

O usuario chega a esta tela apos confirmar o pedido no checkout. Ele confere a mensagem de sucesso, os dados principais do pedido e o resumo final. Ao retornar ao cardapio, o pedido atual pode ser removido e o fluxo pode ser reiniciado.

## Implementacao relacionada

- HTML: `frontend/html/confirmacao.html`
- CSS: `frontend/css/style.css`
- JS: `frontend/js/confirmacao.js`
