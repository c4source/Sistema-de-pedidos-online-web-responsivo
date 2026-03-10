# Diagrama de Caso de Uso

O diagrama de caso de uso apresenta as interações entre os atores do sistema e as principais funcionalidades disponíveis na aplicação.

## Atores do Sistema

- Cliente
- Administrador
- Operador da Cozinha

## Diagrama

![Diagrama de Caso de Uso](../diagrams/caso_de_uso.svg)

## Descrição

### 👥 Atores e Responsabilidades do Sistema

O diagrama de Casos de Uso segmenta as permissões e os fluxos de trabalho de acordo com três perfis operacionais distintos:

* **Cliente:** Interage com a interface para visualizar o cardápio, consultar detalhes dos produtos, realizar pedidos e acompanhar o status de suas solicitações em tempo real.
* **Administrador:** Responsável pela governança do sistema. Realiza login para acessar o painel administrativo, onde executa o controle completo (cadastro, edição, remoção ou inativação) de produtos, gerencia a disponibilidade do estoque e possui controle para visualização, filtragem e atualização do status dos pedidos globais.
* **Operador da Cozinha:** Atua exclusivamente na logística de produção. Visualiza a fila de pedidos pendentes e atualiza o status das ordens em andamento.

---

### 🔗 Relacionamentos e Dinâmicas (UML)

Para garantir a integridade dos processos e a modularidade do sistema, foram aplicados os seguintes estereótipos de relacionamento:

#### 1. Relacionamento `<<include>>` (Inclusão Obrigatória)
Utilizado em fluxos onde uma funcionalidade base depende estritamente da execução de outra para ser concluída com sucesso.
* **Realizar Pedido** -> **Informar Dados do Pedido**
* **Acessar Painel Administrativo** -> **Realizar Login**

#### 2. Relacionamento `<<extend>>` (Extensão Condicional)
Aplicado a comportamentos opcionais que complementam um caso de uso base, sendo acionados apenas mediante a escolha do usuário ou uma condição específica.
* **Visualizar Pedidos** <- **Filtrar Pedidos**: A filtragem atua como um refinamento complementar e opcional à visualização padrão dos pedidos.
