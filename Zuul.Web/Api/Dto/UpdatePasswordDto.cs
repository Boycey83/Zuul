namespace Zuul.Web.Api.Dto
{
    public class UpdatePasswordDto
    {
        public string EmailAddress { get; set; }
        public string AuthenticationCode { get; set; }
        public string Password { get; set; }
        public string PasswordConfirm { get; set; }
    }
}