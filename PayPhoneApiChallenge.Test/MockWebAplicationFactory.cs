using Microsoft.EntityFrameworkCore;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Microsoft.Extensions.DependencyInjection;

using PayPhoneApiChallenge.Infra.Persistence;

namespace PayPhoneApiChallenge.Test
{
    public class MockWebAplicationFactory : WebApplicationFactory<Program> {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            // Eliminar el DbContext real si lo hay
            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<PayPhoneDbContext>));
                if (descriptor != null)
                    services.Remove(descriptor);
            

            // Agregar DbContext en memoria
            services.AddDbContext<PayPhoneDbContext>(options =>
            {
                options.UseInMemoryDatabase("TestDb");
            });

            // Build el service provider
            var sp = services.BuildServiceProvider();

            // Crear base de datos y seed
            using (var scope = sp.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var db = scopedServices.GetRequiredService<PayPhoneDbContext>();
                db.Database.EnsureCreated();

                // TODO: Agregar Seeders si hay tiempo
                TestSeeder.Seed(db);
            }
            });
        }
    }
}
