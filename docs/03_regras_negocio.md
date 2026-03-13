# 📋 Definições de Regras de Negócio (RN)

Este documento detalha as **Regras de Negócio (RN)** do Sistema de Gestão de Pedidos (PIM). Enquanto os requisitos definem *o que* o sistema faz, as regras de negócio definem as **condições, restrições e validações lógicas** que devem ser aplicadas no back-end para que as operações sejam válidas.

---

## 🛑 1. Regras de Produto e Cardápio

| ID | Regra de Negócio | Descrição e Validação |
| :--- | :--- | :--- |
| **RN01** | **Validação de Preço** | O valor de um produto cadastrado deve ser obrigatoriamente maior que zero (`> 0`). Não é permitido o cadastro de produtos com valor negativo ou zerado. |
| **RN02** | **Campos Obrigatórios** | Para cadastrar ou editar um produto, os campos *Nome*, *Preço* e *Status de Disponibilidade* não podem ser nulos ou vazios. |
| **RN03** | **Bloqueio de Exclusão com Histórico** | Um produto que já esteja vinculado a um pedido realizado no passado **não pode ser excluído** do banco de dados para não quebrar o histórico (integridade referencial). Ele deve ser apenas marcado como *Inativo* ou *Indisponível*. |

---

## 🛒 2. Regras de Criação de Pedido

| ID | Regra de Negócio | Descrição e Validação |
| :--- | :--- | :--- |
| **RN04** | **Disponibilidade de Itens** | O sistema não deve permitir a inclusão de um produto em um pedido se o status do produto estiver marcado como *Indisponível* ou *Inativo*. |
| **RN05** | **Pedido Vazio** | Um pedido não pode ser finalizado ou enviado para a cozinha sem conter pelo menos 1 (um) item válido em sua lista. |
| **RN06** | **Horário de Funcionamento** | A criação de novos pedidos só pode ocorrer dentro do horário de operação estabelecido (07h às 23h). Pedidos fora desse horário devem ser bloqueados pela interface e pela API. |
| **RN07** | **Geração do Código (ID)** | O código do pedido gerado deve ser único para garantir a rastreabilidade (ex: um número sequencial diário ou um UUID), não podendo se repetir. |

---

## 🔄 3. Regras de Fluxo e Status do Pedido

| ID | Regra de Negócio | Descrição e Validação |
| :--- | :--- | :--- |
| **RN08** | **Fluxo Sequencial de Status** | O status do pedido deve seguir uma ordem lógica progressiva: <br>`Recebido` ➔ `Em Preparo` ➔ `Pronto` ➔ `Finalizado`.<br>Não é permitido pular etapas (ex: de *Recebido* direto para *Finalizado* sem passar pelo preparo). |
| **RN09** | **Restrição de Cancelamento** | Um pedido só pode ter seu status alterado para `Cancelado` se o status atual for `Recebido`. Se já estiver `Em Preparo`, `Pronto` ou `Finalizado`, o cancelamento via sistema deve ser bloqueado para o cliente (exigindo intervenção gerencial). |
| **RN10** | **Imutabilidade de Pedidos Finalizados** | Uma vez que o pedido atinja o status de `Finalizado` ou `Cancelado`, nenhuma alteração em seus itens, valores ou dados do cliente será permitida. O registro torna-se apenas leitura (Read-Only). |

---

## 🔐 4. Regras de Acesso e Segurança

| ID | Regra de Negócio | Descrição e Validação |
| :--- | :--- | :--- |
| **RN11** | **Criptografia Obrigatória** | Sob nenhuma hipótese a senha do administrador pode transitar ou ser salva em texto puro no banco de dados. O back-end deve aplicar o *hash* antes da persistência. |
| **RN12** | **Sessão Administrativa** | Apenas usuários autenticados com *token* ou sessão válida de Administrador podem acessar as rotas de alteração de status, cadastro e relatórios. A tentativa de acesso sem credencial deve retornar erro de autorização. |

---
*Documento mantido pela equipe de desenvolvimento para validação da lógica de programação.*
