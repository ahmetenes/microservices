using System;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PlatformService.Models;

namespace PlatformService.Data
{
    public static class PrebDB
    {
        public static void prepareDatabase(IApplicationBuilder builder, bool production)
        {
            using (var serviceScope = builder.ApplicationServices.CreateScope())
            {
                SeedData(serviceScope.ServiceProvider.GetService<AppDbContext>(),production);
            }
        }
        private static void SeedData(AppDbContext context,bool production)
        {
            if (production)
            {
                try
                {
                    context.Database.Migrate();     
                }
                catch (System.Exception ex)
                {
                    Console.WriteLine($"Migration failed:{ex}");
                }
                
            }
            
            if (!context.Platforms.Any())
            
            {
                Console.WriteLine("Seeding");
                context.AddRange(
                    new Platform() { Name = "dotnet", Publisher = "Microsoft", Cost = "Free" },
                    new Platform() { Name = "dotdotnet", Publisher = "Microsoft", Cost = "Free" },
                    new Platform() { Name = "dotdotdotnet", Publisher = "Microsoft", Cost = "Free" }
                );
                context.SaveChanges();
            }
            else
            {
                Console.WriteLine("We have data");
            }
            

        }
    }
}