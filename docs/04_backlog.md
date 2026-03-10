# Requisitos Funcionais e Não funcionais e backlog do Sistema.

Este documento apresenta o **backlog de requisitos funcionais** levantados para o desenvolvimento do sistema do Projeto Integrador Multidisciplinar (PIM).

Durante o processo de levantamento de requisitos, foi realizado um **brainstorming entre os integrantes da equipe**, resultando em diversas propostas de funcionalidades.

Após análise técnica e avaliação de escopo, os requisitos foram organizados em três categorias:

- **MVP (Minimum Viable Product)** – funcionalidades essenciais que serão implementadas no projeto
- **Melhorias Planejadas** – funcionalidades adicionais que podem ser implementadas caso haja tempo disponível
- **Ideias Futuras** – funcionalidades registradas para possível evolução futura do sistema

Essa organização permite controlar o escopo do projeto e priorizar o desenvolvimento das funcionalidades mais importantes.

---

### 📎 Anexo: Documento Original

Para acessar a documentação completa com o detalhamento de todos os requisitos na íntegra, consulte o arquivo oficial abaixo:

👉 **[Acessar o Documento de Requisitos Completo (.docx)](./nome-do-seu-arquivo.docx)**

---

# 1. Requisitos MVP (Implementação Prioritária)

Os requisitos abaixo representam o **conjunto mínimo de funcionalidades necessárias para o funcionamento completo do sistema**.

---

## RF01 – Autenticação do Administrador
O sistema deve permitir que o administrador realize login para acessar a área administrativa.

---

## RF02 – Acesso ao Painel Administrativo
O sistema deve disponibilizar uma área administrativa restrita para gerenciamento do sistema.

---

## RF03 – Cadastro de Produtos
O sistema deve permitir ao administrador cadastrar novos produtos contendo informações como nome, descrição e preço.

---

## RF04 – Edição de Produtos
O sistema deve permitir ao administrador editar informações dos produtos cadastrados.

---

## RF05 – Remoção ou Inativação de Produtos
O sistema deve permitir ao administrador remover ou inativar produtos para que deixem de aparecer para os clientes.

---

## RF06 – Listagem de Produtos
O sistema deve exibir aos clientes a lista de produtos disponíveis para realização de pedidos.

---

## RF07 – Visualização de Detalhes do Produto
O sistema deve permitir que o cliente visualize detalhes de cada produto.

---

## RF08 – Criação de Pedido
O sistema deve permitir que o cliente realize pedidos selecionando produtos e quantidades.

---

## RF09 – Registro de Informações do Pedido
O sistema deve registrar informações necessárias para a realização do pedido, como nome do cliente e observações.

---

## RF10 – Geração de Código do Pedido
O sistema deve gerar um identificador único para cada pedido realizado.

---

## RF11 – Registro de Data e Hora do Pedido
O sistema deve registrar automaticamente a data e hora em que o pedido foi realizado.

---

## RF12 – Visualização de Pedidos
O sistema deve permitir que o administrador visualize todos os pedidos realizados pelos clientes.

---

## RF13 – Atualização de Status do Pedido
O sistema deve permitir que o administrador ou operador da cozinha altere o status do pedido.

Exemplos de status:
- Recebido
- Em preparo
- Pronto
- Finalizado
- Cancelado

---

## RF14 – Consulta de Status do Pedido
O sistema deve permitir que o cliente consulte o status do pedido utilizando o código do pedido.

---

## RF15 – Fila de Pedidos para Cozinha
O sistema deve permitir que a cozinha visualize pedidos em andamento e seus respectivos itens.

---

## RF17 – Controle de Disponibilidade de Produtos
O sistema deve permitir que o administrador marque produtos como disponíveis ou indisponíveis.

---

## RF19 – Filtro de Pedidos
O sistema deve permitir que o administrador filtre pedidos por status ou data.

---
# Requisitos Não Funcionais (RNF)

Esta seção descreve os **requisitos não funcionais** do sistema. Esses requisitos definem **características de qualidade, restrições técnicas e padrões de funcionamento** da aplicação.

Diferentemente dos requisitos funcionais, que descrevem **o que o sistema faz**, os requisitos não funcionais especificam **como o sistema deve se comportar**, considerando aspectos como desempenho, segurança, usabilidade, acessibilidade e manutenção.

Esses requisitos são essenciais para garantir que o sistema seja **eficiente, seguro, estável e fácil de utilizar** pelos diferentes perfis de usuários.

---

## RNF01 — Usabilidade (UX/UI)

O sistema deve seguir princípios de **usabilidade baseados nas heurísticas de Nielsen**, garantindo que a interface seja clara, intuitiva e de fácil aprendizagem.

O fluxo de navegação deve permitir que usuários sem treinamento prévio consigam utilizar o sistema e realizar pedidos de forma simples e compreensível.

Este requisito está relacionado à disciplina de **UX/UI Design**, buscando melhorar a experiência do usuário e reduzir dificuldades de navegação.

---

## RNF02 — Responsividade

O sistema deve adaptar sua interface automaticamente a diferentes tamanhos de tela, garantindo boa visualização e funcionamento em diversos dispositivos.

O sistema deve suportar, no mínimo, as seguintes resoluções:

- **320px** — dispositivos móveis  
- **768px** — tablets  
- **1280px ou superior** — computadores

A adaptação da interface não deve causar perda de funcionalidade ou legibilidade.

Este requisito está relacionado à disciplina de **Desenvolvimento Web Responsivo**.

---

## RNF03 — Desempenho

