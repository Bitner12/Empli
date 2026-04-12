namespace Domain.Entities
{
    public class Hour
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public float Hours { get; set; }
        public Guid WorkerId { get; set; }
        

    }
}
