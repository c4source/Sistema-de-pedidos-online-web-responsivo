# Admin Produtos

## Objetivo

A tela Admin Produtos permite gerenciar os produtos do cardapio da pizzaria no MVP. Ela concentra as operacoes administrativas de cadastro, consulta, edicao, inativacao e reativacao de produtos.

## Funcionalidades

- Listar produtos cadastrados.
- Cadastrar novo produto.
- Editar produto existente.
- Inativar produto.
- Reativar produto.
- Filtrar produtos por categoria.
- Filtrar produtos por disponibilidade.
- Exibir resumo com total de produtos, disponiveis, indisponiveis e estoque total.

## Relacao com CRUD

A tela representa o CRUD administrativo de produtos da seguinte forma:

- Create: cadastro de produto.
- Read: listagem e visualizacao dos produtos.
- Update: edicao dos dados do produto.
- Delete: substituido por inativacao logica.

Nao existe exclusao fisica de produtos no MVP. A acao equivalente a exclusao altera `disponivel` para `false`.

## Dados Utilizados

Os produtos sao lidos e salvos na chave `produtos` do `localStorage`.

Os campos usados na tela sao:

- `id`
- `nome`
- `categoria`
- `preco`
- `descricao`
- `imagem`
- `estoque`
- `disponivel`

As categorias disponiveis sao:

- `pizza`
- `doce`
- `bebida`
- `combo`

## Regras de Negocio

A tela possui protecao de rota por meio da chave `adminLogado`.

Produtos nao devem ser apagados fisicamente, pois pedidos antigos podem referenciar produtos ja cadastrados. A inativacao com `disponivel: false` preserva o historico e evita quebra de dados.

Produtos disponiveis aparecem no cardapio do cliente. Produtos indisponiveis continuam aparecendo no Admin, mas deixam de aparecer no cardapio.

## Fluxo da Tela

O administrador acessa a tela e visualiza a lista de produtos. Pode usar filtros por categoria e disponibilidade para encontrar itens especificos.

No formulario, pode cadastrar um novo produto ou editar um produto existente. Ao editar, o formulario e preenchido com os dados atuais do item. A acao de cancelar edicao retorna o formulario ao modo de cadastro.

Nos cards de produto, o administrador pode editar, inativar ou reativar conforme a disponibilidade atual.

## Integracao com localStorage

Todas as alteracoes sao persistidas em `localStorage.produtos`. Como o cardapio do cliente tambem le essa chave, as mudancas feitas no Admin Produtos refletem no fluxo do cliente.

Exemplos:

- Produto disponivel aparece no cardapio.
- Produto indisponivel nao aparece no cardapio.
- Produto reativado volta a aparecer.
- Edicoes de nome, preco, descricao, imagem e categoria passam a ser refletidas no cliente.

## Observacoes

Em uma versao futura, `localStorage.produtos` devera ser substituido por uma API integrada ao banco de dados.

## Testes Realizados

- Bloqueio de acesso sem login.
- Listagem de produtos salvos em `localStorage.produtos`.
- Cadastro de novo produto.
- Edicao de produto existente.
- Inativacao sem exclusao fisica.
- Reativacao de produto.
- Filtros por categoria e disponibilidade.
- Produto inativo sumindo do cardapio do cliente.
- Produto reativado voltando ao cardapio.
