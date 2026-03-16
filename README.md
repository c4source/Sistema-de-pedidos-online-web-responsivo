#  Sistema Web de Pedidos para Restaurante
> **Projeto Integrador Multidisciplinar (PIM III) – Universidade Paulista (UNIP)**

##  Objetivo
Desenvolver um sistema web completo e intuitivo para a gestão de pedidos de um restaurante. O foco da aplicação é oferecer um fluxo de navegação simples e direto, permitindo que o cliente final visualize o cardápio e realize pedidos com facilidade, enquanto a equipe interna gerencia a operação de forma ágil e eficiente.

---

##  Escopo do Projeto (MVP)

A aplicação foi dividida em dois módulos principais para atender às necessidades do negócio:

###  Área do Cliente
- [x] **Catálogo Digital:** Visualização dinâmica do cardápio de produtos.
- [x] **Carrinho de Compras:** Gestão temporária de itens selecionados.
- [x] **Checkout Simplificado:** Finalização de pedido com captura de dados essenciais (Nome, Telefone, Modalidade: Entrega/Retirada).
- [x] **Confirmação:** Feedback visual de pedido realizado com sucesso.

###  Área Administrativa
- [x] **Autenticação:** Sistema de login seguro para o administrador.
- [x] **Gestão de Estoque (CRUD):** Criação, leitura, atualização e exclusão de produtos do cardápio.
- [x] **Painel de Controle:** Visualização de todos os pedidos recebidos.
- [x] **Workflow de Status:** Alteração e acompanhamento do status operacional de cada pedido.
- [x] **Business Intelligence:** Geração de relatórios gerenciais simplificados (ex: total de vendas do dia).

---

## 🛠️ Arquitetura e Tecnologias

O ecossistema do projeto foi construído utilizando as seguintes tecnologias:

* **Frontend:** HTML5, CSS3 e JavaScript (Vanilla)
* **Backend:** C# com ASP.NET Core Web API
* **Banco de Dados:** PostgreSQL

---

##  Estrutura do Projeto

A organização de pastas segue os padrões de mercado para separação de responsabilidades (Clean Architecture):

```text
 raiz-do-projeto
 ┣  backend/           # API REST em C# (ASP.NET Core), Controllers e Models
 ┣  database/          # Scripts SQL de criação e população do PostgreSQL
 ┣  frontend/          # Interfaces do usuário (HTML, CSS, JS) e assets
 ┗  README.md          # Documentação central do projeto
