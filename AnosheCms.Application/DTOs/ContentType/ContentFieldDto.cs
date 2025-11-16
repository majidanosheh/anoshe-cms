using System;

namespace AnosheCms.Application.DTOs.ContentType
{
    public class ContentFieldDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string ApiSlug { get; set; }
        public string FieldType { get; set; }
        public bool IsRequired { get; set; }
    }
}