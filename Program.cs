using System;
using System.IO;
using System.Linq;

namespace ConsoleApp1
{
    class Program
    {
        static void Main()
        {
            Console.Write("Введите путь к папке: ");
            string path = Console.ReadLine();

            try
            {
                if (!System.IO.Directory.Exists(path))
                    throw new DirectoryNotFoundException("Папка не найдена");

                var dir = new System.IO.DirectoryInfo(path);

                Console.WriteLine("\n=== ИНФОРМАЦИЯ ===");
                Console.WriteLine("Путь: " + dir.FullName);
                Console.WriteLine("Создана: " + dir.CreationTime);
                Console.WriteLine("Изменена: " + dir.LastWriteTime);

                var files = dir.GetFiles();
                var dirs = dir.GetDirectories();

                Console.WriteLine("\nФайлов: " + files.Length);
                Console.WriteLine("Папок: " + dirs.Length);

                Console.WriteLine("\n=== ПАПКИ ===");
                foreach (var d in dirs)
                    Console.WriteLine($"{d.Name} | {d.CreationTime}");

                Console.WriteLine("\n=== ФАЙЛЫ ===");
                foreach (var f in files)
                    Console.WriteLine($"{f.Name} | {f.Length} байт | {f.Extension}");

                if (files.Length > 0)
                {
                    var max = files.OrderByDescending(f => f.Length).First();
                    var min = files.OrderBy(f => f.Length).First();

                    Console.WriteLine("\n=== РАЗМЕРЫ ===");
                    Console.WriteLine($"Самый большой: {max.Name} ({max.Length})");
                    Console.WriteLine($"Самый маленький: {min.Name} ({min.Length})");
                }

                int totalFiles = CountFiles(path);
                Console.WriteLine("\nВсего файлов (включая вложенные): " + totalFiles);

                Console.WriteLine("\n=== ДЕРЕВО ===");
                PrintTree(path, "");
            }
            catch (DirectoryNotFoundException e)
            {
                Console.WriteLine("Ошибка: " + e.Message);
            }
            catch (UnauthorizedAccessException)
            {
                Console.WriteLine("Нет доступа к папке");
            }
            catch (ArgumentException)
            {
                Console.WriteLine("Неверный путь");
            }
            catch (Exception e)
            {
                Console.WriteLine("Другая ошибка: " + e.Message);
            }
        }

        static int CountFiles(string path)
        {
            int count = 0;

            try
            {
                count += System.IO.Directory.GetFiles(path).Length;

                foreach (var dir in System.IO.Directory.GetDirectories(path))
                    count += CountFiles(dir);
            }
            catch { }

            return count;
        }

        static void PrintTree(string path, string indent)
        {
            try
            {
                var dir = new System.IO.DirectoryInfo(path);

                Console.WriteLine(indent + dir.Name);

                foreach (var d in dir.GetDirectories())
                    PrintTree(d.FullName, indent + "  ");

                foreach (var f in dir.GetFiles())
                    Console.WriteLine(indent + "  " + f.Name);
            }
            catch { }
        }
    }
}
