# Fila da Cozinha

## Objetivo

A tela Fila da Cozinha oferece uma visão operacional dos pedidos que precisam ser preparados. Ela foi criada para apoiar o acompanhamento da produção na cozinha, separando os pedidos por etapa de preparo.

## Funcionalidades

- Exibicao dos pedidos em tres colunas operacionais.
- Contadores de pedidos recebidos, em preparo, prontos e total na fila.
- Cards com dados essenciais para preparo.
- Avanco de status de pedido recebido para em preparo.
- Avanco de status de pedido em preparo para pronto.
- Estado vazio por coluna.
- Navegacao para Dashboard, Admin Pedidos e Login.

## Diferenca entre Admin Pedidos e Fila da Cozinha

Admin Pedidos é a tela de gestão completa dos pedidos. Ela permite visualizar todos os status, cancelar pedidos recebidos e finalizar pedidos prontos.

Fila da Cozinha é uma tela operacional de preparo. Ela mostra apenas pedidos que fazem parte da produção e permite somente avançar o preparo até `pronto`.

## Dados Utilizados

Os pedidos são lidos da API pela rota protegida `GET /api/Pedido`, usando `Authorization: Bearer {adminToken}`.

A tela exibe apenas pedidos com os status:

- `recebido`
- `em_preparo`
- `pronto`

Os status abaixo nao aparecem na Fila da Cozinha:

- `finalizado`
- `cancelado`

## Regras de Negocio

A tela possui proteção de rota por meio da chave `adminToken`.

As colunas da fila sao:

- Recebidos.
- Em preparo.
- Prontos.

As acoes permitidas sao:

- `recebido` -> `em_preparo`
- `em_preparo` -> `pronto`

A cozinha nao finaliza pedido e nao cancela pedido. A finalizacao e o cancelamento continuam sendo responsabilidade da tela Admin Pedidos.

Dentro de cada coluna, os pedidos sao exibidos dos mais antigos para os mais recentes, usando `dataHora` quando disponivel.

## Fluxo da Tela

O administrador acessa a Fila da Cozinha pelo Dashboard. A tela separa os pedidos em colunas conforme o status atual.

Pedidos recebidos exibem o botao "Iniciar preparo". Pedidos em preparo exibem o botao "Marcar como pronto". Pedidos prontos exibem apenas a indicacao "Pedido pronto".

## Integração com API

Quando a cozinha avança um pedido, o sistema altera apenas o campo `status` do pedido correspondente pela rota `PATCH /api/Pedido/{id}/status`.

Como Admin Pedidos, Dashboard e Fila da Cozinha usam os pedidos persistidos pela API, as alterações feitas na cozinha refletem nas demais telas administrativas quando carregadas novamente.

## Observacoes

A Fila da Cozinha nao cria pedidos, nao remove pedidos e nao altera dados do cliente, endereco, itens ou total. Sua responsabilidade e somente operacional: organizar e avancar a etapa de preparo.

## Testes Realizados

- Bloqueio de acesso sem login.
- Exibicao apenas dos pedidos `recebido`, `em_preparo` e `pronto`.
- Ocultacao de pedidos `finalizado` e `cancelado`.
- Separacao correta por colunas.
- Ordenacao dos pedidos mais antigos primeiro dentro de cada coluna.
- Avanco de `recebido` para `em_preparo`.
- Avanco de `em_preparo` para `pronto`.
- Confirmacao de que a cozinha nao finaliza nem cancela pedidos.
- Reflexo das mudancas em Admin Pedidos e Dashboard.
