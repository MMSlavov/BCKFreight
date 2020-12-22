namespace BCKFreightTMS.Web.ViewModels.Settings
{
    using System.ComponentModel.DataAnnotations;

    public class SettingInputModel
    {
        public string Action { get; set; }

        [Required]
        [RegularExpression(@"^[A-Z][a-z]{1,40}$", ErrorMessage = "Invalid name.")]
        public string Name { get; set; }
    }
}
