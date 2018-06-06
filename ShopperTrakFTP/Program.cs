using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace ShopperTrakFTP
{
    class Program
    {
        
        static String drive = @"C:\ShopperFTP";
        static String logDirectory = drive + @"\\LOG";
        public static String archiveDirectory = drive + @"\\archive";
        public static String errorDirectory = drive + @"\\error";
        static String salesScript = drive + @"\\scripts\\salesScript.sql";
        static String hoursScript = drive + @"\\scripts\\hoursScript.sql";
        static String logFile = logDirectory + @"\\log.txt";
        static String timeStamp = GetTimestamp();
        static String salesOutput = drive + @"\\Sales_" + timeStamp + ".txt";
        static String hoursOutput = drive + @"\\Labor_" + timeStamp + ".txt";
        static String salesArchive = drive + @"\\Archive\\Sales_" + timeStamp + ".txt";
        static String hoursArchive = drive + @"\\Archive\\Labor_" + timeStamp + ".txt";
        public static String salesErrorArchive = drive + @"\\Error\\Sales_" + timeStamp + ".txt";
        public static String laborErrorArchive = drive + @"\\Error\\Labor_" + timeStamp + ".txt";
        public static String salesFTPFile = "Sales_" + timeStamp + ".txt";
        public static String laborFTPFile = "Labor_" + timeStamp + ".txt";
        static Boolean filesExist;

        static StreamWriter w;

        static int dayOfWeek;

        static void Main(string[] args)
        {
            //Determine the day of the week in integer format
            DateTime dateTime = DateTime.Now;
            dayOfWeek = (int)dateTime.DayOfWeek;

            String[] archiveFiles = System.IO.Directory.GetFiles(archiveDirectory, "*" + timeStamp + "*.txt");

            //Check to see if files already existt
            if (archiveFiles.Length < 1)
            {
                filesExist = false;
            }
            else
            {
                filesExist = true;
            }

             //created directories if they dont exist
            createDirectories();

            //Create the log file
            Log log = new Log(w);
            log.append("Beginning");


            if (!filesExist)
            {
                //sqlcmd params for sales
                String salesParam = "-h -1 -W -s , -i " + salesScript + " -o " + salesOutput + " -S LOCALHOST";
                String hoursParam = "-h -1 -W -s , -i " + hoursScript + " -o " + hoursOutput + " -S LOCALHOST";

                //run sqlcmd to generate salesFile

                //load the files from root directory into array
                string[] files = System.IO.Directory.GetFiles(drive, "*.txt");

                //check if there are already files that haven't been sent to FTP if their are dont create new ones
               
                
                //Removing Labor file at Cokes request

                //if (files.Length == 0)             
                //{   //if day of the week = 2(tuesday) send sales and labor if not just send sales.
                //    if (dayOfWeek != 1)
                //    {
                //        Query query = new Query(salesParam, log);
                //    }
                //    else
                //    {
                //        Query query = new Query(salesParam, log);
                //        Query query2 = new Query(hoursParam, log);
                //    }

                //}

                if (files.Length == 0)
                {   
                    Query query = new Query(salesParam, log);
                }

                else
                {
                    log.append("Files already in root directory");
                }


                FTP ftp = new FTP(salesArchive, hoursArchive, log);
                ftp.begin();

                w.Close();
            }
            else
            {
                log.append("Files already created.");
                w.Close();
            }
        }

        public static void createDirectories()
        {


            try
            {
                if (!Directory.Exists(drive))
                    Directory.CreateDirectory(drive);
            }

            catch (Exception ex)
            {
                Console.WriteLine("Error creating logging directory!");
                Console.WriteLine(ex.ToString());
            }

            try
            {
                if (!Directory.Exists(archiveDirectory))
                    Directory.CreateDirectory(archiveDirectory);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error Creating archive directory");
                Console.WriteLine(ex.ToString());
            }

            try
            {
                if (!Directory.Exists(errorDirectory))
                    Directory.CreateDirectory(errorDirectory);
            }
            catch (Exception ex)
            {
                Console.WriteLine("error creating error directory");
                Console.WriteLine(ex.ToString());
            }

            try
            { 
                if(!Directory.Exists(logDirectory))
                {
                    Directory.CreateDirectory(logDirectory);
                }
            }
            catch(Exception e)
            {
                Console.WriteLine("Cannot create Log Directory");
                Console.WriteLine(e.StackTrace);
            }

            w = File.AppendText(logFile);
            File.GetAccessControl(logFile);


           

        }

        public static String GetTimestamp()
        {
            DateTime value = DateTime.Now.AddDays(-1);
            return value.ToString("yyyyMMdd");
        }


    }

}
