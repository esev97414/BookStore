using BookStore.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace BookStore
{
    public class FirstItemPromotionStrategy : ICustomerPromotionStrategy
    {
        private Store _store;
        public FirstItemPromotionStrategy(Store store)
        {
            _store = store;
        }
        public double CalculatePromotion(List<string> list)
        {
            var firstItemDeduction = 0.0;
            foreach (var item in list)
            {
                var book = _store.Catalog.Where(b => b.Name == item).SingleOrDefault();
                var category = _store.Categories.Where(c => c.Name == book.Category).SingleOrDefault();
                firstItemDeduction += book.Price * (1 - category.Discount);
            }
            return firstItemDeduction;
        }
    }
}
