# Modelagem do Banco de Dados

Este documento apresenta a modelagem do banco de dados do sistema de controle de pedidos.

## Modelagem Simples

![Modelagem Simples](../diagrams/modelagem_banco_simples.svg)

A modelagem simples representa as principais entidades do sistema e seus relacionamentos.

## Modelagem Detalhada

![Modelagem Detalhada](../diagrams/modelagem_banco_detalhada.svg)

A modelagem detalhada apresenta as tabelas, atributos, chaves primárias e chaves estrangeiras.

## Entidades Principais

### Pedido
Armazena as informações gerais do pedido realizado pelo cliente.

### Produto
Armazena os produtos disponíveis no cardápio.

### ItemPedido
Representa os itens vinculados a cada pedido, resolvendo o relacionamento entre pedido e produto.

## Atributos das Entidades

### Pedido
- id_pedido (PK)
- codigo
- nome_cliente
- observacoes
- data_hora
- status
- valor_total

### Produto
- id_produto (PK)
- nome
- preco
- descricao
- status_disponibilidade
- imagem_url

### ItemPedido
- id_item_pedido (PK)
- id_pedido (FK)
- id_produto (FK)
- quantidade
- preco_unitario
- subtotal

## Relacionamentos

- Um pedido pode possuir vários itens de pedido.
- Um produto pode estar presente em vários itens de pedido.
- Cada item de pedido pertence a um único pedido e referencia um único produto.

## Considerações

- A tabela ItemPedido resolve o relacionamento muitos-para-muitos entre Pedido e Produto.
- O preço do produto é armazenado no ItemPedido para manter o histórico de valores no momento do pedido.
- O campo imagem_url permite exibir imagens dos produtos no frontend.
- O campo data_hora permite controle e histórico dos pedidos.
- O campo status permite acompanhar o fluxo do pedido (ex: pendente, em preparo, concluído).