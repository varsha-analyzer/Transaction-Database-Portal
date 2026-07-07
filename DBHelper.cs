using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

class Store
{
    public int Id;
    public string Name;
    public string Location;
}

class Product
{
    public int Id;
    public string StoreName;
    public string Name;
    public double Price;
    public int Quantity;
}

class Transaction
{
    public int Id;
    public string CustomerName;
    public string ProductName;
    public int Quantity;
    public double Total;
    public string Date;
}

class DBHelper
{
    static string connectionString = "server=127.0.0.1;user=root;password=;database=multistoredb;";

    public static MySqlConnection GetConnection()
    {
        MySqlConnection conn = new MySqlConnection(connectionString);
        conn.Open();
        return conn;
    }

    public static List<Store> GetStoresList()
    {
        List<Store> list = new List<Store>();
        MySqlConnection conn = GetConnection();
        MySqlCommand cmd = new MySqlCommand("SELECT * FROM stores", conn);
        MySqlDataReader reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            Store s = new Store();
            s.Id = Convert.ToInt32(reader["id"]);
            s.Name = reader["name"].ToString();
            s.Location = reader["location"].ToString();
            list.Add(s);
        }
        reader.Close();
        conn.Close();
        return list;
    }

    public static List<Product> GetProductsList()
    {
        List<Product> list = new List<Product>();
        MySqlConnection conn = GetConnection();
        string query = "SELECT p.id, s.name as store, p.name, p.price, p.quantity FROM products p JOIN stores s ON p.store_id = s.id";
        MySqlCommand cmd = new MySqlCommand(query, conn);
        MySqlDataReader reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            Product p = new Product();
            p.Id = Convert.ToInt32(reader["id"]);
            p.StoreName = reader["store"].ToString();
            p.Name = reader["name"].ToString();
            p.Price = Convert.ToDouble(reader["price"]);
            p.Quantity = Convert.ToInt32(reader["quantity"]);
            list.Add(p);
        }
        reader.Close();
        conn.Close();
        return list;
    }

    public static List<Product> GetProductsByStore(int storeId)
    {
        List<Product> list = new List<Product>();
        MySqlConnection conn = GetConnection();
        string query = "SELECT p.id, s.name as store, p.name, p.price, p.quantity FROM products p JOIN stores s ON p.store_id = s.id WHERE p.store_id = " + storeId;
        MySqlCommand cmd = new MySqlCommand(query, conn);
        MySqlDataReader reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            Product p = new Product();
            p.Id = Convert.ToInt32(reader["id"]);
            p.StoreName = reader["store"].ToString();
            p.Name = reader["name"].ToString();
            p.Price = Convert.ToDouble(reader["price"]);
            p.Quantity = Convert.ToInt32(reader["quantity"]);
            list.Add(p);
        }
        reader.Close();
        conn.Close();
        return list;
    }

    public static List<Transaction> GetAllTransactionsList()
    {
        List<Transaction> list = new List<Transaction>();
        MySqlConnection conn = GetConnection();
        string query = "SELECT t.id, t.customer_name, p.name as product, t.quantity, t.total, t.date FROM transactions t JOIN products p ON t.product_id = p.id";
        MySqlCommand cmd = new MySqlCommand(query, conn);
        MySqlDataReader reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            Transaction t = new Transaction();
            t.Id = Convert.ToInt32(reader["id"]);
            t.CustomerName = reader["customer_name"].ToString();
            t.ProductName = reader["product"].ToString();
            t.Quantity = Convert.ToInt32(reader["quantity"]);
            t.Total = Convert.ToDouble(reader["total"]);
            t.Date = reader["date"].ToString();
            list.Add(t);
        }
        reader.Close();
        conn.Close();
        return list;
    }

    public static List<Transaction> GetTransactionsByCustomer(string customerName)
    {
        List<Transaction> list = new List<Transaction>();
        MySqlConnection conn = GetConnection();
        string query = "SELECT t.id, t.customer_name, p.name as product, t.quantity, t.total, t.date FROM transactions t JOIN products p ON t.product_id = p.id WHERE t.customer_name = '" + customerName + "'";
        MySqlCommand cmd = new MySqlCommand(query, conn);
        MySqlDataReader reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            Transaction t = new Transaction();
            t.Id = Convert.ToInt32(reader["id"]);
            t.CustomerName = reader["customer_name"].ToString();
            t.ProductName = reader["product"].ToString();
            t.Quantity = Convert.ToInt32(reader["quantity"]);
            t.Total = Convert.ToDouble(reader["total"]);
            t.Date = reader["date"].ToString();
            list.Add(t);
        }
        reader.Close();
        conn.Close();
        return list;
    }

    public static void AddStore(string name, string location)
    {
        MySqlConnection conn = GetConnection();
        string query = "INSERT INTO stores (name, location) VALUES ('" + name + "', '" + location + "')";
        MySqlCommand cmd = new MySqlCommand(query, conn);
        cmd.ExecuteNonQuery();
        conn.Close();
    }

    public static void AddProduct(int storeId, string name, double price, int quantity)
    {
        MySqlConnection conn = GetConnection();
        string query = "INSERT INTO products (store_id, name, price, quantity) VALUES (" + storeId + ", '" + name + "', " + price + ", " + quantity + ")";
        MySqlCommand cmd = new MySqlCommand(query, conn);
        cmd.ExecuteNonQuery();
        conn.Close();
    }

    public static void AddTransaction(string customerName, int productId, int qty, double total)
    {
        MySqlConnection conn = GetConnection();
        string query = "INSERT INTO transactions (customer_name, product_id, quantity, total, date) VALUES ('" + customerName + "', " + productId + ", " + qty + ", " + total + ", NOW())";
        MySqlCommand cmd = new MySqlCommand(query, conn);
        cmd.ExecuteNonQuery();
        string updateQty = "UPDATE products SET quantity = quantity - " + qty + " WHERE id = " + productId;
        MySqlCommand updateCmd = new MySqlCommand(updateQty, conn);
        updateCmd.ExecuteNonQuery();
        conn.Close();
    }

    public static double GetProductPrice(int productId)
    {
        MySqlConnection conn = GetConnection();
        MySqlCommand cmd = new MySqlCommand("SELECT price FROM products WHERE id = " + productId, conn);
        MySqlDataReader reader = cmd.ExecuteReader();
        double price = -1;
        if (reader.Read())
        {
            price = Convert.ToDouble(reader["price"]);
        }
        reader.Close();
        conn.Close();
        return price;
    }

    public static int GetProductStock(int productId)
    {
        MySqlConnection conn = GetConnection();
        MySqlCommand cmd = new MySqlCommand("SELECT quantity FROM products WHERE id = " + productId, conn);
        MySqlDataReader reader = cmd.ExecuteReader();
        int stock = 0;
        if (reader.Read())
        {
            stock = Convert.ToInt32(reader["quantity"]);
        }
        reader.Close();
        conn.Close();
        return stock;
    }

    public static void ViewStores()
    {
        List<Store> stores = GetStoresList();
        Console.WriteLine("\n--- Stores ---");
        Console.WriteLine("{0,-5} {1,-20} {2,-20}", "ID", "Name", "Location");
        Console.WriteLine(new string('-', 45));
        foreach (Store s in stores)
        {
            Console.WriteLine("{0,-5} {1,-20} {2,-20}", s.Id, s.Name, s.Location);
        }
    }

    public static void ViewProducts()
    {
        List<Product> products = GetProductsList();
        Console.WriteLine("\n--- All Products ---");
        Console.WriteLine("{0,-5} {1,-15} {2,-20} {3,-10} {4,-10}", "ID", "Store", "Product", "Price", "Qty");
        Console.WriteLine(new string('-', 60));
        foreach (Product p in products)
        {
            Console.WriteLine("{0,-5} {1,-15} {2,-20} {3,-10} {4,-10}", p.Id, p.StoreName, p.Name, "Rs." + p.Price, p.Quantity);
        }
    }

    public static void ViewProductsByStore(int storeId)
    {
        List<Product> products = GetProductsByStore(storeId);
        Console.WriteLine("\n--- Products ---");
        Console.WriteLine("{0,-5} {1,-20} {2,-10} {3,-10}", "ID", "Name", "Price", "Qty");
        Console.WriteLine(new string('-', 45));
        foreach (Product p in products)
        {
            Console.WriteLine("{0,-5} {1,-20} {2,-10} {3,-10}", p.Id, p.Name, "Rs." + p.Price, p.Quantity);
        }
    }

    public static void ViewAllTransactions()
    {
        List<Transaction> transactions = GetAllTransactionsList();
        Console.WriteLine("\n--- All Transactions ---");
        Console.WriteLine("{0,-5} {1,-15} {2,-20} {3,-5} {4,-10} {5,-20}", "ID", "Customer", "Product", "Qty", "Total", "Date");
        Console.WriteLine(new string('-', 75));
        foreach (Transaction t in transactions)
        {
            Console.WriteLine("{0,-5} {1,-15} {2,-20} {3,-5} {4,-10} {5,-20}", t.Id, t.CustomerName, t.ProductName, t.Quantity, "Rs." + t.Total, t.Date);
        }
    }

    public static void ViewTransactionsByCustomer(string customerName)
    {
        List<Transaction> transactions = GetTransactionsByCustomer(customerName);
        Console.WriteLine("\n--- Your Transactions ---");
        Console.WriteLine("{0,-5} {1,-20} {2,-5} {3,-10} {4,-20}", "ID", "Product", "Qty", "Total", "Date");
        Console.WriteLine(new string('-', 60));
        foreach (Transaction t in transactions)
        {
            Console.WriteLine("{0,-5} {1,-20} {2,-5} {3,-10} {4,-20}", t.Id, t.ProductName, t.Quantity, "Rs." + t.Total, t.Date);
        }
    }
}
