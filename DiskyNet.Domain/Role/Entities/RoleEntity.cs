using DiskyNet.Domain.Exceptions;

namespace DiskyNet.Domain.Role.Entities
{
    public sealed class RoleEntity
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public bool IsActive { get; private set; }

        private RoleEntity(
            int id,
            string name,
            string description,
            bool isActive)
        {
            Id = id;
            Name = name;
            Description = description;
            IsActive = isActive;
        }

        /// <summary>
        /// Factory Method: Reconstituye un rol existente desde la base de datos
        /// </summary>
        public static RoleEntity Reconstitute(
            int id,
            string name,
            string description,
            bool isActive)
        {
            return new RoleEntity(
                id: id,
                name: name,
                description: description,
                isActive: isActive);
        }

        /// <summary>
        /// Factory Method: Crea un nuevo rol
        /// </summary>
        public static RoleEntity Create(
            string name,
            string description,
            bool isActive = true)
        {
            ValidateName(name);
            ValidateDescription(description);

            return new RoleEntity(
                id: 0,
                name: name.Trim(),
                description: description.Trim(),
                isActive: isActive);
        }

        public void Update(string name, string description, bool isActive)
        {
            ValidateName(name);
            ValidateDescription(description);

            Name = name.Trim();
            Description = description.Trim();
            IsActive = isActive;
        }

        public void Activate() => IsActive = true;
        public void Deactivate() => IsActive = false;

        private static void ValidateName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new DomainException("El nombre del rol no puede estar vacío");

            if (name.Length > 50)
                throw new DomainException("El nombre del rol no puede exceder los 50 caracteres");
        }

        private static void ValidateDescription(string description)
        {
            if (string.IsNullOrWhiteSpace(description))
                throw new DomainException("La descripción del rol no puede estar vacía");

            if (description.Length > 200)
                throw new DomainException("La descripción del rol no puede exceder los 200 caracteres");
        }
    }
}