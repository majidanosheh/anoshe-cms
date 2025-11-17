
using System;
using System.ComponentModel.DataAnnotations;

namespace AnosheCms.Application.DTOs.Form
{
    public class FormFieldOrderDto
    {
        [Required]
        public Guid FieldId { get; set; }

        [Required]
        public int Order { get; set; }
    }
}