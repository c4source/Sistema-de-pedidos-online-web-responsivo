INSERT INTO produto (nome, preco, descricao, status_disponibilidade, imagem_url)
VALUES
    ('Pizza Calabresa', 39.90, 'Pizza com calabresa e cebola', 'disponivel', 'img/pizza_calabresa.png'),
    ('Pizza Mussarela', 35.00, 'Pizza tradicional de mussarela', 'disponivel', 'img/pizza_mussarela.png'),
    ('Refrigerante 2L', 12.00, 'Refrigerante gelado 2 litros', 'disponivel', 'img/refrigerante_2l.png');

INSERT INTO pedido (codigo, nome_cliente, observacoes, status, valor_total)
VALUES
    ('PED001', 'Gabriel', 'Sem cebola', 'recebido', 39.90),
    ('PED002', 'Maria', 'Borda recheada', 'em_preparo', 47.00);

INSERT INTO item_pedido (id_pedido, id_produto, quantidade, preco_unitario, subtotal)
VALUES
    (1, 1, 1, 39.90, 39.90),
    (2, 2, 1, 35.00, 35.00),
    (2, 3, 1, 12.00, 12.00);