using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace JWT_Identity_Secrets.Configuration
{
    public class DatabaseSetting
    {
        public const string SectionName = "ConnectionStrings";

        public string DefaultConnection { get; set; } = default!;
    }
}
