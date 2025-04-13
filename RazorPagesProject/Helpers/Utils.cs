using Newtonsoft.Json;
using RazorPagesProject.Models;
using System.Collections.Generic;
using System.Linq;

namespace RazorPagesProject.Helpers
{
    public class Utils
    {
        private static Utils _instance;
        public static Utils Instance => _instance ??= new Utils();

       public string ExportToJson(List<ClassInformationTable> data, List<string> selectedColumns)
{
    // Eğer hiç sütun seçilmemişse, tüm sütunları döndür
    if (selectedColumns == null || selectedColumns.Count == 0)
    {
        selectedColumns = new List<string> { "ClassName", "StudentCount", "Description" };
    }

    var result = data.Select(d =>
    {
        var dict = new Dictionary<string, object>();

        // Id her zaman dahil edilsin (gizli olsa bile)
        dict["Id"] = d.Id;
        
        if (selectedColumns.Contains("ClassName")) dict["ClassName"] = d.ClassName ?? string.Empty;
        if (selectedColumns.Contains("StudentCount")) dict["StudentCount"] = d.StudentCount;
        if (selectedColumns.Contains("Description")) dict["Description"] = d.Description ?? string.Empty;

        return dict;
    });

    return JsonConvert.SerializeObject(result, Formatting.Indented);
}
    }
}
