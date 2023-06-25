namespace Product.WebApi.Models.ResponseModel
{
    public class AuthenticatedResponse
    {
        public string UserName { get; set; }
        public string? Token { get; set; }
        public string? RefreshToken { get; set; }

        public DateTime RefreshTokenExpiryTime { get; set; }

        public string Status { get; set; }
        public string Message { get; set; }
    }
}
