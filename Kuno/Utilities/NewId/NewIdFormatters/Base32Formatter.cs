﻿using System;

namespace Kuno.Utilities.NewId.NewIdFormatters
{
    internal class Base32Formatter :
        INewIdFormatter
    {
        const string LowerCaseChars = "abcdefghijklmnopqrstuvwxyz234567";
        const string UpperCaseChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ234567";

        readonly string _chars;

        public Base32Formatter(bool upperCase = false)
        {
            _chars = upperCase ? UpperCaseChars : LowerCaseChars;
        }

        public Base32Formatter(string chars)
        {
            if (chars.Length != 32)
                throw new ArgumentException("The character string must be exactly 32 characters");

            _chars = chars;
        }

        public string Format(byte[] bytes)
        {
            var result = new char[26];

            int offset = 0;
            for (int i = 0; i < 3; i++)
            {
                int indexed = i * 5;
                long number = bytes[indexed] << 12 | bytes[indexed + 1] << 4 | bytes[indexed + 2] >> 4;
                ConvertLongToBase32(result, offset, number, 4, _chars);

                offset += 4;

                number = (bytes[indexed + 2] & 0xf) << 16 | bytes[indexed + 3] << 8 | bytes[indexed + 4];
                ConvertLongToBase32(result, offset, number, 4, _chars);

                offset += 4;
            }

            ConvertLongToBase32(result, offset, bytes[15], 2, _chars);

            return new string(result, 0, 26);
        }

        static void ConvertLongToBase32(char[] buffer, int offset, long value, int count, string chars)
        {
            for (int i = count - 1; i >= 0; i--)
            {
                var index = (int)(value % 32);
                buffer[offset + i] = chars[index];
                value /= 32;
            }
        }
    }
}