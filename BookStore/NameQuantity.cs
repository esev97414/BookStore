using BookStore.Interfaces;

namespace BookStore
{
    class NameQuantity : INameQuantity
    {
        private string _name;
        private int _quantity;

        public NameQuantity(string name, int quantity)
        {
            _name = name;
            _quantity = quantity;
        }
        public string Name
        {
            get
            {
                return _name;
            }
        }

        public int Quantity
        {
            get
            {
                return _quantity;
            }
        }
    }
}
