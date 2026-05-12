# Documentacao de Integracao Front-end - PIM Restaurante

Esta API atende dois perfis:

- Cliente: navega no cardapio, usa carrinho, faz pedido, consulta e cancela o proprio pedido.
- Colaborador: gerencia produtos, clientes, colaboradores e pedidos da loja.

Base URL local:

```text
http://localhost:5000
```

Swagger:

```text
http://localhost:5000/swagger
```

## Autenticacao

A API usa JWT Bearer Token.

Depois do login, enviar o token em todas as rotas protegidas:

```http
Authorization: Bearer SEU_TOKEN
```

Nao colocar aspas no token.

Correto:

```text
Bearer eyJhbGciOi...
```

Errado:

```text
Bearer "eyJhbGciOi..."
```

## Login Cliente

```http
POST /api/Auth/login-cliente
```

Request:

```json
{
  "email": "cliente@email.com",
  "senha": "123456"
}
```

Response:

```json
{
  "token": "jwt",
  "usuario": "Nome do Cliente",
  "perfil": "Cliente"
}
```

## Login Colaborador

```http
POST /api/Auth/login-colaborador
```

Request:

```json
{
  "email": "colaborador@email.com",
  "senha": "123456"
}
```

Response:

```json
{
  "token": "jwt",
  "usuario": "Nome do Colaborador",
  "perfil": "Colaborador"
}
```

## Cadastro de Cliente

Rota publica para criar conta de cliente.

```http
POST /api/Clientes
```

Request:

```json
{
  "nome": "Gabriel Fuentes",
  "cpf": "00000000000",
  "celular": "11999999999",
  "email": "gabriel@email.com",
  "senha": "123456"
}
```

Response:

```json
{
  "id": 1,
  "nome": "Gabriel Fuentes",
  "cpf": "00000000000",
  "celular": "11999999999",
  "email": "gabriel@email.com"
}
```

Observacao: a senha nunca e retornada pela API.

## Cardapio

Requer token de Cliente ou Colaborador.

### Listar Produtos

```http
GET /api/Produto
```

Filtros opcionais:

```http
GET /api/Produto?categoria=Lanches
GET /api/Produto?somenteDisponiveis=true
GET /api/Produto?categoria=Bebidas&somenteDisponiveis=true
```

Response:

```json
[
  {
    "id": 1,
    "nome": "X-Burger",
    "preco": 28.9,
    "descricao": "Hamburguer artesanal com queijo",
    "categoria": "Lanches",
    "status": "Disponivel",
    "estoque": 20,
    "imagemUrl": "https://..."
  }
]
```

### Listar Categorias

```http
GET /api/Produto/categorias
```

Response:

```json
[
  "Bebidas",
  "Lanches",
  "Sobremesas"
]
```

### Buscar Produto por ID

```http
GET /api/Produto/{id}
```

## Carrinho

Requer token de Cliente ou Colaborador.

Importante: cliente so consegue acessar o proprio carrinho. A API valida isso pelo e-mail do token.

### Ver Carrinho

```http
GET /api/Carrinho/{idCliente}
```

Response:

```json
{
  "id": 1,
  "idCliente": 1,
  "dataCriacao": "2026-05-11T20:00:00",
  "atualizadoEm": "2026-05-11T20:05:00",
  "itens": [
    {
      "id": 10,
      "codProd": 1,
      "produto": "X-Burger",
      "precoUnitario": 28.9,
      "quantidade": 2,
      "observacoes": "Sem cebola",
      "subtotal": 57.8
    }
  ],
  "totalProdutos": 57.8
}
```

### Adicionar Item

```http
POST /api/Carrinho/{idCliente}/itens
```

Request:

```json
{
  "codProd": 1,
  "quantidade": 2,
  "observacoes": "Sem cebola"
}
```

### Atualizar Item

```http
PUT /api/Carrinho/{idCliente}/itens/{idItem}
```

Request:

```json
{
  "quantidade": 3,
  "observacoes": "Sem cebola e sem tomate"
}
```

### Remover Item

```http
DELETE /api/Carrinho/{idCliente}/itens/{idItem}
```

### Limpar Carrinho

```http
DELETE /api/Carrinho/{idCliente}/limpar
```

## Checkout e Pedidos do Cliente

### Confirmar Pedido a Partir do Carrinho

Requer token de Cliente ou Colaborador.

```http
POST /api/Pedido/checkout-carrinho
```

Retirada:

```json
{
  "idCliente": 1,
  "tipoEntrega": "Retirada",
  "observacoes": "Vou buscar no balcao"
}
```

Entrega:

```json
{
  "idCliente": 1,
  "tipoEntrega": "Entrega",
  "enderecoEntrega": "Rua Exemplo, 123",
  "observacoes": "Entregar na portaria"
}
```

Regras:

- `tipoEntrega` aceita `Retirada` ou `Entrega`.
- Para `Entrega`, `enderecoEntrega` e obrigatorio.
- Taxa de entrega e fixa no backend: `10`.
- Retirada tem taxa `0`.
- Ao finalizar, o carrinho e limpo.
- O pedido nasce com status `Aguardando Aprovação`.

Response:

```json
{
  "message": "Pedido recebido e aguardando aprovacao da loja.",
  "id_pedido": 1,
  "status": "Aguardando Aprovação",
  "valorTotal": 67.8
}
```

### Ver Pedidos do Cliente

