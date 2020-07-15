namespace Fanda.Core.Auth.ViewModels
{
    using System;
    using System.Text.Json.Serialization;

    public class AuthenticateResponse
    {
        public Guid Id { get; set; }
        public Guid TenantId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string JwtToken { get; set; }
        [JsonIgnore] // refresh token is returned in http only cookie
        public string RefreshToken { get; set; }

        public AuthenticateResponse(UserDto user, string jwtToken, string refreshToken)
        {
            Id = user.Id;
            TenantId = user.TenantId;
            FirstName = user.FirstName;
            LastName = user.LastName;
            Username = user.Name;
            Email = user.Email;
            JwtToken = jwtToken;
            RefreshToken = refreshToken;
        }
    }
}
