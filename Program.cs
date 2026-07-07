using System;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("========================================");
        Console.WriteLine("     Multi-Store Transaction Portal     ");
        Console.WriteLine("========================================");

        bool running = true;

        while (running)
        {
            Console.WriteLine("\n1. Admin");
            Console.WriteLine("2. Customer");
            Console.WriteLine("3. Exit");
            Console.Write("Select: ");

            string choice = Console.ReadLine();

            if (choice == "1")
            {
                AdminMenu();
            }
            else if (choice == "2")
            {
                CustomerMenu();
            }
            else if (choice == "3")
            {
                running = false;
                Console.WriteLine("Thank You for Visting Our Store!");
            }
            else
            {
                Console.WriteLine("Invalid option.");
            }
        }
    }

    static void AdminMenu()
    {
        Console.Write("\nAdmin password: ");
        string pw = Console.ReadLine();
        if (pw != "vrs3")
        {
            Console.WriteLine("Wrong password.");
            return;
        }

        bool back = false;

        while (!back)
        {
            Console.WriteLine("\n--- Admin Menu ---");
            Console.WriteLine("1. Add Store");
            Console.WriteLine("2. View Stores");
            Console.WriteLine("3. Add Product");
            Console.WriteLine("4. View All Products");
            Console.WriteLine("5. View All Transactions");
            Console.WriteLine("6. Back");
            Console.Write("Select: ");

            string choice = Console.ReadLine();

            if (choice == "1")
            {
                Console.Write("Store Name: ");
                string name = Console.ReadLine();
                Console.Write("Location: ");
                string location = Console.ReadLine();

                try
                {
                    DBHelper.AddStore(name, location);
                    Console.WriteLine("Store added.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
            }
            else if (choice == "2")
            {
                try
                {
                    DBHelper.ViewStores();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
            }
            else if (choice == "3")
            {
                try
                {
                    Console.Write("Store ID: ");
                    int storeId = Convert.ToInt32(Console.ReadLine());
                    Console.Write("Product Name: ");
                    string name = Console.ReadLine();
                    Console.Write("Price (Rs.): ");
                    double price = Convert.ToDouble(Console.ReadLine());
                    Console.Write("Quantity: ");
                    int qty = Convert.ToInt32(Console.ReadLine());
                    DBHelper.AddProduct(storeId, name, price, qty);
                    Console.WriteLine("Product added.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
            }
            else if (choice == "4")
            {
                try
                {
                    DBHelper.ViewProducts();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
            }
            else if (choice == "5")
            {
                try
                {
                    DBHelper.ViewAllTransactions();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
            }
            else if (choice == "6")
            {
                back = true;
            }
            else
            {
                Console.WriteLine("Invalid option.");
            }
        }
    }

    static void CustomerMenu()
    {
        Console.Write("\nEnter your name: ");
        string customerName = Console.ReadLine();

        bool back = false;

        while (!back)
        {
            Console.WriteLine("\n--- Customer Menu ---");
            Console.WriteLine("1. View All Stores");
            Console.WriteLine("2. View Products by Store");
            Console.WriteLine("3. Buy Product");
            Console.WriteLine("4. My Transactions");
            Console.WriteLine("5. Back");
            Console.Write("Select: ");

            string choice = Console.ReadLine();

            if (choice == "1")
            {
                try
                {
                    DBHelper.ViewStores();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
            }
            else if (choice == "2")
            {
                try
                {
                    Console.Write("Store ID: ");
                    int storeId = Convert.ToInt32(Console.ReadLine());
                    DBHelper.ViewProductsByStore(storeId);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
            }
            else if (choice == "3")
            {
                try
                {
                    Console.Write("Product ID: ");
                    int productId = Convert.ToInt32(Console.ReadLine());

                    double price = DBHelper.GetProductPrice(productId);
                    int stock = DBHelper.GetProductStock(productId);

                    if (price == -1 || stock == 0)
                    {
                        Console.WriteLine("Product not available or out of stock.");
                    }
                    else
                    {
                        Console.WriteLine("Price per unit: Rs." + price);
                        Console.WriteLine("Available stock: " + stock);
                        Console.Write("Quantity: ");
                        int qty = Convert.ToInt32(Console.ReadLine());

                        if (qty <= 0 || qty > stock)
                        {
                            Console.WriteLine("Invalid quantity.");
                        }
                        else
                        {
                            double total = NativeCalc.GetTotal(price, qty);
                            Console.WriteLine("Total: Rs." + total);
                            Console.Write("Confirm? (y/n): ");
                            string confirm = Console.ReadLine();

                            if (confirm == "y" || confirm == "Y")
                            {
                                DBHelper.AddTransaction(customerName, productId, qty, total);
                                Console.WriteLine("Purchase successful. Total paid: Rs." + total);
                            }
                            else
                            {
                                Console.WriteLine("Cancelled.");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
            }
            else if (choice == "4")
            {
                try
                {
                    DBHelper.ViewTransactionsByCustomer(customerName);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
            }
            else if (choice == "5")
            {
                back = true;
            }
            else
            {
                Console.WriteLine("Invalid option.");
            }
        }
    }
}
