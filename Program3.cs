using System;
using System.Collections.Generic;

namespace ConsoleApp4
{
    public interface IShip
    {
        void Info();
    }

    public interface IStation
    {
        void Info();
    }

    public class TerranShip : IShip
    {
        public void Info() => Console.WriteLine("Корабль людей");
    }

    public class TerranStation : IStation
    {
        public void Info() => Console.WriteLine("Станция людей");
    }

    public class CyborgShip : IShip
    {
        public void Info() => Console.WriteLine("Корабль роботов");
    }

    public class CyborgStation : IStation
    {
        public void Info() => Console.WriteLine("Станция роботов");
    }

    public interface IFactory
    {
        IShip CreateShip();
        IStation CreateStation();
    }

    public class TerranFactory : IFactory
    {
        public IShip CreateShip() => new TerranShip();
        public IStation CreateStation() => new TerranStation();
    }

    public class CyborgFactory : IFactory
    {
        public IShip CreateShip() => new CyborgShip();
        public IStation CreateStation() => new CyborgStation();
    }

    public abstract class SpacePort
    {
        public void DeliverShip()
        {
            IShip ship = CreateShip();
            Console.WriteLine("Порт отправил корабль:");
            ship.Info();
        }

        public abstract IShip CreateShip();
    }

    public class ScoutPort : SpacePort
    {
        public override IShip CreateShip()
        {
            return new TerranShip();
        }
    }

    public class DestroyerPort : SpacePort
    {
        public override IShip CreateShip()
        {
            return new CyborgShip();
        }
    }


    public class Cruiser
    {
        public string Engine;
        public string Weapon;
        public string Armor;

        public void Show()
        {
            Console.WriteLine($"Крейсер: {Engine}, {Weapon}, {Armor}");
        }
    }

    public interface ICruiserBuilder
    {
        void SetEngine();
        void SetWeapon();
        void SetArmor();
        Cruiser GetResult();
    }

    public class CruiserBuilder : ICruiserBuilder
    {
        private Cruiser cruiser = new Cruiser();

        public void SetEngine() => cruiser.Engine = "Двигатель X";
        public void SetWeapon() => cruiser.Weapon = "Лазеры";
        public void SetArmor() => cruiser.Armor = "Тяжелая броня";

        public Cruiser GetResult() => cruiser;
    }

    public class ShipyardDirector
    {
        public void BuildScienceCruiser(ICruiserBuilder builder)
        {
            builder.SetEngine();
            builder.SetWeapon();
            builder.SetArmor();
        }
    }

    public class CombatDrone
    {
        public string Serial;
        public int Energy;

        public CombatDrone(string serial, int energy)
        {
            Serial = serial;
            Energy = energy;
        }

        public CombatDrone Clone()
        {
            return new CombatDrone(Serial, Energy);
        }

        public void Show()
        {
            Console.WriteLine($"Дрон {Serial}, энергия {Energy}");
        }
    }

    class Program
    {
        static void Main()
        {
            Console.WriteLine("=== Abstract Factory ===");

            IFactory factory = new TerranFactory();

            var ship = factory.CreateShip();
            var station = factory.CreateStation();

            ship.Info();
            station.Info();


            Console.WriteLine("\n=== Factory Method ===");

            SpacePort port = new ScoutPort();
            port.DeliverShip();


            Console.WriteLine("\n=== Builder ===");

            ICruiserBuilder builder = new CruiserBuilder();
            ShipyardDirector director = new ShipyardDirector();

            director.BuildScienceCruiser(builder);
            Cruiser cruiser = builder.GetResult();

            cruiser.Show();


            Console.WriteLine("\n=== Prototype ===");

            CombatDrone prototype = new CombatDrone("D-1", 100);

            List<CombatDrone> army = new List<CombatDrone>();

            for (int i = 0; i < 10; i++)
            {
                var clone = prototype.Clone();
                clone.Serial = "D-" + (i + 1);
                clone.Energy -= i * 5;

                army.Add(clone);
            }

            foreach (var d in army)
                d.Show();
        }
    }
}
