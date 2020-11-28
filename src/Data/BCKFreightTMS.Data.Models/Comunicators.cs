namespace BCKFreightTMS.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using BCKFreightTMS.Data.Common.Models;

    public class Comunicators : BaseDeletableModel<int>
    {
        [Required]
        [Column(TypeName ="varchar(10)")]
        public string Mobile1 { get; set; }

        [Column(TypeName = "varchar(10)")]
        public string Mobile2 { get; set; }

        [Column(TypeName = "varchar(10)")]
        public string Phone1 { get; set; }

        [Column(TypeName = "varchar(10)")]
        public string Phone2 { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string Email1 { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string Email2 { get; set; }

        [Column(TypeName = "nvarchar(MAX)")]
        public string Details { get; set; }

        public Company Company { get; set; }

        public Person Person { get; set; }
    }
}
