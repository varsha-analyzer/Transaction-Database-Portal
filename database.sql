CREATE DATABASE IF NOT EXISTS multistoredb;
USE multistoredb;

CREATE TABLE IF NOT EXISTS stores (
    id INT AUTO_INCREMENT PRIMARY KEY,
    name VARCHAR(100),
    location VARCHAR(100)
);

CREATE TABLE IF NOT EXISTS products (
    id INT AUTO_INCREMENT PRIMARY KEY;
    store_id INT,
    name VARCHAR(100),
    price DECIMAL(10,2),
    quantity INT,
    FOREIGN KEY (store_id) REFERENCES stores(id)
);

CREATE TABLE IF NOT EXISTS transactions (
    id INT AUTO_INCREMENT PRIMARY KEY,
    customer_name VARCHAR(100),
    product_id INT,
    quantity INT,
    total DECIMAL(10,2),
    date DATETIME,
    FOREIGN KEY (product_id) REFERENCES products(id)
);

INSERT INTO stores (name, location) VALUES ('Store A', 'New York');
INSERT INTO stores (name, location) VALUES ('Store B', 'Los Angeles');

INSERT INTO products (store_id, name, price, quantity) VALUES (1, 'Apple', 1.50, 100);
INSERT INTO products (store_id, name, price, quantity) VALUES (1, 'Bread', 2.00, 50);
INSERT INTO products (store_id, name, price, quantity) VALUES (2, 'Milk', 3.00, 80);
INSERT INTO products (store_id, name, price, quantity) VALUES (2, 'Eggs', 4.50, 60);
