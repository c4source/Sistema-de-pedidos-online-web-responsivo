# User Stories do Sistema

Este documento apresenta as User Stories derivadas dos requisitos funcionais do sistema.  
As User Stories descrevem funcionalidades a partir da perspectiva do usuário, facilitando a organização do backlog, o planejamento do desenvolvimento e a entrega de valor contínua (Metodologia Ágil).

---

## Backlog de User Stories
---


### US01 – Acesso administrativo
Como administrador  
Quero realizar login no sistema  
Para que eu possa acessar a área administrativa com segurança.

### US02 – Acesso ao painel administrativo
Como administrador  
Quero acessar um painel administrativo (Dashboard)  
Para que eu possa gerenciar produtos, pedidos e visualizar métricas básicas.

### US03 – Cadastro de produtos
Como administrador  
Quero cadastrar novos produtos (nome, descrição, preço, imagem)  
Para que eles fiquem disponíveis para os clientes realizarem pedidos.

### US04 – Edição de produtos
Como administrador  
Quero editar informações de produtos  
Para manter os dados atualizados conforme a mudança de preços ou ingredientes.

### US05 – Remoção ou desativação de produtos
Como administrador  
Quero remover ou desativar produtos  
Para que eles deixem de aparecer no cardápio quando saírem de linha.

### US06 – Visualizar produtos
Como cliente  
Quero visualizar os produtos disponíveis divididos por categorias  
Para escolher o que desejo pedir com mais facilidade.

### US07 – Ver detalhes de um produto
Como cliente  
Quero visualizar detalhes de um produto  
Para entender melhor suas características (ingredientes, tamanho) antes de realizar um pedido.

### US08 – Criar pedido
Como cliente  
Quero adicionar produtos a um carrinho e realizar um pedido  
Para solicitar os itens desejados ao estabelecimento.

### US09 – Informar dados do pedido
Como cliente  
Quero informar meu nome, método de entrega/mesa e observações no pedido  
Para que o pedido seja identificado corretamente e preparado ao meu gosto.

### US10 – Gerar código do pedido
Como sistema  
Quero gerar um código único (hash ou numérico) para cada pedido  
Para permitir o rastreamento unívoco do pedido no banco de dados.

### US11 – Registrar data e hora
Como sistema  
Quero registrar a data e hora (timestamp) do pedido  
Para organizar a fila de preparação na ordem correta (FIFO - First In, First Out).

### US12 – Visualizar pedidos
Como administrador  
Quero visualizar todos os pedidos realizados  
Para acompanhar as solicitações dos clientes e o fluxo de caixa do dia.

### US13 – Atualizar status do pedido
Como administrador ou operador da cozinha  
Quero atualizar o status dos pedidos (ex: Recebido, Em Preparo, Pronto, Entregue)  
Para informar o andamento da preparação ao cliente e manter a organização interna.

### US14 – Consultar status do pedido
Como cliente  
Quero consultar o status do meu pedido usando o código  
Para saber em tempo real quando ele estará pronto para retirada ou entrega.

### US15 – Visualizar fila de pedidos
Como operador da cozinha  
Quero visualizar a lista de pedidos em andamento em formato de esteira (Kanban)  
Para organizar a preparação dos pratos de forma eficiente.

### US16 – Controlar disponibilidade de produtos
Como administrador  
Quero marcar produtos como "disponíveis" ou "esgotados" de forma rápida  
Para controlar o que aparece no cardápio sem precisar deletar o item do banco de dados.

### US17 – Filtrar pedidos
Como administrador  
Quero filtrar pedidos por status ou data  
Para facilitar a auditoria e a gestão operacional.

### US18 – Realizar Pagamento Integrado *(Nova)*
Como cliente  
Quero selecionar a forma de pagamento (PIX, Cartão) e pagar via sistema  
Para confirmar meu pedido de forma rápida e segura.

### US19 – Processamento de Pagamento *(Nova)*
Como sistema  
Quero enviar os dados da transação para o Gateway de Pagamento  
Para validar a cobrança e liberar o pedido para a cozinha automaticamente.

---

## Ordem Lógica de Desenvolvimento (Roadmap)

Embora as User Stories estejam organizadas no backlog, o desenvolvimento seguirá uma abordagem incremental, baseada em dependências arquiteturais e entrega de valor (MVP).

### 1. Autenticação e Base Administrativa
*Estabelece a segurança e o controle do sistema.*
- US01 – Login do administrador  
- US02 – Acessar painel administrativo  

### 2. Gestão de Catálogo (CRUD de Produtos)
*Permite alimentar o banco de dados com os itens que serão vendidos.*
- US03 – Cadastro de produtos  
- US04 – Editar produtos  
- US05 – Inativar produtos  
- US16 – Controlar disponibilidade de produtos  

### 3. Vitrine e Seleção (Interface do Cliente)
*Consumo da API de produtos para exibição ao usuário final.*
- US06 – Visualizar produtos  
- US07 – Ver detalhes do produto  

### 4. Motor de Pedidos e Pagamentos (Checkout)
*Core business do sistema: consolida a venda e integra com serviços externos.*
- US08 – Criar pedido  
- US09 – Informar dados do pedido  
- US18 – Realizar Pagamento Integrado
- US19 – Processamento de Pagamento
- US10 – Gerar código do pedido  
- US11 – Registrar data e hora do pedido  

### 5. Gestão de Fila e Produção (KDS - Kitchen Display System)
*Organiza o fluxo interno de trabalho.*
- US12 – Visualizar pedidos  
- US15 – Visualizar fila da cozinha  
- US13 – Atualizar status do pedido  
- US17 – Filtrar pedidos  

### 6. Rastreamento e Pós-Venda
*Garante a transparência para o cliente final.*
- US14 – Consultar status do pedido  

---

## Critérios de Aceitação (Exemplo de Padrão Adotado)

Para garantir que cada US atenda aos requisitos de qualidade, utilizamos **Critérios de Aceite**. Exemplo aplicado à **US08 (Criar pedido)**:

1. O sistema só deve permitir finalizar o pedido se houver pelo menos 1 item no carrinho.
2. O valor total deve ser calculado e exibido em tempo real, incluindo possíveis taxas.
3. Se o cliente tentar pedir um item com status "esgotado" (US16), o sistema deve bloquear a ação e exibir uma mensagem de erro amigável.
