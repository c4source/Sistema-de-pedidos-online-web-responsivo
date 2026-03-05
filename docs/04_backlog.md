# Backlog de Requisitos do Sistema

Este documento apresenta o **backlog de requisitos funcionais** levantados para o desenvolvimento do sistema do Projeto Integrador Multidisciplinar (PIM).

Durante o processo de levantamento de requisitos, foi realizado um **brainstorming entre os integrantes da equipe**, resultando em diversas propostas de funcionalidades.

Após análise técnica e avaliação de escopo, os requisitos foram organizados em três categorias:

- **MVP (Minimum Viable Product)** – funcionalidades essenciais que serão implementadas no projeto
- **Melhorias Planejadas** – funcionalidades adicionais que podem ser implementadas caso haja tempo disponível
- **Ideias Futuras** – funcionalidades registradas para possível evolução futura do sistema

Essa organização permite controlar o escopo do projeto e priorizar o desenvolvimento das funcionalidades mais importantes.

---

# 1. Requisitos MVP (Implementação Prioritária)

Os requisitos abaixo representam o **conjunto mínimo de funcionalidades necessárias para o funcionamento completo do sistema**.

---

## RF01 – Autenticação do Administrador
O sistema deve permitir que o administrador realize login para acessar a área administrativa.

---

## RF02 – Acesso ao Painel Administrativo
O sistema deve disponibilizar uma área administrativa restrita para gerenciamento do sistema.

---

## RF03 – Cadastro de Produtos
O sistema deve permitir ao administrador cadastrar novos produtos contendo informações como nome, descrição e preço.

---

## RF04 – Edição de Produtos
O sistema deve permitir ao administrador editar informações dos produtos cadastrados.

---

## RF05 – Remoção ou Inativação de Produtos
O sistema deve permitir ao administrador remover ou inativar produtos para que deixem de aparecer para os clientes.

---

## RF06 – Listagem de Produtos
O sistema deve exibir aos clientes a lista de produtos disponíveis para realização de pedidos.

---

## RF07 – Visualização de Detalhes do Produto
O sistema deve permitir que o cliente visualize detalhes de cada produto.

---

## RF08 – Criação de Pedido
O sistema deve permitir que o cliente realize pedidos selecionando produtos e quantidades.

---

## RF09 – Registro de Informações do Pedido
O sistema deve registrar informações necessárias para a realização do pedido, como nome do cliente e observações.

---

## RF10 – Geração de Código do Pedido
O sistema deve gerar um identificador único para cada pedido realizado.

---

## RF11 – Registro de Data e Hora do Pedido
O sistema deve registrar automaticamente a data e hora em que o pedido foi realizado.

---

## RF12 – Visualização de Pedidos
O sistema deve permitir que o administrador visualize todos os pedidos realizados pelos clientes.

---

## RF13 – Atualização de Status do Pedido
O sistema deve permitir que o administrador ou operador da cozinha altere o status do pedido.

Exemplos de status:
- Recebido
- Em preparo
- Pronto
- Finalizado
- Cancelado

---

## RF14 – Consulta de Status do Pedido
O sistema deve permitir que o cliente consulte o status do pedido utilizando o código do pedido.

---

## RF15 – Fila de Pedidos para Cozinha
O sistema deve permitir que a cozinha visualize pedidos em andamento e seus respectivos itens.

---

## RF17 – Controle de Disponibilidade de Produtos
O sistema deve permitir que o administrador marque produtos como disponíveis ou indisponíveis.

---

## RF19 – Filtro de Pedidos
O sistema deve permitir que o administrador filtre pedidos por status ou data.

---

# 2. Melhorias Planejadas

Estes requisitos representam melhorias identificadas durante o levantamento de ideias e podem ser implementados caso haja disponibilidade de tempo durante o desenvolvimento.

---

## RF20 – Cadastro de Cliente
O sistema pode permitir que clientes realizem cadastro na plataforma para acessar funcionalidades adicionais.

---

## RF21 – Login de Cliente
O sistema pode permitir que clientes autenticados realizem login na plataforma.

---

## RF22 – Carrinho de Compras
O sistema pode permitir que clientes adicionem produtos a um carrinho antes de finalizar o pedido.

---

## RF23 – Histórico de Pedidos
O sistema pode permitir que clientes consultem o histórico de pedidos realizados.

---

## RF24 – Confirmação de Pedido pelo Atendente
O sistema pode permitir que um atendente confirme o recebimento e processamento de pedidos realizados.

---

## RF25 – Painel de Monitoramento de Pedidos
O sistema pode fornecer um painel para acompanhamento de pedidos em tempo real.

---

# 3. Ideias Futuras (Backlog de Expansão)

Os requisitos abaixo representam funcionalidades mais avançadas que podem ser consideradas em futuras evoluções do sistema.

---

## RF26 – Processamento de Pagamento Online
O sistema pode permitir que clientes realizem pagamento online utilizando métodos como cartão ou PIX.

---

## RF27 – Integração com Sistemas de Pagamento
O sistema pode integrar serviços externos de pagamento para processamento de transações.

---

## RF28 – Relatórios de Vendas
O sistema pode gerar relatórios contendo dados de vendas, faturamento e quantidade de pedidos por período.

---

## RF29 – Promoções e Campanhas
O sistema pode permitir que o administrador configure promoções e campanhas de marketing.

---

## RF30 – Gestão de Usuários
O sistema pode permitir que o administrador gerencie usuários cadastrados na plataforma.

---

## RF31 – Configuração Geral do Sistema
O sistema pode disponibilizar configurações administrativas para controle do funcionamento geral da aplicação.

---

## RF32 – Notificação de Status do Pedido
O sistema pode notificar o cliente quando houver atualização no status do pedido.