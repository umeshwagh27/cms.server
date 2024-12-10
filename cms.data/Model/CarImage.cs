using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace cms.data.Model
{
    public class CarImage
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string Name { get; set; } = default!;
        public byte[] Data { get; set; } = default!;
        public virtual long? ClassId { get; set; }
        [ForeignKey("ClassId")]
        public virtual CarModelClass ClassIdFk { get; set; }
    }
}
