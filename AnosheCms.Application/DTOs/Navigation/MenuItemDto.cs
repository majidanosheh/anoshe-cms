// File: AnosheCms.Application/DTOs/Navigation/MenuItemDto.cs
using System.Collections.Generic;

namespace AnosheCms.Application.DTOs.Navigation
{
    // (یک آیتم منو)
    public class MenuItemDto
    {
        public string Title { get; set; }
        public string Icon { get; set; }
        public string Path { get; set; }
        public List<MenuItemDto> Children { get; set; } = new List<MenuItemDto>();
    }

    // (یک گروه در سایدبار)
    public class MenuGroupDto
    {
        public string Title { get; set; }
        public string Icon { get; set; }

        public List<MenuItemDto> Children { get; set; } = new List<MenuItemDto>();
    }
}