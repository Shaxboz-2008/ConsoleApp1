using System;
using System.Collections.Generic;

namespace ConsoleApp2
{

    public interface IMenuItem
    {
        string GetName();
        double GetPrice();
    }

    public class Dish : IMenuItem
    {
        private string name;
        private double price;

        public Dish(string name, double price)
        {
            this.name = name;
            this.price = price;
        }

        public string GetName() => name;

        public double GetPrice() => price;
    }

    public class ComboDeal : IMenuItem
    {
        private List<IMenuItem> items = new List<IMenuItem>();
        private string name;

        public ComboDeal(string name)
        {
            this.name = name;
        }

        public void Add(IMenuItem item)
        {
            items.Add(item);
        }

        public string GetName() => name;

        public double GetPrice()
        {
            double sum = 0;

            foreach (var item in items)
                sum += item.GetPrice();

            return sum * 0.9;
        }

        public void Show(string indent = "")
        {
            Console.WriteLine(indent + name);

            foreach (var item in items)
            {
                if (item is ComboDeal combo)
                    combo.Show(indent + "  ");
                else
                    Console.WriteLine(indent + "  " + item.GetName() + " - " + item.GetPrice());
            }
        }
    }

    public abstract class DishDecorator : IMenuItem
    {
        protected IMenuItem item;

        public DishDecorator(IMenuItem item)
        {
            this.item = item;
        }

        public abstract string GetName();
        public abstract double GetPrice();
    }

    public class CheeseDecorator : DishDecorator
    {
        public CheeseDecorator(IMenuItem item) : base(item) { }

        public override string GetName()
        {
            return item.GetName() + " + Сыр";
        }

        public override double GetPrice()
        {
            return item.GetPrice() + 50;
        }
    }

    public class MushroomDecorator : DishDecorator
    {
        public MushroomDecorator(IMenuItem item) : base(item) { }

        public override string GetName()
        {
            return item.GetName() + " + Грибы";
        }

        public override double GetPrice()
        {
            return item.GetPrice() + 40;
        }
    }

    class Program
    {
        static void Main()
        {
            IMenuItem pizza = new Dish("Пицца Маргарита", 300);

            pizza = new CheeseDecorator(pizza);
            pizza = new CheeseDecorator(pizza);
            pizza = new MushroomDecorator(pizza);

            Console.WriteLine("Одиночная пицца:");
            Console.WriteLine(pizza.GetName() + " = " + pizza.GetPrice());

            ComboDeal miniCombo = new ComboDeal("Мини-комбо");
            miniCombo.Add(new Dish("Картофель", 100));
            miniCombo.Add(new Dish("Соус", 30));

            ComboDeal megaCombo = new ComboDeal("Мега-комбо");

            megaCombo.Add(pizza);
            megaCombo.Add(new Dish("Напиток", 80));
            megaCombo.Add(miniCombo);

            Console.WriteLine("\n=== СТРУКТУРА ЗАКАЗА ===");
            megaCombo.Show();

            Console.WriteLine("\nИТОГО: " + megaCombo.GetPrice());
        }
    }
}
