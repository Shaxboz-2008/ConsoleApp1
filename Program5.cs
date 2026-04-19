namespace ConsoleApp6
{
    using System;
    using System.Text.RegularExpressions;

    class Program
    {
        static void Main()
        {
            string email = "user@example.com";
            Console.WriteLine(Regex.IsMatch(email, @"^[\w\.-]+@[\w\.-]+\.\w+$"));

            string text = "Позвони +7 (999) 123-45-67 или +7 (111) 000-00-00";
            var phones = Regex.Matches(text, @"\+7 \(\d{3}\) \d{3}-\d{2}-\d{2}");
            foreach (Match p in phones)
                Console.WriteLine(p.Value);

            string dates = "Сегодня 12.05.2024";
            string newDates = Regex.Replace(dates, @"(\d{2})\.(\d{2})\.(\d{4})", "$3-$2-$1");
            Console.WriteLine(newDates);

            string spaces = "Привет    мир   !";
            Console.WriteLine(Regex.Replace(spaces, @"\s{2,}", " "));

            string html = "<h1>Заголовок</h1>";
            var h1 = Regex.Match(html, @"<h1>(.*?)</h1>");
            Console.WriteLine(h1.Groups[1].Value);

            string password = "Test1234";
            Console.WriteLine(Regex.IsMatch(password, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,}$"));

            string priceText = "Товар стоит 1500 руб., скидка 200 руб.";
            var prices = Regex.Matches(priceText, @"\d+(?= руб)");
            foreach (Match pr in prices)
                Console.WriteLine(pr.Value);

            string ip = "192.168.1.1";
            Console.WriteLine(Regex.IsMatch(ip,
                @"^((25[0-5]|2[0-4]\d|1\d\d|\d\d|\d)\.){3}(25[0-5]|2[0-4]\d|1\d\d|\d\d|\d)$"));

            string code = "int myVar = 5; int AnotherVar = 10;";
            var vars = Regex.Matches(code, @"\b[a-z][a-zA-Z0-9]*\b");
            foreach (Match v in vars)
                Console.WriteLine(v.Value);

            string line = "Привет, мир! Как дела.";
            var parts = Regex.Split(line, @"[,.!]");
            foreach (var part in parts)
                Console.WriteLine(part.Trim());
        }
    }
}
