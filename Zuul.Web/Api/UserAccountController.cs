using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;
using Zuul.BusinessLogic.Services;
using Zuul.Web.Api.Dto;

namespace Zuul.Web.Api
{
    [RoutePrefix("api/UserAccount")]
    public class UserAccountController : ApiController
    {
        private readonly UserAccountService _userAccountService;

        public UserAccountController(UserAccountService userAccountService)
        {
            _userAccountService = userAccountService;
        }

        [HttpPost]
        [Route("Register")]
        public void Register(UserRegistrationSubmissionDto userSubmission)
        {
            _userAccountService.CreateUser(userSubmission.EmailAddress, userSubmission.Username, userSubmission.Password, userSubmission.PasswordConfirm);
        }

        [HttpPost]
        [Route("RequestPasswordReset")]
        public bool RequestPasswordReset(RequestPasswordResetDto requestPasswordResetDto)
        {
            return _userAccountService.RequestPasswordReset(requestPasswordResetDto.EmailAddress);
        }

        [HttpPost]
        [Route("VerifyPasswordResetEmail")]
        public bool VerifyPasswordResetEmail(RequestPasswordResetDto requestPasswordResetDto)
        {
            return _userAccountService.VerifyPasswordResetEmail(requestPasswordResetDto.EmailAddress);
        }

        [HttpPost]
        [Route("UpdatePassword")]
        public void UpdatePassword (UpdatePasswordDto updatePasswordDto)
        {
            _userAccountService.UpdatePassword(
                updatePasswordDto.EmailAddress,
                updatePasswordDto.AuthenticationCode,
                updatePasswordDto.Password,
                updatePasswordDto.PasswordConfirm);
        }

        [HttpPost]
        [Route("Login")]
        public bool Login(LoginCredentialsDto loginCredentials)
        {
            var owinContext = Request.GetOwinContext();
            owinContext.Authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            var userId = _userAccountService.ValidateUser(loginCredentials.Username, loginCredentials.Password);
            if (userId > 0)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, loginCredentials.Username),
                    new Claim(ClaimTypes.NameIdentifier, userId.ToString())
                };
                var id = new ClaimsIdentity(claims, DefaultAuthenticationTypes.ApplicationCookie);
                owinContext.Authentication.SignIn(
                    new AuthenticationProperties {
                        IsPersistent = true,
                        ExpiresUtc = DateTime.UtcNow.AddDays(60)
                    }, id);
                return true;
            }
            return false;
        }

        [HttpPost]
        [Route("Logout")]
        public void Logout()
        {
            var owinContext = Request.GetOwinContext();
            owinContext.Authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
        }
    }
}