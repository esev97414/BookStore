using BookStore.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace BookStore
{
    public class NoPromotionStrategy : ICustomerPromotionStrategy
    {
        private Store _store;
        public NoPromotionStrategy(Store store)
        {
            _store = store;
        }
        public double CalculatePromotion(List<string> list)
        {
            var sumOfItemsWithoutReduction = 0.0;
            foreach (var item in list)
            {
                var book = _store.Catalog.Where(b => b.Name == item).SingleOrDefault();
                sumOfItemsWithoutReduction += book.Price;
            }
            return sumOfItemsWithoutReduction;
        }
    }
}
