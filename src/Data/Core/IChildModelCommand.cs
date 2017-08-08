namespace BioEngine.Data.Core
{
    public interface IChildModelCommand
    {
        int? GameId { get; set; }
        int? DeveloperId { get; set; }
        int? TopicId { get; set; } 
    }
}