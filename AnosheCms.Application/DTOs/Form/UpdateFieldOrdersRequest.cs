

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AnosheCms.Application.DTOs.Form
{
    public class UpdateFieldOrdersRequest
    {
        [Required]
        public Guid FormId { get; set; }

        [Required]
        [MinLength(1)]
        public List<FormFieldOrderDto> Fields { get; set; }
    }
}