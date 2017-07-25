using System.Linq;
using Zuul.BusinessLogic.Helpers;
using Zuul.Data.Repositories;
using Zuul.Model;
using Zuul.BusinessLogic.Exceptions;
using Zuul.Resources;
using System;

namespace Zuul.BusinessLogic.Services
{
    public class UserAccountService
    {
        private IUserAccountRepository _userAccountRepository;
        private AuthenticationHelper _passwordService;

        public UserAccountService(IUserAccountRepository userAccountRepositoy)
        {
            _userAccountRepository = userAccountRepositoy;
            _passwordService = new AuthenticationHelper();
        }

        public int CreateUser(string userEmail, string username, string password, string passwordConfirm)
        {
            userEmail = userEmail.Trim();
            username = username.Trim();
            ValidateCreate(userEmail, username, password, passwordConfirm);
            var passwordSalt = _passwordService.GetPasswordSalt();
            var passwordHash = _passwordService.GetPasswordHash(password, passwordSalt);
            var token = Guid.NewGuid();
            var userAccount = new UserAccount(userEmail, username, passwordSalt, passwordHash, token);
            var userAccountId = _userAccountRepository.CreateUser(userAccount);
            EmailHelper.SendUserRegistrationEmail(userEmail, username, userAccountId, token);
            return userAccountId;
        }

        public bool RequestPasswordReset(string emailAddress)
        {
            emailAddress = emailAddress.Trim();
            var userAccount = _userAccountRepository.GetByEmail(emailAddress);
            if (userAccount == null)
            {
                return false;
            }
            userAccount.ResetToken = Guid.NewGuid();
            userAccount.ResetTokenExpiry = DateTime.UtcNow.AddDays(1);
            _userAccountRepository.UpdateUser(userAccount);
            EmailHelper.SendUserPasswordResetEmail(userAccount);
            return true;
        }

        public void UpdatePassword(string emailAddress, string authenticationCode, string password, string passwordConfirm)
        {
            emailAddress = emailAddress.Trim();
            var userAccount = _userAccountRepository.GetByEmail(emailAddress);
            ValidateUpdatePassword(authenticationCode, password, passwordConfirm, userAccount);
            userAccount.PasswordSalt = _passwordService.GetPasswordSalt();
            userAccount.PasswordHash = _passwordService.GetPasswordHash(password, userAccount.PasswordSalt);
            userAccount.ResetTokenExpiry = null;
            userAccount.ResetToken = null;
            _userAccountRepository.UpdateUser(userAccount);
        }

        public bool IsPasswordResetTokenValid(int userAccountId, Guid token)
        {
            var userAccount = _userAccountRepository.GetById(userAccountId);
            return userAccount.ResetToken == token && userAccount.ResetTokenExpiry > DateTime.UtcNow;
        }

        public int ValidateUser(string username, string password)
        {
            var userAccount = GetByUserAccountByUsernameAndValidateUsername(username, ExceptionMessages.ValidateUsernameNotFound);
            ValidateAccountIsActivated(userAccount);
            if (_passwordService.ValidatePassword(userAccount.PasswordSalt, userAccount.PasswordHash, password))
            {
                return userAccount.Id;
            }
            return 0;
        }

        public bool ActivateUser(int id, Guid token)
        {
            var userAccount = GetByUserAccountByIdAndValidateId(id);
            if (userAccount.Token == token)
            {
                _userAccountRepository.ActivateAccount(id);
                return true;
            }
            return false;
        }

        public object GetByUsername(string username)
        {
            return _userAccountRepository.GetByUsername(username);
        }

        #region Validation Methods

        private UserAccount GetByUserAccountByUsernameAndValidateUsername(string username, string exceptionMessage)
        {
            var userAccount = _userAccountRepository.GetByUsername(username);
            if (userAccount == null)
            {
                throw new ValidationException(string.Format(exceptionMessage, username));
            }
            return userAccount;
        }

