using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace cms.data.Model
{
    public class CarModelClass 
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public virtual long? CarId { get; set; }
        [ForeignKey("CarId")]
        public virtual Car CarIdFk { get; set; }
        [StringLength(20)]
        public string ClassName { get; set; } = default!;
        public string Features { get; set; } = default!;
        public decimal Price { get; set; } = default!;
    }
}
