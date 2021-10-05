using System;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using ClientServer;
using Microsoft.Win32;

namespace BroadcastMessengerConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Server server = new Server("127.0.0.1",8000);
            server.Start();
            Task task = new Task(()=>server.ConnectionUpdate());
            task.Start();
            while (true)
            {
                //   Mouse.SetCursorPos(0,0);
                if (server.handler.Count != 0)
                {

                    Console.WriteLine("1.Start App\n2.Remove User\n3.Change Client Window Size\n4.Change Client IP");
                    Console.Write("MSG: ");
                    string str = Console.ReadLine();
                    switch (str) {
                        case "1":
                            Console.Write("Enter path:");
                            str = Console.ReadLine();
                            break;
                        case "2":
                            for (int i = 0; i < server.handler.Count; i++) {
                                Console.WriteLine(i + ". " + server.handler[i].socket.RemoteEndPoint.ToString());
                            }
                            Console.WriteLine("Enter index:");
                            str = Console.ReadLine();
                            if (str.Length >= 1 && str.All(c => char.IsDigit(c))) {
                                if (int.Parse(str) >= 0 && int.Parse(str) < server.handler.Count)
                                {
                                    server.RemoveClient(int.Parse(str));
                                }
                            }
                            str = "--end--";
                            break;

                        case "3":
                            Console.WriteLine("Write Width:");
                            var w = Console.ReadLine();
                            Console.WriteLine("Write Height:");
                            var h = Console.ReadLine();

                            RegistryKey key = Registry.CurrentUser;
                            if (w.Length != 0 && h.Length != 0)
                            {
                                if (w.All(c => char.IsDigit(c)) && h.All(c => char.IsDigit(c)))
                                {

                                    if (key.GetSubKeyNames().Contains("ClientData"))
                                    {
                                        RegistryKey KeyData = key.OpenSubKey("ClientData", true);
                                        KeyData.SetValue("Height", w);
                                        KeyData.SetValue("Width", h);

                                        str = "--size " + w + " " + h;
                                        KeyData.Close();
                                    }
                                }
                            }
                            break;
                        case "4":
                            Console.WriteLine("Write new IP:");
                            var ipclient = Console.ReadLine();

                            RegistryKey keyclient = Registry.CurrentUser;

                            if (IPAddress.Parse(ipclient) != null)
                            {

                                if (keyclient.GetSubKeyNames().Contains("ClientData"))
                                {
                                    RegistryKey KeyData = keyclient.OpenSubKey("ClientData", true);
                                    KeyData.SetValue("serverip", ipclient);

                                    str = "--end--";
                                    KeyData.Close();
                                }
                            }
                    



                            break;
                        default:
                            str = "--end--";
                            break;
                    }
                    if (str == "--end--") {
                        continue;
                    }
                    for (int i = 0; i < server.handler.Count; i++)
                    {
                        server.Send(Server.FromStringToBytes(str), i);
                    }
                }
            }
            Console.ReadLine();

            server.Close();
        }




    }




}

public class Mouse {
    [DllImport("user32.dll")]
    public static extern long SetCursorPos(int x, int y);
    [DllImport("user32.dll")]
    public static extern long ClientToScreen(IntPtr hWnd, ref POINT point);
    [DllImport("user32.dll", SetLastError = false)]
    public static extern IntPtr GetDesktopWindow();
    [StructLayout(LayoutKind.Sequential)]
    public struct POINT {
        public int x;
        public int y;
        public POINT(int X,int Y)
        {
            x = X;
            y = Y;

        }
    }


}