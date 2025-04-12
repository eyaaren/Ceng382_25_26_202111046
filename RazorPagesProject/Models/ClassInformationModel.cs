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

        // Parametresiz constructor (zorunlu)
        public ClassInformationModel() { }

        // Parametreli constructor
        public ClassInformationModel(string className, int studentCount, string description)
        {
            ClassName = className;
            StudentCount = studentCount;
            Description = description;
        }
    }

    // Yeni tablo modeli (sadece tablo gösterimi için)
    public class ClassInformationTable
    {
        public int Id { get; set; } // Tabloda gösterilmeyecek ama işlemlerde kullanılacak
        public string ClassName { get; set; }
        public int StudentCount { get; set; }
        public string Description { get; set; }
    }
}
