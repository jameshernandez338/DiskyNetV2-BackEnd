using DiskyNet.Domain.Exceptions;

namespace DiskyNet.Domain.User.Entities
{
    public sealed class UserEntity
    {
        public long Id { get; private set; }
        public string UserName { get; private set; }
        public string Email { get; private set; }
        public string FirstName { get; private set; }
        public string? MiddleName { get; private set; }
        public string LastName { get; private set; }
        public string? SecondLastName { get; private set; }
        public bool IsActive { get; private set; }
        public string? CompanyCode { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }

        // Relationship - Single role
        public int RoleId { get; private set; }

        private UserEntity(
            long id,
            string userName,
            string email,
            string firstName,
            string? middleName,
            string lastName,
            string? secondLastName,
            bool isActive,
            DateTime createdAt,
            DateTime? updatedAt,
            int roleId)
        {
            Id = id;
            UserName = userName;
            Email = email;
            FirstName = firstName;
            MiddleName = middleName;
            LastName = lastName;
            SecondLastName = secondLastName;
            IsActive = isActive;
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
            RoleId = roleId;
        }

        /// <summary>
        /// Factory Method: Reconstituye un usuario existente desde la base de datos
        /// </summary>
        public static UserEntity Reconstitute(
            long id,
            string userName,
            string email,
            string firstName,
            string? middleName,
            string lastName,
            string? secondLastName,
            bool isActive,
            DateTime createdAt,
            DateTime? updatedAt,
            int roleId)
        {
            return new UserEntity(
                id: id,
                userName: userName,
                email: email,
                firstName: firstName,
                middleName: middleName,
                lastName: lastName,
                secondLastName: secondLastName,
                isActive: isActive,
                createdAt: createdAt,
                updatedAt: updatedAt,
                roleId: roleId);
        }

        /// <summary>
        /// Factory Method: Crea un nuevo usuario
        /// </summary>
        public static UserEntity Create(
            string userName,
            string email,
            string firstName,
            string? middleName,
            string lastName,
            string? secondLastName,
            int roleId,
            bool isActive = true)
        {
            ValidateUserName(userName);
            ValidateEmail(email);
            ValidateFirstName(firstName);
            ValidateLastName(lastName);
            ValidateRole(roleId);

            return new UserEntity(
                id: 0,
                userName: userName,
                email: email,
                firstName: firstName,
                middleName: middleName,
                lastName: lastName,
                secondLastName: secondLastName,
                isActive: isActive,
                createdAt: DateTime.UtcNow,
                updatedAt: null,
                roleId: roleId);
        }

        /// <summary>
        /// Actualiza la información del usuario
        /// </summary>
        public void Update(
            string userName,
            string email,
            string firstName,
            string? middleName,
            string lastName,
            string? secondLastName,
            int roleId)
        {
            ValidateUserName(userName);
            ValidateEmail(email);
            ValidateFirstName(firstName);
            ValidateLastName(lastName);
            ValidateRole(roleId);

            UserName = userName;
            Email = email;
            FirstName = firstName;
            MiddleName = middleName;
            LastName = lastName;
            SecondLastName = secondLastName;
            RoleId = roleId;
            UpdatedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Activa el usuario
        /// </summary>
        public void Activate()
        {
            IsActive = true;
            UpdatedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Desactiva el usuario
        /// </summary>
        public void Deactivate()
        {
            IsActive = false;
            UpdatedAt = DateTime.UtcNow;
        }

        // Domain Validations
        private static void ValidateUserName(string userName)
        {
            if (string.IsNullOrWhiteSpace(userName))
                throw new DomainException("User name is required");

            if (userName.Length > 50)
                throw new DomainException("User name cannot exceed 50 characters");
        }

        private static void ValidateEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new DomainException("Email is required");

            if (email.Length > 100)
                throw new DomainException("Email cannot exceed 100 characters");

            if (!email.Contains('@'))
                throw new DomainException("Invalid email format");
        }

        private static void ValidateFirstName(string firstName)
        {
            if (string.IsNullOrWhiteSpace(firstName))
                throw new DomainException("First name is required");

            if (firstName.Length > 50)
                throw new DomainException("First name cannot exceed 50 characters");
        }

        private static void ValidateLastName(string lastName)
        {
            if (string.IsNullOrWhiteSpace(lastName))
                throw new DomainException("Last name is required");

            if (lastName.Length > 50)
                throw new DomainException("Last name cannot exceed 50 characters");
        }

        private static void ValidateRole(int roleId)
        {
            if (roleId <= 0)
                throw new DomainException("Invalid role ID");
        }

        /// <summary>
        /// Obtiene el nombre completo del usuario
        /// </summary>
        public string GetFullName()
        {
            var names = new List<string> { FirstName };

            if (!string.IsNullOrWhiteSpace(MiddleName))
                names.Add(MiddleName);

            names.Add(LastName);

            if (!string.IsNullOrWhiteSpace(SecondLastName))
                names.Add(SecondLastName);

            return string.Join(" ", names);
        }
    }
}
