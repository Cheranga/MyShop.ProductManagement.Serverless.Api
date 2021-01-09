using System.Threading.Tasks;
using DbUp;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using MyShop.ProductManagement.DataAccess;

namespace MyShop.ProductManagement.Serverless.Api.Functions
{
    public class MigrateDatabaseFunction
    {
        private readonly string _connectionString;
        private readonly ILogger<MigrateDatabaseFunction> _logger;

        public MigrateDatabaseFunction(DatabaseConfig databaseConfig, ILogger<MigrateDatabaseFunction> logger)
        {
            _logger = logger;
            _connectionString = databaseConfig?.ConnectionString;
        }

        [FunctionName(nameof(MigrateDatabaseFunction))]
        public Task<IActionResult> MigrateDatabaseAsync([HttpTrigger(AuthorizationLevel.Function, "post", Route = "database")]
            HttpRequest request)
        {
            EnsureDatabase.For.SqlDatabase(_connectionString);

            var upgrader = DeployChanges.To
                .SqlDatabase(_connectionString)
                .WithScriptsEmbeddedInAssembly(typeof(MigrateDatabaseFunction).Assembly)
                .LogToAutodetectedLog()
                .Build();

            if (upgrader.IsUpgradeRequired())
            {
                _logger.LogInformation("Migrating database.");

                var upgradeOperation = upgrader.PerformUpgrade();

                if (upgradeOperation.Successful)
                {
                    return Task.FromResult<IActionResult>(new OkResult());
                }

                return Task.FromResult<IActionResult>(new BadRequestObjectResult(upgradeOperation.Error));
            }

            return Task.FromResult<IActionResult>(new OkResult());
        }
    }
}