namespace BCKFreightTMS.Data.Common.Models
{
    public abstract class SettingModel : BaseDeletableModel<int>
    {
        public abstract string Name { get; set; }
    }
}
