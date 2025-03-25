namespace RazorPagesProject.Models
{
    public class ClassInformationModel
    {
        // Auto-incremented Id
        public int Id { get; set; }

        // Class name
        public string ClassName { get; set; }

        // Number of students
        public int StudentCount { get; set; }

        // Description of the class
        public string Description { get; set; }

        // Constructor (optional, for initializing properties)
        public ClassInformationModel(string className, int studentCount, string description)
        {
            ClassName = className;
            StudentCount = studentCount;
            Description = description;
        }
    }
}

