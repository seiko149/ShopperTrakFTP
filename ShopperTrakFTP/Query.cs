using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.IO;
using System.Diagnostics;

namespace ShopperTrakFTP
{
    class Query
    {
        public Query(String sqlcmdParam, Log log)
        {

            try
            {
                Process RunSQLScript = new Process();
                RunSQLScript.StartInfo.FileName = "sqlcmd";
                RunSQLScript.StartInfo.Arguments = sqlcmdParam;
                RunSQLScript.Start();
            }
            catch (Exception f)
            {
                Console.WriteLine("An error occurred: '{0}'", f);
                log.append("Cannot Execute Queries");
                
            }
        }
    }
}
