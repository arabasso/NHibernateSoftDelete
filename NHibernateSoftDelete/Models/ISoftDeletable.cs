namespace NHibernateSoftDelete.Models
{
    public interface IEntity
    {
        long Id { get; set; }
    }

    public interface ISoftDeletable :
        IEntity
    {
        bool Deleted { get; set; }
    }
}