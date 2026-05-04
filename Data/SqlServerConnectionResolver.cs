using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace APICRUD.Data;

public static class SqlServerConnectionResolver
{
    public static string Resolve(IConfiguration configuration)
    {
        var overrideConnection = configuration.GetConnectionString("DefaultConnection");
        if (!string.IsNullOrWhiteSpace(overrideConnection))
            return overrideConnection;

        var server = configuration["SqlServer:Server"] ?? "PSL-BD-35";
        var database = configuration["SqlServer:Database"] ?? "Test_DB";
        var user = configuration["SqlServer:User"];
        var password = configuration["SqlServer:Password"];

        var csb = new SqlConnectionStringBuilder
        {
            DataSource = server,
            InitialCatalog = database,
            TrustServerCertificate = true,
            Encrypt = true,
            MultipleActiveResultSets = true
        };

        if (!string.IsNullOrWhiteSpace(user) && !string.IsNullOrWhiteSpace(password))
        {
            csb.IntegratedSecurity = false;
            csb.UserID = user.Trim();
            csb.Password = password;
        }
        else
        {
            csb.IntegratedSecurity = true;
        }

        return csb.ConnectionString;
    }
}
