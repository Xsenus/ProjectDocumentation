using Core.NET.Interfaces;
using HashLib;
using HashLib.Checksum;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Core.NET.Algorithms
{
    public class CRC32 : IAlgorithm
    {
        private uint[] _table;

        private uint ComputeChecksum(IEnumerable<byte> bytes)
        {
            uint num1 = uint.MaxValue;
            foreach (byte num2 in bytes)
            {
                var index = (num1 & byte.MaxValue ^ num2);
                num1 = num1 >> 8 ^ _table[(int)index];
            }
            return ~num1;
        }

        public IEnumerable<byte> ComputeChecksumBytes(IEnumerable<byte> bytes) => BitConverter.GetBytes(ComputeChecksum(bytes));

        public ComputeHash ComputeHashFile(string path)
        {
            byte[] source = File.ReadAllBytes(path);
            Start();
            uint checksum = ComputeChecksum(((IEnumerable<byte>)source).ToList());
            return new ComputeHash(BitConverter.ToString(((IEnumerable<byte>)BitConverter.GetBytes(checksum)).Reverse().ToArray()).Replace("-", string.Empty), ((IEnumerable<byte>)BitConverter.GetBytes(checksum)).Reverse().ToArray());
        }

        public ComputeHash ComputeHashFile(System.IO.Stream stream)
        {
            byte[] source = Extensions.AppExtensions.ReadStream(stream).ToArray();
            Start();
            uint checksum = ComputeChecksum(((IEnumerable<byte>)source).ToList());
            return new ComputeHash(BitConverter.ToString(((IEnumerable<byte>)BitConverter.GetBytes(checksum)).Reverse().ToArray()).Replace("-", string.Empty), ((IEnumerable<byte>)BitConverter.GetBytes(checksum)).Reverse().ToArray());
        }

        public ComputeHash ComputeHashFile(byte[] data)
        {
            HashResult chars = HashFactory.Checksum.CreateCRC32(CRC32Polynomials.IEEE_802_3).ComputeBytes(data);
            return new ComputeHash(chars.ToString().Reverse().ToString(), chars.GetBytes());
        }

        public ComputeHash ComputeHashString(string str)
        {
            HashResult chars = HashFactory.Checksum.CreateCRC32(CRC32Polynomials.IEEE_802_3).ComputeChars(str.ToCharArray());
            return new ComputeHash(chars.ToString().Reverse().ToString(), chars.GetBytes());
        }

        public ComputeHash ComputeHashString(System.IO.Stream stream)
        {
            return ComputeHashFile(stream);
        }

        public ComputeHash ComputeHashString(byte[] data)
        {
            return ComputeHashFile(data);
        }

        public void Start()
        {
            _table = new uint[256];
            for (uint index1 = 0; index1 < _table.Length; ++index1)
            {
                uint num = index1;
                for (int index2 = 8; index2 > 0; --index2)
                {
                    if (((int)num & 1) == 1)
                        num = num >> 1 ^ 3988292384U;
                    else
                        num >>= 1;
                }
                _table[(int)index1] = num;
            }
        }
    }
}
