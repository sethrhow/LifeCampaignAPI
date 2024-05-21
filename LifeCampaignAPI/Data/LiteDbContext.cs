using LifeCampaignAPI.Models;
using LiteDB;
using System;

namespace LifeCampaignAPI.Data
{
    public class LiteDbContext(string databasePath = "Data.db") : IDisposable
    {
        private readonly LiteDatabase _database = new(databasePath);

        public ILiteCollection<Campaign> Campaign => _database.GetCollection<Campaign>("Campaign");
        public ILiteCollection<Mission> Mission => _database.GetCollection<Mission>("Mission");
        public ILiteCollection<Objective> Objective => _database.GetCollection<Objective>("Objective");
        public ILiteCollection<PlayerHistory> PlayerHistory => _database.GetCollection<PlayerHistory>("PlayerHistory");
        public ILiteCollection<Player> Player => _database.GetCollection<Player>("Player");

        public void Dispose()
        {
            _database?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
