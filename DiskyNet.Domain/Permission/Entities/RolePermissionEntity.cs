namespace DiskyNet.Domain.Permission.Entities
{
    public sealed class RolePermissionEntity
    {
        public int Id { get; private set; }
        public int RoleId { get; private set; }
        public int MenuId { get; private set; }
        public int ActionId { get; private set; }

        // Navigation properties for enriched data
        public string? MenuCode { get; private set; }
        public string? ActionCode { get; private set; }

        private RolePermissionEntity(
            int id,
            int roleId,
            int menuId,
            int actionId,
            string? menuCode = null,
            string? actionCode = null)
        {
            Id = id;
            RoleId = roleId;
            MenuId = menuId;
            ActionId = actionId;
            MenuCode = menuCode;
            ActionCode = actionCode;
        }

        /// <summary>
        /// Factory Method: Reconstituye un permiso existente desde la base de datos
        /// </summary>
        public static RolePermissionEntity Reconstitute(
            int id,
            int roleId,
            int menuId,
            int actionId,
            string? menuCode = null,
            string? actionCode = null)
        {
            return new RolePermissionEntity(id, roleId, menuId, actionId, menuCode, actionCode);
        }
    }
}