O sistema deve apresentar tempo de resposta adequado para garantir uma boa experiência de uso.

Critérios esperados:

- As páginas do sistema devem carregar em **até 3 segundos** em uma conexão de **10 Mbps**.
- Operações de **cadastro, edição, remoção e consulta de dados (CRUD)** devem apresentar tempo médio de resposta inferior a **1 segundo**.

Este requisito garante que o sistema permaneça **rápido e eficiente durante sua utilização**.

---

## RNF04 — Segurança e Autenticação

O sistema deve proteger o acesso à área administrativa por meio de **autenticação com login e senha**, impedindo acesso não autorizado.

As senhas cadastradas não devem ser armazenadas em **texto puro**, devendo utilizar mecanismos de **criptografia ou hash** para garantir a proteção das credenciais.

Este requisito contribui para a **segurança das informações e controle de acesso do sistema**.

---

## RNF05 — Disponibilidade

O sistema deve permanecer disponível durante o horário de funcionamento simulado do restaurante.

Horário de operação esperado:

**07h às 23h**

A indisponibilidade do sistema deve ocorrer apenas em casos de manutenção ou falhas externas.

---

## RNF06 — Manutenibilidade

O sistema deve ser desenvolvido seguindo **boas práticas de programação e princípios de orientação a objetos**, permitindo facilidade na manutenção, correção de erros e evolução do software.

O código deve:

- possuir organização modular  
- utilizar nomes claros para classes, métodos e variáveis  
- conter comentários nos trechos principais

Este requisito está relacionado à disciplina de **Programação Orientada a Objetos com C#**.

---

## RNF07 — Acessibilidade

A interface do sistema deve seguir recomendações mínimas de acessibilidade baseadas nas diretrizes **WCAG 2.1 nível AA**, garantindo maior inclusão digital.

Entre as práticas recomendadas estão:

- uso de **textos alternativos em imagens**
- **contraste adequado entre texto e fundo**
- organização semântica adequada da interface

Além disso, o projeto deve considerar ao menos **uma proposta de recurso inclusivo**, como um glossário de termos relacionados ao sistema em **LIBRAS**.

Este requisito está relacionado à disciplina de **LIBRAS e acessibilidade digital**.

---

## RNF08 — Rastreabilidade de Dados

O sistema deve registrar informações essenciais de cada pedido realizado, permitindo rastreabilidade das operações.

Cada pedido deve conter:

- identificador único do pedido  
- data de registro  
- horário do pedido  
- status atual do pedido  

Essas informações permitem o **acompanhamento do fluxo de pedidos e geração de relatórios para análise de dados**.

---

## RNF09 — Portabilidade

O sistema deve funcionar corretamente nos principais navegadores modernos, sem dependência de plugins externos.

Navegadores suportados:

- **Google Chrome**
- **Mozilla Firefox**
- **Microsoft Edge**
- **Safari**

Este requisito garante maior compatibilidade e acesso ao sistema em diferentes ambientes.

---

## Considerações Finais

Os requisitos não funcionais apresentados definem **padrões de qualidade essenciais para o sistema**, garantindo que a aplicação seja:

- eficiente
- segura
- acessível
- fácil de utilizar
- fácil de manter

Esses requisitos complementam os **requisitos funcionais**, assegurando que o sistema não apenas execute suas funcionalidades, mas também ofereça **boa experiência de uso, estabilidade e confiabilidade operacional**.

# 2. Melhorias Planejadas

Estes requisitos representam melhorias identificadas durante o levantamento de ideias e podem ser implementados caso haja disponibilidade de tempo durante o desenvolvimento.

---

## RF20 – Cadastro de Cliente
O sistema pode permitir que clientes realizem cadastro na plataforma para acessar funcionalidades adicionais.

---

## RF21 – Login de Cliente
O sistema pode permitir que clientes autenticados realizem login na plataforma.

---

## RF22 – Carrinho de Compras
O sistema pode permitir que clientes adicionem produtos a um carrinho antes de finalizar o pedido.

---

## RF23 – Histórico de Pedidos
O sistema pode permitir que clientes consultem o histórico de pedidos realizados.

---

## RF24 – Confirmação de Pedido pelo Atendente
O sistema pode permitir que um atendente confirme o recebimento e processamento de pedidos realizados.

---

## RF25 – Painel de Monitoramento de Pedidos
O sistema pode fornecer um painel para acompanhamento de pedidos em tempo real.

---

# 3. Ideias Futuras (Backlog de Expansão)

Os requisitos abaixo representam funcionalidades mais avançadas que podem ser consideradas em futuras evoluções do sistema.

---

## RF26 – Processamento de Pagamento Online
O sistema pode permitir que clientes realizem pagamento online utilizando métodos como cartão ou PIX.

---

## RF27 – Integração com Sistemas de Pagamento
O sistema pode integrar serviços externos de pagamento para processamento de transações.

---

## RF28 – Relatórios de Vendas
O sistema pode gerar relatórios contendo dados de vendas, faturamento e quantidade de pedidos por período.

---

## RF29 – Promoções e Campanhas
O sistema pode permitir que o administrador configure promoções e campanhas de marketing.

---

## RF30 – Gestão de Usuários
O sistema pode permitir que o administrador gerencie usuários cadastrados na plataforma.

---

## RF31 – Configuração Geral do Sistema
O sistema pode disponibilizar configurações administrativas para controle do funcionamento geral da aplicação.

---

## RF32 – Notificação de Status do Pedido
O sistema pode notificar o cliente quando houver atualização no status do pedido.

