using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace cms.data.Model
{
    public class Car
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        [StringLength(10)]
        public string Brand { get; set; } = default!;       
        public string ModelName { get; set; } = default!;
        [StringLength(30)]
        public string ModelCode { get; set; } = default!;
        [StringLength(100)]
        public string Description { get; set; } = default!;  
   
        [DataType(DataType.DateTime)]
        public DateTime DateofManufacturing { get; set; } = default!;
        [DefaultValue(true)]
        public bool IsActive { get; set; }
    }
}

