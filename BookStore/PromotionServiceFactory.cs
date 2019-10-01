using BookStore.Interfaces;

namespace BookStore
{
    public abstract class PromotionServiceFactory
    {
        public abstract IPromotionService Create(Store store);
    }


}
