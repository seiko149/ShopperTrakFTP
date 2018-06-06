using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Renci.SshNet;
using System.IO;
using System.Collections;

namespace ShopperTrakFTP
{
    class FTP
    {
        Boolean successfulConnection = true;
        string username = "ecocacola";
        string password = "M@y2016!";
        string host = "transfer.shoppertrak.com";
        static string drive = @"C:\shopperFTP";
        FileStream fileStream;

        String salesArchive;
        String hoursArchive;
        String errorArchive;
        Log log;

        public FTP(string salesArchive, string hoursArchive, Log log)
        {
            this.log = log;
            this.salesArchive = salesArchive;
            this.hoursArchive = hoursArchive;
                      
        }

        public void begin()
        {
            System.Threading.Thread.Sleep(2000);


            string[] files = System.IO.Directory.GetFiles(drive, "*.txt");
           
            var sftp = new SftpClient(host, 22, username, password);

            try
            {
                
                sftp.Connect();
                sftp.ChangeDirectory("everythingcocacola");
                log.append("Successful connection to FTP server");
            }
            catch (Exception ex)
            {
                successfulConnection = false;
                Console.WriteLine("cannot connect to FTP Site");
                log.append("Cannot connect to FTP Server");
                log.append(ex.StackTrace);
                
            }


            if (successfulConnection)
            {
                for (int i = 0; i < files.Length; i++)
                {

                    if(files[i].Contains("Sales"))
                    {
                        fileStream = new FileStream(files[i], FileMode.Open);
                        sftp.UploadFile(fileStream, Program.salesFTPFile);
                        fileStream.Close();
                        log.append("Succesfully uploaded files to FTP site");
                    }
                    else if (files[i].Contains("Labor"))
                    {
                        fileStream = new FileStream(files[i], FileMode.Open);
                        sftp.UploadFile(fileStream, Program.laborFTPFile);
                        fileStream.Close();
                        log.append("Succesfully uploaded files to FTP site");
                    }
                }

            }
            else
            {
                Console.WriteLine("Cannot Upload files unsuccessful FTP Conection");
                log.append("Cannot Upload files unsuccessful FTP Conection");
                successfulConnection = false;
            }
            System.Threading.Thread.Sleep(2000);


            if (successfulConnection)
            {
                for (int i = 0; i < files.Length; i++)
                {

                    if (files[i].Contains("Sales"))
                    {

                        File.Move(files[i], salesArchive);
                        log.append("moved salesFile to archive");

                    }
                    else if (files[i].Contains("Labor"))
                    {

                        File.Move(files[i], hoursArchive);
                        log.append("moved labor file to archive");
                    }

                }
            }
            else
            {
                for (int i = 0; i < files.Length; i++)
                {
                    if (files[i].Contains("Sales"))
                    {

                        File.Move(files[i], Program.salesErrorArchive);
                        log.append("moved salesFile to archive");

                    }
                    else if (files[i].Contains("Labor"))
                    {

                        File.Move(files[i], Program.laborErrorArchive);
                        log.append("moved labor file to archive");
                    }
                }
            }


        }
    }
}
