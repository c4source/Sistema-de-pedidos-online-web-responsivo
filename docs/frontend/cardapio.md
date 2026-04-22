# Tela: Cardapio

## Objetivo

A tela de cardapio permite que o cliente visualize os produtos da pizzaria e inicie o fluxo de compra. Ela e a primeira etapa da jornada do usuario e concentra as funcionalidades de busca, filtragem e selecao de produtos.

## Funcionalidades

- Exibicao dos produtos cadastrados no array local do frontend.
- Cards com imagem, nome, descricao, preco e botao de acao.
- Campo de busca para localizar produtos pelo nome.
- Filtros por categoria.
- Cabecalho com nome da pizzaria, localizacao, status da loja e atalho para o carrinho.
- Badge no carrinho indicando a quantidade total de itens armazenados no `localStorage`.
- Secao de categorias com scroll horizontal, permitindo navegacao em telas menores.
- Icones SVG inline nos chips de categoria, mantendo visual minimalista.

## Regras e comportamento

As categorias disponiveis no cardapio sao:

- Pizza
- Doce
- Bebida
- Combo

A busca filtra os produtos conforme o texto digitado no campo de pesquisa. Os filtros utilizam a categoria do produto e podem ser ativados ou desativados ao clicar nos chips. A categoria `Doce` segue o mesmo comportamento das demais categorias.

Ao clicar em um card de produto, o sistema salva o produto selecionado no `localStorage` e direciona o usuario para a tela de detalhe do produto. O badge do carrinho soma as quantidades dos itens existentes em `carrinho` e permanece oculto quando nao ha itens.

## Fluxo do usuario

O usuario acessa o cardapio, pesquisa ou filtra os produtos e seleciona um item de interesse. A partir dessa acao, o fluxo segue para a tela de detalhe do produto, onde sera possivel definir a quantidade e adicionar o item ao carrinho.

## Implementacao relacionada

- HTML: `frontend/html/index.html`
- CSS: `frontend/css/style.css`
- JS: `frontend/js/app.js`

