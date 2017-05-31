/* 
 * Copyright (c) Stacks Contributors
 * 
 * This file is subject to the terms and conditions defined in
 * the LICENSE file, which is part of this source code package.
 */

using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Kuno.Text;
using Kuno.Validation;

namespace Kuno.Security
{
    /// <summary>
    /// Provides simplified methods for encrypting and decrypting text.
    /// </summary>
    public static class Encryption
    {
        /// <summary>
        /// Decrypts the specified text with the specified provider and key.
        /// </summary>
        /// <typeparam name="T">The type of SymmetricAlgorithm to use.</typeparam>
        /// <param name="text">The specified text.</param>
        /// <param name="key">The specified key.</param>
        /// <returns>
        /// The decrypted text.
        /// </returns>
        /// <exception cref="System.ArgumentNullException"></exception>
        public static byte[] Decrypt<T>(byte[] text, string key) where T : SymmetricAlgorithm, new()
        {
            Argument.NotNull(text, nameof(text));

            using (var provider = Activator.CreateInstance<T>())
            {
                return Decrypt(text, key, provider);
            }
        }

        /// <summary>
        /// Decrypts the specified text with the specified provider and key.
        /// </summary>
        /// <typeparam name="T">The type of SymmetricAlgorithm to use.</typeparam>
        /// <param name="text">The specified text.</param>
        /// <param name="key">The specified key.</param>
        /// <returns>
        /// The decrypted text.
        /// </returns>
        /// <exception cref="System.ArgumentNullException"></exception>
        public static string Decrypt<T>(string text, string key) where T : SymmetricAlgorithm, new()
        {
            Argument.NotNull(text, nameof(text));

            using (var provider = Activator.CreateInstance<T>())
            {
                return Decrypt(text, key, provider);
            }
        }

        /// <summary>
        /// Decrypts the specified text with the specified provider and key.
        /// </summary>
        /// <param name="text">The specified text.</param>
        /// <param name="key">The specified key.</param>
        /// <returns>
        /// The decrypted text.
        /// </returns>
        /// <exception cref="System.ArgumentNullException"></exception>
        public static string Decrypt(string text, string key)
        {
            Argument.NotNull(text, nameof(text));

            return Decrypt(text, key, null);
        }

        /// <summary>
        /// Decrypts the specified text with the specified provider and key.
        /// </summary>
        /// <param name="text">The specified text.</param>
        /// <param name="key">The specified key.</param>
        /// <returns>
        /// The decrypted text.
        /// </returns>
        /// <exception cref="System.ArgumentNullException"></exception>
        public static byte[] Decrypt(byte[] text, string key)
        {
            Argument.NotNull(text, nameof(text));

            return Decrypt(text, key, null);
        }

        /// <summary>
        /// Decrypts the specified text.
        /// </summary>
        /// <param name="text">The specified text.</param>
        /// <returns>The decrypted text.</returns>
        /// <exception cref="System.ArgumentNullException"></exception>
        /// <example>
        ///   <c>Encryption.Decrypt("encrypted content");</c>
        /// </example>
        public static string Decrypt(string text)
        {
            Argument.NotNull(text, nameof(text));

            return Decrypt(text, null, null);
        }

        /// <summary>
        /// Decrypts the specified text.
        /// </summary>
        /// <param name="text">The specified text.</param>
        /// <returns>The decrypted text.</returns>
        /// <exception cref="System.ArgumentNullException"></exception>
        public static byte[] Decrypt(byte[] text)
        {
            Argument.NotNull(text, nameof(text));

            return Decrypt(text, null, null);
        }

        /// <summary>
        /// Encrypts the specified text with specified key.
        /// </summary>
        /// <param name="text">The specified text.</param>
        /// <param name="key">The specified key.</param>
        /// <returns>
        /// The encrypted text.
        /// </returns>
        /// <exception cref="System.ArgumentNullException"></exception>
        public static string Encrypt(string text, string key)
        {
            Argument.NotNull(text, nameof(text));

            return Encrypt(text, null, key);
        }

        /// <summary>
        /// Encrypts the specified text with specified key.
        /// </summary>
        /// <param name="text">The specified text.</param>
        /// <param name="key">The specified key.</param>
        /// <returns>
        /// The encrypted text.
        /// </returns>
        /// <exception cref="System.ArgumentNullException"></exception>
        public static byte[] Encrypt(byte[] text, string key)
        {
            return Encrypt(text, null, key);
        }

        /// <summary>
        /// Encrypts the specified text with specified key.
        /// </summary>
        /// <typeparam name="T">The type of SymmetricAlgorithm to use.</typeparam>
        /// <param name="text">The specified text.</param>
        /// <param name="key">The specified key.</param>
        /// <returns>
        /// The encrypted text.
        /// </returns>
        /// <exception cref="System.ArgumentNullException"></exception>
        public static string Encrypt<T>(string text, string key) where T : SymmetricAlgorithm, new()
        {
            Argument.NotNull(text, nameof(text));

            using (var provider = Activator.CreateInstance<T>())
            {
                return Encrypt(text, provider, key);
            }
        }

        /// <summary>
        /// Encrypts the specified text with specified key.
        /// </summary>
        /// <typeparam name="T">The type of SymmetricAlgorithm to use.</typeparam>
        /// <param name="text">The specified text.</param>
        /// <param name="key">The specified key.</param>
        /// <returns>
        /// The encrypted text.
        /// </returns>
        /// <exception cref="System.ArgumentNullException"></exception>
        public static byte[] Encrypt<T>(byte[] text, string key) where T : SymmetricAlgorithm, new()
        {
            Argument.NotNull(text, nameof(text));

            using (var provider = Activator.CreateInstance<T>())
            {
                return Encrypt(text, provider, key);
            }
        }

