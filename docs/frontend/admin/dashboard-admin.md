# Dashboard Admin

## Objetivo

O Dashboard Admin e a tela principal da area administrativa. Ele apresenta uma visao geral dos pedidos e oferece atalhos para as demais telas de operacao do sistema.

## Funcionalidades

- Exibicao de metricas administrativas.
- Listagem dos pedidos recentes.
- Acoes rapidas para navegar entre as telas Admin.
- Botao para visualizar o cardapio do cliente.
- Botao para sair da area administrativa.

## Dados Utilizados

O Dashboard le os pedidos a partir da chave `pedidos` no `localStorage`.

As metricas exibidas sao:

- Total de pedidos.
- Pedidos recebidos.
- Pedidos em preparo.
- Pedidos finalizados.
- Faturamento total.

## Regras de Negocio

A tela possui protecao de rota. Se `adminLogado` nao estiver definido como `true`, o usuario e redirecionado para o Login Admin.

As metricas sao calculadas a partir dos pedidos salvos localmente. O faturamento total corresponde a soma dos valores dos pedidos registrados.

## Fluxo da Tela

Apos o login, o administrador acessa o Dashboard. A tela carrega os dados de `localStorage.pedidos`, calcula as metricas e exibe os pedidos recentes.

Na area de acoes rapidas, o administrador pode acessar:

- Ver pedidos.
- Gerenciar produtos.
- Fila da cozinha.
- Ver cardapio.
- Sair.

## Integracao com localStorage

As chaves utilizadas sao:

- `adminLogado`: valida o acesso administrativo.
- `pedidos`: fornece os dados para metricas e pedidos recentes.

Ao clicar em sair, o sistema remove `adminLogado` e redireciona para a tela de login.

## Relacao com outras telas Admin

O Dashboard funciona como ponto central da area administrativa. Alteracoes feitas em Admin Pedidos ou Fila da Cozinha refletem no Dashboard quando a tela e aberta novamente, pois todas usam os mesmos dados de `localStorage.pedidos`.

## Testes Realizados

- Bloqueio de acesso sem `adminLogado`.
- Exibicao das metricas com base nos pedidos salvos.
- Listagem de pedidos recentes.
- Navegacao para Admin Pedidos, Admin Produtos e Fila da Cozinha.
- Remocao de `adminLogado` ao sair.
