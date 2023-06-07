using Microsoft.EntityFrameworkCore;

namespace Task6.Models
{
    public class ApplicationContext : DbContext
    {
        public DbSet<UserModel> Users { get; set; } = null!;
        public DbSet<MessageModel> Messages { get; set; } = null!;

        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
            //Database.EnsureCreated();   // создаем базу данных при первом обращении
        }
    }
}
