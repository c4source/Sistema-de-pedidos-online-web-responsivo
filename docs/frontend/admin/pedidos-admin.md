# Admin Pedidos

## Objetivo

A tela Admin Pedidos permite que o administrador acompanhe o histórico de pedidos e atualize o status de cada pedido conforme o fluxo operacional da pizzaria.

## Funcionalidades

- Listagem dos pedidos salvos.
- Filtros por status.
- Visualização dos dados completos de cada pedido.
- Atualização de status conforme regras do fluxo.
- Cancelamento de pedido quando ainda está recebido.
- Estado vazio quando não há pedidos ou quando nenhum pedido corresponde ao filtro.

## Dados Utilizados

Os pedidos são lidos da API pela rota protegida `GET /api/Pedido`, usando `Authorization: Bearer {adminToken}`.

Cada card de pedido exibe:

- Código do pedido.
- Cliente.
- Telefone.
- Tipo de entrega.
- Itens do pedido.
- Endereço, quando o tipo de entrega é entrega.
- Total.
- Data/hora.
- Status.
- Forma de pagamento.
- Status do pagamento.

## Regras de Negocio

A tela possui proteção de rota por meio da chave `adminToken`. Sem login administrativo, o usuário é redirecionado para o Login Admin.

Os filtros disponiveis sao:

- Todos.
- Recebido.
- Em preparo.
- Pronto.
- Finalizado.
- Cancelado.

O fluxo principal de status e:

recebido -> em_preparo -> pronto -> finalizado

O cancelamento é permitido apenas quando o pedido está com status `recebido`. Pedidos `finalizado` ou `cancelado` são exibidos em modo somente leitura, sem ações de avançar status.

O pagamento é exibido apenas como informação operacional. Nesta etapa, o Admin Pedidos não altera forma de pagamento nem status do pagamento.

## Fluxo da Tela

O administrador acessa a tela, visualiza os pedidos e pode filtrar por status. Em cada card, as acoes disponiveis dependem do status atual do pedido.

Quando o pedido esta recebido, o administrador pode iniciar o preparo ou cancelar. Quando esta em preparo, pode marcar como pronto. Quando esta pronto, pode finalizar o pedido. Pedidos finalizados ou cancelados permanecem apenas para consulta.

## Integração com API

Ao alterar o status de um pedido, o sistema envia a atualização para a API pela rota `PATCH /api/Pedido/{id}/status`.

Essa persistência no PostgreSQL permite que o Dashboard e a Fila da Cozinha reflitam as alterações quando carregados novamente.

Quando o pedido possui dados de pagamento, a tela exibe a forma escolhida pelo cliente e o status do pagamento.

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
- Exibicao de forma de pagamento e status do pagamento.
- Compatibilidade com pedidos antigos sem dados de pagamento.
