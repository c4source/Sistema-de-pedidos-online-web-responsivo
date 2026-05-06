# Documentação da Área Admin

## Visão geral

Este diretório documenta a área administrativa do sistema web de pizzaria desenvolvido no PIM 3. A documentação está organizada por tela e funcionalidade, com foco no comportamento esperado do MVP e na operação administrativa do sistema.

A área Admin permite acompanhar pedidos, gerenciar produtos do cardápio e organizar a fila de preparo da cozinha. No estado atual do projeto, todas as telas usam HTML, CSS e JavaScript puro, sem frameworks e sem integração com API/backend.

## Objetivo do Admin no MVP

O objetivo da área administrativa é oferecer uma visão operacional básica para a pizzaria. Ela permite que o administrador acompanhe pedidos recebidos, atualize seus status, cadastre e mantenha produtos do cardápio e visualize a fila de preparo da cozinha.

Como o projeto ainda está em fase de MVP, a persistência dos dados é temporária e feita no navegador por meio do `localStorage`.

## Telas administrativas

- [Login Admin](./login-admin.md)
- [Dashboard Admin](./dashboard-admin.md)
- [Admin Pedidos](./pedidos-admin.md)
- [Admin Produtos](./produtos-admin.md)
- [Fila da Cozinha](./fila-cozinha.md)

## Fluxo de navegação Admin

O Dashboard Admin funciona como ponto central de navegação da área administrativa.

Fluxo principal:

```text
Login Admin
→ Dashboard Admin
   → Pedidos
   → Produtos
   → Fila da Cozinha
```

A partir do Dashboard, o administrador acessa as telas operacionais conforme a necessidade: gestão completa de pedidos, manutenção de produtos ou acompanhamento da cozinha.

## Integração com localStorage

No estado atual do MVP, a área Admin utiliza as seguintes chaves principais no `localStorage`:

- `adminLogado`: controla se o administrador está autenticado na sessão local.
- `pedidos`: armazena os pedidos realizados pelo cliente, incluindo dados do cliente, itens, total, status e data/hora.
- `produtos`: armazena os produtos exibidos no cardápio e gerenciados pela tela Admin Produtos.

Esses dados são utilizados temporariamente no frontend. Em uma versão futura, a autenticação, os pedidos e os produtos devem ser integrados a uma API/backend e persistidos no banco de dados.

## Observações

A área Admin não representa uma autenticação definitiva, pois ainda não existe backend. O login e os dados administrativos são simulados no frontend para permitir a demonstração funcional do MVP.

A documentação desta pasta descreve o comportamento atual da interface administrativa, sem assumir funcionalidades que ainda não foram implementadas.