        private UserAccount GetByUserAccountByIdAndValidateId(int userId)
        {
            var userAccount = _userAccountRepository.GetById(userId);
            if (userAccount == null)
            {
                throw new ValidationException(string.Format(ExceptionMessages.UserAccountUserIdNotFound));
            }
            return userAccount;
        }

        private void ValidateAccountIsActivated(UserAccount userAccount)
        {
            if (!userAccount.IsActivated)
            {
                throw new ValidationException(string.Format(ExceptionMessages.UserAccountNotActivated, userAccount.EmailAddress));
            }
        }

        private void ValidateCreate(string userEmail, string username, string password, string passwordConfirm)
        {
            ValidateUserAccountEmail(userEmail);
            ValidateUserAccountUsername(username);
            ValidateUserAccountPassword(password, passwordConfirm);
        }

        private void ValidateUserAccountPassword(string password, string passwordConfirm)
        {
            if (string.IsNullOrEmpty(password))
            {
                throw new ValidationException(ExceptionMessages.CreateUserAccountPasswordMustBeSupplied);
            }
            if (password != passwordConfirm)
            {
                throw new ValidationException(ExceptionMessages.CreateUserAccountPasswordsMustMatch);
            }
            if (password.Length < 8)
            {
                throw new ValidationException(ExceptionMessages.UserAccountPasswordTooShort);
            }
        }

        private void ValidateUserAccountEmail(string userEmail)
        {
            if (userEmail.Length == 0)
            {
                throw new ValidationException(ExceptionMessages.CreateUserAccountEmptyEmailAddress);
            }
            if (userEmail.Length > 256)
            {
                throw new ValidationException(ExceptionMessages.CreateUserAccountEmailAddressTooLong);
            }
            if (!userEmail.Contains("@"))
            {
                throw new ValidationException(string.Format(ExceptionMessages.CreateUserAccountEmailAddressNoAtSymbol, userEmail));
            }
            if (!userEmail.Contains("."))
            {
                throw new ValidationException(string.Format(ExceptionMessages.CreateUserAccountEmailAddressNoDotSymbol, userEmail));
            }
            ValidateDuplicateEmail(userEmail);
        }

        private void ValidateUserAccountUsername(string username)
        {
            if (username.Length == 0)
            {
                throw new ValidationException(ExceptionMessages.CreateUserAccountEmptyUsername);
            }
            if (username.Length > 30)
            {
                throw new ValidationException(ExceptionMessages.CreateUserAccountUsernameTooLong);
            }
            ValidateDuplicateUsername(username);
        }

        private void ValidateDuplicateEmail(string userEmail)
        {
            if (_userAccountRepository.ExistsWithEmail(userEmail))
            {
                throw new ValidationException(string.Format(ExceptionMessages.CreateUserAccountEmailAddressDuplicate, userEmail));
            }
        }

        private void ValidateDuplicateUsername(string username)
        {
            if (_userAccountRepository.ExistsWithUsername(username))
            {
                throw new ValidationException(string.Format(ExceptionMessages.CreateUserAccountUsernameDuplicate, username));
            }
        }

        private static void ValidateUpdatePassword(string authenticationCode, string password, string passwordConfirm, UserAccount userAccount)
        {
            if (userAccount == null)
            {
                throw new ValidationException(ExceptionMessages.UpdatePasswordUserAccountNotFound);
            }
            if (userAccount.ResetTokenExpiry < DateTime.UtcNow)
            {
                throw new ValidationException(ExceptionMessages.UpdatePasswordTokenExpired);
            }
            if (userAccount.ResetToken.ToString() != authenticationCode.ToLower())
            {
                throw new ValidationException(ExceptionMessages.UpdatePasswordTokenIncorrect);
            }
            if (string.IsNullOrEmpty(password))
            {
                throw new ValidationException(ExceptionMessages.UpdatePasswordPasswordMustBeSupplied);
            }
            if (password != passwordConfirm)
            {
                throw new ValidationException(ExceptionMessages.UpdatePasswordPasswordsMustMatch);
            }
            if (password.Length < 8)
            {
                throw new ValidationException(ExceptionMessages.UpdatePasswordPasswordTooShort);
            }
        }

        #endregion
    }
}
