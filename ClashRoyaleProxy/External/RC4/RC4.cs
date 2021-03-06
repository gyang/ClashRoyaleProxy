﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClashRoyaleProxy
{
    class RC4
    {
        /// <summary>
        /// Encrypts a byte array with RC4
        /// </summary>
        /// <param name="RC4_Key">The public RC4 key</param>
        /// <param name="data">The data to encrypt</param>
        /// <returns>Encrypted data</returns>
        public static byte[] Encrypt(byte[] RC4_Key, byte[] data)
        {
            int a, i, j, k, tmp;
            int[] key, box;
            byte[] cipher;

            key = new int[256];
            box = new int[256];
            cipher = new byte[data.Length];

            for (i = 0; i < 256; i++)
            {
                key[i] = RC4_Key[i % RC4_Key.Length];
                box[i] = i;
            }
            for (j = i = 0; i < 256; i++)
            {
                j = (j + box[i] + key[i]) % 256;
                tmp = box[i];
                box[i] = box[j];
                box[j] = tmp;
            }
            for (a = j = i = 0; i < data.Length; i++)
            {
                a++;
                a %= 256;
                j += box[a];
                j %= 256;
                tmp = box[a];
                box[a] = box[j];
                box[j] = tmp;
                k = box[((box[a] + box[j]) % 256)];
                cipher[i] = (byte)(data[i] ^ k);
            }
            return cipher;
        }

        /// <summary>
        /// Decrypts a byte array with RC4
        /// </summary>
        /// <param name="RC4_Key">The public RC4 key</param>
        /// <param name="data">The data to decrypt</param>
        /// <returns>Decrypted data</returns>
        public static byte[] Decrypt(byte[] RC4_Key, byte[] data)
        {
            return Encrypt(RC4_Key, data);
        }
    }
}
