using System;
using System.Collections.Generic;
using System.Text;

namespace HttpForwarder.Abstract.DataModels
{
    public class Menu
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; } = "#";
        public string Class { get; set; }
        public string ParentId { get; set; }
        public IEnumerable<Menu> Submenus { get; set; }
    }

    public class MainMenu
    {
        public IEnumerable<Menu> Menus { get; set; }
    }
}
