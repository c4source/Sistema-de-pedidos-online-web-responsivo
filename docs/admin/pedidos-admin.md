# Admin Pedidos

## Objetivo

A tela Admin Pedidos permite que o administrador acompanhe o historico de pedidos e atualize o status de cada pedido conforme o fluxo operacional da pizzaria.

## Funcionalidades

- Listagem dos pedidos salvos.
- Filtros por status.
- Visualizacao dos dados completos de cada pedido.
- Atualizacao de status conforme regras do fluxo.
- Cancelamento de pedido quando ainda esta recebido.
- Estado vazio quando nao ha pedidos ou quando nenhum pedido corresponde ao filtro.

## Dados Utilizados

Os pedidos sao lidos da chave `pedidos` no `localStorage`.

Cada card de pedido exibe:

- Codigo do pedido.
- Cliente.
- Telefone.
- Tipo de entrega.
- Itens do pedido.
- Endereco, quando o tipo de entrega e entrega.
- Total.
- Data/hora.
- Status.

## Regras de Negocio

A tela possui protecao de rota por meio da chave `adminLogado`. Sem login administrativo, o usuario e redirecionado para o Login Admin.

Os filtros disponiveis sao:

- Todos.
- Recebido.
- Em preparo.
- Pronto.
- Finalizado.
- Cancelado.

O fluxo principal de status e:

recebido -> em_preparo -> pronto -> finalizado

O cancelamento e permitido apenas quando o pedido esta com status `recebido`. Pedidos `finalizado` ou `cancelado` sao exibidos em modo somente leitura, sem acoes de avancar status.

## Fluxo da Tela

O administrador acessa a tela, visualiza os pedidos e pode filtrar por status. Em cada card, as acoes disponiveis dependem do status atual do pedido.

Quando o pedido esta recebido, o administrador pode iniciar o preparo ou cancelar. Quando esta em preparo, pode marcar como pronto. Quando esta pronto, pode finalizar o pedido. Pedidos finalizados ou cancelados permanecem apenas para consulta.

## Integracao com localStorage

Ao alterar o status de um pedido, o sistema atualiza o objeto correspondente dentro do array `pedidos` e salva novamente em `localStorage.pedidos`.

Essa persistencia local permite que o Dashboard e a Fila da Cozinha reflitam as alteracoes quando carregados novamente.

## Observacoes

A tela Admin Pedidos representa a visao administrativa completa do pedido. Ela permite finalizar e cancelar, enquanto a Fila da Cozinha possui escopo mais restrito e operacional.

## Testes Realizados

- Bloqueio de acesso sem login.
- Listagem correta dos pedidos.
- Filtros por status.
- Alteracao de `recebido` para `em_preparo`.
- Alteracao de `em_preparo` para `pronto`.
- Alteracao de `pronto` para `finalizado`.
- Cancelamento apenas quando o pedido esta recebido.
- Reflexo das alteracoes no Dashboard.
