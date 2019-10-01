namespace BookStore
{
    class Program
    {
        static void Main(string[] args)
        {
            Store store = new Store();
            store.Import("example.json");

            var quantity = store.Quantity("Ayn Rand - FountainHead");
            System.Console.WriteLine(quantity);

            var total = store.Buy("Ayn Rand - FountainHead", //Philosophy
                "J.K Rowling - Goblet Of fire", //Fantastique
                "J.K Rowling - Goblet Of fire", //Fantastique
                "Robin Hobb - Assassin Apprentice", //Fantastique
                "Robin Hobb - Assassin Apprentice", //Fantastique
                "Isaac Asimov - Robot series", //Science Fiction
                "Isaac Asimov - Foundation"); //Science Fiction

            //var total = store.Buy("J.K Rowling - Goblet Of fire",
            //    "Isaac Asimov - Foundation");

            //var total = store.Buy("J.K Rowling - Goblet Of fire",
            //    "Robin Hobb - Assassin Apprentice",
            //    "Robin Hobb - Assassin Apprentice"
            //    );

            System.Console.WriteLine(total);

        }
    }
}
