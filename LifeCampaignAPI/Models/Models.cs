using LiteDB;

namespace LifeCampaignAPI.Models
{
    public abstract class ObjectiveBase
    {
        [BsonId]
        public Guid Id { get; set; } = new Guid();
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

    public class PlayerHistory
    {
        [BsonId]
        public Guid Id { get; set; } = Guid.NewGuid();
        
        public required Guid ObjectiveId { get; set; }
        public required Guid PlayerId { get; set; }
        public string Feedback { get; set; } = string.Empty;
        public required Vibe Vibe { get; set; }
    }

    public class Player
    {
        [BsonId]
        public Guid Id { get; set; } = Guid.NewGuid();
        public required string Username { get; set; }
    }

    public enum Vibe
    {
        Happy,
        Meh,
        Sad
    }
}