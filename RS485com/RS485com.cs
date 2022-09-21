using System;
using System.Threading;
using System.IO.Ports;

namespace RS232ComDozimetr
{ 
    class RS232com
    {
        static int Start,Len;
        static RS232Port port = new RS232Port("COM3");
        static void Main(string[] args)
        {
            int k = 0;
            if (args.Length <= 0) { Start = 1; Len = 24; }
            else { Start = int.Parse(args[0]); Len = int.Parse(args[1]); }
            int nomer=0;

            if (!port.Open()) return;

            while (true)
            {
                nomer++;
                for (k = Start; k < Len; k++)
                {
                    port.Write(k,nomer);
                    Thread.Sleep(25);
                }                
            }


        }
    }
}
