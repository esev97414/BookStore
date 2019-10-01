using BookStore.Interfaces;

namespace BookStore
{
    public class PromotionFactory : PromotionServiceFactory
    {
        public override IPromotionService Create(Store store) => new FirstItemPromotionService(store);
    }

}
