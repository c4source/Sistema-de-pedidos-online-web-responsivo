# Documentacao da Area Admin

## Visao geral

Este diretorio documenta a area administrativa do sistema web de pizzaria desenvolvido no PIM 3. A documentacao esta organizada por tela e funcionalidade, com foco no comportamento esperado do MVP e na operacao administrativa do sistema.

A area Admin permite acompanhar pedidos, gerenciar produtos do cardapio e organizar a fila de preparo da cozinha. No estado atual do projeto, todas as telas usam HTML, CSS e JavaScript puro, sem frameworks e sem integracao com API ou backend.

## Objetivo do Admin no MVP

O objetivo da area administrativa e oferecer uma visao operacional basica para a pizzaria. Ela permite que o administrador acompanhe pedidos recebidos, atualize seus status, cadastre e mantenha produtos do cardapio e visualize a fila de preparo da cozinha.

Como o projeto ainda esta em fase de MVP, a persistencia dos dados e temporaria e feita no navegador por meio de `localStorage`.

## Telas administrativas

- [Login Admin](./login-admin.md)
- [Dashboard Admin](./dashboard-admin.md)
- [Admin Pedidos](./pedidos-admin.md)
- [Admin Produtos](./produtos-admin.md)
- [Fila da Cozinha](./fila-cozinha.md)

## Fluxo de navegacao Admin

O fluxo principal da area administrativa segue a sequencia:

Login Admin -> Dashboard Admin -> Pedidos

Login Admin -> Dashboard Admin -> Produtos

Login Admin -> Dashboard Admin -> Fila da Cozinha

O Dashboard funciona como ponto central de navegacao, reunindo metricas e atalhos para as principais operacoes administrativas.

## Integracao com localStorage

No estado atual do MVP, a area Admin utiliza as seguintes chaves principais no `localStorage`:

- `adminLogado`: controla se o administrador esta autenticado na sessao local.
- `pedidos`: armazena os pedidos realizados pelo cliente, incluindo dados do cliente, itens, total, status e data/hora.
- `produtos`: armazena os produtos exibidos no cardapio e gerenciados pela tela Admin Produtos.

Esses dados sao utilizados temporariamente no frontend. Em uma versao futura, a autenticacao, os pedidos e os produtos devem ser integrados a uma API/backend e persistidos no banco de dados.

## Observacoes

A area Admin nao representa uma autenticacao definitiva, pois ainda nao existe backend. O login e os dados administrativos sao simulados no frontend para permitir a demonstracao funcional do MVP.
