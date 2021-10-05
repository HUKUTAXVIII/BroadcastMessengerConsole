using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using ClientServer;
using Microsoft.Win32;

namespace Clients
{
    class Program
    {
        static void Main(string[] args)
        {
            string login = string.Empty;
            string pass = string.Empty;
            string ip = "127.0.0.1";


            RegistryKey key = Registry.CurrentUser;
            //key.DeleteSubKey("ClientData");

            if (key.GetSubKeyNames().Contains("ClientData")) {
                RegistryKey KeyData=key.OpenSubKey("ClientData");
                var h = int.Parse(KeyData.GetValue("Height").ToString());
                var w = int.Parse(KeyData.GetValue("Width").ToString());
                ip = KeyData.GetValue("serverip").ToString();
                Console.SetWindowSize(w , h);
                KeyData.Close();
            }
            else{
                RegistryKey newKeyData = key.CreateSubKey("ClientData", true);

                newKeyData.SetValue("Height",Console.WindowHeight);
                newKeyData.SetValue("Width", Console.WindowWidth);
                newKeyData.SetValue("serverip", "127.0.0.1");
                newKeyData.Close();
            }


            Client client = new Client(ip,8000);
            client.Connect();
            client.Send(Client.FromStringToBytes(client.ToString()));
            while (true)
            {
                var data = Client.FromBytesToString(client.Get());
                if (data.Split(" ").Length >= 2) {
                    var components = data.Split(" ");
                    if (components[0] == "--start") {
                        var reference = data.Remove(0,components[0].Length);
                        Process.Start(reference.ToString());
                        continue;
                    }
                   
                    if (components[0] == "--size")
                    {

                        Console.SetWindowSize(int.Parse(components[1].ToString()), int.Parse(components[2].ToString()));
                        continue;
                    }
                }
                Console.WriteLine(data);
            }
            Console.ReadLine();

            client.Close();
        }
    }
    public class Mouse
    {
        [DllImport("user32.dll")]
        public static extern long SetCursorPos(int x, int y);
        [DllImport("user32.dll")]
        public static extern long ClientToScreen(IntPtr hWnd, ref POINT point);
        [DllImport("user32.dll", SetLastError = false)]
        public static extern IntPtr GetDesktopWindow();
        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int x;
            public int y;
            public POINT(int X, int Y)
            {
                x = X;
                y = Y;

            }
        }


    }
}
