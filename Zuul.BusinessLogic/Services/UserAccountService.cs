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

        //public void ChangePassword(string userEmail, string oldPassword, string newPassword)
        //{
        //	var userAccount = GetByUserAccountByEmailAndValidateEmailAddress(userEmail, ExceptionMessages.ChangePasswordUserAccountEmailAddressNotFound);
        //	ValidateChangePassword(oldPassword, newPassword, userAccount);
        //	var newPasswordSalt = _passwordService.GetPasswordSalt();
        //	var newPasswordHash = _passwordService.GetPasswordHash(newPassword, newPasswordSalt);
        //	_userAccountRepository.ChangePassword(userAccount.Id, newPasswordSalt, newPasswordHash);
        //}

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

        //private void ValidateChangePassword(string oldPassword, string newPassword, UserAccount userAccount)
        //{
        //    ValidateUserAccountPassword(newPassword);
        //    if (oldPassword == newPassword)
        //    {
        //        throw new ValidationException(ExceptionMessages.ChangePasswordNewPasswordMustNotMatchOld);
        //    }
        //    if (!_passwordService.ValidatePassword(userAccount.PasswordSalt, userAccount.PasswordHash, oldPassword))
        //    {
        //        throw new ValidationException(ExceptionMessages.ChangePasswordOldPasswordNotCorrect);
        //    }
        //}

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

        #endregion
    }
}
