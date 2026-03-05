# User Stories do Sistema

Este documento apresenta as User Stories derivadas dos requisitos funcionais do sistema.  
As User Stories descrevem funcionalidades a partir da perspectiva do usuário, facilitando a organização do backlog e o planejamento do desenvolvimento.

---

## US01 – Acesso administrativo

Como administrador  
Quero realizar login no sistema  
Para que eu possa acessar a área administrativa.

---

## US02 – Acesso ao painel administrativo

Como administrador  
Quero acessar um painel administrativo  
Para que eu possa gerenciar produtos e pedidos.

---

## US03 – Cadastro de produtos

Como administrador  
Quero cadastrar novos produtos  
Para que eles fiquem disponíveis para os clientes realizarem pedidos.

---

## US04 – Edição de produtos

Como administrador  
Quero editar informações de produtos  
Para manter os dados atualizados.

---

## US05 – Remoção ou desativação de produtos

Como administrador  
Quero remover ou desativar produtos  
Para que eles deixem de aparecer no cardápio.

---

## US06 – Visualizar produtos

Como cliente  
Quero visualizar os produtos disponíveis  
Para escolher o que desejo pedir.

---

## US07 – Ver detalhes de um produto

Como cliente  
Quero visualizar detalhes de um produto  
Para entender melhor suas características antes de realizar um pedido.

---

## US08 – Criar pedido

Como cliente  
Quero selecionar produtos e realizar um pedido  
Para solicitar itens do sistema.

---

## US09 – Informar dados do pedido

Como cliente  
Quero informar meu nome e observações no pedido  
Para que o pedido seja identificado corretamente.

---

## US10 – Gerar código do pedido

Como sistema  
Quero gerar um código único para cada pedido  
Para permitir o rastreamento do pedido.

---

## US11 – Registrar data e hora

Como sistema  
Quero registrar a data e hora do pedido  
Para organizar os pedidos na ordem correta.

---

## US12 – Visualizar pedidos

Como administrador  
Quero visualizar os pedidos realizados  
Para acompanhar as solicitações dos clientes.

---

## US13 – Atualizar status do pedido

Como administrador ou operador da cozinha  
Quero atualizar o status dos pedidos  
Para informar o andamento da preparação.

---

## US14 – Consultar status do pedido

Como cliente  
Quero consultar o status do meu pedido usando o código  
Para saber quando ele estará pronto.

---

## US15 – Visualizar fila de pedidos

Como operador da cozinha  
Quero visualizar a lista de pedidos em andamento  
Para organizar a preparação dos pedidos.

---

## US16 – Controlar disponibilidade de produtos

Como administrador  
Quero marcar produtos como disponíveis ou indisponíveis  
Para controlar o que aparece no cardápio.

---

## US17 – Filtrar pedidos

Como administrador  
Quero filtrar pedidos por status ou data  
Para facilitar a gestão dos pedidos.

## Ordem Lógica de Desenvolvimento do Sistema

Embora as User Stories estejam organizadas no backlog para gestão de tarefas, o desenvolvimento do sistema segue uma ordem lógica baseada nas dependências entre as funcionalidades.

Essa sequência foi definida para facilitar a implementação do sistema e garantir que funcionalidades fundamentais estejam disponíveis antes das demais.

---

### 1. Autenticação e Acesso Administrativo

Primeiramente é necessário implementar o acesso administrativo ao sistema, permitindo que o administrador gerencie produtos e pedidos.

- US01 – Login do administrador  
- US02 – Acessar painel administrativo  

---

### 2. Gestão de Produtos

Antes de realizar pedidos, o sistema precisa possuir produtos cadastrados e gerenciáveis pelo administrador.

- US03 – Cadastro de produtos  
- US04 – Editar produtos  
- US05 – Inativar produtos  
- US16 – Controlar disponibilidade de produtos  

---

### 3. Visualização de Produtos pelo Cliente

Após o cadastro dos produtos, o cliente deve ser capaz de visualizar o cardápio e consultar detalhes dos itens disponíveis.

- US06 – Visualizar produtos  
- US07 – Ver detalhes do produto  

---

### 4. Sistema de Pedidos

Com os produtos disponíveis, o cliente pode realizar pedidos no sistema.

- US08 – Criar pedido  
- US09 – Informar dados do pedido  
- US10 – Gerar código do pedido  
- US11 – Registrar data e hora do pedido  

---

### 5. Gestão de Pedidos

Após a criação dos pedidos, o administrador ou operador da cozinha deve ser capaz de acompanhar e atualizar o andamento das solicitações.

- US12 – Visualizar pedidos  
- US13 – Atualizar status do pedido  
- US15 – Visualizar fila da cozinha  
- US17 – Filtrar pedidos  

---

### 6. Consulta de Status pelo Cliente

Por fim, o cliente deve poder consultar o andamento do seu pedido utilizando o código gerado pelo sistema.

- US14 – Consultar status do pedido  

---

Essa organização representa o fluxo natural de funcionamento do sistema:

Administração → Produtos → Visualização → Pedidos → Gestão → Consulta.