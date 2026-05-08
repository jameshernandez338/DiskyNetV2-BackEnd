namespace DiskyNet.Application.Secrets
{
    public interface ISecretProvider
    {
        /// <summary>
        /// Get a secret value by key. Returns null when not found.
        /// </summary>
        string? GetSecret(string key);
    }
}
