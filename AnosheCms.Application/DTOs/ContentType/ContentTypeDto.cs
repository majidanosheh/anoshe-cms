using System;
using System.Collections.Generic;

namespace AnosheCms.Application.DTOs.ContentType
{
    public class ContentTypeDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string ApiSlug { get; set; }
        public string Description { get; set; }
        public List<ContentFieldDto> Fields { get; set; }
    }
}