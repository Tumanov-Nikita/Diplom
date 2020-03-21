using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DIPLOM.Model
{
    public class Order
    {
        public int Id { get; set; }
        public string Client { get; set; }
        public Complectation complectation;
        public double Price { get; set; }
        public OrderStatus Status { get; set; }
    }
}
