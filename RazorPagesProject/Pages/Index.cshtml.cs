using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPagesProject.Models;
using System.Collections.Generic;

namespace RazorPagesProject.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        // In-memory list to store the class information
        public List<ClassInformationModel> ClassList { get; set; }

        // Constructor to initialize the logger
        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
            // Initialize the list when the page model is created
            ClassList = new List<ClassInformationModel>();
        }

        // OnGet method to load existing data
        public void OnGet()
        {
            // Sample data added if the list is empty
            if (ClassList.Count == 0)
            {
                ClassList.Add(new ClassInformationModel("Math 101", 30, "Introduction to Mathematics"));
                ClassList.Add(new ClassInformationModel("Science 101", 25, "Basic Science Principles"));
            }
        }

        // OnPostAddClass method to add new class
        public void OnPostAddClass(string className, int studentCount, string description)
        {
            // Generate the next Id by incrementing the last Id
            int newId = ClassList.Count > 0 ? ClassList[ClassList.Count - 1].Id + 1 : 1;
            ClassList.Add(new ClassInformationModel(className, studentCount, description) { Id = newId });
        }
    }
}
