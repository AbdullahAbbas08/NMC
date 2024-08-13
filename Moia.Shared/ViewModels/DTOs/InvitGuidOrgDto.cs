namespace Moia.Shared.ViewModels.DTOs
{
    public class DepartmentDto
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public NameIdViewModel Manager { get; set; }
    }
}
 