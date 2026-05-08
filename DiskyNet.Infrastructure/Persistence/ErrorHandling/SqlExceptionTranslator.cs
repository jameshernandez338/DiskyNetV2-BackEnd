using DiskyNet.Domain.Exceptions;
using Microsoft.Data.SqlClient;

namespace DiskyNet.Infrastructure.Persistence.ErrorHandling
{
    public static class SqlExceptionTranslator
    {
        private const int UniqueConstraintViolation = 2627;
        private const int UniqueIndexViolation = 2601;
        private const int ForeignKeyViolation = 547;
        private const int DeadlockVictim = 1205;
        private const int Timeout = -2;

        /// <summary>
        /// Traduce una SqlException a una excepción de dominio apropiada
        /// </summary>
        public static Exception Translate(SqlException sqlException, string operation)
        {
            return sqlException.Number switch
            {
                UniqueConstraintViolation or UniqueIndexViolation
                    => new ConflictException(
                        $"Ya existe un registro con los mismos datos. {operation}",
                        sqlException),

                ForeignKeyViolation
                    => new ConflictException(
                        $"No se puede completar la operación debido a referencias existentes. {operation}",
                        sqlException),

                DeadlockVictim
                    => new InfrastructureException(
                        $"La operación falló debido a contención en la base de datos. Por favor intente nuevamente. {operation}",
                        sqlException),

                Timeout
                    => new InfrastructureException(
                        $"La operación tardó demasiado tiempo. Por favor intente nuevamente. {operation}",
                        sqlException),

                _ => new InfrastructureException(
                        $"Error inesperado al ejecutar la operación de base de datos: {operation}",
                        sqlException)
            };
        }
    }
}

