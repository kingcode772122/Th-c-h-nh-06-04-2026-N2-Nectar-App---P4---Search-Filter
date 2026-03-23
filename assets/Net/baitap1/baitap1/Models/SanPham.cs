using System.ComponentModel.DataAnnotations;

namespace baitap1.Models
{
    public enum ProductType
    {
        Latest,
        Featured,
        Special
    }

    public class SanPham
    {
        public int Id { get; set; }
        public string Ten { get; set; }
        public decimal Gia { get; set; }
        public decimal? GiaCu { get; set; }
        public string Hinh { get; set; }
        public bool Sale { get; set; }
        public bool SoldOut { get; set; }
        public int Rating { get; set; }
        public ProductType Type { get; set; }
    }
}
