namespace WebApplication1.Contracts.Authentication
{
    public record LoginRequest
    {
        public string Email { get; }
        public string Password { get; }
    }
}
