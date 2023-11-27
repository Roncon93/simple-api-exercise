namespace SimpleApiProject.Configuration
{
    public class ApplicationConfiguration
    {
        public SqliteConfiguration Sqlite { get; private set; } = new();
    }
}
