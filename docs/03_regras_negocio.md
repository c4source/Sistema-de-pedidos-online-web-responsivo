#  Regras de Negócio (RN)

## Sistema de Controle de Pedidos — PIM

Este documento descreve as **Regras de Negócio (RN)** do sistema de controle de pedidos desenvolvido no Projeto Integrador Multidisciplinar (PIM).

Enquanto os **Requisitos Funcionais (RF)** definem *o que o sistema deve fazer*, as **Regras de Negócio** estabelecem **condições, restrições e validações** que garantem o funcionamento correto das operações do sistema.

Essas regras devem ser aplicadas principalmente na **camada de back-end**, podendo também ser reforçadas pelo **front-end** e pelo **banco de dados**, garantindo integridade e segurança das informações.

---

#  1. Regras de Produto e Cardápio

| ID | Regra de Negócio | Descrição |
|----|------------------|-----------|
| RN01 | Validação de Preço | O valor de um produto cadastrado deve ser obrigatoriamente maior que zero (> 0). Não é permitido cadastrar produtos com valor negativo ou igual a zero. |
| RN02 | Campos Obrigatórios | Para cadastrar ou editar um produto, os campos **Nome**, **Preço** e **Status de Disponibilidade** são obrigatórios e não podem ser nulos ou vazios. |
| RN03 | Integridade de Histórico | Um produto que já tenha sido utilizado em pedidos anteriores não pode ser excluído do banco de dados para não comprometer o histórico de pedidos. Nesses casos o produto deve ser marcado como **Inativo** ou **Indisponível**. |

---

#  2. Regras de Criação de Pedido

| ID | Regra de Negócio | Descrição |
|----|------------------|-----------|
| RN04 | Disponibilidade de Itens | O sistema não deve permitir a inclusão de um produto em um pedido se o status do produto estiver marcado como **Indisponível** ou **Inativo**. |
| RN05 | Pedido com Itens Obrigatórios | Um pedido não pode ser finalizado ou enviado para preparo sem conter pelo menos **um item válido** em sua lista. |
| RN06 | Identificação Única do Pedido | Todo pedido deve possuir um **identificador único**, gerado automaticamente pelo sistema, garantindo rastreabilidade das operações. |

---

#  3. Regras de Fluxo e Status do Pedido

| ID | Regra de Negócio | Descrição |
|----|------------------|-----------|
| RN07 | Fluxo Sequencial de Status | O status de um pedido deve seguir obrigatoriamente a sequência: **Recebido → Em Preparo → Pronto → Finalizado**. Não é permitido pular etapas. |
| RN08 | Restrição de Cancelamento | Um pedido só pode ser cancelado quando estiver no status **Recebido**. Caso já esteja **Em Preparo**, **Pronto** ou **Finalizado**, o cancelamento deve ser restrito ao administrador. |
| RN09 | Imutabilidade de Pedidos Finalizados | Após atingir o status **Finalizado** ou **Cancelado**, o pedido torna-se apenas leitura (*read-only*), não sendo permitidas alterações em seus itens ou valores. |

---

#  4. Regras de Acesso e Segurança

| ID | Regra de Negócio | Descrição |
|----|------------------|-----------|
| RN10 | Criptografia de Senha | Senhas de administradores não podem ser armazenadas em texto puro no banco de dados. O sistema deve utilizar **hash criptográfico** antes da persistência. |
| RN11 | Autenticação Administrativa | Apenas usuários autenticados com perfil **Administrador** podem acessar funcionalidades administrativas do sistema, como cadastro de produtos ou alteração de status de pedidos. |

---
 Resumo

As regras de negócio estabelecem as restrições necessárias para garantir o funcionamento correto do sistema de controle de pedidos.

Essas regras abrangem:

- validação de produtos do cardápio  
- criação e integridade dos pedidos  
- fluxo de status dos pedidos  
- controle de acesso administrativo  
- segurança das informações do sistema  

A aplicação dessas regras assegura **consistência dos dados, rastreabilidade das operações e controle adequado do processo de pedidos**.