# Tela: Checkout

## Objetivo

A tela de checkout coleta os dados basicos do cliente e apresenta o resumo do pedido antes da confirmacao. Ela representa a etapa final de preenchimento antes de concluir o pedido no MVP.

## Funcionalidades

- Campo para preenchimento do nome do cliente.
- Campo para preenchimento do telefone.
- Mascara simples de telefone durante a digitacao.
- Escolha entre entrega e retirada.
- Exibicao condicional dos campos de endereco quando a opcao "Entrega" esta selecionada.
- Escolha da forma de pagamento.
- Validacao dos dados antes da confirmacao do pedido.
- Mensagens de erro inline abaixo dos campos.
- Resumo dos itens do pedido.
- Exibicao do total do pedido.
- Botao para confirmar o pedido.
- Botao de retorno ao carrinho.

## Regras e comportamento

O resumo do pedido e carregado a partir da chave `carrinho` no `localStorage`. Se nao houver itens, a tela informa que nao ha itens no carrinho e exibe total zerado.

O tipo de entrega inicia como "Entrega", exibindo os campos de endereco. Quando o usuario seleciona "Retirada", a secao de endereco e ocultada. Quando a opcao "Entrega" esta selecionada, os campos de rua, numero e bairro passam a ser obrigatorios.

A tela possui validacao bloqueante para o nome, telefone, forma de pagamento e, de forma condicional, endereco. O nome deve ser preenchido de forma valida, o telefone deve conter DDD e quantidade adequada de digitos, e o endereco deve ser informado quando o pedido for de entrega. As mensagens de erro aparecem abaixo dos respectivos campos e sao removidas conforme o usuario corrige as informacoes.

A forma de pagamento e apenas informativa no MVP. As opcoes disponiveis sao dinheiro, cartao e Pix. Nao existe gateway real, validacao de cartao, QR Code Pix, comprovante ou pagamento online nesta etapa.

Ao confirmar o pedido com dados validos, o frontend envia os dados para `POST /api/Pedido/checkout-mvp`, rota publica que nao exige login de cliente nem JWT. O pedido definitivo e salvo no PostgreSQL, incluindo pedido, itens e pagamento. O frontend tambem cria a chave `pedidoAtual` no `localStorage` para exibir o resumo na tela de confirmacao.

## Fluxo do usuario

O usuario acessa o checkout a partir do carrinho, revisa o resumo do pedido, informa seus dados, escolhe o tipo de entrega e seleciona a forma de pagamento desejada. Em seguida, confirma o pedido e segue para a tela de confirmacao.

## Implementacao relacionada

- HTML: `frontend/html/checkout.html`
- CSS: `frontend/css/style.css`
- JS: `frontend/js/checkout.js`
