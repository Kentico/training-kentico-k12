namespace Kentico.Dto.Menu
{
    public class MenuItemDto : IDto
    {
        public string Caption { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
    }
}
