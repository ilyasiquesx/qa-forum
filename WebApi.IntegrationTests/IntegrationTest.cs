using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using QAForum.Application.Users.Commands.RegisterCommand;
using QAForum.Infrastructure.Context;
using WebApi.Models;

namespace WebApi.IntegrationTests
{
    public class IntegrationTest
    {
        protected readonly HttpClient HttpClient;
        private const string Account = "Account";
        private const string Register = "Register";

        protected IntegrationTest()
        {
            var appFactory = new WebApplicationFactory<Startup>()
                .WithWebHostBuilder(b => b.ConfigureServices(s =>
                {
                    var descriptor = s.SingleOrDefault(
                        d => d.ServiceType ==
                             typeof(DbContextOptions<AppDbContext>));
                    s.Remove(descriptor);
                    s.AddDbContext<AppDbContext>(options =>
                    {
                        options.UseInMemoryDatabase("InMemoryDbForTesting");
                        options.ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning));
                    });
                }));

            HttpClient = appFactory.CreateClient();
        }

        protected async Task<string> AuthenticateTestUser(string username)
        {
            var registerTestUserCommand = new RegisterUserCommand
            {
                Username = username,
                Password = "SecretPassword",
                ConfirmPassword = "SecretPassword",
                Email = "not@empty.empty"
            };

            var serverResponse = await HttpClient.PostAsJsonAsync($"/{Account}/{Register}", registerTestUserCommand);
            var loginResponseModel = await serverResponse.Content.ReadFromJsonAsync<ApiLoginResponse>();
            HttpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", loginResponseModel!.Token);
            return loginResponseModel.Id;
        }
    }
}