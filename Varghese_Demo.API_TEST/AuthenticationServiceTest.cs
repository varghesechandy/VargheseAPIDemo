using AutoMapper;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Net;
using System.Net.Http; 
using CodeDemo.API.AppDatabase.Entities;
using CodeDemo.API.AppDatabase.PaymentProcessRepository;
using CodeDemo.API.AuthenticationService;
using CodeDemo.API.Models;
using CodeDemo.API.AppDatabase.UsersRepository;
using Xunit;

namespace CodeDemo.API_TEST
{
    public class AuthenticationServiceTest
    { 
        private readonly Mock<IOptions<AppSettings>> appSettings = new Mock<IOptions<AppSettings>>();
        private readonly Mock<IPaymentRepository> paymentRepository = new Mock<IPaymentRepository>();
        private readonly Mock<IUsersRepository> userRepository = new Mock<IUsersRepository>();
        private readonly Mock<IMapper> mapper = new Mock<IMapper>();
        private readonly IAuthService authService;  
        public AuthenticationServiceTest()
        {
            //Mock Payment Repository & IMapper
            var retValue = string.Empty;
            var loginUser = new LoginUsers { UserName = "userreporting", Password = "password123", Role = "User" };
            var user = new User { Username = "userreporting", Password = "password123", Role = "User" };
            userRepository.Setup(s => s.UserLogin("userreporting", "password123", out retValue)).Returns(loginUser);
            mapper.Setup(s => s.Map<User>(loginUser)).Returns(user);
            
            //Mock Configuration for appsettings key
            var appKeySettings = new AppSettings() { Key = "varghesechandypallickaparampil682884" };
            appSettings.Setup(app => app.Value).Returns(appKeySettings);

            authService = new AuthService(appSettings.Object, paymentRepository.Object, userRepository.Object, mapper.Object); 
        }

        [Fact]
        public void VerifyAuthenticationService()
        {
            //Get authentication Token
            var user = authService.Authenticate("userreporting", "password123", out string errMsg);

            //Verify the token
            var result = authService.IsUserAuthenticatedForPaymentProcess(GetHttpRequestMessage(user.Token), out HttpStatusCode sCode, out errMsg);
            
            //Check the result (the user 'userreporting' is allowed to process a payment. This user is only allowed to view the reports.
            Assert.False(result);
            Assert.Equal("The user is not allowed to process the payment!", errMsg);
        }

        private HttpRequestMessage GetHttpRequestMessage(string token)
        {
            return new HttpRequestMessage
            {
                RequestUri = new Uri("http://localhost:12121"),
                Method = HttpMethod.Post,
                Headers = {
                    { "X-Version", "1" },
                    { HttpRequestHeader.Authorization.ToString(), token },
                    { HttpRequestHeader.ContentType.ToString(), "application/json" }
                }
            };
        }
    }
}
