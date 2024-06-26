﻿using System.Diagnostics.CodeAnalysis;

namespace LinFx.Security.Encryption;

/// <summary>
/// Can be used to simply encrypt/decrypt texts.
/// Use <see cref="StringEncryptionOptions"/> to configure default values.
/// </summary>
public interface IStringEncryptionService
{
    /// <summary>
    /// Encrypts a text.
    /// </summary>
    /// <param name="plainText">The text in plain format</param>
    /// <param name="passPhrase">A phrase to use as the encryption key (optional, uses default if not provided)</param>
    /// <param name="salt">Salt value (optional, uses default if not provided)</param>
    /// <returns>Enrypted text</returns>
    string Encrypt([AllowNull] string plainText, string passPhrase = null, byte[] salt = null);

    /// <summary>
    /// Decrypts a text that is encrypted by the <see cref="Encrypt"/> method.
    /// </summary>
    /// <param name="cipherText">The text in encrypted format</param>
    /// <param name="passPhrase">A phrase to use as the encryption key (optional, uses default if not provided)</param>
    /// <param name="salt">Salt value (optional, uses default if not provided)</param>
    /// <returns>Decrypted text</returns>
    string Decrypt([AllowNull] string cipherText, string passPhrase = null, byte[] salt = null);
}