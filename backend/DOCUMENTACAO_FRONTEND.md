# Documentacao de Integracao Front-end - PIM Restaurante

> Nota de atualização do MVP: este arquivo contém documentação histórica de integração. O contrato oficial atual da API está em `docs/api/contrato-api-mvp.md`. No MVP atual, o cliente comum não usa login/JWT, o carrinho permanece no `localStorage`, o checkout público usa `POST /api/Pedido/checkout-mvp`, o Admin usa JWT de colaborador, e o banco oficial é PostgreSQL.

Esta API atende dois perfis:

- Cliente: navega no cardapio, usa carrinho local e faz pedido pelo checkout publico.
- Colaborador: gerencia produtos e pedidos da loja com JWT.

Base URL local:

```text
http://localhost:5162
```

Swagger:

```text
http://localhost:5162/swagger
```

## Autenticacao

A API usa JWT Bearer Token nas rotas protegidas da area Admin/Colaborador. O cliente comum nao usa JWT no MVP atual.

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

Fora do MVP atual. O cliente comum não faz login e não usa JWT para navegar, montar carrinho ou finalizar pedido.

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

Fora do MVP atual. A tabela de cliente pode apoiar evolução futura, mas o checkout público atual não exige cadastro nem `idCliente`.

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

No MVP atual, o carrinho do cliente fica no `localStorage` do navegador. Não há rota de carrinho persistido no contrato oficial atual.

## Checkout e Pedidos do Cliente

### Confirmar Pedido Público

Não requer login de cliente nem JWT.

```http
POST /api/Pedido/checkout-mvp
```

Campos principais:

- `nomeCliente`
- `telefoneCliente`
- `tipoEntrega`
- endereço, obrigatório somente quando `tipoEntrega = "entrega"`
- `observacoes`
- `itens`
- `formaPagamento`

Regras:

- `tipoEntrega` aceita `entrega` ou `retirada`.
- `formaPagamento` aceita `pix`, `cartao` ou `dinheiro`.
- O checkout cria registros em `pedido`, `itempedido` e `pagamento`.
- O pedido nasce com status `recebido`.

## Pedidos da Loja

Rotas protegidas por JWT de Colaborador.

### Listar Todos os Pedidos

```http
GET /api/Pedido
```

### Atualizar Status

```http
PATCH /api/Pedido/{id}/status
```

Request:

```json
{
  "status": "em_preparo"
}
```

Status aceitos:

```text
recebido
em_preparo
pronto
finalizado
cancelado
```

Fluxo operacional usado no Admin: `recebido -> em_preparo -> pronto -> finalizado`, com possibilidade de `cancelado` conforme regra da tela administrativa.

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

- Sempre salvar o token apos login administrativo.
- Usar `perfil` retornado no login para decidir qual interface administrativa mostrar.
- Cliente comum nao usa login/JWT no MVP atual.
- Colaborador deve usar `login-colaborador`.
- Cliente nao deve acessar telas administrativas.
- Mesmo que o front esconda botoes, o backend tambem bloqueia acessos indevidos.
- O token vale por 8 horas.
- Campos sensiveis como senha/hash nao sao retornados pela API.
- Para atualizar o Swagger apos mudancas no backend, reiniciar a API e recarregar a pagina.

