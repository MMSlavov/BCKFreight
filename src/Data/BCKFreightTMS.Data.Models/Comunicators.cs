namespace BCKFreightTMS.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using BCKFreightTMS.Data.Common.Models;

    public class Comunicators : BaseDeletableModel<int>
    {
        [Column(TypeName ="varchar(20)")]
        public string Mobile1 { get; set; }

        [Column(TypeName = "varchar(20)")]
        public string Mobile2 { get; set; }

        [Column(TypeName = "varchar(20)")]
        public string Phone1 { get; set; }

        [Column(TypeName = "varchar(20)")]
        public string Phone2 { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string Email1 { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string Email2 { get; set; }

        [Column(TypeName = "nvarchar(MAX)")]
        public string Details { get; set; }

        public virtual Company Company { get; set; }

        public virtual Person Person { get; set; }
    }
}
