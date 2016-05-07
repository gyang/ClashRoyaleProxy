using System;

namespace ClashRoyaleProxy
{
    class Keys
    {
        /// <summary>
        /// The generated private key, according to the modded public key.
        /// </summary>
        public static byte[] GeneratedPrivateKey
        {
            get
            {
                return Helper.HexToByteArray("1891d401fadb51d25d3a9174d472a9f691a45b974285d47729c45c6538070d85");
            }
        }

        /// <summary>
        /// The modded Clash Royale public key.
        /// Offset 0x0039A01C [ARM / ANDROID]
        /// </summary>
        public static byte[] ModdedPublicKey
        {
            get
            {
                return Helper.HexToByteArray("72f1a4a4c48e44da0c42310f800e96624e6dc6a641a9d41c3b5039d8dfadc27e");
            }
        }

        /// <summary>
        /// The original, unmodified Clash Royale public key.
        /// Offset 0x0039A01C [ARM / ANDROID]
        /// </summary>
        public static byte[] OriginalPublicKey
        {
            get
            {
                return Helper.HexToByteArray("2971520277bb38734fc36e2a0d95e76e969379a5372a44c1b2fd3c1766b0016a");

            }
        }

        /// <summary>
        /// An old RC4 nonce used by Supercell.
        /// </summary>
        public static byte[] RC4_Nonce
        {
            get
            {
                return System.Text.Encoding.UTF8.GetBytes("nonce");
            }
        }
    }
}
