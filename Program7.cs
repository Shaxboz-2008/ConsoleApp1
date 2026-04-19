using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp8
{
    public partial class Product
    {
        public int Id;
        public string Name;
        public decimal Price;
        public string Category;
    }

    public partial class Product
    {
        public void DisplayInfo()
        {
            Console.WriteLine($"{Id} | {Name} | {Price} | {Category}");
        }
    }

    public static class ProductExtensions
    {
        public static decimal ApplyDiscount(this Product p, decimal percent)
        {
            return p.Price - (p.Price * percent / 100);
        }

        public static string ToTechnicalString(this Product p)
        {
            return $"[{p.Category}] {p.Name} : {p.Price}";
        }
    }

    class Program
    {
        static (decimal Min, decimal Max, decimal Avg) GetStats(decimal[] prices)
        {
            decimal min = prices.Min();
            decimal max = prices.Max();
            decimal avg = prices.Average();

            return (min, max, avg);
        }

        static void Main()
        {
            var products = new List<Product>
        {
            new Product { Id = 1, Name = "Телефон", Price = 1000, Category = "Электроника" },
            new Product { Id = 2, Name = "Ноутбук", Price = 2000, Category = "Электроника" },
            new Product { Id = 3, Name = "Хлеб", Price = 50, Category = "Еда" }
        };

            Console.WriteLine("=== Товары ===");
            foreach (var p in products)
                p.DisplayInfo();

            Console.WriteLine("\n=== Скидка ===");
            Console.WriteLine(products[0].ApplyDiscount(10));

            Console.WriteLine("\n=== Формат ===");
            Console.WriteLine(products[0].ToTechnicalString());

            var prices = products.Select(p => p.Price).ToArray();
            var stats = GetStats(prices);

            Console.WriteLine("\n=== Статистика ===");
            Console.WriteLine($"Min: {stats.Min}, Max: {stats.Max}, Avg: {stats.Avg}");

            Console.WriteLine("\n=== Анонимные типы ===");

            var simpleList = products
                .Select(p => new { p.Name, p.Category });

            foreach (var item in simpleList)
                Console.WriteLine($"{item.Name} - {item.Category}");
        }
    }
}
