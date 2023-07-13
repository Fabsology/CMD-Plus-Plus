using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CMD__
{
    internal class Program
    {

        //public static string outputBuffer = "";
        public static char[] numerals = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' , '░', '║', '═', '╚', '╔', '╣', '╗', '╝', '\\' };
        public static char[] special = { '(', ')', '[', ']', '\"', '\'', '<', '>', '.', '_', '#' };
        public static char[] backgrounds = { '\t'};
        public static int buffer = 0;
        private static MouseEventHandler mouseWheel;




        static void Main(string[] args)
        {

            bool running = true;
            while(running) {

                header();
                ConsoleColor backgroundcolor = Console.BackgroundColor;
                ConsoleColor foregroundcolor = Console.ForegroundColor;

                Console.ForegroundColor = ConsoleColor.Green;
                ColorfulOutput(" "+ System.IO.Directory.GetCurrentDirectory() + "\n");

                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("╓");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write(System.Security.Principal.WindowsIdentity.GetCurrent().Name);
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine(DateTime.Now.ToString(" dd/MM/yyyy h:mm "));
                Console.ForegroundColor = ConsoleColor.Red;
                Console.BackgroundColor = backgroundcolor;
                Console.Write("╙>");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("\\");
                Console.BackgroundColor = backgroundcolor;
                Console.ForegroundColor = ConsoleColor.Cyan;
                string COMMAND = Console.ReadLine();

                Console.BackgroundColor = backgroundcolor;
                Console.ForegroundColor = foregroundcolor;

                ColorfulOutput("  ╔".PadRight(COMMAND.Length +3, '═') + "╗\n");
                ColorfulOutput("╔═╣" + COMMAND + "║\n");

                ColorfulOutput("║ ╚".PadRight(COMMAND.Length + 3, '═') + "╝\n");
                if (COMMAND == "exit")
                {
                    running = false;
                }
                else if (COMMAND.StartsWith("ls"))
                {
                    dirplusplus.Run(COMMAND.Substring(2).Split(' '));
                }
                else if (COMMAND.StartsWith("help"))
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    ColorfulOutput("║░ CMD++ COMMANDS\n");
                    Console.ForegroundColor = ConsoleColor.Green;
                    ColorfulOutput("║░   edit FILENAME");
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    ColorfulOutput("    Edit A File\n");
                    Console.ForegroundColor = ConsoleColor.Green;
                    ColorfulOutput("║░   ls");
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    ColorfulOutput("               List Content Of folder\n");
                    Console.ForegroundColor = foregroundcolor;
                    ColorfulOutput("║░ ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~\n");

                }
                else if (COMMAND.StartsWith("cd "))
                {
                    System.IO.Directory.SetCurrentDirectory(System.IO.Directory.GetCurrentDirectory() + "\\" + COMMAND.Substring(3));
                    Console.WriteLine("Directory:  " + System.IO.Directory.GetCurrentDirectory());
                }
                else if (COMMAND.StartsWith("edit "))
                {
                    string filename = COMMAND.Substring(5);


                    if (System.IO.File.Exists(System.IO.Directory.GetCurrentDirectory() + "\\" + filename))
                    {
                        int bh = Console.BufferHeight;
                        int bw = Console.BufferWidth;
                        int sh = Console.WindowHeight;
                        int sw = Console.WindowWidth;

                        Console.SetWindowSize(120, 40);
                        Console.SetBufferSize(120, 40);

                        editor edit = new editor();
                        edit.Editor(System.IO.Directory.GetCurrentDirectory() + "\\" + filename);
                        Console.SetBufferSize(bw, bh);
                        Console.SetWindowSize(sw, sh);
                    } else
                    {
                        try
                        {
                            System.IO.File.WriteAllText(filename, "YOUR CONTENT HERE");
                            int bh = Console.BufferHeight;
                            int bw = Console.BufferWidth;
                            int sh = Console.WindowHeight;
                            int sw = Console.WindowWidth;

                            Console.SetWindowSize(120, 40);
                            Console.SetBufferSize(120, 40);
                            editor edit = new editor();
                            edit.Editor(System.IO.Directory.GetCurrentDirectory() + "\\" + filename);
                            Console.SetBufferSize(bw, bh);
                            Console.SetWindowSize(sw, sh);

                        }
                        catch (Exception e)
                        {
                            ERRORLOG(e.Message);
                        }
                    }
                }
                    var process = new Process()
                {
                    StartInfo = new ProcessStartInfo("cmd")
                    {
                        UseShellExecute = false,
                        RedirectStandardInput = true,
                        RedirectStandardOutput = true,
                        CreateNoWindow = true,
                        Arguments = String.Format("/c \"{0}\"", COMMAND),
                    }
                };
                process.OutputDataReceived += (s, e) => ColorfulOutput("║░ " + e.Data + "\n");
                process.Start();
                process.BeginOutputReadLine();
                process.WaitForExit();
                ColorfulOutput("╚".PadRight(System.Console.WindowWidth, '═'));
            }

        }

        public static void header()
        {
            double percent = Convert.ToInt32(Math.Floor(System.Console.WindowWidth / 8.00));

            ConsoleColor backgroundcolor = Console.BackgroundColor;
            ConsoleColor foregroundcolor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.BackgroundColor = ConsoleColor.DarkYellow;
            for (int i = 0; i < percent; i++)
            {
                Console.Write("░");
            }
            for (int i = 0; i < percent; i++)
            {
                Console.Write("▒");
            }
            Console.ForegroundColor = ConsoleColor.Red;
            for (int i = 0; i < percent; i++)
            {
                Console.Write("▓");
            }
            for (int i = 0; i < percent; i++)
            {
                Console.Write("█");
            }
            for (int i = 0; i < percent; i++)
            {
                Console.Write("▓");
            }
            for (int i = 0; i < percent; i++)
            {
                Console.Write("▒");
            }
            for (int i = 0; i < percent*2; i++)
            {
                Console.Write("░");
            }
            Console.BackgroundColor = backgroundcolor;
            Console.ForegroundColor = foregroundcolor;
            Console.Write("\n");
        }


        public static void ERRORLOG(string MESSAGE)
        {
            double percent = Convert.ToInt32(Math.Floor(System.Console.WindowWidth / 8.00));

            ConsoleColor backgroundcolor = Console.BackgroundColor;
            ConsoleColor foregroundcolor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.BackgroundColor = ConsoleColor.Red;
            for (int i = 0; i < percent; i++)
            {
                Console.Write("░");
            }
            for (int i = 0; i < percent; i++)
            {
                Console.Write("▒");
            }
            for (int i = 0; i < percent; i++)
            {
                Console.Write("▓");
            }
            for (int i = 0; i < percent; i++)
            {
                Console.Write("█");
            }
            for (int i = 0; i < percent; i++)
            {
                Console.Write("▓");
            }
            for (int i = 0; i < percent; i++)
            {
                Console.Write("▒");
            }
            for (int i = 0; i < percent * 2; i++)
            {
                Console.Write("░");
            }
            Console.BackgroundColor = backgroundcolor;
            Console.ForegroundColor = foregroundcolor;
            Console.Write("\n");
            Console.WriteLine(MESSAGE);
        }
        static void ColorfulOutput(string outputBuffer)
        {
            ConsoleColor backgroundcolor = Console.BackgroundColor;
            ConsoleColor foregroundcolor = Console.ForegroundColor;
            if (outputBuffer != null) { 
                for (int i = buffer; i < outputBuffer.Length; i++)
                {
                    char c = outputBuffer[i];
                    if (numerals.Contains(c))
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                    }
                    else if (special.Contains(c))
                    {
                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                    }
                    else if (backgrounds.Contains(c))
                    {
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.BackgroundColor = ConsoleColor.DarkYellow;
                    }




                    Console.Write(outputBuffer[i]);
                    Console.ForegroundColor = foregroundcolor;
                    Console.BackgroundColor = backgroundcolor;
                }
            }
            outputBuffer = "";
        }
    }
}