        /// <summary>
        /// Encrypts the specified text using a default encryption provider.
        /// </summary>
        /// <param name="text">The specified text.</param>
        /// <returns>
        /// The encrypted text.
        /// </returns>
        /// <exception cref="System.ArgumentNullException"></exception>
        public static string Encrypt(string text)
        {
            Argument.NotNull(text, nameof(text));

            return Encrypt(text, null, null);
        }

        /// <summary>
        /// Encrypts the specified text using a default encryption provider.
        /// </summary>
        /// <param name="text">The specified text.</param>
        /// <returns>
        /// The encrypted text.
        /// </returns>
        /// <exception cref="System.ArgumentNullException"></exception>
        public static byte[] Encrypt(byte[] text)
        {
            Argument.NotNull(text, nameof(text));

            return Encrypt(text, null, null);
        }

        /// <summary>
        /// Creates a hash from the from the specified content using the specified salt.
        /// </summary>
        /// <param name="text">The text to hash.</param>
        /// <param name="salt">The string to use for a salt.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">
        /// </exception>
        public static string Hash(string text, string salt)
        {
            Argument.NotNull(text, nameof(text));
            Argument.NotNull(salt, nameof(salt));

            return Hash(salt + Hash(text));
        }

        /// <summary>
        /// Creates a hash from the specified text.
        /// </summary>
        /// <param name="text">The text to hash.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentException"></exception>
        public static string Hash(string text)
        {
            Argument.NotNull(text, nameof(text));

            using (var provider = MD5.Create())
            {
                var data = provider.ComputeHash(Encoding.UTF8.GetBytes(text));
                return Convert.ToBase64String(data);
            }
        }

        #region Non-Public Methods

        private static void SetValidKey(this SymmetricAlgorithm provider, string key)
        {
            key = key ?? "";
            provider.GenerateKey();
            provider.Key = Encoding.ASCII.GetBytes(key.Resize(provider.Key.Length, ' '));
        }

        [SuppressMessage("ReSharper", "InconsistentNaming")]
        private static void SetValidIV(this SymmetricAlgorithm provider, string key)
        {
            key = key ?? "";
            provider.GenerateIV();
            provider.IV = Encoding.ASCII.GetBytes(key.Resize(provider.IV.Length, ' '));
        }

        private static byte[] Encrypt(byte[] text, SymmetricAlgorithm provider, string key)
        {
            var created = false;
            try
            {
                if (provider == null)
                {
                    provider = Aes.Create();
                    created = true;
                }
                provider.SetValidKey(key);
                provider.SetValidIV(key);


                using (var memoryStream = new MemoryStream())
                {
                    var cryptoStream = new CryptoStream(memoryStream, provider.CreateEncryptor(), CryptoStreamMode.Write);
                    cryptoStream.Write(text, 0, text.Length);
                    cryptoStream.FlushFinalBlock();
                    return memoryStream.ToArray();
                }
            }
            finally
            {
                if (created)
                {
                    provider.Dispose();
                }
            }
        }

        private static string Encrypt(string text, SymmetricAlgorithm provider, string key)
        {
            string target = null;
            var created = false;
            try
            {
                if (provider == null)
                {
                    provider = Aes.Create();
                    created = true;
                }
                provider.SetValidKey(key);
                provider.SetValidIV(key);


                using (var memoryStream = new MemoryStream())
                {
                    var content = Encoding.ASCII.GetBytes(text);
                    var cryptoStream = new CryptoStream(memoryStream, provider.CreateEncryptor(), CryptoStreamMode.Write);
                    cryptoStream.Write(content, 0, content.Length);
                    cryptoStream.FlushFinalBlock();
                    target = Convert.ToBase64String(memoryStream.ToArray());
                }
            }
            finally
            {
                if (created)
                {
                    provider.Dispose();
                }
            }
            return target;
        }

        private static string Decrypt(string text, string key, SymmetricAlgorithm provider)
        {
            string target = null;
            var created = false;
            try
            {
                if (provider == null)
                {
                    provider = Aes.Create();
                    created = true;
                }
                provider.SetValidKey(key);
                provider.SetValidIV(key);

                var content = Convert.FromBase64String(text);

                using (var memoryStream = new MemoryStream(content, 0, content.Length))
                {
                    var cryptoStream = new CryptoStream(memoryStream, provider?.CreateDecryptor(), CryptoStreamMode.Read);
                    using (var reader = new StreamReader(cryptoStream))
                    {
                        target = reader.ReadToEnd();
                    }
                }
            }
            catch (Exception exception)
            {
                throw new InvalidOperationException("Unable to decrypt the string.", exception);
            }
            finally
            {
                if (created)
                {
                    provider?.Dispose();
                }
            }
            return target;
        }

        private static byte[] Decrypt(byte[] text, string key, SymmetricAlgorithm provider)
        {
            var created = false;
            try
            {
                if (provider == null)
                {
                    provider = Aes.Create();
                    created = true;
                }
                provider.SetValidKey(key);
                provider.SetValidIV(key);

                using (var memoryStream = new MemoryStream())
                {
                    using (var cryptoStream = new CryptoStream(memoryStream, provider?.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cryptoStream.Write(text, 0, text.Length);
                        cryptoStream.FlushFinalBlock();
                        return memoryStream.ToArray();
                    }
                }
            }
            catch (Exception exception)
            {
                throw new InvalidOperationException("Unable to decrypt the bytes.", exception);
            }
            finally
            {
                if (created)
                {
                    provider?.Dispose();
                }
            }
        }

        #endregion
    }
}