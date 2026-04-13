# Wireframes do Sistema

Este documento apresenta os wireframes desenvolvidos para o sistema de controle de pedidos, com foco na experiência do usuário (UX) e na definição das principais telas.

---

# Fluxo de Navegação do Usuário (User Flow)

## Objetivo

Representar a jornada do usuário desde o acesso ao cardápio até a finalização do pedido.

## Diagrama do Fluxo

![Fluxo do Usuário](./wireframes/fluxo-user.png)

## Estrutura do Fluxo

Cardápio → Detalhe do Produto → Carrinho → Checkout → Pedido Confirmado

## Fluxo Detalhado

Cardápio  
↓  
Detalhe do Produto  
↓  
Adicionar ao Carrinho  
↓  
Decisão: Adicionar mais itens?  
→ Sim: retorna ao Cardápio  
→ Não: segue para Carrinho  

Carrinho  
↓  
Finalizar Pedido  
↓  
Checkout  
↓  
Decisão: Tipo de entrega?  
→ Entrega: preencher endereço  
→ Retirada: não exige endereço  

↓  
Confirmar Pedido  
↓  
Pedido Confirmado  

## Decisões de UX

- Fluxo com navegação contínua (loop de compra)
- Carrinho como ponto central de revisão
- Checkout simplificado em uma única tela
- Exibição condicional de endereço
- Sistema sem autenticação (MVP)

## Integração com o Sistema

- Tabela `pedido`
- Tabela `item_pedido`
- Regras RN07–RN11
- User Story US09

---

# Telas do Sistema

---

## Tela: Cardápio

### Objetivo

Permitir que o cliente visualize os produtos disponíveis de forma rápida e intuitiva.

### Estrutura da Tela

- Header
- Campo de busca
- Filtro por categorias
- Lista de produtos

### Card de Produto

- Imagem
- Nome
- Preço
- Botão “+ Adicionar”

### Comportamento

- Clique abre detalhe
- Botão adiciona direto ao pedido
- Scroll contínuo

### Decisões de UX

- Mobile-first
- Layout em lista
- Interface simples
- Foco na ação principal

### Integração

- Tabela `produto`
- User Story US06

### Wireframe

![Wireframe Cardápio](./wireframes/wireframe-cardapio.png)

---

## Tela: Detalhe do Produto

### Objetivo

Exibir informações detalhadas do produto e permitir adição ao pedido.

### Estrutura

- Imagem
- Nome e preço
- Descrição
- Controle de quantidade
- Botão “Adicionar ao pedido”

### Wireframe

![Wireframe Detalhe](./wireframes/wireframe-detalhe-produto.png)

---

## Tela: Carrinho

### Objetivo

Permitir revisão do pedido antes da finalização.

### Estrutura

#### Cabeçalho
- Botão voltar
- Título

#### Lista de Produtos
- Imagem
- Nome
- Preço
- Quantidade (+ / -)
- Botão remover

#### Total
- Valor total atualizado

#### Ação
- Botão “Finalizar Pedido”

### Decisões de UX

- Layout simples
- Baixa fidelidade
- Foco na funcionalidade

### Fora de Escopo

- Tamanho da pizza
- Meio a meio
- Adicionais
- Cupons

### Critérios

- Visualizar itens
- Alterar quantidade
- Remover itens
- Ver total
- Finalizar pedido

### Wireframe

![Wireframe Carrinho](./wireframes/wireframe-carrinho.png)

---

## Tela: Checkout

### Objetivo

Permitir finalização do pedido com dados do cliente e escolha de entrega.

### Estrutura

#### Dados do Cliente
- Nome
- Telefone

#### Tipo de Entrega
- Entrega
- Retirada

#### Endereço (condicional)
- Rua
- Número
- Bairro

> Exibido apenas se “Entrega”

#### Resumo
- Lista de produtos
- Total do pedido

#### Ação
- Botão “Confirmar Pedido”

### Regras

- Nome e telefone obrigatórios
- Tipo de entrega obrigatório
- Endereço obrigatório se entrega
- Exibição do resumo antes de confirmar

### Wireframe

![Wireframe Checkout](./wireframes/checkout.png)

---

## Tela: Pedido Confirmado

### Objetivo

Informar ao usuário que o pedido foi realizado com sucesso.

### Estrutura

- Mensagem de confirmação
- Resumo do pedido
- Tipo de entrega
- Endereço (se houver)
- Tempo estimado
- Botão “Voltar ao cardápio”

### Wireframe

![Wireframe Pedido Confirmado](./wireframes/pedido-confirmado.png)

---