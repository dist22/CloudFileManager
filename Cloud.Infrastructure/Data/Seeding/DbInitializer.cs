using Cloud.Domain.Interfaces.BlobStorage;
using Cloud.Domain.Interfaces.PasswordHasher;
using Cloud.Domain.Interfaces.Repositories;
using Cloud.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Cloud.Infrastructure.Data.Seeding;

public static class DbInitializer
{
    public static async Task SeedAdminAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var userRepository = scope.ServiceProvider.GetRequiredService<IBaseRepository<User>>();
        var passwordHasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher>();
        var blobStorage = scope.ServiceProvider.GetRequiredService<IBlobStorage>();


        var ifAdminExist = await userRepository.IfExistAsync(u => u.role == "Admin");

        if (!ifAdminExist)
        {
            var adminUserReg = new User
            {
                username = "admin",
                email = "admin",
                role = "Admin"
            };
            var adminUser = await userRepository.AddAsync(adminUserReg);
            adminUser.containerName = await blobStorage.CreateUserContainerAsync(adminUser.userId.ToString());
            adminUser.password = passwordHasher.Generate("admin");
            await userRepository.Update(adminUser);
        }


    }
}