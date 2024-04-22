namespace WebApplication1.Contracts.Authentication
{
    public record LoginRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
