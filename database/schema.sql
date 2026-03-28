CREATE TABLE produto (
    id_produto SERIAL PRIMARY KEY,
    nome VARCHAR(100) NOT NULL
        CHECK (TRIM(nome) <> ''),
    preco DECIMAL(10, 2) NOT NULL
        CHECK (preco > 0),
    descricao TEXT,
    status_disponibilidade VARCHAR(20) NOT NULL
        CHECK (status_disponibilidade IN ('disponivel', 'indisponivel')),
    imagem_url TEXT
);

CREATE TABLE pedido (
    id_pedido SERIAL PRIMARY KEY,
    codigo VARCHAR(50) NOT NULL UNIQUE
        CHECK (TRIM(codigo) <> ''),
    nome_cliente VARCHAR(100) NOT NULL
        CHECK (TRIM(nome_cliente) <> ''),
    telefone_cliente VARCHAR(20) NOT NULL
        CHECK (TRIM(telefone_cliente) <> ''),
    tipo_entrega VARCHAR(20) NOT NULL
        CHECK (tipo_entrega IN ('entrega', 'retirada')),
    rua_entrega VARCHAR(150),
    numero_entrega VARCHAR(20),
    bairro_entrega VARCHAR(100),
    complemento_entrega VARCHAR(150),
    observacoes TEXT,
    data_hora TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    status VARCHAR(20) NOT NULL
        CHECK (status IN ('recebido', 'em_preparo', 'pronto', 'finalizado', 'cancelado')),
    valor_total DECIMAL(10, 2) NOT NULL
        CHECK (valor_total >= 0),

    CONSTRAINT chk_endereco_obrigatorio_entrega
        CHECK (
            tipo_entrega = 'retirada'
            OR (
                tipo_entrega = 'entrega'
                AND rua_entrega IS NOT NULL
                AND TRIM(rua_entrega) <> ''
                AND numero_entrega IS NOT NULL
                AND TRIM(numero_entrega) <> ''
                AND bairro_entrega IS NOT NULL
                AND TRIM(bairro_entrega) <> ''
            )
        )
);

CREATE TABLE item_pedido (
    id_item_pedido SERIAL PRIMARY KEY,
    id_pedido INT NOT NULL,
    id_produto INT NOT NULL,
    quantidade INT NOT NULL
        CHECK (quantidade > 0),
    preco_unitario DECIMAL(10, 2) NOT NULL
        CHECK (preco_unitario > 0),
    subtotal DECIMAL(10, 2) GENERATED ALWAYS AS (quantidade * preco_unitario) STORED,

    CONSTRAINT fk_item_pedido_pedido
        FOREIGN KEY (id_pedido)
        REFERENCES pedido(id_pedido)
        ON DELETE RESTRICT,

    CONSTRAINT fk_item_pedido_produto
        FOREIGN KEY (id_produto)
        REFERENCES produto(id_produto)
        ON DELETE RESTRICT,

    CONSTRAINT uq_item_pedido_produto_por_pedido
        UNIQUE (id_pedido, id_produto)
);