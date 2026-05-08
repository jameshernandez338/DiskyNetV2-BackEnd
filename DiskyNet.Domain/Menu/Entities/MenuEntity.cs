namespace DiskyNet.Domain.Menu.Entities
{
    public sealed class MenuEntity
    {
        public int Id { get; private set; }
        public string MenuName { get; private set; }
        public string? Code { get; private set; }
        public string? MenuRoute { get; private set; }
        public string? Icon { get; private set; }
        public int DisplayOrder { get; private set; }
        public int? ParentId { get; private set; }
        public string MenuType { get; private set; }

        private MenuEntity(
            int id,
            string menuName,
            string? code,
            string? menuRoute,
            string? icon,
            int displayOrder,
            int? parentId,
            string menuType)
        {
            Id = id;
            MenuName = menuName;
            Code = code;
            MenuRoute = menuRoute;
            Icon = icon;
            DisplayOrder = displayOrder;
            ParentId = parentId;
            MenuType = menuType;
        }

        /// <summary>
        /// Factory Method: Reconstituye un menú existente desde la base de datos
        /// </summary>
        public static MenuEntity Reconstitute(
            int id,
            string menuName,
            string? code,
            string? menuRoute,
            string? icon,
            int displayOrder,
            int? parentId,
            string menuType)
        {
            return new MenuEntity(
                id: id,
                menuName: menuName,
                code: code,
                menuRoute: menuRoute,
                icon: icon,
                displayOrder: displayOrder,
                parentId: parentId,
                menuType: menuType
            );
        }
    }
}