using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RS232ComDozimetr
{
    class formula
    {
        static public int ushort2byte(ushort[] mas,byte[] buf, int pos)
        {
            int startpos = pos;
            for (int i = 0; i < mas.Length; i++)
            {
                buf[pos++] = (byte)((mas[i] >> 8) & 0xff);
                buf[pos++] = (byte)(mas[i] & 0xff);
            }
            return pos - startpos;
        } 
        static public ushort[] outInteger(int inInt, int len, bool blink)
        {
            ushort[] rez;
            byte[] rbyte;
            string sres = "0";
            if (len == 5) sres = inInt.ToString("00000");
            if (len == 2) sres = inInt.ToString("00");
            if (len == 1) sres = inInt.ToString("0");
            rbyte = convertToDisplay(sres);
            rez = upak(rbyte, blink);
            return rez;
        }
        static public ushort[] outTimer(int thr, int tmm, int tsec)
        {
            ushort[] rez;
            byte[] rbyte;
            string sres = String.Format("{0:00}:{1:00}:{2:00}", thr, tmm, tsec);
            rbyte = convertToDisplay(sres);
            rez = upak(rbyte, false);
            return rez;
        }


        static private ushort[] upak(byte[] rbyte, bool blink)
        {
            int len = (rbyte.Length % 2 == 0) ? rbyte.Length / 2 : (rbyte.Length / 2) + 1;
            ushort[] rez = new ushort[++len];
            rez[0] = (ushort)(blink ? 0 : 1);
            int j = 1;
            for (int i = 0; i < rbyte.Length; i++)
            {
                rez[j] = (ushort)((i % 2) == 0 ? (rez[j] | (rbyte[i] << 8)) : (rez[j] | (rbyte[i])));
                j += (i % 2) == 0 ? 0 : 1;
            }
            return rez;
        }

        static public byte[] convertToDisplay(string sres)
        {
            bool point = (sres.IndexOf(".") > 0) || (sres.IndexOf(",") > 0);

            byte[] rez = new byte[sres.Length - (point ? 1 : 0)];
            int j = 0;
            for (int i = 0; i < sres.Length; i++)
            {
                byte b = 0xf;
                switch (sres[i])
                {
                    case '0': b = 0; break;
                    case '1': b = 1; break;
                    case '2': b = 2; break;
                    case '3': b = 3; break;
                    case '4': b = 4; break;
                    case '5': b = 5; break;
                    case '6': b = 6; break;
                    case '7': b = 7; break;
                    case '8': b = 8; break;
                    case '9': b = 9; break;
                    case ':': b = 0x2f; break;
                    case ' ': b = 0x7f; break;
                    case ',': b = 0xff; break;
                    case '.': b = 0xff; break;
                }
                if (b != 0xff)
                {
                    rez[j++] = b;
                }
                else rez[j - 1] = (byte)(rez[j - 1] | 0x40);
            }
            return rez;
        }
    }
}
