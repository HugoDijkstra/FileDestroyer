using System;
using Microsoft.Win32;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileDestroyer
{
    public static class ArrayExtensions
    {
        public static void Fill<T>(this T[] originalArray, T with)
        {
            for (int i = 0; i < originalArray.Length; i++)
            {
                originalArray[i] = with;
            }
        }
    }

    class Program
    {
        public static List<String> DirSearch(string sDir)
        {
            List<String> files = new List<String>();
            try
            {
                foreach (string f in Directory.GetFiles(sDir))
                {
                    files.Add(f);
                }
                foreach (string d in Directory.GetDirectories(sDir))
                {
                    files.AddRange(DirSearch(d));
                }
            }
            catch (System.Exception excpt)
            {
                Console.Write(excpt.Message);
            }

            return files;
        }
        static string path;
        static void Main(string[] args)
        {
            try
            {
                if (args.Count() == 1)
                {
                    if (File.Exists(args[0]))
                    {
                        deleteSecure(new string[] { args[0] });
                        Console.Write("File deleted");
                        return;
                    }
                    else if(Directory.Exists(args[0]))
                    {
                        deleteSecure(DirSearch(args[0]).ToArray());
                        Console.Write("Directory emptied");
                        return;
                    }
                }
                for (int i = 0; i < args.Length; i++)
                {
                    switch (args[i])
                    {
                        case "-help":
                            Console.Write(
                            "------------FileDestroyer------------\n" +
                            "-help:		Show all Commands\n" +
                            "-setPath:	Set path of Temp folder\n" +
                            "       Usage: -setPath \"D:/temp\"\n" +
                             "-Secure:	Delete securly\n" +
                            "-------------------------------------");
                            return;
                        case "-setPath":
                            RegistryKey skey = Registry.CurrentUser.OpenSubKey("Software/FileDeleter", true);
                            if (skey == null)
                            {
                                skey = Registry.CurrentUser.CreateSubKey("Software/FileDeleter", true);
                            }
                            i++;
                            try
                            {
                                skey.SetValue("path", args[i]);
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e.ToString());
                            }
                            Console.WriteLine("Path set to: " + args[i]);
                            if (!Directory.Exists(args[i]))
                            {
                                Directory.CreateDirectory(args[i]);
                                Console.WriteLine("tmp folder created: " + args[i]);
                            }
                            return;
                        case "-Secure":
                            path = "C:/tmp/";
                            RegistryKey akey = Registry.CurrentUser.OpenSubKey("Software/FileDeleter", true);
                            if (akey == null)
                            {
                                akey = Registry.CurrentUser.CreateSubKey("Software/FileDeleter", true);
                            }
                            try
                            {
                                if (akey.GetValue("path") == null)
                                    akey.SetValue("path", path);
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e.ToString());
                            }
                            try
                            {
                                string s = akey.GetValue("path") as string;
                                if (s == null)
                                {
                                    akey.SetValue("path", "C:/tmp/");
                                }
                                else
                                {
                                    path = s;
                                }
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e.ToString());
                            }

                            if (!Directory.Exists(path))
                            {
                                Directory.CreateDirectory(path);
                                Console.WriteLine("tmp folder created");
                            }
                            else
                            {
                                string[] files = DirSearch(path).ToArray();
                                string[] directories = Directory.GetDirectories(path);
                                foreach (string s in files)
                                {
                                    Console.WriteLine("Delete file: " + s + "?");
                                }
                                foreach (string s in directories)
                                {
                                    Console.WriteLine("Delete Directory: " + s + "?");
                                }
                                if (files.Count() == 0 && directories.Count() == 0)
                                    return;
                                Console.WriteLine("Do you want to DELETE these files\n[Y]es or [N]o");
                                string awnser = Console.ReadLine();
                                switch (awnser)
                                {
                                    case "Y":
                                        deleteSecure(files);
                                        deleteFolders(directories);
                                        break;
                                    case "y":
                                        deleteSecure(files);
                                        deleteFolders(directories);
                                        break;
                                    case "Yes":
                                        deleteSecure(files);
                                        deleteFolders(directories);
                                        break;
                                    case "yes":
                                        deleteSecure(files);
                                        deleteFolders(directories);
                                        break;
                                    default:
                                        break;
                                }
                            }
                            return;
                    }
                }

                //get path
                RegistryKey key = Registry.CurrentUser.OpenSubKey("Software/FileDeleter", true);
                if (key == null)
                {
                    key = Registry.CurrentUser.CreateSubKey("Software/FileDeleter", true);
                }
                try
                {
                    if (key.GetValue("path") == null)
                        key.SetValue("path", path);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
                try
                {
                    string s = key.GetValue("path") as string;
                    if (s == null)
                    {
                        key.SetValue("path", "C:/tmp/");
                    }
                    else
                    {
                        path = s;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                    Console.WriteLine("tmp folder created");
                }
                else
                {
                    string[] files = DirSearch(path).ToArray();
                    string[] directories = Directory.GetDirectories(path);
                    foreach (string s in files)
                    {
                        Console.WriteLine("Delete file: " + s + "?");
                    }
                    foreach (string s in directories)
                    {
                        Console.WriteLine("Delete Directory: " + s + "?");
                    }
                    if (files.Count() == 0 && directories.Count() == 0)
                        return;
                    Console.WriteLine("Do you want to DELETE these files\n[Y]es or [N]o");
                    string awnser = Console.ReadLine();
                    switch (awnser)
                    {
                        case "Y":
                            delete(files);
                            deleteFolders(directories);
                            break;
                        case "y":
                            delete(files);
                            deleteFolders(directories);
                            break;
                        case "Yes":
                            delete(files);
                            deleteFolders(directories);
                            break;
                        case "yes":
                            delete(files);
                            deleteFolders(directories);
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception e)
            {
                Console.Write("Send this to the Devoloper @: hugodijkstra@live.nl\n\n\n\n" + e.ToString());
            }
        }

        static void delete(string[] files)
        {
            foreach (string f in files)
            {
                File.Delete(f);
                Console.WriteLine("Deleted " + f);
            }
        }

        static void deleteSecure(string[] files)
        {
            try
            {
                foreach (string f in files)
                {
                    FileInfo info = new FileInfo(f);
                    Byte[] byteArray = new Byte[info.Length];
                    byteArray.Fill((Byte)0x1);
                    File.WriteAllBytes(f, byteArray);
                    File.Delete(f);
                    Console.WriteLine("Deleted " + f);
                }
            }
            catch
            {

            }
        }

        static void deleteFolders(string[] directories)
        {
            foreach (string dir in directories)
            {
                Console.WriteLine(Directory.GetAccessControl(dir).ToString());
                Directory.Delete(dir, true);
            }
        }
    }
}
