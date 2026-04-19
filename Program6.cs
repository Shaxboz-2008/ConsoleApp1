using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ConsoleApp7
{

    class Money
    {
        public decimal Amount;
        public string Currency;

        public Money(decimal amount, string currency)
        {
            Amount = amount;
            Currency = currency;
        }

        static decimal Convert(decimal amount, string from, string to)
        {
            if (from == to) return amount;
            if (from == "USD" && to == "RUB") return amount * 100;
            if (from == "RUB" && to == "USD") return amount / 100;
            return amount;
        }

        public static Money operator +(Money a, Money b)
        {
            return new Money(a.Amount + Convert(b.Amount, b.Currency, a.Currency), a.Currency);
        }

        public static Money operator -(Money a, Money b)
        {
            return new Money(a.Amount - Convert(b.Amount, b.Currency, a.Currency), a.Currency);
        }
    }

    class Transaction
    {
        public string Category;
        public decimal Amount;
    }

    class Wallet
    {
        public Dictionary<string, List<Transaction>> data = new();

        public event Action<string> BudgetExceeded;

        public void Add(Transaction t)
        {
            if (!data.ContainsKey(t.Category))
                data[t.Category] = new List<Transaction>();

            data[t.Category].Add(t);

            decimal sum = 0;
            foreach (var x in data[t.Category])
                sum += x.Amount;

            if (sum > 1000)
                BudgetExceeded?.Invoke(t.Category);
        }
    }

    class Stats
    {
        public int Str, Agi, Int;

        public static Stats operator +(Stats a, Stats b)
        {
            return new Stats
            {
                Str = a.Str + b.Str,
                Agi = a.Agi + b.Agi,
                Int = a.Int + b.Int
            };
        }
    }

    class Character
    {
        public int HP = 100;

        public event Action<int> OnHealthChanged;
        public event Action OnDeath;

        public void TakeDamage(int dmg)
        {
            HP -= dmg;
            OnHealthChanged?.Invoke(HP);

            if (HP <= 0)
                OnDeath?.Invoke();
        }
    }

    class Product
    {
        public string Name;
        public double Price;
        public int Count;
    }

    class Warehouse
    {
        public ObservableCollection<Product> items = new();

        public event Action<string> LowStock;

        public void Add(Product p)
        {
            items.Add(p);
        }

        public void Check()
        {
            foreach (var p in items)
                if (p.Count < 5)
                    LowStock?.Invoke(p.Name);
        }
    }

    class Packet
    {
        public string Data;

        public static bool operator ==(Packet a, Packet b) => a.Data == b.Data;
        public static bool operator !=(Packet a, Packet b) => a.Data != b.Data;

        public override bool Equals(object obj) => base.Equals(obj);
        public override int GetHashCode() => base.GetHashCode();
    }

    class Monitor
    {
        public Action<Packet> pipeline;
        public event Action<string> ThreatDetected;

        public void Process(Packet p)
        {
            pipeline?.Invoke(p);

            if (p.Data.Contains("virus"))
                ThreatDetected?.Invoke("Угроза!");
        }
    }

    class TimeSlot
    {
        public DateTime Start;
        public DateTime End;

        public static bool operator &(TimeSlot a, TimeSlot b)
        {
            return a.Start < b.End && b.Start < a.End;
        }
    }

    class MyFile
    {
        public string Name;
        public int Size;
    }

    class MyDirectory
    {
        public List<MyFile> files = new();
        public Dictionary<string, MyDirectory> dirs = new();

        public event Action<string> OnFileAccess;

        public void Read(string name)
        {
            OnFileAccess?.Invoke(name);
        }
    }

    class Packet2
    {
        public string Data;

        public static Packet2 operator +(Packet2 a, Packet2 b)
        {
            return new Packet2 { Data = a.Data + b.Data };
        }
    }

    class Pipeline
    {
        public List<Func<Packet2, Packet2>> steps = new();

        public event Action<string> Error;

        public Packet2 Run(Packet2 p)
        {
            try
            {
                foreach (var step in steps)
                    p = step(p);
            }
            catch
            {
                Error?.Invoke("Ошибка обработки");
            }

            return p;
        }
    }

    class Stat
    {
        public int Value;

        public static Stat operator +(Stat a, int b)
        {
            return new Stat { Value = a.Value + b };
        }

        public static implicit operator int(Stat s)
        {
            return s.Value;
        }

        public event Action<int> OnChange;
    }

    class Price
    {
        public decimal Value;

        public static bool operator >(Price a, Price b) => a.Value > b.Value;
        public static bool operator <(Price a, Price b) => a.Value < b.Value;

        public static decimal operator %(Price p, int percent)
        {
            return p.Value * percent / 100;
        }
    }

    class User
    {
        public string Name;
    }

    class Notifier
    {
        public event Action Done;

        public void Send(List<User> users)
        {
            foreach (var u in users)
                Console.WriteLine("Отправлено: " + u.Name);

            Done?.Invoke();
        }
    }

    class Program
    {
        static void Main()
        {
            var wallet = new Wallet();
            wallet.BudgetExceeded += c => Console.WriteLine("Лимит превышен: " + c);
            wallet.Add(new Transaction { Category = "Food", Amount = 800 });
            wallet.Add(new Transaction { Category = "Food", Amount = 300 });

            var hero = new Character();
            hero.OnHealthChanged += hp => Console.WriteLine("HP: " + hp);
            hero.OnDeath += () => Console.WriteLine("Герой умер");
            hero.TakeDamage(120);

            var wh = new Warehouse();
            wh.LowStock += n => Console.WriteLine("Мало товара: " + n);
            wh.Add(new Product { Name = "Телефон", Count = 3 });
            wh.Check();

            var monitor = new Monitor();
            monitor.pipeline += p => Console.WriteLine("Пакет: " + p.Data);
            monitor.ThreatDetected += msg => Console.WriteLine(msg);
            monitor.Process(new Packet { Data = "virus detected" });

            var t1 = new TimeSlot { Start = DateTime.Now, End = DateTime.Now.AddHours(2) };
            var t2 = new TimeSlot { Start = DateTime.Now.AddHours(1), End = DateTime.Now.AddHours(3) };
            Console.WriteLine("Пересечение: " + (t1 & t2));

            var dir = new MyDirectory();
            dir.OnFileAccess += f => Console.WriteLine("Открыт файл: " + f);
            dir.Read("test.txt");

            var pipe = new Pipeline();
            pipe.steps.Add(p => { p.Data += " step1"; return p; });
            var result = pipe.Run(new Packet2 { Data = "data" });
            Console.WriteLine(result.Data);

            Stat s = new Stat { Value = 10 };
            s += 5;
            Console.WriteLine("Stat: " + (int)s);

            var pr = new Price { Value = 1000 };
            Console.WriteLine("Комиссия 10%: " + (pr % 10));

            var notifier = new Notifier();
            notifier.Done += () => Console.WriteLine("Все отправлено");
            notifier.Send(new List<User> { new User { Name = "Alex" } });
        }
    }
}
