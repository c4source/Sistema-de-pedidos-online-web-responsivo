# Login Admin

## Objetivo

A tela Login Admin controla o acesso inicial à área administrativa do sistema. Seu objetivo é impedir que as telas administrativas sejam acessadas sem uma sessão válida de administrador.

## Funcionalidades

- Campo para usuário/e-mail.
- Campo para senha.
- Validação dos campos obrigatórios.
- Mensagem de erro para usuário/e-mail ou senha inválidos.
- Botão para entrar no painel administrativo.
- Botão para voltar ao cardápio do cliente.
- Redirecionamento para o Dashboard quando o login é válido.

## Dados Utilizados

O login administrativo usa a API real:

```http
POST /api/Auth/login-colaborador
```

A tela salva o JWT retornado na chave `adminToken` do `localStorage`.

## Regras de Negocio

O usuário/e-mail é normalizado antes da validação, evitando problemas com letras maiúsculas, comuns em teclados de celular.

Os campos de usuário/e-mail e senha são obrigatórios. Quando a API rejeita as credenciais, a tela exibe uma mensagem de erro sem usar `alert()`.

## Fluxo da Tela

O administrador acessa a tela de login, informa e-mail e senha e confirma o acesso. Se as credenciais estiverem corretas, o sistema salva `adminToken` e redireciona para o Dashboard Admin.

Caso o usuário já possua token válido, a aplicação pode redirecioná-lo diretamente para o Dashboard Admin, evitando novo preenchimento de login.

## Integração com API e localStorage

A chave utilizada é:

- `adminToken`: token JWT do administrador autenticado.

Ao fazer login corretamente, essa chave é gravada. Ao sair pela área administrativa ou receber `401 Unauthorized`, ela é removida.

## Observacoes

A autenticação não é mais simulada no frontend. Ela é validada pela rota `POST /api/Auth/login-colaborador`.

## Testes Realizados

- Validação de campos vazios.
- Validação de usuário/e-mail e senha incorretos.
- Login com credenciais de colaborador cadastradas no backend.
- Redirecionamento para o Dashboard após login.
- Retorno ao cardápio pelo botão da tela.
