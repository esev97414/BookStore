using System.Collections.Generic;

namespace BookStore.Interfaces
{
    public interface ICustomerPromotionStrategy
    {
        double CalculatePromotion(List<string> list);
    }
}
