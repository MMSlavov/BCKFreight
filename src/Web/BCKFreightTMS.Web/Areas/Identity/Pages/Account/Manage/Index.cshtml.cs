namespace BCKFreightTMS.Web.Areas.Identity.Pages.Account.Manage
{
    using System.ComponentModel.DataAnnotations;
    using System.Threading.Tasks;

    using BCKFreightTMS.Data.Models;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;

    public partial class IndexModel : PageModel
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;

        public IndexModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        public string Username { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [TempData]
        public string UserNameChangeLimitMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Display(Name = "First Name")]
            public string FirstName { get; set; }

            [Display(Name = "Last Name")]
            public string LastName { get; set; }

            [Display(Name = "Username")]
            public string Username { get; set; }

            [Phone]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await this.userManager.GetUserAsync(this.User);
            if (user == null)
            {
                return this.NotFound($"Unable to load user with ID '{this.userManager.GetUserId(this.User)}'.");
            }

            await this.LoadAsync(user);
            return this.Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await this.userManager.GetUserAsync(this.User);
            if (user == null)
            {
                return this.NotFound($"Unable to load user with ID '{this.userManager.GetUserId(this.User)}'.");
            }

            if (!this.ModelState.IsValid)
            {
                await this.LoadAsync(user);
                return this.Page();
            }

            var phoneNumber = await this.userManager.GetPhoneNumberAsync(user);
            if (this.Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await this.userManager.SetPhoneNumberAsync(user, this.Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    this.StatusMessage = "Unexpected error when trying to set phone number.";
                    return this.RedirectToPage();
                }
            }

            var firstName = user.FirstName;
            var lastName = user.LastName;
            if (this.Input.FirstName != firstName)
            {
                user.FirstName = this.Input.FirstName;
                await this.userManager.UpdateAsync(user);
            }

            if (this.Input.LastName != lastName)
            {
                user.LastName = this.Input.LastName;
                await this.userManager.UpdateAsync(user);
            }

            if (user.UsernameChangeLimit > 0)
            {
                if (this.Input.Username != user.UserName)
                {
                    var userNameExists = await this.userManager.FindByNameAsync(this.Input.Username);
                    if (userNameExists != null)
                    {
                        this.StatusMessage = "User name already taken. Select a different username.";
                        return this.RedirectToPage();
                    }

                    var setUserName = await this.userManager.SetUserNameAsync(user, this.Input.Username);
                    if (!setUserName.Succeeded)
                    {
                        this.StatusMessage = "Unexpected error when trying to set user name.";
                        return this.RedirectToPage();
                    }
                    else
                    {
                        user.UsernameChangeLimit -= 1;
                        await this.userManager.UpdateAsync(user);
                    }
                }
            }

            await this.signInManager.RefreshSignInAsync(user);
            this.UserNameChangeLimitMessage = $"You can change your username {user.UsernameChangeLimit} more time(s).";
            this.StatusMessage = "Your profile has been updated";
            return this.RedirectToPage();
        }

        private async Task LoadAsync(ApplicationUser user)
        {
            var userName = await this.userManager.GetUserNameAsync(user);
            var phoneNumber = await this.userManager.GetPhoneNumberAsync(user);
            var firstName = user.FirstName;
            var lastName = user.LastName;
            this.Username = userName;

            this.Input = new InputModel
            {
                PhoneNumber = phoneNumber,
                Username = userName,
                FirstName = firstName,
                LastName = lastName,
            };
        }
    }
}
