namespace DiskyNet.Domain.Permission.Entities
{
    public sealed class MenuActionEntity
    {
        public int Id { get; private set; }
        public int MenuId { get; private set; }
        public int ActionId { get; private set; }

        private MenuActionEntity(int id, int menuId, int actionId)
        {
            Id = id;
            MenuId = menuId;
            ActionId = actionId;
        }

        /// <summary>
        /// Factory Method: Reconstituye una relación menú-acción existente desde la base de datos
        /// </summary>
        public static MenuActionEntity Reconstitute(int id, int menuId, int actionId)
        {
            return new MenuActionEntity(id, menuId, actionId);
        }
    }
}

