using System;
using System.Web;
using System.Linq;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text;
using System.Text.Json;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

using Radzen;

using WebAdmin.Models;

namespace WebAdmin
{
    public partial class SecurityService
    {

        private readonly HttpClient httpClient;

        private readonly Uri baseUri;

        private readonly NavigationManager navigationManager;

        public ApplicationUser User { get; private set; } = new ApplicationUser { Name = "Anonymous" };

        public ClaimsPrincipal Principal { get; private set; }

        public SecurityService(NavigationManager navigationManager, IHttpClientFactory factory)
        {
            this.baseUri = new Uri($"{navigationManager.BaseUri}odata/Identity/");
            this.httpClient = factory.CreateClient("WebAdmin");
            this.navigationManager = navigationManager;
        }

        public bool IsInRole(params string[] roles)
        {
#if DEBUG
            if (User.Name == "admin")
            {
                return true;
            }
#endif

            if (roles.Contains("Everybody"))
            {
                return true;
            }

            if (!IsAuthenticated())
            {
                return false;
            }

            if (roles.Contains("Authenticated"))
            {
                return true;
            }

            return roles.Any(role => Principal.IsInRole(role));
        }

        public bool IsAuthenticated()
        {
            return Principal?.Identity.IsAuthenticated == true;
        }

        public async Task<bool> InitializeAsync(AuthenticationState result)
        {
            //             Principal = result.User;
            // #if DEBUG
            //             if (Principal.Identity.Name == "admin")
            //             {
            //                 User = new ApplicationUser { Name = "Admin" };

            //                 return true;
            //             }
            // #endif
            Principal = result.User;
            if (Principal.Identity.Name == User?.UserName)
            {
                User = new ApplicationUser { Name = User?.UserName };

                return true;
            }

            var userId = Principal?.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId != null && User?.Id != userId)
            {
                User = await GetUserById(userId);
            }

            return IsAuthenticated();
        }


        public async Task<ApplicationAuthenticationState> GetAuthenticationStateAsync()
        {
            var uri = new Uri($"{navigationManager.BaseUri}Account/CurrentUser");

            var response = await httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Post, uri));

            return await response.ReadAsync<ApplicationAuthenticationState>();
        }

        public void Logout()
        {
            navigationManager.NavigateTo("Account/Logout", true);
        }

        public void Login()
        {
            navigationManager.NavigateTo("Login", true);
        }

        public async Task<IEnumerable<ApplicationRole>> GetRoles()
        {
            var uri = new Uri(baseUri, $"ApplicationRoles");

            uri = uri.GetODataUri();

            var response = await httpClient.GetAsync(uri);

            var result = await response.ReadAsync<ODataServiceResult<ApplicationRole>>();

            return result.Value;
        }

        public async Task<ApplicationRole> CreateRole(ApplicationRole role)
        {
            var uri = new Uri(baseUri, $"ApplicationRoles");

            var content = new StringContent(ODataJsonSerializer.Serialize(role), Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync(uri, content);

            return await response.ReadAsync<ApplicationRole>();
        }

        public async Task<HttpResponseMessage> DeleteRole(string id)
        {
            var uri = new Uri(baseUri, $"ApplicationRoles('{id}')");

            return await httpClient.DeleteAsync(uri);
        }

        public async Task<IEnumerable<ApplicationUser>> GetUsers()
        {
            var uri = new Uri(baseUri, $"ApplicationUsers");


            uri = uri.GetODataUri();

            var response = await httpClient.GetAsync(uri);

            var result = await response.ReadAsync<ODataServiceResult<ApplicationUser>>();

            return result.Value;
        }

        public async Task<ApplicationUser> CreateUser(ApplicationUser user)
        {
            var uri = new Uri(baseUri, $"ApplicationUsers");

            var content = new StringContent(JsonSerializer.Serialize(user), Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync(uri, content);

            return await response.ReadAsync<ApplicationUser>();
        }

        public async Task<HttpResponseMessage> DeleteUser(string id)
        {
            var uri = new Uri(baseUri, $"ApplicationUsers('{id}')");

            return await httpClient.DeleteAsync(uri);
        }

        public async Task<ApplicationUser> GetUserById(string id)
        {
            var uri = new Uri(baseUri, $"ApplicationUsers('{id}')?$expand=Roles");

            var response = await httpClient.GetAsync(uri);

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }

            return await response.ReadAsync<ApplicationUser>();
        }
        // [Inject]
        // protected SecurityService Security { get; set; }
        public async Task<ApplicationUser> UpdateUser(SecurityService Security, ApplicationUser user)
        {
            var uri = new Uri(baseUri, $"ApplicationUsers('{Security.User.Id}')");
            // user.Password = GenerateRandomPassword();
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, uri)
            {
                Content = new StringContent(JsonSerializer.Serialize(user), Encoding.UTF8, "application/json")
            };

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await response.ReadAsync<ApplicationUser>();
        }
        public async Task ChangePassword(string oldPassword, string newPassword)
        {
            var uri = new Uri($"{navigationManager.BaseUri}Account/ChangePassword");

            var content = new FormUrlEncodedContent(new Dictionary<string, string> {
                { "oldPassword", oldPassword },
                { "newPassword", newPassword }
            });

            var response = await httpClient.PostAsync(uri, content);

            if (!response.IsSuccessStatusCode)
            {
                var message = await response.Content.ReadAsStringAsync();

                throw new ApplicationException(message);
            }
        }

        public async Task ChangeEmail(string oldEmail, string newEmail)
        {
            var uri = new Uri($"{navigationManager.BaseUri}Account/ChangeEmail");

            var content = new FormUrlEncodedContent(new Dictionary<string, string> {
                   { "oldEmail", oldEmail },
                { "newEmail", newEmail }
            });

            var response = await httpClient.PostAsync(uri, content);

            if (!response.IsSuccessStatusCode)
            {
                var message = await response.Content.ReadAsStringAsync();

                throw new ApplicationException(message);
            }
        }

        public async Task Register(string userName, string password, string companyName, string companyId)
        {
            var uri = new Uri($"{navigationManager.BaseUri}Account/Register");
            password = GenerateRandomPassword();

            var content = new FormUrlEncodedContent(new Dictionary<string, string> {
                { "userName", userName },
                { "password", password },
                { "companyName", companyName },
                { "companyId", companyId }


            });

            var response = await httpClient.PostAsync(uri, content);

            if (!response.IsSuccessStatusCode)
            {
                var message = await response.Content.ReadAsStringAsync();

                throw new ApplicationException(message);
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


        public async Task ResetPassword(string userName)
        {
            var uri = new Uri($"{navigationManager.BaseUri}Account/ResetPassword");

            var content = new FormUrlEncodedContent(new Dictionary<string, string> {
                { "userName", userName }
            });

            var response = await httpClient.PostAsync(uri, content);

            if (!response.IsSuccessStatusCode)
            {
                var message = await response.Content.ReadAsStringAsync();

                throw new ApplicationException(message);
            }
        }
    }
}