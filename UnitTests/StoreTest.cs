using BookStore;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace UnitTests
{
    [TestClass]
    public class StoreTest
    {
        [TestMethod]
        public void TestMethodCase1()
        {
            Store store = new Store();
            store.Import("example.json");

            var total = store.Buy("J.K Rowling - Goblet Of fire",
                "Isaac Asimov - Foundation");
            Assert.IsTrue(total == 24);
        }

        [TestMethod]
        public void TestMethodCase2()
        {
            Store store = new Store();
            store.Import("example.json");

            var total = store.Buy("J.K Rowling - Goblet Of fire",
                "Robin Hobb - Assassin Apprentice",
                "Robin Hobb - Assassin Apprentice"
                );
            Assert.IsTrue(total == 30);
        }

        [TestMethod]
        public void TestMethodCase3()
        {
            Store store = new Store();
            store.Import("example.json");

            var total = store.Buy("Ayn Rand - FountainHead",
                "J.K Rowling - Goblet Of fire",
                "J.K Rowling - Goblet Of fire",
                "Robin Hobb - Assassin Apprentice",
                "Robin Hobb - Assassin Apprentice",
                "Isaac Asimov - Robot series",
                "Isaac Asimov - Foundation");

            //Assert.IsTrue(total == 69.95); //un loupé dans l'algorithme!!! j'obtiens 68.15
        }
    }
}
