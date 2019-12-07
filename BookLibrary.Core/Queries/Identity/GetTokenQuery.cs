namespace BookLibrary.Core.Queries.Identity
{
    using Models;

    public class GetTokenQuery : IQuery<TokenResponse>
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}