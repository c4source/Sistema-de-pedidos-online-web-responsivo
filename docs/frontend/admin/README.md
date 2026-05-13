# Documentação da Área Admin

## Visão geral

Este diretório documenta a área administrativa do sistema web de pizzaria desenvolvido no PIM 3. A documentação está organizada por tela e funcionalidade, com foco no comportamento esperado do MVP e na operação administrativa do sistema.

A área Admin permite acompanhar pedidos, gerenciar produtos do cardápio e organizar a fila de preparo da cozinha. No estado atual do projeto, todas as telas usam HTML, CSS e JavaScript puro, sem frameworks, com integração à API/backend para autenticação administrativa, pedidos e produtos.

## Objetivo do Admin no MVP

O objetivo da área administrativa é oferecer uma visão operacional básica para a pizzaria. Ela permite que o administrador acompanhe pedidos recebidos, atualize seus status, cadastre e mantenha produtos do cardápio e visualize a fila de preparo da cozinha.

Como o projeto ainda está em fase de MVP, o frontend público mantém dados temporários no navegador, mas a área Admin já utiliza a API real para autenticação, pedidos e produtos.

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

## Integração com API e localStorage

No estado atual do MVP, a área Admin utiliza a API/backend como fonte principal dos dados administrativos e mantém no `localStorage` apenas informações necessárias para a sessão:

- `adminToken`: token JWT retornado por `POST /api/Auth/login-colaborador`.

Dashboard, Pedidos e Fila da Cozinha consultam pedidos reais pela API. Admin Produtos consulta, cadastra, edita e exclui produtos pela API. As chaves antigas `localStorage.pedidos` e `localStorage.produtos` não são mais a fonte principal da área Admin.

## Observações

A autenticação administrativa é feita pela API com JWT. O cliente comum continua sem login no MVP.

A documentação desta pasta descreve o comportamento atual da interface administrativa, sem assumir funcionalidades que ainda não foram implementadas.
