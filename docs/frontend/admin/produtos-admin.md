# Admin Produtos

## Objetivo

A tela Admin Produtos permite gerenciar os produtos do cardápio da pizzaria no MVP. Ela concentra as operações administrativas de cadastro, consulta, edição e exclusão de produtos.

## Funcionalidades

- Listar produtos cadastrados.
- Cadastrar novo produto.
- Editar produto existente.
- Excluir produto.
- Filtrar produtos por categoria.
- Filtrar produtos por disponibilidade.
- Exibir resumo com total de produtos, disponíveis, indisponíveis e estoque total.

## Relacao com CRUD

A tela representa o CRUD administrativo de produtos da seguinte forma:

- Create: cadastro de produto.
- Read: listagem e visualizacao dos produtos.
- Update: edicao dos dados do produto.
- Delete: exclusão de produto pela API.

No estado atual do backend, `DELETE /api/Produto/{id}` realiza exclusão física. A inativação lógica deve ser tratada como melhoria futura.

## Dados Utilizados

Os produtos são lidos e salvos pela API, usando as rotas `GET /api/Produto`, `POST /api/Produto`, `PUT /api/Produto/{id}` e `DELETE /api/Produto/{id}`. As rotas administrativas de escrita usam `Authorization: Bearer {adminToken}`.

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

A tela possui proteção de rota por meio da chave `adminToken`.

Como regra desejável de evolução, produtos não deveriam ser apagados fisicamente, pois pedidos antigos podem referenciar produtos já cadastrados. Porém, no estado atual do backend, a exclusão implementada é física.

Produtos disponíveis aparecem no cardápio do cliente. Produtos indisponíveis continuam aparecendo no Admin, mas deixam de aparecer no cardápio quando a API os retorna com esse status.

## Fluxo da Tela

O administrador acessa a tela e visualiza a lista de produtos. Pode usar filtros por categoria e disponibilidade para encontrar itens especificos.

No formulário, pode cadastrar um novo produto ou editar um produto existente. Ao editar, o formulário é preenchido com os dados atuais do item. A ação de cancelar edição retorna o formulário ao modo de cadastro.

Nos cards de produto, o administrador pode editar ou excluir conforme a disponibilidade atual da API.

## Integração com API

Todas as alterações são persistidas pela API no PostgreSQL. Como o cardápio do cliente também consulta `GET /api/Produto`, as mudanças feitas no Admin Produtos refletem no fluxo do cliente.

Exemplos:

- Produto disponível aparece no cardápio.
- Produto indisponível não aparece no cardápio quando filtrado pela API/frontend.
- Edições de nome, preço, descrição, imagem e categoria passam a ser refletidas no cliente.

## Observacoes

Em uma versão futura, a exclusão física poderá ser substituída por inativação lógica para preservar histórico.

## Testes Realizados

- Bloqueio de acesso sem login.
- Listagem de produtos retornados pela API.
- Cadastro de novo produto.
- Edição de produto existente.
- Exclusão conforme comportamento atual da API.
- Filtros por categoria e disponibilidade.
- Reflexo das alterações no cardápio do cliente.
