namespace Singleton
{
    using System;
    using System.Collections.Generic;

    public class SingletonDataContainer : ISingletonContainer
    {
        private Dictionary<string, int> capitals = new Dictionary<string, int>();
        private static SingletonDataContainer instance = new SingletonDataContainer();

        public SingletonDataContainer()
        {
            Console.WriteLine("Initializing singleton object");

            var elements = new string[] {"ivan", "10", "sianf", "20", "sao", "30", "lksadf", "40"};
            for (int i = 0; i < elements.Length; i += 2)
            {
                this.capitals.Add(elements[i], int.Parse(elements[i + 1]));
            }
        }

        public static SingletonDataContainer Instance => instance;

        public int GetPopulation(string name)
        {
            return this.capitals[name];
        }
    }
}
