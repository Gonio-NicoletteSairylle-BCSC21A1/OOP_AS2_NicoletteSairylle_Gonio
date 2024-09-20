using System;
using System.Linq;
using System.Collections.Generic;

namespace Coffee
{
    public class User
    {
        public string Username { get; }
        public string Password { get; }

        public User(string username, string password)
        {
            Username = username;
            Password = password;
        }
    }

    public class Menu
    {
        private readonly Dictionary<string, decimal> _items = new Dictionary<string, decimal>();

        public void AddItem(string item, decimal price)
        {
            if (price > 0)
            {
                _items[item] = price;
                Console.WriteLine("Item Added Successfully");
            }
            else
            {
                Console.WriteLine("Invalid Price!");
            }
        }

        public void ViewMenu()
        {
            if (_items.Count == 0)
            {
                Console.WriteLine("No menu items available.");
                return;
            }

            Console.WriteLine("Menu:");
            int i = 1;
            foreach (var item in _items)
            {
                Console.WriteLine($"{i}. {item.Key} - {item.Value:F2}");
                i++;
            }
        }

        public Dictionary<string, decimal> GetItems() => new Dictionary<string, decimal>(_items);
    }

    public class Order
    {
        private readonly Dictionary<string, decimal> _orderItems = new Dictionary<string, decimal>();

        public void AddItem(string item, decimal price)
        {
            if (_orderItems.ContainsKey(item))
            {
                _orderItems[item] += price;
            }
            else
            {
                _orderItems[item] = price;
            }
            Console.WriteLine("Item Added to Order!");
        }

        public void ViewOrder()
        {
            if (_orderItems.Count == 0)
            {
                Console.WriteLine("No items in the order.");
                return;
            }

            Console.WriteLine("Your Order:");
            foreach (var item in _orderItems)
            {
                Console.WriteLine($"{item.Key} - {item.Value:C}");
            }
        }

        public decimal CalculateTotal() => _orderItems.Values.Sum();
    }

    public class KapeShop
    {
        private readonly Menu _menu = new Menu();
        private readonly Order _order = new Order();
        private readonly List<User> _users = new List<User>
        {
            new User("nico", "nicgon"),
            new User("student", "162823"),
            new User("prof", "1234")
        };

        private bool Authenticate()
        {
            Console.Write("Enter Username: ");
            string username = Console.ReadLine();

            Console.Write("Enter Password: ");
            string password = Console.ReadLine();

            var user = _users.FirstOrDefault(u => u.Username == username && u.Password == password);
            if (user != null)
            {
                Console.WriteLine("Login successful!");
                return true;
            }

            Console.WriteLine("Invalid credentials. Please try again.");
            return false;
        }

        public void Run()
        {
            bool authenticated = false;

            // Login loop
            while (!authenticated)
            {
                Console.WriteLine("Please log in to access the Coffee Shop.");
                authenticated = Authenticate();
            }

            // Main menu loop
            bool exit = false;
            while (!exit)
            {
                Console.WriteLine("Welcome to the Coffee Shop!");
                Console.WriteLine("1. Add Menu Item");
                Console.WriteLine("2. View Menu");
                Console.WriteLine("3. Place Order");
                Console.WriteLine("4. View Order");
                Console.WriteLine("5. Calculate Total");
                Console.WriteLine("6. Exit");
                Console.Write("Please Select Above: ");

                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AddMenuItem();
                        break;
                    case "2":
                        _menu.ViewMenu();
                        break;
                    case "3":
                        PlaceOrder();
                        break;
                    case "4":
                        _order.ViewOrder();
                        break;
                    case "5":
                        CalculateTotal();
                        break;
                    case "6":
                        Console.WriteLine("Thank you! Come Again!");
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("Invalid Choice! Please select a valid option.");
                        break;
                }
            }
        }

        private void AddMenuItem()
        {
            Console.Write("Enter Item: ");
            string item = Console.ReadLine();

            Console.Write("Enter Item Price: ");
            if (decimal.TryParse(Console.ReadLine(), out decimal price))
            {
                _menu.AddItem(item, price);
            }
            else
            {
                Console.WriteLine("Invalid Price Input!");
            }
        }

        private void PlaceOrder()
        {
            var menuItems = _menu.GetItems();
            if (menuItems.Count == 0)
            {
                Console.WriteLine("No menu items available.");
                return;
            }

            _menu.ViewMenu();

            Console.Write("Select Item Number To Order: ");
            if (int.TryParse(Console.ReadLine(), out int orderNumber) && orderNumber >= 1 && orderNumber <= menuItems.Count)
            {
                var item = menuItems.ElementAt(orderNumber - 1);
                _order.AddItem(item.Key, item.Value);
            }
            else
            {
                Console.WriteLine("Invalid Order Number.");
            }
        }

        private void CalculateTotal()
        {
            decimal total = _order.CalculateTotal();
            Console.WriteLine($"Total Amount Payable: {total:C}");
        }
    }

    public static class Program
    {
        public static void Main()
        {
            var kapeShop = new KapeShop();
            kapeShop.Run();
        }
    }
}
