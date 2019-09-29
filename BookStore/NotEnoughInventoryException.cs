using BookStore.Interfaces;
using System;
using System.Collections.Generic;

namespace BookStore
{
    public class NotEnoughInventoryException : Exception
    {
        public NotEnoughInventoryException(List<INameQuantity> notFound) : base()
        {
            Missing = notFound;
        }
        public IEnumerable<INameQuantity> Missing { get; }
    }
}
