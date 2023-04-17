using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DmyrychMessenger
{
    static class Container
    {
        static readonly int rsaKeyLength = 2048;//оптимальна довжина ключа для алгоритма RSA
        static readonly int aesBlockSize = 128;//довжина блоку шифрування для AES
        static readonly int aesKeySize = 256;//довжина ключа при генерації ключа AES
        static readonly int port = 45675;//обрано порт який з високою долею ймовірності буде не зайнято


        public static int getPort()
        {
            return port;
        }
        public static int getRSAKeyLength()
        {
            return rsaKeyLength;
        }
        public static int getAESBlockSize()
        {
            return aesBlockSize;
        }
        public static int getAESKeySize()
        {
            return aesKeySize;
        }

    }
}
