using Microsoft.AspNetCore.Identity;

namespace ST10384311PROG6212POE.Data
{
    public static class DatabaseSeeder
    {
        public static async Task SeedUsersAndRoles(IServiceProvider serviceProvider)
        {
            // Get required services
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
            //------------------------------------------------------------------------------------------------------------------------------------------------------//
            // Define roles
            var roles = new[] { "Lecturer", "Administrator", "HR" };

            // Create roles if they don't exist
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }
            //------------------------------------------------------------------------------------------------------------------------------------------------------//
            // Create default Lecturer
            var lecturerUser = new IdentityUser
            {
                UserName = "lecturer@example.com",
                Email = "lecturer@example.com"
            };
            if (userManager.Users.All(u => u.UserName != lecturerUser.UserName))
            {
                var result = await userManager.CreateAsync(lecturerUser, "Lecturer@123");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(lecturerUser, "Lecturer");
                }
            }
            //------------------------------------------------------------------------------------------------------------------------------------------------------//
            // Create default Administrator
            var adminUser = new IdentityUser
            {
                UserName = "admin@example.com",
                Email = "admin@example.com"
            };
            if (userManager.Users.All(u => u.UserName != adminUser.UserName))
            {
                var result = await userManager.CreateAsync(adminUser, "Admin@123");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Administrator");
                }
            }
            //------------------------------------------------------------------------------------------------------------------------------------------------------//
            // Create default HR
            var hrUser = new IdentityUser
            {
                UserName = "hr@example.com",
                Email = "hr@example.com"
            };
            if (userManager.Users.All(u => u.UserName != hrUser.UserName))
            {
                var result = await userManager.CreateAsync(hrUser, "Hrelation@123");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(hrUser, "HR");
                }
            }
        }
    }
}
//-------------------------------------------------------------------------------------------End Of File--------------------------------------------------------------------//