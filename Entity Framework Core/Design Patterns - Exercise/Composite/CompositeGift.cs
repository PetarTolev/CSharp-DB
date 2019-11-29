namespace Composite
{
    using System;
    using System.Collections.Generic;

    public class CompositeGift : GiftBase, IGiftOperations
    {
        private List<GiftBase> gifts;

        public CompositeGift(string name, int price) 
            : base(name, price)
        {
            this.gifts = new List<GiftBase>();
        }

        public override int CalculateTotalPrice()
        {
            int total = 0;

            Console.WriteLine($"{name} contains the following products with prices:");

            foreach (var gift in gifts)
            {
                total += gift.CalculateTotalPrice();
            }

            return total;
        }

        public void Add(GiftBase gift)
        {
            this.gifts.Add(gift);
        }

        public void Remove(GiftBase gift)
        {
            this.gifts.Remove(gift);
        }
    }
}