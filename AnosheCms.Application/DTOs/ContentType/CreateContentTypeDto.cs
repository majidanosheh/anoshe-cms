using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AnosheCms.Application.DTOs.ContentType
{
    public class CreateContentTypeDto
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string ApiSlug { get; set; }

        public string? Description { get; set; }

        public List<CreateContentFieldDto> Fields { get; set; }
    }
}