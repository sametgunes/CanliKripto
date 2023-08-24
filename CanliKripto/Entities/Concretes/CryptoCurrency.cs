using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CanliKripto.Entities.Concretes
{
    public class CryptoCurrency {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(10)]
        public string Symbol { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        
        public List<CryptoCurrencyValue> Values { get; set; } = new List<CryptoCurrencyValue>();
      
    }
    
}
