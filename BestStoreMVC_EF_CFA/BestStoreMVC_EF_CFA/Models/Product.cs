using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace BestStoreMVC_EF_CFA.Models
{
    public class Product
    {
        [MaxLength(100)]
        public int ID { get; set; }
        [MaxLength(100)]
        public string Name { get; set; }
        [MaxLength(100)]
        public string Brand { get; set; }
        [MaxLength(100)]
        public string Catagory { get; set; }
        [Precision(16,2)]
        public decimal price { get; set; }
        public string Description { get; set; }
        [MaxLength(100)]
        public string ImageFileName { get; set; }
        public DateTime CreateAt { get; set; }
        
    }
}
