namespace BCKFreightTMS.Data.Models
{
    using BCKFreightTMS.Data.Common.Models;

    public class Setting : BaseDeletableModel<int>
    {
        public string Name { get; set; }

        public string Value { get; set; }
    }
}
