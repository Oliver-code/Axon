using System.Diagnostics;

public class Forcecrash : Exception
{

    public Forcecrash() : base("Program was force terminated by user")
    {

    }

}

class Program
{

    static bool IsFullPath(string path)
    {

        if (!path.Contains('\\'))
        {

            return false;

        }

        else
        {

            return true;

        }

    }

    static void Bluescreen(Exception ex)
    {


        Console.BackgroundColor = ConsoleColor.Blue;
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.White;
        DateTime datetime = DateTime.Now;
        Console.WriteLine($"An error has occurred, crash data is being collected and can be accessed by using the 'crash dumpview' command");
        File.WriteAllText("Reg\\crashdump\\crashdump.dump", $"Exeption: \n{ex.Message} \n\n\nStack Trace: \n{ex.StackTrace} \nTime & date: {datetime}");
        Console.Write("Press any key to exit . . . ");
        Console.ReadKey();
        Environment.Exit(0);

    }

    static string Splitfirst(string input)
    {
        int index = input.IndexOf(' ');

        return input.Substring(0, index);
    }

    static string Splitsecond(string input)
    {
        int index = input.IndexOf(' ');
        return input.Substring(index + 1);
    }

    static bool firstboot = true;
    static string syspath = "C:\\";
    static string[] restart = { "restart" };
    static void Main(string[] args)
    {
        if (args.Length <= 0)
        {

            goto restart;

        }
        string arg = string.Join(null, args);

        if (arg == "restart")
        {

            goto restart;

        }

        if (!Directory.Exists(arg))
        {
            goto restart;
        }   

        syspath = arg;

        restart:

        if (firstboot)
        {
            Console.Title = "Axon";
            Console.WriteLine("Welcome to Axon! version 0.8");
            firstboot = false;

        }

        Console.Write(Path.Combine(syspath, ">"));
        string? input = Console.ReadLine();
        string? inp = input?.ToLower();

        if (inp == "help")
        {

            Console.WriteLine("about \ndir \ncd <folder> \necho \ncls \ncrash <command> \ntaskstart <application> \nforcecrash");
            Main(restart);



        }

        if (inp == "tasklist")
        {
            Process[] processes = Process.GetProcesses();
            foreach (Process process in processes)
            {
                Console.WriteLine($"Process Name: {process.ProcessName}");
            }
            Main(restart);
        }

        if (inp == "cls")
        {
            Console.Clear();
            Main(restart);
        }

        if  (inp == "about")
        {
            Console.WriteLine("Version: 0.8 \nAxon is a simple command line interface that is written in C# and is open source");
            

            Main(restart);
        }

        if (inp == "dir")
        {

            try
            {



                if (!Directory.Exists(syspath))
                {

                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("There has been a error and the path is not valid. enter the new path below");

                    Console.ForegroundColor = ConsoleColor.White;
                    syspath = Console.ReadLine();
                    if (!syspath.EndsWith('\\'))
                    {
                        syspath = $"{syspath}\\";
                    }


                    Main(restart);

                }

                DirectoryInfo[] folders = new DirectoryInfo(syspath).GetDirectories();
                FileInfo[] files = new DirectoryInfo(syspath).GetFiles();

                foreach (DirectoryInfo folder in folders)
                {



                    Console.WriteLine($"Directory: {folder.Name}");

                }

                foreach (FileInfo file in files)
                {

                    int filekb = Convert.ToInt32(file.Length / 1024);

                    if (file.Length > 1024 && filekb > 1000)
                    {
                        Console.WriteLine($"File: {file.Name}, Size: {filekb} KB");
                        goto end;
                    }

                    if (file.Length / 1024 > 1000)
                    {

                        long filemb = file.Length / 1024 / 1000;

                        Console.WriteLine($"File: {file.Name}, Size: {filemb} MB");
                        goto end;
                    }

                    else
                    {
                        Console.WriteLine($"File: {file.Name}, Size: {file.Length} Bytes");
                        goto end;
                    }


                end:;


                }

                Main(restart);
            }

            catch (Exception ex)
            {
                Bluescreen(ex);
            }

        }

        if (inp == "")
        {
            Main(restart);
        }

        if (inp == "forcecrash")
        {

            try
            {

                throw new Forcecrash();

            }

            catch(Forcecrash ex)
            {

                Bluescreen(ex);

            }

        }

        if (inp == "exit")
        {

            Environment.Exit(0);

        }


        if (input.Contains(' '))
        {
            string com = Splitfirst(input).ToLower();
            string exp = Splitsecond(input);

            if (com == "echo")
            {

                Console.WriteLine($"{exp} \n");
                Main(restart);
            }
            
            if (com == "cd")
            {

                if (exp.Contains(':'))
                {

                    DriveInfo[] drives = DriveInfo.GetDrives();


                    foreach (DriveInfo drive in drives)
                    {
                        if (drive.Name.Equals(exp, StringComparison.OrdinalIgnoreCase))
                        {
                            syspath = drive.Name;
                            Main(restart);
                        }
                    }
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Drive does not exist");
                    Console.ResetColor();
                    Main(restart);
                } 

                if (exp.Contains(".."))
                {
                    string dir = Path.Combine(syspath, "..");

                    string[] parts = dir.Split('\\');


                    try
                    {


                        if (parts[1] == "..")
                        {

                            Main(restart);

                        }




                        Array.Resize(ref parts, parts.Length - 2);

                        syspath = string.Join("\\", parts);

                        Main(restart);

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("An error occurred: " + ex.Message);
                        Main(restart);
                    }


                }

                if (!Directory.Exists($"{syspath}\\{exp}"))
                {

                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Directory does not exist");
                    Console.ResetColor();
                    Main(restart);

                }

                syspath = syspath + exp + '\\';
                Main(restart);

            }
            
            if (com == "taskstart")
            {

                if (IsFullPath(exp))
                {
                    if (!File.Exists(exp))
                    {

                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Application does not exist");
                        Console.ResetColor();
                        Main(restart);
                    }

                    Process.Start(exp);
                    Main(restart);

                }

                else
                {

                    if (!File.Exists($"{syspath}\\{exp}"))
                    {

                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Application does not exist");
                        Console.ResetColor();
                        Main(restart);
                    }
                    Process.Start($"{syspath}\\{exp}");
                    Main(restart);
                }

            }

            if (com == "crash")
            {

                if (exp == "dumpview")
                {

                    if (!File.Exists("Reg\\crashdump\\crashdump.dump"))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("No crashdump data has been found");
                        Console.ResetColor();
                        Main(restart);
                    }

                    string[] data = File.ReadAllLines("Reg\\crashdump\\crashdump.dump");
                    foreach (string line in data)
                    {

                        Console.WriteLine(line);

                    }
                    Main(restart);
                   
                }

            }
            
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Command '{input}' does not exist");
                Console.ResetColor();
                Main(restart);


            }

        }

        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Command '{input}' does not exist");
            Console.ResetColor();
            Main(restart);


        }
    }
}