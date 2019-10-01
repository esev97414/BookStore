using System.Collections.Generic;

namespace BookStore.Interfaces
{
    public interface IPromotionService
    {
        double Calculate(Dictionary<string, int> numberOfOccurences,
            List<List<string>> sameCategoryItems);
    }


}
