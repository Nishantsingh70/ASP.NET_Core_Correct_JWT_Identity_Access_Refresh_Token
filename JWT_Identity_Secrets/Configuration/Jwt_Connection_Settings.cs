namespace JWT_Identity_Secrets.Configuration
{
    public class Jwt_Connection_Settings
    {
        public const string SectionName = "Jwt_ConnectionString_Settings";

        public string ValidateIssuer { get; set; } = default!;
        public string ValidateAudience { get; set; } = default!;
        public string SecretKey { get; set; } = default!;
        public string DefaultConnection { get; set; } = default!;
    }
}
