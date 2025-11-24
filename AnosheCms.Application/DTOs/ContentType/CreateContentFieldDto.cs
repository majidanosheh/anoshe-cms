
using System.ComponentModel.DataAnnotations;

namespace AnosheCms.Application.DTOs.ContentType
{
    public class CreateContentFieldDto
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Label { get; set; }

        [Required]
        public string FieldType { get; set; }

        public bool IsRequired { get; set; }

        public string? Options { get; set; } 
    }
}