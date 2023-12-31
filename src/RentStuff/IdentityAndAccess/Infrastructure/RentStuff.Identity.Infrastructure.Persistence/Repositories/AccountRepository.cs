﻿using System;
using System.Data.Common;
using System.Linq;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using NLog;
using RentStuff.Common.Utilities;
using RentStuff.Identity.Domain.Model.Entities;
using RentStuff.Identity.Infrastructure.Services.Hashers;
using RentStuff.Identity.Infrastructure.Services.Identity;
using RentStuff.Identity.Infrastructure.Services.PasswordReset;
using RentStuff.Identity.Infrastructure.Services.Validators;

namespace RentStuff.Identity.Infrastructure.Persistence.Repositories
{
    public class AccountRepository : IAccountRepository, IDisposable
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();
        private AuthContext _ctx;

        private UserManager<CustomIdentityUser> _userManager;
        
        public AccountRepository()
        {
            var dbConnection = CreateDbConnection();
            _ctx = new AuthContext(dbConnection, true);       
            _userManager = new UserManager<CustomIdentityUser>(new UserStore<CustomIdentityUser>(_ctx));
            _userManager.UserValidator = new CustomUserValidator<CustomIdentityUser>(_userManager);
            _userManager.PasswordHasher = new CustomPasswordHasher();
            _userManager.UserTokenProvider = new UserTokenProviderService();
        }

        /// <summary>
        /// Creates the DbConnection required for the AuthContext
        /// </summary>
        /// <returns></returns>
        public DbConnection CreateDbConnection()
        {
            var connection = DbProviderFactories.GetFactory("MySql.Data.MySqlClient").CreateConnection();
            if (connection == null)
            {
                _logger.Error("Could not create connection to DB for Entity Framework.");
                throw new NullReferenceException("Could not create DB connection for Entity Framework");
            }
            connection.ConnectionString = StringCipher.DecipheredConnectionString;
            return connection;
        }

        /// <summary>
        /// Saves an ExternalAccessTokenIdentifier to the database
        /// </summary>
        /// <param name="externalAccessTokenIdentifier"></param>
        public ExternalAccessTokenIdentifier SaveExternalAccessTokenIdentifier(ExternalAccessTokenIdentifier externalAccessTokenIdentifier)
        {
            var accessTokenIdentifier = _ctx.ExternalAccessExternalTokenIdentifiers.Add(externalAccessTokenIdentifier);
            _ctx.SaveChanges();
            return accessTokenIdentifier;
        }

        public void DeleteExternalAccessTokenIdentifier(ExternalAccessTokenIdentifier externalAccessTokenIdentifier)
        {
            _ctx.ExternalAccessExternalTokenIdentifiers.Remove(externalAccessTokenIdentifier);
            _ctx.SaveChanges();
        }

        /// <summary>
        /// Finds the ExtenralAccessTokenIdentifier given the InternalIdentifier
        /// </summary>
        /// <param name="internalIdentifier"></param>
        /// <returns></returns>
        public ExternalAccessTokenIdentifier GetExternalAccessIdentifierByInternalId(string internalIdentifier)
        {
            return _ctx.ExternalAccessExternalTokenIdentifiers.Find(internalIdentifier);
        }
        
        public IdentityResult RegisterUser(string name, string email, string password, bool isExternalUser = false)
        {
            // Assign email to the uername property, as we will use email in place of username
            CustomIdentityUser user = new CustomIdentityUser
            {
                UserName = email,
                Email = email,
                FullName = name
            };
            IdentityResult result;
            if (!isExternalUser)
            {
                result = _userManager.Create(user, password);
            }
            else
            {
                result = _userManager.Create(user);
            }

            return result;
        }

        /// <summary>
        /// get user by UserLoginInfo
        /// </summary>
        /// <param name="loginInfo"></param>
        /// <returns></returns>
        public CustomIdentityUser GetUserByUserLoginInfo(UserLoginInfo loginInfo)
        {
            return _userManager.Find(loginInfo);
        }

        public CustomIdentityUser GetUserByEmail(string email)
        {
            return _userManager.FindByEmail(email);
        }

        public CustomIdentityUser GetUserByPassword(string userName, string password)
        {
            return _userManager.Find(userName, password);
        }
        
        public IdentityResult AddLogin(string userId, UserLoginInfo userLoginInfo)
        {
            var result =  _userManager.AddLogin(userId, userLoginInfo);

            return result;
        }

        public IdentityResult UpdateUser(CustomIdentityUser customerIdentityUser)
        {
            return _userManager.Update(customerIdentityUser);
        }

        public bool IsEmailConfirmed(string userId)
        {
            return _userManager.IsEmailConfirmed(userId);
        }

        public string GetPasswordResetToken(string userId)
        {
            return _userManager.GeneratePasswordResetToken(userId);
        }
        
        public string GetEmailActivationToken(string userId)
        {
            return _userManager.GenerateEmailConfirmationToken(userId);
        }

        public bool ConfirmEmail(string userId, string token)
        {
            var identityResult = _userManager.ConfirmEmail(userId, token);
            if (identityResult.Succeeded)
            {
                return true;
            }
            throw new ArgumentException($"Could not confirm user email. UserId: {userId}");
        }

        /// <summary>
        /// Update password for a user
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="token"></param>
        /// <param name="newPassword"></param>
        /// <returns></returns>
        public bool ResetPassword(string userId, string token, string newPassword)
        {
            var identityResult = _userManager.ResetPassword(userId, token, newPassword);
            if (identityResult.Succeeded)
            {
                return true;
            }
            return false;
        }

        public void Dispose()
        {
            _ctx.Dispose();
            _userManager.Dispose();
        }
    }
}