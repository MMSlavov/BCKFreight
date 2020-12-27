namespace BCKFreightTMS.Web.Helpers
{
    public static class IdValidator
    {
        public static string Validate<T>(T id)
        {
            if (id.ToString() == "null" || id.ToString() == "0")
            {
                return null;
            }

            return id.ToString();
        }
    }
}
