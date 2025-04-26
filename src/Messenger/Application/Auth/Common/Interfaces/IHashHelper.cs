namespace Messenger.Application.Auth.Common.Interfaces;

public interface IHashHelper
{
    /// <summary>
    /// Hash value
    /// </summary>
    /// <param name="value">value to hash</param>
    /// <returns>hash string</returns>
    string Hash(string value);

    /// <summary>
    /// Verify hash
    /// </summary>
    /// <param name="hash">expected hash</param>
    /// <param name="value">value</param>
    /// <returns>Is hash valid</returns>
    bool Verify(string hash, string value);
}
