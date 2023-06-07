using Microsoft.EntityFrameworkCore;
using Task6.Models;

namespace Task6.Services
{
    public interface IUserService
    {
        public List<UserModel> GetUsers();
        public Task CreateUser(string name);
        Task<UserModel> GetByNameAsync(string name);
        Task<IEnumerable<UserModel>> FindByNameAsync(string? substring);
        Task<int> CreateMessage(UserModel sender, UserModel recipient, string title, string text);
        List<MessageModel> GetUserMessages(UserModel user);
        MessageModel GetMessageById(int id);
    }
    public class UserService : IUserService
    {
        private readonly ApplicationContext db;

        public UserService(ApplicationContext db)
        {
            this.db = db;
        }

        public List<UserModel> GetUsers()
        {
            return db.Users.ToList();
        }
        public List<MessageModel> GetUserMessages(UserModel user)
        {
            return db.Messages.Include(x => x.Sender).Include(x => x.Recipient).Where(x => x.Recipient == user).ToList();
        }
        public MessageModel GetMessageById(int id)
        {
            return db.Messages.First(x => x.Id == id);
        }

        public async Task CreateUser(string name)
        {
            db.Users.Add(new UserModel {  Name = name });
            await db.SaveChangesAsync();
        }

        public async Task<UserModel> GetByNameAsync(string name) =>
            await db.Users.FirstOrDefaultAsync(x => string.Equals(x.Name, name));

        public async Task<IEnumerable<UserModel>> FindByNameAsync(string? substring)
        {
            if (substring is null)
            {
                return await db.Users.Take(10).ToListAsync();
            }
            return await db.Users.Where(x => x.Name.Contains(substring)).Take(10).ToListAsync();
        }
        public async Task<int> CreateMessage(UserModel sender, UserModel recipient,  string title, string text)
        {
            var message = new MessageModel { Title = title, Text = text, Sender = sender, Recipient = recipient, SentAt = DateTime.UtcNow };
            db.Messages.Add(message);
            await db.SaveChangesAsync();
            return message.Id;
        }
    }
}
