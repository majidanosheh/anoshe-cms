// File: AnosheCms.Application/DTOs/Navigation/MenuItemDto.cs
using System.Collections.Generic;

namespace AnosheCms.Application.DTOs.Navigation
{
    // (یک آیتم منو)
    public class MenuItemDto
    {
        public string Title { get; set; }
        public string Icon { get; set; }
        public string RouteName { get; set; } // (نام مسیر در Vue Router)
        public Dictionary<string, string> RouteParams { get; set; }
    }

    // (یک گروه در سایدبار)
    public class MenuGroupDto
    {
        public string Title { get; set; }
        public List<MenuItemDto> Items { get; set; } = new List<MenuItemDto>();
    }
}