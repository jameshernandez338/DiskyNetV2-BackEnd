using DiskyNet.Domain.User.Entities;
using DiskyNet.Infrastructure.Persistence.DTOs.User;
using System;
using System.Collections.Generic;
using System.Text;

namespace DiskyNet.Infrastructure.Persistence.Mappers.User
{
    public static class UserRepositoryMapper
    {
        public static UserEntity ToDomain(UserDto dto, int roleId)
        {
            return UserEntity.Reconstitute(
                id: dto.Id,
                userName: dto.UserName,
                email: dto.Email,
                firstName: dto.FirstName,
                middleName: dto.MiddleName,
                lastName: dto.LastName,
                secondLastName: dto.SecondLastName,
                isActive: dto.IsActive,
                createdAt: dto.CreatedAt,
                updatedAt: dto.UpdatedAt,
                roleId: roleId
            );
        }
    }
}
