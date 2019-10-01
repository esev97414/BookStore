using BookStore.Interfaces;
using BookStore.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BookStore
{
    public class Store : IStore
    {
        public string Name { get; set; }
        public List<Book> Catalog { get; set; }
        public List<Category> Categories { get; set; }

        public Store()
        {
            Catalog = new List<Book>();
            Categories = new List<Category>();
        }

        public void Import(string catalogAsJson)
        {
            var json = File.ReadAllText(catalogAsJson);
            try
            {
                var jObject = JObject.Parse(json);

                if (jObject != null)
                {
                    JArray categoryArray = (JArray)jObject["Category"];
                    if(categoryArray != null)
                    {
                        Categories = categoryArray.
                            Select(c => new Category { Name = c["Name"].ToString(), Discount = double.Parse(c["Discount"].ToString()) }).
                            ToList();
                    }

                    JArray catalogArray = (JArray)jObject["Catalog"];
                    if (catalogArray != null)
                    {
                        foreach (var item in catalogArray)
                        {
                            Book b = new Book
                            {
                                Name = item["Name"].ToString(),
                                Category = item["Category"].ToString(),
                                Price = int.Parse(item["Price"].ToString()),
                                Quantity = int.Parse(item["Quantity"].ToString())

                            };
                            Catalog.Add(b);
                        }
                    }
                }
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public int Quantity(string name)
        {
            var book = Catalog.Where(b => b.Name == name).SingleOrDefault();
            return book != null ? book.Quantity : 0;
        }

        public double Buy(params string[] basketByNames)
        {
            var total = 0.0;

            // count number of item occurences
            Dictionary<string, int> NumberOfOccurencesPerItem = new Dictionary<string, int>();
            var ReducedInitialList = basketByNames.Distinct().ToList();
            int nbr = 0;
            for (int i = 0; i < ReducedInitialList.Count(); i++)
            {
                for (int j = 0; j < basketByNames.Count(); j++)
                {
                    if (ReducedInitialList[i] == basketByNames[j])
                        nbr++;
                }
                NumberOfOccurencesPerItem.Add(ReducedInitialList[i], nbr);
                nbr = 0;
            }

            //find all books with same category
            List<List<string>> ItemsWithSameCategory = new List<List<string>>();
            List<string> backupList = new List<string>();
            for (int i = 0; i < ReducedInitialList.Count(); i++)
            {
                var book1 = Catalog.Where(b => b.Name == ReducedInitialList[i]).SingleOrDefault();
                if (!backupList.Contains(book1.Name))
                {
                    List<string> concatList = new List<string>();
                    for (int j = 0; j < ReducedInitialList.Count(); j++)
                    {
                        var book2 = Catalog.Where(b => b.Name == ReducedInitialList[j]).SingleOrDefault();
                        if (book1.Category == book2.Category)
                        {
                            if (book1.Name != book2.Name)
                            {
                                concatList.Add(book2.Name);
                            }
                        }
                    }
                    concatList.Add(book1.Name);
                    ItemsWithSameCategory.Add(concatList);
                    backupList = concatList;
                }
            }

            //Add all items with a discount on first item
            total = new PromotionFactory().Create(this).Calculate(NumberOfOccurencesPerItem, ItemsWithSameCategory);

            return total;
        }
    }
}
