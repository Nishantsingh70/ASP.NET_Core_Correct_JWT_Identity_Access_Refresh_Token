namespace JWT_Identity_Secrets.Configuration
{
    public class JWTSettings
    {
        public const string SectionName = "JwtSettings";

        public string ValidateIssuer { get; set; } = default!;

        public string ValidateAudience { get; set; } = default!;

        public string SecretKey { get; set; } = default!;
    }
}
