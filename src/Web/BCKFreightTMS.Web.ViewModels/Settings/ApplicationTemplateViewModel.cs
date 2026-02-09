namespace BCKFreightTMS.Web.ViewModels.Settings
{
    using System.ComponentModel.DataAnnotations;

    public class ApplicationTemplateViewModel
    {
        public string Id { get; set; }

        [Required]
        [Display(Name = "Template Name")]
        [MaxLength(200)]
        public string TemplateName { get; set; }

        public string CompanyId { get; set; }

        [Required]
        [Display(Name = "HTML Template")]
        [DataType(DataType.MultilineText)]
        public string HtmlTemplate { get; set; }

        [Display(Name = "CSS Styles")]
        [DataType(DataType.MultilineText)]
        public string CssStyles { get; set; }

        [Display(Name = "JavaScript (Advanced)")]
        [DataType(DataType.MultilineText)]
        public string JavaScript { get; set; }

        [Display(Name = "Set as Default")]
        public bool IsDefault { get; set; }

        [Display(Name = "Description")]
        [MaxLength(1000)]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        public int Version { get; set; }
    }
}
