using AutoMapper;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using CodeDemo.API.AppDatabase.PaymentProcessRepository;
using CodeDemo.API.AppDatabase.UsersRepository;
using CodeDemo.API.Models;

namespace CodeDemo.API.AuthenticationService
{
    /// <summary>
    /// Authentication service
    /// </summary>
    public class AuthService : IAuthService
    {
        private readonly AppSettings appSettings;
        private readonly IPaymentRepository paymentRepository;
        private readonly IUsersRepository userRepository;
        private readonly IMapper mapper;
        private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_appSettings"></param>
        /// <param name="_paymentRepository"></param>
        /// <param name="_usersRepository"></param>
        /// <param name="_mapper"></param>
        public AuthService(IOptions<AppSettings> _appSettings, IPaymentRepository _paymentRepository, IUsersRepository _usersRepository, IMapper _mapper)
        {
            appSettings = _appSettings.Value;
            paymentRepository = _paymentRepository;
            userRepository = _usersRepository;
            mapper = _mapper;
        } 

        /// <summary>
        /// Authenticate a user. If authenticated, generate a token.
        /// There is currently no option to add or delete users from the database
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        public User Authenticate(string username, string password, out string errorMessage)
        { 
            var databaseUser = userRepository.UserLogin(username, password, out errorMessage);
            var user = mapper.Map<User>(databaseUser);
            if (user == null)
            {
                errorMessage = "Invalid login request! Username or password is incorrect";
                return null;
            }

            var token = GenerateToken(user.Username, user.Role); 
            user.Token = token!=string.Empty? token: "Error Occured! Please check the log file.";
            //Not returning security password
            user.Password = null;

            return user;
        } 
        private string GenerateToken(string username, string role)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(appSettings.Key);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                    new Claim(ClaimTypes.Name, username),
                    new Claim(ClaimTypes.Role, role)
                    }),
                    Expires = DateTime.UtcNow.AddDays(1),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);

                return tokenHandler.WriteToken(token);
            }
            catch(Exception ex)
            {
                logger.Error(ex.ToString());
            }

            return string.Empty;
        }

        /// <summary>
        /// This method verifies the authentication of a payment process through Ocelot
        /// </summary>
        /// <param name="request"></param>
        /// <param name="statusCode"></param>
        /// <param name="errorMessage"></param> 
        /// <returns></returns>
        public bool IsUserAuthenticatedForPaymentProcess(HttpRequestMessage request, out HttpStatusCode statusCode, out string errorMessage)
        {

            statusCode = HttpStatusCode.OK;
            errorMessage = string.Empty;

            var authHeader = request.Headers.Authorization;
            if (authHeader == null)
            {
                statusCode = HttpStatusCode.Unauthorized;
                errorMessage = "JWT security token is missing";
                logger.Error(errorMessage);
                return false;
            }

            if(!TryRetrieveToken(request, out string token))
            {
                statusCode = HttpStatusCode.Unauthorized;
                errorMessage = "JWT security token is missing";
                logger.Error(errorMessage);
                return false;
            }

            try
            {
                var key = Encoding.ASCII.GetBytes(appSettings.Key);
                JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
                TokenValidationParameters validationParameters =
                    new TokenValidationParameters()
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };

                IPrincipal userPrincipal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
                if (!userPrincipal.IsInRole("Admin"))
                {
                    statusCode = HttpStatusCode.Unauthorized;
                    errorMessage = "The user is not allowed to process the payment!";
                    logger.Error(errorMessage);
                    return false;
                }
            }
            catch (Exception ex)
            {
                statusCode = HttpStatusCode.Unauthorized;
                errorMessage = "Security token is invalid! Please get a new token;" + ex.ToString();
                logger.Error(errorMessage);
                return false;
            }

            return true;
        }

        private static bool TryRetrieveToken(HttpRequestMessage request, out string token)
        {
            token = null;
            try
            { 
                if (!request.Headers.TryGetValues("Authorization", out IEnumerable<string> authzHeaders) || authzHeaders.Count() > 1)
                    return false;
                
                var bearerToken = authzHeaders.ElementAt(0);
                token = bearerToken.StartsWith("Bearer ") ? bearerToken.Substring(7) : bearerToken;
                return true;
            }
            catch(Exception ex)
            {
                logger.Error(ex.ToString()); ;
            }

            return false;
        }
    }
}
