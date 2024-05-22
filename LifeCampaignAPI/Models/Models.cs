using LiteDB;

namespace LifeCampaignAPI.Models
{
    public abstract class DbItem
    {
        [BsonId]
        public Guid Id { get; set; } = Guid.NewGuid();
    }
    public abstract class ObjectiveBase : DbItem
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }

    public class Campaign : ObjectiveBase
    {
        public List<Guid> MissionIds { get; set; } = [];
    }

    public class Mission : ObjectiveBase
    {
        public List<Guid> ObjectiveIds { get; set; } = [];
    }

    public class Objective : ObjectiveBase
    {
        public string Detail { get; set; } = string.Empty;
    }

    public class PlayerHistory : DbItem
    {
        public required Guid ObjectiveId { get; set; }
        public required Guid PlayerId { get; set; }
        public string Feedback { get; set; } = string.Empty;
        public required Vibe Vibe { get; set; }
    }

    public class Player : DbItem
    {
        public required string Username { get; set; }
    }

    public enum Vibe
    {
        Happy,
        Meh,
        Sad
    }
}