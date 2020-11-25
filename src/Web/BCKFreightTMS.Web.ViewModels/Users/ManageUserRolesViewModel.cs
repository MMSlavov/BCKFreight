namespace BCKFreightTMS.Web.ViewModels.Users
{
    using System.Collections.Generic;

    public class ManageUserRolesViewModel
    {
        public string UserId { get; set; }

        public string UserName { get; set; }

        public List<UserRoleInputModel> RoleModels { get; set; }
    }
}
