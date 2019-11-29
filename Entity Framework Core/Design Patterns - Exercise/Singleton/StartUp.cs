namespace Singleton
{
    using System;

    public class StartUp
    {
        public static void Main()
        {
            var db = SingletonDataContainer.Instance;
            Console.WriteLine(db.GetPopulation("ivan"));
            var db2 = SingletonDataContainer.Instance;

            var db3 = new SingletonDataContainer();
            var db4 = SingletonDataContainer.Instance;
        }
    }
}
