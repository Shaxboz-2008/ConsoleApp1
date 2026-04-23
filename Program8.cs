using System;

namespace ConsoleApp1
{
    class Transaction
    {
        public decimal Amount;
        public string Currency;
        public string Sender;
        public string Receiver;
    }

    abstract class Handler
    {
        protected Handler next;

        public Handler SetNext(Handler nextHandler)
        {
            next = nextHandler;
            return nextHandler;
        }

        public virtual void Handle(Transaction t)
        {
            if (next != null)
                next.Handle(t);
        }
    }

    class ValidationHandler : Handler
    {
        public override void Handle(Transaction t)
        {
            Console.WriteLine("Проверка данных...");

            if (string.IsNullOrEmpty(t.Sender) || string.IsNullOrEmpty(t.Receiver))
            {
                Console.WriteLine("Ошибка: пустые поля!");
                return;
            }

            if (t.Amount <= 0)
            {
                Console.WriteLine("Ошибка: сумма <= 0!");
                return;
            }

            base.Handle(t);
        }
    }

    class FraudCheckHandler : Handler
    {
        public override void Handle(Transaction t)
        {
            Console.WriteLine("Проверка на мошенничество...");

            if (t.Amount > 50000)
            {
                Console.WriteLine("Требуется дополнительное подтверждение!");
            }

            base.Handle(t);
        }
    }

    class CurrencyHandler : Handler
    {
        public override void Handle(Transaction t)
        {
            Console.WriteLine("Проверка валюты...");

            if (t.Currency != "USD")
            {
                Console.WriteLine($"Конвертация {t.Currency} -> USD");
                t.Currency = "USD";
            }

            base.Handle(t);
        }
    }

    class LimitHandler : Handler
    {
        public override void Handle(Transaction t)
        {
            Console.WriteLine("Проверка лимита...");

            if (t.Amount > 100000)
            {
                Console.WriteLine("Превышен лимит!");
                return;
            }

            Console.WriteLine("Транзакция успешно выполнена ✅");
            base.Handle(t);
        }
    }

    class Program
    {
        static void Main()
        {
            var validation = new ValidationHandler();
            var fraud = new FraudCheckHandler();
            var currency = new CurrencyHandler();
            var limit = new LimitHandler();

            validation
                .SetNext(fraud)
                .SetNext(currency)
                .SetNext(limit);

            var t = new Transaction
            {
                Amount = 60000,
                Currency = "EUR",
                Sender = "Alice",
                Receiver = "Bob"
            };

            validation.Handle(t);
        }
    }
}
