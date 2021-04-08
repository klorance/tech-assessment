using System.Diagnostics;

namespace CSharp.Models
{
       public class Order
    {
        public string Description { get; set; }
        public int CustomerId { get; set; }
        public int Id { get; set; }
        public bool Cancelled { get; set; }

        public Order()
        {
            
        }
        public Order(int Id, int CustomerId, string description )
        {
            this.Id = Id;
            this.CustomerId = CustomerId;
            this.Description = description;
        }

    }
}