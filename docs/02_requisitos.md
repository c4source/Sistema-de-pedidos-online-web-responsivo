## - Documentação dos Atores e Requisitos

## Levantamento de Atores do Sistema

Os atores representam os usuários ou sistemas externos que interagem diretamente com o sistema. A identificação dos atores é essencial para a modelagem dos casos de uso e definição das funcionalidades do sistema.

### 1. Cliente

O **Cliente** é o usuário final que acessa o sistema através da interface web para realizar pedidos e acompanhar o andamento dos mesmos.

**Responsabilidades do Cliente:**

- Acessar o sistema através do navegador
- Visualizar os produtos disponíveis
- Realizar pedidos
- Consultar o status do pedido
- Interagir com a interface do sistema para efetuar suas escolhas

---

### 2. Administrador

O **Administrador** é o usuário responsável pela gestão do sistema e pelo controle das informações operacionais.

**Responsabilidades do Administrador:**

- Acessar a área administrativa do sistema (login)
- Gerenciar os produtos disponíveis no sistema
- Visualizar e gerenciar pedidos realizados pelos clientes
- Atualizar o status dos pedidos
- Manter as informações do sistema atualizadas

---

### 3. Operador da Cozinha

O **Operador da Cozinha** é responsável por acompanhar os pedidos recebidos e atualizar seu status conforme o andamento da preparação.

**Responsabilidades do Operador da Cozinha:**

- Visualizar pedidos recebidos
- Atualizar o status dos pedidos (ex: em preparo, pronto)
- Acompanhar a fila de pedidos em andamento

---

### 4. Sistema de Pagamento (Ator Externo)

O **Sistema de Pagamento** representa um serviço externo responsável por processar pagamentos realizados pelos clientes.

**Responsabilidades do Sistema de Pagamento:**

- Processar transações financeiras
- Confirmar pagamentos realizados
- Retornar o status da transação ao sistema

Este ator representa uma integração externa ao sistema.