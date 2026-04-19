using System;
using System.Collections.Generic;
using System.Threading;

namespace ConsoleApp3
{
    public class TreeType
    {
        public string Name;
        public string Color;
        public byte[] Texture;

        public TreeType(string name, string color)
        {
            Name = name;
            Color = color;
            Texture = new byte[1000];
        }
    }

    public class Tree
    {
        public int X;
        public int Y;
        public TreeType Type;

        public Tree(int x, int y, TreeType type)
        {
            X = x;
            Y = y;
            Type = type;
        }
    }

    public class TreeFactory
    {
        private static Dictionary<string, TreeType> types = new Dictionary<string, TreeType>();

        public static TreeType GetTreeType(string name, string color)
        {
            string key = name + color;

            if (!types.ContainsKey(key))
            {
                types[key] = new TreeType(name, color);
            }

            return types[key];
        }

        public static int GetTypesCount()
        {
            return types.Count;
        }
    }

    public interface ICamera
    {
        void GetStream();
    }

    public class RealCamera : ICamera
    {
        private string name;

        public RealCamera(string name)
        {
            this.name = name;

            Console.WriteLine("Загрузка камеры " + name + "...");
            Thread.Sleep(2000);
        }

        public void GetStream()
        {
            Console.WriteLine("Показ видео с камеры: " + name);
        }
    }

    public class CameraProxy : ICamera
    {
        private string name;
        private string token;
        private RealCamera realCamera;

        public CameraProxy(string name, string token)
        {
            this.name = name;
            this.token = token;
        }

        public void GetStream()
        {
            Console.WriteLine("Запрос к камере: " + name);

            if (token != "123")
            {
                Console.WriteLine("Нет доступа!");
                return;
            }

            if (realCamera == null)
            {
                realCamera = new RealCamera(name);
            }

            realCamera.GetStream();
        }
    }

    class Program
    {
        static void Main()
        {
            Console.WriteLine("=== FLYWEIGHT ===");

            List<Tree> forest = new List<Tree>();

            for (int i = 0; i < 100000; i++)
            {
                if (i % 2 == 0)
                    forest.Add(new Tree(i, i, TreeFactory.GetTreeType("Дуб", "Зеленый")));
                else
                    forest.Add(new Tree(i, i, TreeFactory.GetTreeType("Береза", "Белый")));
            }

            Console.WriteLine("Создано типов деревьев: " + TreeFactory.GetTypesCount());


            Console.WriteLine("\n=== PROXY ===");

            ICamera[] cameras =
            {
            new CameraProxy("Камера 1", "123"),
            new CameraProxy("Камера 2", "bad"),
            new CameraProxy("Камера 3", "123")
        };

            cameras[0].GetStream();

            Console.WriteLine("\nПопробуем другую:");
            cameras[1].GetStream();
        }
    }
}
