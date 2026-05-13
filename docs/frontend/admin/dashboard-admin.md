# Dashboard Admin

## Objetivo

O Dashboard Admin é a tela principal da área administrativa. Ele apresenta uma visão geral dos pedidos e oferece atalhos para as demais telas de operação do sistema.

## Funcionalidades

- Exibição de métricas administrativas.
- Listagem dos pedidos recentes.
- Ações rápidas para navegar entre as telas Admin.
- Botão para visualizar o cardápio do cliente.
- Botão para sair da área administrativa.

## Dados Utilizados

O Dashboard lê os pedidos reais pela rota protegida `GET /api/Pedido`, usando `Authorization: Bearer {adminToken}`.

As metricas exibidas sao:

- Total de pedidos.
- Pedidos recebidos.
- Pedidos em preparo.
- Pedidos finalizados.
- Faturamento total.

## Regras de Negocio

A tela possui proteção de rota. Se `adminToken` não estiver disponível, o usuário é redirecionado para o Login Admin.

As métricas são calculadas a partir dos pedidos retornados pela API. O faturamento total corresponde à soma dos valores dos pedidos registrados.

## Fluxo da Tela

Após o login, o administrador acessa o Dashboard. A tela carrega os dados da API, calcula as métricas e exibe os pedidos recentes.

Na area de acoes rapidas, o administrador pode acessar:

- Ver pedidos.
- Gerenciar produtos.
- Fila da cozinha.
- Ver cardapio.
- Sair.

## Integração com API e localStorage

As chaves utilizadas são:

- `adminToken`: valida o acesso administrativo e autoriza chamadas protegidas na API.

Ao clicar em sair, o sistema remove `adminToken` e redireciona para a tela de login.

## Relacao com outras telas Admin

O Dashboard funciona como ponto central da área administrativa. Alterações feitas em Admin Pedidos ou Fila da Cozinha refletem no Dashboard quando a tela é aberta novamente, pois todas usam os pedidos persistidos no PostgreSQL por meio da API.

## Testes Realizados

- Bloqueio de acesso sem `adminToken`.
- Exibição das métricas com base nos pedidos retornados pela API.
- Listagem de pedidos recentes.
- Navegação para Admin Pedidos, Admin Produtos e Fila da Cozinha.
- Remoção de `adminToken` ao sair.
