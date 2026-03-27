# Wireframes do Sistema

Este documento apresenta os wireframes desenvolvidos para o sistema de controle de pedidos, com foco na experiência do usuário (UX) e na definição das principais telas.

---

## Tela: Cardápio (Lista de Produtos)

### Objetivo

Permitir que o cliente visualize os produtos disponíveis de forma rápida, simples e intuitiva, facilitando a escolha e o início do pedido.

---

### Decisões de UX

- Abordagem mobile-first
- Layout em lista vertical
- Navegação por scroll contínuo
- Interface simples e limpa
- Foco na ação principal: realizar pedido

---

### Estrutura da Tela

- Header com identificação do sistema
- Campo de busca
- Filtro por categorias (pizza, bebida, combo)
- Lista de produtos em formato de cards

---

### Estrutura do Card de Produto

Cada produto contém:
- Imagem
- Nome
- Preço
- Botão “+ Adicionar”

---

### Comportamento do Usuário

- Clique no card abre a tela de detalhe do produto
- Clique no botão “+ Adicionar” adiciona diretamente ao pedido
- Navegação contínua por scroll

---

### Feedback Visual

- Alteração visual ao adicionar produto
- Confirmação da ação realizada

---

### Decisões Estratégicas

- Não utilizar banners promocionais pesados
- Não incluir funcionalidades fora do MVP
- Priorizar usabilidade e rapidez

---

### Integração com o Sistema

- Utiliza dados da tabela `produto`
- Atende à user story US06
- Inicia o fluxo de pedido
- Conecta com a tela de detalhe do produto

---

### Wireframe

![Wireframe Cardápio](./wireframes/wireframe-cardapio.png)

---

### Critério de Sucesso

- Usuário consegue visualizar os produtos rapidamente
- Usuário entende o que está disponível
- Usuário consegue iniciar um pedido com poucos cliques
- Interface clara, simples e funcional

---

## Tela: Detalhe do Produto

### Objetivo

Permitir que o usuário visualize informações detalhadas de um produto e o adicione ao pedido de forma simples e rápida.

### Estrutura da Tela

- Imagem do produto em destaque
- Nome e preço
- Descrição
- Controle de quantidade
- Botão de ação “Adicionar ao pedido”

### Wireframe

![Wireframe Detalhe](./wireframes/wireframe-detalhe-produto.png)

#  Wireframe — Tela de Carrinho de Compras

##  Descrição
Esta tela representa o carrinho de compras do sistema, onde o usuário pode visualizar os produtos adicionados ao pedido, alterar quantidades, remover itens e visualizar o valor total antes de finalizar.

O objetivo principal é permitir a montagem do pedido de forma simples, clara e eficiente.

---

##  Objetivo da Tela
- Exibir os produtos adicionados ao carrinho
- Permitir alteração de quantidade
- Permitir remoção de itens
- Atualizar o valor total automaticamente
- Permitir a finalização do pedido

---

##  Estrutura do Wireframe

###  Cabeçalho
- Botão de voltar
- Título: **Carrinho**

---

###  Lista de Produtos
Cada item contém:

- Imagem do produto (representada por um placeholder)
- Nome do produto
- Preço unitário
- Controle de quantidade:
  - (-) diminuir
  - quantidade atual
  - (+) aumentar
- Botão **Remover**

---

###  Total do Pedido
- Exibição do valor total atualizado

---

###  Ação Principal
- Botão **Finalizar Pedido**

---

##  Decisões de UX

- Wireframe de baixa fidelidade para focar na estrutura
- Uso de placeholder de imagem para melhorar identificação visual
- Botão "Remover" alinhado à direita do nome do produto
- Layout simples e funcional, respeitando o escopo do projeto

---

##  Fora de Escopo

- Tamanho da pizza
- Meio a meio
- Adicionais
- Cupons ou descontos

---

##  Critérios de Pronto

- Usuário visualiza os itens
- Usuário altera quantidades
- Usuário remove itens
- Total atualizado corretamente
- Possibilidade de finalizar pedido

---

##  Wireframe

![Wireframe Carrinho](./wireframes/carrinho.png)

---


