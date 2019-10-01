using BookStore.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace BookStore
{
    public class FirstItemPromotionService : IPromotionService
    {
        private Store _store;

        public FirstItemPromotionService(Store store)
        {
            _store = store;
        }

        public double Calculate(Dictionary<string, int> numberOfOccurences,
            List<List<string>> sameCategoryItems)
        {
            var subtotal = 0.0;
            foreach (var item in numberOfOccurences)
            {
                var book = _store.Catalog.Where(b => b.Name == item.Key).SingleOrDefault();
                var category = _store.Categories.Where(c => c.Name == book.Category).SingleOrDefault();

                if (item.Value > 1)
                {
                    subtotal += book.Price * (1 - category.Discount) + (book.Price * (item.Value - 1));
                }
                else if (item.Value == 1)
                {
                    foreach (var catItem in sameCategoryItems)
                    {
                        if (catItem.Contains(book.Name))
                        {
                            if (catItem.Count() > 1)
                            {
                                subtotal += book.Price * (1 - category.Discount);
                                break;
                            }
                            else
                            {
                                subtotal += book.Price;//nopromotionstrategy
                                break;
                            }
                        }
                    }
                }
            }
            return subtotal;
        }
    }
}
