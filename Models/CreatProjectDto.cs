namespace IlmHub04.Models
{
    public class CreatProjectDto
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public IFormFile Photo { get; set; }
        public string ProjectWebLink { get; set; }
        public string SourceCodeLink { get; set; }
    }
}
