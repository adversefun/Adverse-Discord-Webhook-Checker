using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.IO;


namespace AdverseWebhookChecker
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Discord Webhook Checker.  I  https://github.com/drippinn ";
            Console.ForegroundColor = ConsoleColor.DarkCyan;

            string text = @"   ___     __                    ";
            string text2 = @"  / _ |___/ /  _____ _______ ___ ";
            string text3 = @" / __ / _  / |/ / -_) __(_-</ -_)";
            string text4 = @"/_/ |_\_,_/|___/\__/_/ /___/\__/ ";
            string text5 = "Free Discord Webhook Checker";



            Console.WriteLine("");
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine(string.Format("{0," + (Console.WindowWidth / 2 + text.Length / 2).ToString() + "}", text));
            Console.WriteLine(string.Format("{0," + (Console.WindowWidth / 2 + text2.Length / 2).ToString() + "}", text2));
            Console.WriteLine(string.Format("{0," + (Console.WindowWidth / 2 + text3.Length / 2).ToString() + "}", text3));
            Console.WriteLine(string.Format("{0," + (Console.WindowWidth / 2 + text4.Length / 2).ToString() + "}", text4));
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine(string.Format("{0," + (Console.WindowWidth / 2 + text5.Length / 2).ToString() + "}", text5));

            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("");
            Console.WriteLine("Please enter a txt path:");
            Console.ForegroundColor = ConsoleColor.DarkGray;
            string filepath = Console.ReadLine();
            while (!File.Exists(filepath))
            {
                Console.WriteLine("Invalid file.");
                filepath = Console.ReadLine();
            }
            string[] webhooks;
            try
            {
                webhooks = File.ReadAllLines(filepath);
            }
            catch
            {
                Console.ForegroundColor= ConsoleColor.DarkGray;
                Console.WriteLine("Run with admin perms .");

                Console.ReadLine();
                return;
            }
            List<string> validHooks = new List<string>();
            using (WebClient hookCheckerClient = new WebClient())
            {
                foreach (string webhook in webhooks)
                {


                    try
                    {
                        string resp = hookCheckerClient.DownloadString(webhook);
                        if (resp.Contains("Invalid"))
                        {
                            Console.ForegroundColor = ConsoleColor.DarkRed;
                            Console.WriteLine("[+] Invalid ");
                            continue;
                        }
                        else
                        {
                            ConsoleColor color = ConsoleColor.Green;
                            Console.WriteLine("[+] Valid Webhook Found.");
                            validHooks.Add(webhook);
                        }
                    }
                    catch (WebException hookException)
                    {
                        if (hookException.Response is HttpWebResponse wr && wr.StatusCode == HttpStatusCode.NotFound)
                        {
                            Console.ForegroundColor = ConsoleColor.DarkRed;
                            Console.WriteLine("[+] Invalid ");
                            continue;
                        }
                    }
                    catch
                    {
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.WriteLine("[+] Invalid ");
                    }
                }
            }
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("Saving valid webhooks");
            try
            {
                File.WriteAllLines("validwebhooks.txt", validHooks);
            }
            catch
            {
                Console.WriteLine("Enviroment Fail.");
                Thread.Sleep(1500);
                Environment.Exit(0);
            }
            Console.ReadLine();
        }
    }
}
