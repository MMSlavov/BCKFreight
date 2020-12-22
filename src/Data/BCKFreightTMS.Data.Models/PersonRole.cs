namespace BCKFreightTMS.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using BCKFreightTMS.Data.Common.Models;

    public class PersonRole : SettingModel
    {
        public PersonRole()
        {
            this.People = new HashSet<Person>();
        }

        [Required]
        [MaxLength(50)]
        public override string Name { get; set; }

        public virtual ICollection<Person> People { get; set; }
    }
}
