using System;
using System.IO.Ports;

namespace RS485com
{
    public class Rs485Read
    {
        private const string Rs485Device = "COM1";
        private const int ReadBufferSize = 1000;
        private static byte[] ReadBuffer = new byte[ReadBufferSize];

        public int Read()
        {
            SerialPort serialPort = new SerialPort(Rs485Device);
            
                /* try to open the port */
                Console.Write("opening device {0}...", Rs485Device);
                try { serialPort.Open(); }
                catch
                {
                    Console.WriteLine("failed Read Com port.");
                    Console.ReadKey();
                    return -1;
                }

                Console.WriteLine("successful.");

                /* configure the port */
                try { serialPort.BaudRate = 38400; }
                catch
                {
                    Console.WriteLine("error configuring the rs-485 port.");
                    Console.ReadKey();
                    return -2;
                };

                
                Console.WriteLine("start reading from the rs-485 port a maximum of {0} bytes", ReadBufferSize);
                int readByte, i = 0;
                while ((readByte = serialPort.ReadByte()) != -1 &&  /* try to read only 1 byte at once */
                        i < ReadBufferSize)                         /* read until the buffer is full */
                {
                    ReadBuffer[i++] = (byte)readByte; /* store it in the read buffer */
                }
                                
                Console.WriteLine("we received the following bytes:");
                for (i = 0; i < ReadBufferSize; i++)
                {
                    //Console.Write("[{0}]: 0x{1:X2}", i, ReadBuffer[i]);
                    if (ReadBuffer[i] >= 32 && ReadBuffer[i] <= 126);   /* if the character is a printable ascii code, print it to the stdout */
                        Console.Write(" - '{0}'", (char)ReadBuffer[i]);                    
                }

            Console.WriteLine();           

            //Console.WriteLine("Read byte: "+ReadBuffer.ToString());
            Console.WriteLine("closing the device again");
            Console.ReadKey();
            return 0;
        }
    }


    public class Rs485Write
    {
        private const string Rs485Device = "COM1";
        byte[] buffer = new byte[8]{ 13, 10, 40, 58, 158, 89, 13, 23 };

        public int Write()
        {
            SerialPort serialPort = new SerialPort(Rs485Device);
            
                Console.Write(string.Format("opening device {0}...", Rs485Device));
                                
                try { serialPort.Open(); }
                catch
                {
                    Console.WriteLine("failed.");
                    Console.ReadKey();
                    return -1;
                }
                Console.WriteLine("successful.");

                /* configure the port */
                try { serialPort.BaudRate = 38400; }
                catch
                {
                    Console.WriteLine("error configuring the rs-485 port.");
                    Console.ReadKey();
                    return -2;
                };
                
                try {
                    serialPort.Read(buffer,0,buffer.Length);                    
                }
                catch
                {
                    Console.WriteLine("writing to the serial port failed.");
                    return -3;
                }

                /* close the device again (end of 'using' statement) */
                Console.WriteLine("closing the device again");
            return 0;
        }
    }



    class Program
    {
        static void Main(string[] args)
        {
            Rs485Write w485 = new Rs485Write();
            Rs485Read r485 = new Rs485Read();

            w485.Write();
            r485.Read();


        }
    }
}
