namespace RestaurantManagementSystem.Models
{
    public class Position
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Employer> Employers { get; set; }
        public bool IsDeactive { get; set; }
    }
}
