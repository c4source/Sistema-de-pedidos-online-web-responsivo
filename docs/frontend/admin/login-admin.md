# Login Admin

## Objetivo

A tela Login Admin controla o acesso inicial a area administrativa do sistema. Seu objetivo e impedir que as telas administrativas sejam acessadas sem uma sessao local de administrador.

## Funcionalidades

- Campo para usuario.
- Campo para senha.
- Validacao dos campos obrigatorios.
- Mensagem de erro para usuario ou senha invalidos.
- Botao para entrar no painel administrativo.
- Botao para voltar ao cardapio do cliente.
- Redirecionamento para o Dashboard quando o login e valido.

## Dados Utilizados

As credenciais temporarias usadas no MVP sao:

- Usuario: `admin`
- Senha: `123456`

A tela utiliza a chave `adminLogado` no `localStorage` para registrar que o administrador esta logado.

## Regras de Negocio

O usuario e normalizado antes da validacao, evitando problemas com letras maiusculas, comuns em teclados de celular. Dessa forma, entradas como `Admin` ou `ADMIN` podem ser tratadas corretamente.

Os campos de usuario e senha sao obrigatorios. Quando os dados nao correspondem as credenciais temporarias, a tela exibe uma mensagem de erro sem usar `alert()`.

## Fluxo da Tela

O administrador acessa a tela de login, informa usuario e senha e confirma o acesso. Se as credenciais estiverem corretas, o sistema salva `adminLogado` como `true` e redireciona para o Dashboard Admin.

Caso o usuario ja esteja logado, a aplicacao pode redireciona-lo diretamente para o Dashboard Admin, evitando novo preenchimento de login.

## Integracao com localStorage

A chave utilizada e:

- `adminLogado`: indica se o administrador esta autenticado na sessao local.

Ao fazer login corretamente, essa chave e gravada. Ao sair pela area administrativa, ela e removida.

## Observacoes

A autenticacao e simulada no frontend enquanto nao ha backend. Em uma versao futura, o login devera ser validado por uma API, com controle real de usuario, senha, sessao e permissao.

## Testes Realizados

- Validacao de campos vazios.
- Validacao de usuario e senha incorretos.
- Login com credenciais temporarias corretas.
- Redirecionamento para o Dashboard apos login.
- Retorno ao cardapio pelo botao da tela.
