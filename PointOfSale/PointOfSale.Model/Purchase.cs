using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PointOfSale.Model
{
    public class Purchase
    {
        [Key]
        public int id { get; set; }
        public string P_Supplier_Name { get; set; }
        //public string P_Supplier_Address { get; set; }
        public string Product_Name { get; set; }
        public decimal Purchase_Price { get; set; }
        public int Quantity { get; set; }
        public int Supplier_Id { get; set; }
        [ForeignKey("Supplier_Id")]
        public Supplier Supplier { get; set; }
        public int Product_Id { get; set; }
        [ForeignKey("Product_Id")]
        public Product Product { get; set; }

    }
}
