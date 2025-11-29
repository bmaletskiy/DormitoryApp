namespace DormitoryApp.Models
{
    public class StudentInfo
    {
        public string? Name { get; set; }
        public string? Faculty { get; set; }
        public string? Department { get; set; }
        public string? Room { get; set; }
        public string? Bed { get; set; }
        public string? Course { get; set; }
        public string? StartDate { get; set; }
        public string? EndDate { get; set; }

        public override string ToString()
        {
            return $"{Name} ({Faculty}, {Department}, Room {Room}, Bed {Bed}, Course {Course}, StartDate {StartDate}, EndDate {EndDate})";
        }
    }
}