```http
GET /api/Pedido/cliente/{idCliente}
```

Response:

```json
[
  {
    "id": 1,
    "idCliente": 1,
    "observacoes": "Vou buscar no balcao",
    "dataHora": "2026-05-11T20:10:00",
    "status": "Aguardando Aprovação",
    "valorTotal": 57.8,
    "tipoEntrega": "Retirada",
    "enderecoEntrega": null,
    "taxaEntrega": 0,
    "tempoEstimadoMinutos": null,
    "aprovadoEm": null,
    "canceladoEm": null,
    "canceladoPor": null,
    "motivoCancelamento": null,
    "itens": [
      {
        "id": 1,
        "codProd": 1,
        "produto": "X-Burger",
        "quantidade": 2,
        "precoUnitario": 28.9,
        "subtotal": 57.8,
        "observacoes": "Sem cebola"
      }
    ]
  }
]
```

### Ver Pedido por ID

```http
GET /api/Pedido/{id}
```

Cliente so acessa pedido proprio. Colaborador pode acessar qualquer pedido.

### Cliente Cancelar Pedido

Requer token de Cliente.

```http
PATCH /api/Pedido/{id}/cancelar-cliente
```

Request:

```json
{
  "motivo": "Desisti do pedido"
}
```

Regra: cliente so pode cancelar antes da preparacao comecar, ou seja, enquanto estiver `Aguardando Aprovação` ou `Aprovado`.

## Pedidos da Loja

Rotas exclusivas de Colaborador.

### Listar Todos os Pedidos

```http
GET /api/Pedido
```

### Aprovar Pedido

```http
PATCH /api/Pedido/{id}/aprovar
```

Request:

```json
{
  "tempoEstimadoMinutos": 30
}
```

Response:

```json
{
  "message": "Pedido 1 aprovado.",
  "status": "Aprovado",
  "tempoEstimadoMinutos": 30
}
```

### Atualizar Status

```http
PATCH /api/Pedido/{id}/status
```

Request:

```json
{
  "status": "Em Preparação"
}
```

Status aceitos:

```text
Aprovado
Em Preparação
Pronto para Retirada
Saiu para Entrega
Entregue
Retirado
```

Fluxo recomendado para Retirada:

```text
Aguardando Aprovação -> Aprovado -> Em Preparação -> Pronto para Retirada -> Retirado
```

Fluxo recomendado para Entrega:

```text
Aguardando Aprovação -> Aprovado -> Em Preparação -> Saiu para Entrega -> Entregue
```

### Loja Cancelar Pedido

```http
PATCH /api/Pedido/{id}/cancelar-loja
```

Request:

```json
{
  "motivo": "Produto indisponivel"
}
```

Regra: loja nao pode cancelar pedido ja `Entregue` ou `Retirado`.

## Administracao de Produtos

Rotas exclusivas de Colaborador.

### Criar Produto

```http
POST /api/Produto
```

Request:

```json
{
  "nome": "X-Burger",
  "preco": 28.9,
  "descricao": "Hamburguer artesanal com queijo",
  "categoria": "Lanches",
  "status": "Disponivel",
  "estoque": 20,
  "imagemUrl": "https://..."
}
```

### Atualizar Produto

```http
PUT /api/Produto/{id}
```

Request:

```json
{
  "nome": "X-Burger Especial",
  "preco": 31.9,
  "descricao": "Hamburguer artesanal com queijo e bacon",
  "categoria": "Lanches",
  "status": "Disponivel",
  "estoque": 15,
  "imagemUrl": "https://..."
}
```

### Excluir Produto

```http
DELETE /api/Produto/{id}
```

## Administracao de Clientes

Listagem, busca, edicao e exclusao sao exclusivas de Colaborador.

```http
GET /api/Clientes
GET /api/Clientes/{id}
PUT /api/Clientes/{id}
DELETE /api/Clientes/{id}
```

Response de cliente nunca retorna senha.

## Administracao de Colaboradores

Rotas exclusivas de Colaborador.

```http
GET /api/Colaboradores
GET /api/Colaboradores/{id}
POST /api/Colaboradores
PUT /api/Colaboradores/{id}
DELETE /api/Colaboradores/{id}
```

Cadastro de colaborador:

```json
{
  "nome": "Atendente",
  "email": "atendente@email.com",
  "senha": "123456"
}
```

Response de colaborador nunca retorna senha.

## Codigos de Resposta Comuns

```text
200 OK: operacao realizada com sucesso.
201 Created: recurso criado.
204 No Content: alteracao/exclusao sem corpo de resposta.
400 Bad Request: dados invalidos ou regra de negocio violada.
401 Unauthorized: token ausente, invalido ou expirado.
403 Forbidden: token valido, mas perfil/usuario sem permissao.
404 Not Found: recurso nao encontrado.
500 Internal Server Error: erro inesperado no backend.
```

## Observacoes Importantes Para o Front

- Sempre salvar o token apos login.
- Usar `perfil` retornado no login para decidir qual interface mostrar.
- Cliente deve usar `login-cliente`.
- Colaborador deve usar `login-colaborador`.
- Cliente nao deve acessar telas administrativas.
- Mesmo que o front esconda botoes, o backend tambem bloqueia acessos indevidos.
- O token vale por 8 horas.
- Campos sensiveis como senha/hash nao sao retornados pela API.
- Para atualizar o Swagger apos mudancas no backend, reiniciar a API e recarregar a pagina.

