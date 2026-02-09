namespace BCKFreightTMS.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using BCKFreightTMS.Data.Common.Models;

    public class ApplicationTemplate : BaseDeletableModel<string>
    {
        public ApplicationTemplate()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Version = 1;
        }

        [Required]
        public string CompanyId { get; set; }

        public virtual Company Company { get; set; }

        [Required]
        [MaxLength(200)]
        public string TemplateName { get; set; }

        // Full HTML template with placeholders like {{CompanyName}}, {{OrderNumber}}, etc.
        [Required]
        public string HtmlTemplate { get; set; }

        // CSS styles
        public string CssStyles { get; set; }

        // JavaScript (optional, for advanced customization)
        public string JavaScript { get; set; }

        public bool IsDefault { get; set; }

        // Template version for tracking changes
        public int Version { get; set; }

        // Description/notes about this template
        [MaxLength(1000)]
        public string Description { get; set; }
    }
}
