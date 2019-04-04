using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PH.Core3.TestContext;
using PH.PicoCrypt2;

namespace PH.Core3.Test.CreateUser
{

     public class PasswordHasher : IPasswordHasher<User>
    {
        private PH.PicoCrypt2.IPicoCrypt _picoCrypt;

        public PasswordHasher([NotNull] IPicoCrypt picoCrypt)
        {
            _picoCrypt = picoCrypt;
        }

        /// <summary>
        /// Returns a hashed representation of the supplied <paramref name="password" /> for the specified <paramref name="user" />.
        /// </summary>
        /// <param name="user">The user whose password is to be hashed.</param>
        /// <param name="password">The password to hash.</param>
        /// <returns>A hashed representation of the supplied <paramref name="password" /> for the specified <paramref name="user" />.</returns>
        public string HashPassword(User user, string password)
        {
            return HashPwd(user.UserName, password);
        }

        private string HashPwd(string username, string p)
        {
            var cr   = _picoCrypt.EncryptUtf8(p, username);
            var hash = _picoCrypt.GenerateSha512String(cr);
            return hash;

        }

        /// <summary>
        /// Returns a <see cref="T:Microsoft.AspNetCore.Identity.PasswordVerificationResult" /> indicating the result of a password hash comparison.
        /// </summary>
        /// <param name="user">The user whose password should be verified.</param>
        /// <param name="hashedPassword">The hash value for a user's stored password.</param>
        /// <param name="providedPassword">The password supplied for comparison.</param>
        /// <returns>A <see cref="T:Microsoft.AspNetCore.Identity.PasswordVerificationResult" /> indicating the result of a password hash comparison.</returns>
        /// <remarks>Implementations of this method should be time consistent.</remarks>
        public PasswordVerificationResult VerifyHashedPassword(User user, string hashedPassword, string providedPassword)
        {
            var hashed = HashPwd(user.UserName, providedPassword); //_crypt.EncryptUtf8(providedPassword, user.UserName);
            if (hashed == hashedPassword)
                return PasswordVerificationResult.Success;

            return PasswordVerificationResult.Failed;
        }

        public void Initialize()
        {
            //
        }
    }

    public class ApplicationUserManager : UserManager<User> 
    {
        /// <summary>
        /// Constructs a new instance of <see cref="T:Microsoft.AspNetCore.Identity.UserManager`1" />.
        /// </summary>
        /// <param name="store">The persistence store the manager will operate over.</param>
        /// <param name="optionsAccessor">The accessor used to access the <see cref="T:Microsoft.AspNetCore.Identity.IdentityOptions" />.</param>
        /// <param name="passwordHasher">The password hashing implementation to use when saving passwords.</param>
        /// <param name="userValidators">A collection of <see cref="T:Microsoft.AspNetCore.Identity.IUserValidator`1" /> to validate users against.</param>
        /// <param name="passwordValidators">A collection of <see cref="T:Microsoft.AspNetCore.Identity.IPasswordValidator`1" /> to validate passwords against.</param>
        /// <param name="keyNormalizer">The <see cref="T:Microsoft.AspNetCore.Identity.ILookupNormalizer" /> to use when generating index keys for users.</param>
        /// <param name="errors">The <see cref="T:Microsoft.AspNetCore.Identity.IdentityErrorDescriber" /> used to provider error messages.</param>
        /// <param name="services">The <see cref="T:System.IServiceProvider" /> used to resolve services.</param>
        /// <param name="logger">The logger used to log messages, warnings and errors.</param>
        public ApplicationUserManager(IUserStore<User> store, IOptions<IdentityOptions> optionsAccessor, IPasswordHasher<User> passwordHasher, IEnumerable<IUserValidator<User>> userValidators
                                      , IEnumerable<IPasswordValidator<User>> passwordValidators, ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors, IServiceProvider services
                                      , ILogger<UserManager<User>> logger) 
            : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
        {
        }

        public void Initialize()
        {
            //
        }

        public override Task<bool> CheckPasswordAsync(User user, string password)
        {
            return base.CheckPasswordAsync(user, password);
        }

        public override async Task<IdentityResult> CreateAsync(User user)
        {
            user.Deleted = false;
            

            
            try
            {

                return await  base.CreateAsync(user);
            }
            catch (Exception e)
            {
                Logger.LogCritical(e.Message, e);
                throw;
            }
        }

        public override async Task<IdentityResult> CreateAsync(User user, string password)
        {
            user.Deleted = false;
            try
            {

                return await  base.CreateAsync(user, password);
            }
            catch (Exception e)
            {
                Logger.LogCritical(e.Message, e);
                throw;
            }
        }
    }
}