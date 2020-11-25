namespace BCKFreightTMS.Web.ViewModels.Comunicators
{
    using System.ComponentModel.DataAnnotations;

    public class ComunicatorsInputModel
    {
        [Required]
        [Phone]
        [MaxLength(10)]
        public string Mobile1 { get; set; }

        [Phone]
        [MaxLength(10)]
        public string Mobile2 { get; set; }

        [Phone]
        [MaxLength(10)]
        public string Phone1 { get; set; }

        [Phone]
        [MaxLength(10)]
        public string Phone2 { get; set; }

        [EmailAddress]
        [MaxLength(10)]
        public string Email1 { get; set; }

        [EmailAddress]
        [MaxLength(10)]
        public string Email2 { get; set; }

        [MaxLength(300)]
        public string Details { get; set; }
    }
}
