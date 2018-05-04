using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;
namespace MobileWifi
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {

                WriteLine($"Running at {DateTime.Now},Wifi已经{(IsRunningWifi() ? "开启" : "关闭")}", ForegroundColor = ConsoleColor.Green);
                Write("0.Exit ", ForegroundColor = ConsoleColor.Green);
                Write("1.StartWifi ", ForegroundColor = ConsoleColor.Green);
                Write("2.StopWifi ", ForegroundColor = ConsoleColor.Green);
                Write("3.ShowCilents ", ForegroundColor = ConsoleColor.Green);
                WriteLine();
                ForegroundColor = ConsoleColor.Cyan;
                var b = Int32.TryParse(ReadLine(), out int i);
                if (b)
                {
                    switch (i)
                    {
                        case 0:
                            WriteLine("Bye", ForegroundColor = ConsoleColor.Yellow);
                            System.Threading.Thread.Sleep(1000);
                            return;
                        case 1:
                            CallCMD("netsh wlan start hostednetwork");
                            break;
                        case 2:
                            CallCMD("netsh wlan stop hostednetwork");
                            break;
                        case 3:
                            var s = CallCMD("netsh wlan show hostednetwork", false);
                            string[] striparr = s.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                            striparr = striparr.Where(str => !string.IsNullOrEmpty(str)).ToArray();
                            foreach (var str in striparr)
                            {
                                if (str.Contains("已经过身份验证"))
                                {
                                    WriteLine(str, ForegroundColor = ConsoleColor.White);
                                }
                            }
                            break;
                        default:
                            WriteLine("???", ForegroundColor = ConsoleColor.Red);
                            break;
                    }
                }
                else
                {
                    WriteLine("不合法的输入", ForegroundColor = ConsoleColor.Red);
                }


            }
        }
        public static string CallCMD(string s, bool isWriteLine = true)
        {
            string output = "";
            System.Diagnostics.Process p = new System.Diagnostics.Process();
            p.StartInfo.FileName = "cmd.exe";
            p.StartInfo.UseShellExecute = false;    //是否使用操作系统shell启动
            p.StartInfo.RedirectStandardInput = true;//接受来自调用程序的输入信息
            p.StartInfo.RedirectStandardOutput = true;//由调用程序获取输出信息
            p.StartInfo.RedirectStandardError = true;//重定向标准错误输出
            p.StartInfo.CreateNoWindow = true;//不显示程序窗口
            p.Start();//启动程序

            p.StandardInput.WriteLine(s + "&exit");//向cmd窗口发送输入信息
            p.StandardInput.AutoFlush = true;

            output = p.StandardOutput.ReadToEnd(); //获取cmd窗口的输出信息
            p.WaitForExit();//等待程序执行完退出进程
            p.Close();
            if (isWriteLine)
            {
                string[] striparr = output.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                striparr = striparr.Where(str => !string.IsNullOrEmpty(str)).ToArray();
                foreach (var str in striparr)
                {
                    if (str != "" && !str.Contains("Micro"))
                    {
                        WriteLine(str, ForegroundColor = ConsoleColor.White);
                    }
                }
            }

            return output;
        }
        static bool IsRunningWifi()
        {
            string s = CallCMD("netsh wlan show interfaces", false);
            return !s.Contains("未启动");
        }
    }
}
