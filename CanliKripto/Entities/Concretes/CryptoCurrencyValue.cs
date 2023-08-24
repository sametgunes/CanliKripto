using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CanliKripto.Entities.Concretes
{
    [Table("CryptoCurrencyValue")]
    public class CryptoCurrencyValue
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public decimal Change24h { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [ForeignKey("CryptoCurrency")]
        public int CryptoCurrencyId { get; set; }

        public CryptoCurrency CryptoCurrency { get; set; }
    }
}
