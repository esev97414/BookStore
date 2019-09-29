using BookStore.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace BookStore
{
    class UniqueItemPromotionStrategy : ICustomerPromotionStrategy
    {
        private Store _store;
        int _reducedListCount;
        Dictionary<string, string> _refDict;

        public UniqueItemPromotionStrategy(Store store, int count, Dictionary<string, string> dict)
        {
            _store = store;
            _reducedListCount = count;
            _refDict = dict;
        }
        public double CalculatePromotion(List<string> list)
        {
            var uniqueItemsDeduction = 0.0;
            foreach (var item in list)
            {
                var book = _store.Catalog.Where(b => b.Name == item).SingleOrDefault();
                var category = _store.Categories.Where(c => c.Name == book.Category).SingleOrDefault();
                if (_reducedListCount != 0 && _refDict.Contains(new KeyValuePair<string, string>(item, book.Category)))
                {
                    uniqueItemsDeduction += book.Price * (1 - category.Discount);
                }
                else
                    uniqueItemsDeduction += book.Price;
            }
            return uniqueItemsDeduction;
        }
    }
}
