using System;

namespace ConsoleApp5
{
    public interface ICustomLogger
    {
        void LogMessage(string message);
    }

    public class LegacyScanner
    {
        public void ExecuteScan(string data)
        {
            Console.WriteLine("Сканирование: " + data);
        }
    }

    public class ScannerAdapter : ICustomLogger
    {
        private LegacyScanner scanner;

        public ScannerAdapter(LegacyScanner scanner)
        {
            this.scanner = scanner;
        }

        public void LogMessage(string message)
        {
            scanner.ExecuteScan(message);
        }
    }

    public interface IMessageSender
    {
        void SendMessage(string text);
    }

    public class SmsSender : IMessageSender
    {
        public void SendMessage(string text)
        {
            Console.WriteLine("SMS: " + text);
        }
    }

    public class EmailSender : IMessageSender
    {
        public void SendMessage(string text)
        {
            Console.WriteLine("Email: " + text);
        }
    }
    public abstract class Notification
    {
        protected IMessageSender sender;

        public Notification(IMessageSender sender)
        {
            this.sender = sender;
        }

        public abstract void Notify(string message);
    }
    public class UrgentNotification : Notification
    {
        public UrgentNotification(IMessageSender sender) : base(sender) { }

        public override void Notify(string message)
        {
            sender.SendMessage("ПРИОРИТЕТ! " + message);
        }
    }

    public class InfoNotification : Notification
    {
        public InfoNotification(IMessageSender sender) : base(sender) { }

        public override void Notify(string message)
        {
            sender.SendMessage(message);
        }
    }
    public class TVSystem
    {
        public void On() => Console.WriteLine("Телевизор включен");
    }

    public class AudioSystem
    {
        public void SetVolume() => Console.WriteLine("Громкость настроена");
    }

    public class LightingControl
    {
        public void DimLight() => Console.WriteLine("Свет приглушен");
    }

    public class SubscriptionService
    {
        public void Check() => Console.WriteLine("Подписка проверена");
    }
    public class SmartHomeFacade
    {
        private TVSystem tv = new TVSystem();
        private AudioSystem audio = new AudioSystem();
        private LightingControl light = new LightingControl();
        private SubscriptionService sub = new SubscriptionService();

        public void WatchMovie()
        {
            Console.WriteLine("\n=== Запуск фильма ===");

            tv.On();
            audio.SetVolume();
            light.DimLight();
            sub.Check();

            Console.WriteLine("Фильм запущен!");
        }
    }

    class Program
    {
        static void Main()
        {
            Console.WriteLine("=== Adapter ===");

            ICustomLogger logger = new ScannerAdapter(new LegacyScanner());
            logger.LogMessage("Проверка безопасности");


            Console.WriteLine("\n=== Bridge ===");

            IMessageSender sms = new SmsSender();
            IMessageSender email = new EmailSender();

            Notification n1 = new UrgentNotification(sms);
            Notification n2 = new InfoNotification(email);

            n1.Notify("Сервер упал");
            n2.Notify("Обновление прошло успешно");


            Console.WriteLine("\n=== Facade ===");

            SmartHomeFacade home = new SmartHomeFacade();
            home.WatchMovie();
        }
    }
}
