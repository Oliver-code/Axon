using System.Diagnostics;
class Program
{

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
            Console.WriteLine("Welcome to Axon! version 0.6");
            firstboot = false;

        }

        Console.Write(Path.Combine(syspath, ">"));
        string? input = Console.ReadLine();
        string? inp = input?.ToLower();

        if (inp == "help")
        {

            Console.WriteLine("about \ndir \ncd <folder> \necho \ncls");
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
            Console.WriteLine("Version: 0.6 \nAxon is a simple command line interface that is written in C# and is open source");
            Main(restart);
        }

        if (inp == "dir")
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

        if (inp == "")
        {
            Main(restart);
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