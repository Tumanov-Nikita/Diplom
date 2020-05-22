using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DIPLOM.Infrastructure
{
    public static class Encrypting
    {
        public static string ComputeHash(string input)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            Byte[] inputBytes = Encoding.UTF8.GetBytes(input);
            Byte[] hashedBytes = md5.ComputeHash(inputBytes);
            return BitConverter.ToString(hashedBytes);
        }
    }
}
