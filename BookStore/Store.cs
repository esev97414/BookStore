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

        PromotionService _promotionService;

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
            List<INameQuantity> NotFoundItem = new List<INameQuantity>();
            foreach (var item in basketByNames)
            {
                var book = Catalog.Where(b => b.Name == item).SingleOrDefault();
                if (book.Quantity <= 0)
                {
                    NotFoundItem.Add(new NameQuantity(book.Name, book.Quantity));
                }
            }

            if (NotFoundItem.Count > 0)
            {
                throw new NotEnoughInventoryException(NotFoundItem);
            }

            //Create internal collections
            //1. Dict<Name, CategoryName> representing basketByNames
            //2. List<Name> reprenting the duplicated elements if any
            Dictionary<string, string> NameToCategoryDict = new Dictionary<string, string>();
            List<string> duplicateList = ExtractDuplicateItems(NameToCategoryDict, basketByNames);

            var sumOfItemsWithoutReduction = 0.0;
            if (duplicateList.Any())
            {
                _promotionService = new PromotionService(new NoPromotionStrategy(this));
                sumOfItemsWithoutReduction = _promotionService.CalculateCustomerPromotion(duplicateList);
            }

            //Reduce DuplicateList
            var ReducedList = duplicateList.Distinct().ToList();
            var firstItemReduction = 0.0;
            if (ReducedList.Any())
            {
                _promotionService = new PromotionService(new FirstItemPromotionStrategy(this));
                firstItemReduction = _promotionService.CalculateCustomerPromotion(duplicateList);
            }
            
            //Get unique items (no duplicates)
            var namesFromDict = NameToCategoryDict.Select(d => d.Key).ToList();
            var uniqueItems = namesFromDict.Except(ReducedList).ToList();

            var uniqueItemsReduction = 0.0;
            if (uniqueItems.Any())
            {
                _promotionService = new PromotionService(new UniqueItemPromotionStrategy(this, ReducedList.Count, NameToCategoryDict));
                uniqueItemsReduction = _promotionService.CalculateCustomerPromotion(uniqueItems);
            }

            return sumOfItemsWithoutReduction + firstItemReduction + uniqueItemsReduction;

        }

        private List<string> ExtractDuplicateItems(Dictionary<string, string> initialList, params string[] basketByNames)
        {
            List<string> duplicateList = new List<string>();
            foreach (var item in basketByNames)
            {
                var book = Catalog.Where(b => b.Name == item).SingleOrDefault();
                if (initialList.ContainsKey(book.Name))
                {
                    duplicateList.Add(book.Name);
                }
                else
                    initialList.Add(book.Name, book.Category);
            }
            return duplicateList;
        }
    }
}
