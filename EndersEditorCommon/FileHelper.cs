using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;

namespace EndersEditorCommon
{
    /// <summary>
    /// Helpful methods for dealing with files
    /// </summary>
    public static class FileHelper
    {
        /// <summary>
        /// Get the directory where this assembly resides
        /// </summary>
        public static string GetAssemblyDirectory()
        {
            return Path.GetDirectoryName(Path.GetFullPath(Assembly.GetExecutingAssembly().Location));
        }

        /// <summary>
        /// Compute MD5 checksum of a given string
        /// </summary>
        public static string ComputerStringHash(string Value)
        {
            System.Security.Cryptography.MD5CryptoServiceProvider x = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] data = System.Text.Encoding.ASCII.GetBytes(Value);
            data = x.ComputeHash(data);
            string ret = "";
            for (int i = 0; i < data.Length; i++)
                ret += data[i].ToString("x2").ToLower();
            return ret;
        }
    }
}
