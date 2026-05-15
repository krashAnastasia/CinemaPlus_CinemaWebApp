using System.Net;
using CinemaPlus.CinemaWebApp.Data;
using CinemaPlus.CinemaWebApp.Tests.Infrastructure;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;

namespace CinemaPlus.CinemaWebApp.Tests.Integration;

public class RegistrationAndProfileFlowTests
{
    [Fact]
    public async Task RegistrationLoginAndProfileFlow_Works()
    {
        await using var factory = new TestWebApplicationFactory();
        using var client = factory.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });

        const string email = "integration.user@test.local";
        const string updatedEmail = "updated.integration.user@test.local";
        const string password = "integration123";

        var registerPage = await client.GetStringAsync("/account/register");
        var registerToken = TestHtml.ExtractAntiForgeryToken(registerPage);
        var registerResponse = await client.PostAsync(
            "/account/register",
            new FormUrlEncodedContent(new Dictionary<string, string>
            {
                ["__RequestVerificationToken"] = registerToken,
                ["FirstName"] = "Інтеграційний",
                ["LastName"] = "Користувач",
                ["Phone"] = "+380671234567",
                ["Email"] = email,
                ["Password"] = password,
                ["ConfirmPassword"] = password
            }));

        Assert.Equal(HttpStatusCode.Redirect, registerResponse.StatusCode);
        Assert.Equal("/profile", registerResponse.Headers.Location?.OriginalString);

        var profilePage = await client.GetStringAsync("/profile");
        Assert.Contains("МІЙ КАБІНЕТ", profilePage);
        Assert.Contains(email, profilePage);

        var profileEditToken = TestHtml.ExtractAntiForgeryToken(profilePage);
        var updateProfileResponse = await client.PostAsync(
            "/profile/edit",
            new FormUrlEncodedContent(new Dictionary<string, string>
            {
                ["__RequestVerificationToken"] = profileEditToken,
                ["FullName"] = "Оновлений Користувач",
                ["Email"] = updatedEmail,
                ["Phone"] = "+380991112233"
            }));

        Assert.Equal(HttpStatusCode.Redirect, updateProfileResponse.StatusCode);
        Assert.Equal("/profile", updateProfileResponse.Headers.Location?.OriginalString);

        await using (var db = factory.CreateDbContext())
        {
            var user = await db.Users.SingleAsync(item => item.Email == updatedEmail);
            Assert.Equal("Client", user.Role);
            Assert.NotEqual(password, user.PasswordHash);
            Assert.Contains("PBKDF2-SHA256", user.PasswordHash);
            Assert.Equal("Оновлений Користувач", user.FullName);
            Assert.Equal("+380991112233", user.Phone);
        }

        var logoutResponse = await client.GetAsync("/account/logout");
        Assert.Equal(HttpStatusCode.Redirect, logoutResponse.StatusCode);

        var loginPage = await client.GetStringAsync("/account/login");
        var loginToken = TestHtml.ExtractAntiForgeryToken(loginPage);
        var loginResponse = await client.PostAsync(
            "/account/login",
            new FormUrlEncodedContent(new Dictionary<string, string>
            {
                ["__RequestVerificationToken"] = loginToken,
                ["Email"] = updatedEmail,
                ["Password"] = password
            }));

        Assert.Equal(HttpStatusCode.Redirect, loginResponse.StatusCode);
        Assert.Equal("/", loginResponse.Headers.Location?.OriginalString);

        var secondProfilePage = await client.GetStringAsync("/profile");
        Assert.Contains("МІЙ КАБІНЕТ", secondProfilePage);
        Assert.Contains(updatedEmail, secondProfilePage);
    }
}
