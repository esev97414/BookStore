using BookStore.Interfaces;
using System.Collections.Generic;

namespace BookStore
{
    public class PromotionService
    {
        private ICustomerPromotionStrategy _customerPromotion;

        public PromotionService(ICustomerPromotionStrategy customerPromotion)
        {
            _customerPromotion = customerPromotion;
        }

        public double CalculateCustomerPromotion(List<string> list)
        {
            return _customerPromotion.CalculatePromotion(list);
        }
    }
}
