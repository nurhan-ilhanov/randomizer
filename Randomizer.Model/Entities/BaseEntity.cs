using System.ComponentModel.DataAnnotations;

namespace Randomizer.Model
{
    public class BaseEntity: IBaseEntity
    {
        [Key]
        public int ID { get; set; }
    }
}
