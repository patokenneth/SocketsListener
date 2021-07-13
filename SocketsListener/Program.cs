using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace SocketsListener
{
    class Program
    {
        static int Main(string[] args)
        {
            //Console.WriteLine("Hello World!");
            StartListener();
            return 0;
        }

        public static void StartListener()
        {
            IPHostEntry host = Dns.GetHostEntry("localhost");
            IPAddress address = Dns.GetHostAddresses("localhost")[0];

            IPEndPoint endPoint = new IPEndPoint(address, 11500);

            
            try
            {
                Socket listener = new Socket(address.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                listener.Bind(endPoint);
                listener.Listen();

                Console.WriteLine("Listening...");
                Socket handler = listener.Accept();

                string data = null;
                byte[] bytes = null;

                while (true)
                {
                    bytes = new byte[1024];
                    int bytereceived = handler.Receive(bytes);

                    //test array to transform the received test.
                    //byte[] transformedbytes = new byte[2048];

                    //transformedbytes = Encoding.ASCII.GetBytes("Several ", 0, 2);
                    //Array.Copy(bytes, transformedbytes, bytereceived);
                    
                    data += Encoding.ASCII.GetString(bytes, 0, bytereceived);
                    //data += Encoding.ASCII.GetString(transformedbytes, 0, bytereceived+3);

                    if (data.IndexOf("<EOF>") > -1)
                    {
                        break;
                    }

                }

                Console.WriteLine("Data received: {0}", data);

                byte[] msg = Encoding.ASCII.GetBytes(data);
                handler.Send(msg);
                handler.Shutdown(SocketShutdown.Both);
                handler.Close();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                //throw;
            }

            Console.WriteLine("\n Press any key to continue...");
            Console.ReadKey();

        }
    }
}
