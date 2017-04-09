namespace Zuul.Web.Api.Dto
{
    public class UserRegistrationSubmissionDto
    {
        public string EmailAddress { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string PasswordConfirm { get; set; }
    }
}