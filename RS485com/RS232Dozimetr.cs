using System;
using System.IO.Ports;
using System.Text;

namespace RS232ComDozimetr
{

    public class RS232Port
    {
        public SerialPort ComPort;
        public const int bufsize = 1024;
        public byte[] writeBuffer=new byte[bufsize];
        public int writeBufferLen;
        //public byte[] ReadBuffer = new byte[bufsize];
        //string strl = " | ";

    public RS232Port(String RS232Device)
        {
            ComPort = new SerialPort(RS232Device);            
        }    

        public bool Open()
        {            
            try
            {
                ComPort.BaudRate = 38400;
                ComPort.Parity = Parity.None;
                ComPort.DataBits = 8;
                ComPort.StopBits = StopBits.One;
                ComPort.Open();
                Console.WriteLine("successful.");
                return true;
            }
            catch
            {
                Console.WriteLine("error configuring the rs-485 port.");
                Console.ReadKey();
                return false;
            };
        }
        public void Read()
        {
            Console.Write("Reading from COM Port ");
            try {

                int buferSize = ComPort.ReadBufferSize;
                byte[] readingbyte = new byte[buferSize];

                //if (buferSize > 0) Console.WriteLine("we Received the following bytes:  " + buferSize.ToString() + strl);
                //else /**/ { Console.WriteLine("   We have EMPTY response....." + strl); return; }

                ComPort.Read(readingbyte, 0, readingbyte.Length);
                string reading = Encoding.GetEncoding("Windows-1252").GetString(readingbyte);
            }
            catch(Exception)
            {
                Console.WriteLine("ERROR Read COM Port ");
            }           
        }

        public void Write(int slave, int nomer)
        {

            try
            {

                writeBuffer[0] = (byte)(slave & 0xff);
                writeBuffer[1] = 0x10;
                writeBuffer[2] = 0x0;
                writeBuffer[3] = 0x0;
                //int countbyte = 8;
                int countbyte=formula.ushort2byte(formula.outInteger(nomer, 5, false),writeBuffer,7);                
/*
                writeBuffer[7] = (byte)0;
                writeBuffer[8] = (byte)1;
                writeBuffer[9] = (byte)1;
                writeBuffer[10] = (byte)0;
                writeBuffer[11] = (byte)0x11;
                writeBuffer[12] = (byte)0x03;
                writeBuffer[13] = (byte)0x8;
                writeBuffer[14] = (byte)0x16; 
                */
                int countregs = countbyte / 2;
                writeBuffer[4] = (byte)((countregs>>8) & 0xff);
                writeBuffer[5] = (byte)(countregs  & 0xff);
                writeBuffer[6] = (byte)(countbyte & 0xff);
                int[] crc = CalcCRC.calculateCRC(writeBuffer, 0, countbyte+7);
                writeBuffer[countbyte + 7] = (byte)(crc[0] & 0xff);
                writeBuffer[countbyte + 8] = (byte)(crc[1] & 0xff);

                ComPort.DiscardInBuffer();

                ComPort.Write(writeBuffer, 0, countbyte + 9);
                
            }
            catch
            {
                Console.WriteLine("writing -> COM port failed.");            
            }
            //Console.Write("Writing -> COM Port " + strl);

        }
    }

}
