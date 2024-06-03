namespace Motorcycles.Tables
{
    public class Motorcycle
    {
        public int Id { get; set; }

        public string? Make { get; set; }

        public string? Model { get; set; }

        public int Year { get; set; }

        public bool IsDeleted { get; set; }

        public int CreatedByUserId { get; set; }

        public int UpdatedByUserId { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime DateUpdated { get; set; }
    }
}
