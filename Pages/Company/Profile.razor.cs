using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Components.Routing;
using WebAdmin.Pages.Employees;

namespace WebAdmin.Pages.Company
{
    public partial class Profile
    {
        [Inject]
        protected IJSRuntime JSRuntime { get; set; }

        [Inject]
        protected NavigationManager NavigationManager { get; set; }

        [Inject]
        protected DialogService DialogService { get; set; }

        [Inject]
        protected TooltipService TooltipService { get; set; }

        [Inject]
        protected ContextMenuService ContextMenuService { get; set; }

        [Inject]
        protected NotificationService NotificationService { get; set; }
        protected RadzenDataGrid<WebAdmin.Models.ApplicationUser> grid0;



        protected string confirmPassword = "";
        protected WebAdmin.Models.ApplicationUser user;
        protected string oldPassword = "";
        protected string newPassword = "";
        protected string oldEmail = "";
        protected string newEmail = "";
        protected string error;
        protected bool errorVisible;
        protected string info;
        protected bool infoVisible;
        protected bool successVisible;

        [Inject]
        protected SecurityService Security { get; set; }
        private async Task OnBeforeInternalNavigation(LocationChangingContext context)
        {

            if (user.isFirstLogin == false)
            {
                var confirm = await JSRuntime.InvokeAsync<bool>("confirm", "Please Update password for first time!");
                if (!confirm)
                {
                    context.PreventNavigation();
                }
                else
                {
                    context.PreventNavigation();
                }

                // errorVisible = true;
                // error = "Silahkan isi";
                // NavigationManager.NavigateTo("profile", true);
                // await DialogService.OpenAsync<Profile>("Update Password", null);
                // await grid0.Reload();
                // var confirm = await JSRuntime.InvokeAsync<bool>("confirm", "Update password first");
                // if (confirm)
                // {
                //     //   DialogService.Close(true);
                //     NavigationManager.NavigateTo("profile");

                // }
                // else
                // {
                //     NavigationManager.NavigateTo("profile", true);
                // }

            }
        }

        protected override async Task OnInitializedAsync()
        {
            user = await Security.GetUserById(Security.User.Id);

        }
        [Parameter]
        public string Id { get; set; }
        protected async Task FormSubmit()
        {
            try
            {
                // user.isFirstLogin = true;
                await Security.ChangePassword(oldPassword, newPassword);
                successVisible = true;
            }
            catch (Exception ex)
            {
                errorVisible = true;
                error = ex.Message;
            }
        }
        protected async Task UpdateEmail()
        {
            try
            {
                // user.Password = GenerateRandomPassword();
                // await Security.UpdateUser(Security, user);
                await Security.ChangeEmail(oldEmail = user.Email, newEmail);

                infoVisible = true;
                info = "Email was updated succesfully!";
            }
            catch (Exception ex)
            {
                errorVisible = true;
                error = ex.Message;
            }
        }
        private static string GenerateRandomPassword()
        {
            var options = new PasswordOptions
            {
                RequiredLength = 8,
                RequiredUniqueChars = 4,
                RequireDigit = true,
                RequireLowercase = true,
                RequireNonAlphanumeric = true,
                RequireUppercase = true
            };

            var randomChars = new[] {
                "ABCDEFGHJKLMNOPQRSTUVWXYZ",
                "abcdefghijkmnopqrstuvwxyz",
                "0123456789",
                "!@$?_-"
            };

            var rand = new Random(Environment.TickCount);
            var chars = new List<char>();

            if (options.RequireUppercase)
            {
                chars.Insert(rand.Next(0, chars.Count), randomChars[0][rand.Next(0, randomChars[0].Length)]);
            }

            if (options.RequireLowercase)
            {
                chars.Insert(rand.Next(0, chars.Count), randomChars[1][rand.Next(0, randomChars[1].Length)]);
            }

            if (options.RequireDigit)
            {
                chars.Insert(rand.Next(0, chars.Count), randomChars[2][rand.Next(0, randomChars[2].Length)]);
            }

            if (options.RequireNonAlphanumeric)
            {
                chars.Insert(rand.Next(0, chars.Count), randomChars[3][rand.Next(0, randomChars[3].Length)]);
            }

            for (int i = chars.Count; i < options.RequiredLength || chars.Distinct().Count() < options.RequiredUniqueChars; i++)
            {
                string rcs = randomChars[rand.Next(0, randomChars.Length)];
                chars.Insert(rand.Next(0, chars.Count), rcs[rand.Next(0, rcs.Length)]);
            }

            return new string(chars.ToArray());
        }
    }
}