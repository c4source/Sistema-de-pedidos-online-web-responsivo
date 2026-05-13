# Contrato da API - MVP

Este documento resume o contrato usado pelo frontend do MVP da pizzaria.

## Autenticação

O cliente comum não usa login nem JWT no MVP.

A área Admin/Colaborador usa JWT:

```text
Authorization: Bearer {token}
```

O token é obtido em `POST /api/Auth/login-colaborador` e salvo no frontend como `localStorage.adminToken`.

## Rotas Públicas

### GET /api/Produto

Lista produtos do PostgreSQL para o cardápio.

Campos principais retornados:

- `id`
- `nome`
- `preco`
- `descricao`
- `categoria`
- `status`
- `estoque`
- `imagemUrl`

### GET /api/Produto/{id}

Retorna o detalhe de um produto.

### GET /api/Produto/categorias

Retorna as categorias cadastradas, quando existem produtos com categoria preenchida.

### POST /api/Pedido/checkout-mvp

Cria um pedido público sem JWT e sem `idCliente`.

O pedido guarda diretamente:

- `nomeCliente`
- `telefoneCliente`
- `tipoEntrega`
- endereço, quando houver entrega
- `observacoes`
- itens
- forma de pagamento

Endereço é obrigatório somente quando `tipoEntrega = "entrega"`.

Formas de pagamento aceitas:

- `pix`
- `cartao`
- `dinheiro`

O checkout cria registros em:

- `pedido`
- `itempedido`
- `pagamento`

## Login Admin/Colaborador

### POST /api/Auth/login-colaborador

Autentica Admin/Colaborador e retorna JWT.

Essa rota não exige token, pois é usada justamente para gerar o token de acesso administrativo.
Exemplo:

```json
{
  "email": "admin@pim.com",
  "senha": "123456"
}
```

Resposta esperada:

```json
{
  "token": "...",
  "usuario": "Administrador",
  "perfil": "Colaborador"
}
```

Observação: na revisão atual, `database/seed.sql` não contém o colaborador `admin@pim.com` com senha `123456`. Se esse usuário for usado como padrão de testes, ele deve ser incluído no seed em uma tarefa separada.

## Rotas Protegidas/Admin

Todas as rotas abaixo exigem token JWT de Admin/Colaborador.

Enviar no cabeçalho da requisição:

Authorization: Bearer {token}

 ### GET /api/Pedido

Lista pedidos reais do PostgreSQL. Requer JWT de colaborador.

Campos principais:

- `id`
- `codigo`
- `nomeCliente`
- `telefoneCliente`
- `dataHora`
- `status`
- `valorTotal`
- `tipoEntrega`
- `ruaEntrega`
- `numeroEntrega`
- `bairroEntrega`
- `formaPagamento`
- `statusPagamento`
- `itens`

Status de pedido:

- `recebido`
- `em_preparo`
- `pronto`
- `finalizado`
- `cancelado`

### PATCH /api/Pedido/{id}/status

Atualiza o status operacional do pedido.

Exemplo:

```json
{
  "status": "em_preparo"
}
```

### POST /api/Produto

Cadastra produto no banco. Requer JWT.

Payload:

```json
{
  "nome": "Pizza Calabresa",
  "preco": 49.9,
  "descricao": "Molho, mussarela e calabresa",
  "categoria": "pizza",
  "status": "disponivel",
  "estoque": 20,
  "imagemUrl": "../img/calabresa800.jpg"
}
```

### PUT /api/Produto/{id}

Edita produto no banco. Requer JWT.

Usa o mesmo formato de payload do cadastro.

### DELETE /api/Produto/{id}

Exclui produto no banco. Requer JWT.

No estado atual do backend, esta rota faz exclusão física. Inativação lógica de produto deve ser tratada como melhoria futura.

## Observações do MVP

- Carrinho do cliente fica no `localStorage`.
- `checkout-mvp` é público e não usa cliente logado.
- Admin usa `adminToken` no `localStorage`.
- Redis não está implementado.
