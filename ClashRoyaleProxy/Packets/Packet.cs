﻿using System;
using System.Text;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;

namespace ClashRoyaleProxy
{
    class Packet : IDisposable
    {
        private byte[] rawPacket;
        private int packetID;
        private int payloadLen;
        private int messageVer;
        private byte[] payload;
        private string packetType;
        private byte[] encryptedPayload;
        private byte[] decryptedPayload;
        private DataDestination destination;

        public Packet(byte[] buf, DataDestination d)
        {
            using (var PacketReader = new BinaryReader(new MemoryStream(buf)))
            {           
                this.rawPacket = buf;
                this.destination = d;
                this.packetID = PacketReader.ReadUShortWithEndian();
                var tmp = PacketReader.ReadBytes(3);
                this.payloadLen = ((0x00 << 24) | (tmp[0] << 16) | (tmp[1] << 8) | tmp[2]);
                this.messageVer = PacketReader.ReadUShortWithEndian();
                this.payload = PacketReader.ReadBytes(this.payloadLen);
                this.packetType = PacketType.GetPacketType(packetID);
                PacketReader.Close();
            }

            // En/Decrypt payload
            this.decryptedPayload = EnDecrypt.DecryptPacket(this);
            this.encryptedPayload = EnDecrypt.EncryptPacket(this);
        }

        /// <summary>
        /// Raw, encrypted packet (header included)
        /// 7 byte header + n byte payload
        /// Reverse() because of little endian byte order
        /// </summary>
        public byte[] Raw
        {
            get
            {
                List<Byte> builtPacket = new List<Byte>();
                builtPacket.AddRange(BitConverter.GetBytes(this.packetID).Reverse().Skip(2));
                builtPacket.AddRange(BitConverter.GetBytes(this.encryptedPayload.Length).Reverse().Skip(1));
                builtPacket.AddRange(BitConverter.GetBytes(this.messageVer).Reverse().Skip(2));
                builtPacket.AddRange(this.encryptedPayload);

                return builtPacket.ToArray();
            }
        }

        /// <summary>
        /// Self-explaining.
        /// 10100, 20100, 10101, 20104 [...]
        /// </summary>
        public int ID
        {
            get
            {
                return this.packetID;
            }
        }

        /// <summary>
        /// 2 bytes nobody has exact info about.
        /// </summary>
        public int MessageVersion
        {
            get
            {
                return this.messageVer;
            }
        }
        /// <summary>
        /// String representation according to the ID.
        /// 10100 => LoginMessage
        /// 10108 => KeepAlive
        /// </summary>
        public string Type
        {
            get
            {
                return this.packetType;
            }
        }

        /// <summary>
        /// Destination. Either client or server.
        /// Admittedly, the Substring method is pretty nasty.
        /// </summary>
        public DataDestination Destination
        {
            get
            {
                return this.destination;
            }
        }

        /// <summary>
        /// Normal payload from the received packet.
        /// </summary>
        public byte[] Payload
        {
            get
            {
                return this.payload;
            }
        }
        /// <summary>
        /// Encrypted payload by <seealso cref="EnDecrypt.EncryptPacket(Packet)"/>
        /// </summary>
        public byte[] EncryptedPayload
        {
            get
            {
                return this.encryptedPayload;
            }
        }

        /// <summary>
        /// Decrypted payload by <seealso cref="EnDecrypt.DecryptPacket(Packet)"/>
        /// </summary>
        public byte[] DecryptedPayload
        {
            get
            {
                return this.decryptedPayload;
            }
        }

        /// <summary>
        /// Returns packet info; Used for debugging
        /// </summary>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Destination: " + Destination);
            sb.AppendLine("ID: " + ID);
            sb.AppendLine("Type: " + Type);
            sb.AppendLine("PayloadLen: " + DecryptedPayload.Length);
            sb.AppendLine("Payload: " + Encoding.UTF8.GetString(DecryptedPayload));
            return sb.ToString();
        }

        /// <summary>
        /// Memory-friendly dispose method
        /// </summary>
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}