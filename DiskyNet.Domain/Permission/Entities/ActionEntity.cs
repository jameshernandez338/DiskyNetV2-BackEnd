namespace DiskyNet.Domain.Permission.Entities
{
    public sealed class ActionEntity
    {
        public int Id { get; private set; }
        public string Code { get; private set; }
        public string? Description { get; private set; }

        private ActionEntity(int id, string code, string? description)
        {
            Id = id;
            Code = code;
            Description = description;
        }

        /// <summary>
        /// Factory Method: Reconstituye una acción existente desde la base de datos
        /// </summary>
        public static ActionEntity Reconstitute(int id, string code, string? description)
        {
            return new ActionEntity(id, code, description);
        }
    }
}
