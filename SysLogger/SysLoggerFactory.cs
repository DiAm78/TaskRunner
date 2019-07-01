using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using TaskRunner.Core;

namespace SysLogger
{
    public static class SysLoggerFactory
    {
        public static void SaveSyslogEvent(SysLog oSysLog)
        {
            try
            {
                SysLogDB oSysLogDB = new SysLogDB();

                // get all properties from oSysLog
                oSysLogDB.Map(oSysLog);

                // get other information for the log
                //oSysLogDB.UserId = TU_GetUserName();
                //oSysLogDB.MachineName = Environment.MachineName;

                var AppConn = ConfigurationManager.ConnectionStrings["DBConnectionString"].ConnectionString;
                //var DBName = TU_GetValueFromConnectionStringByKey(AppConn, "Database");

                DataTable sysLogResult = oSysLogDB.ToDataTableOne();
                SaveSyslogEventToDB(sysLogResult);
            }
            catch (Exception ex)
            {
                // Do nothing
                // DB Method will write to a file if there is an exception
                Console.WriteLine("Error in Syslogger.saveSyslogEvent: " + ex.ToString());
            }
        }

        private static void SaveSyslogEvents(List<SysLog> lstSysLog)
        {
            try
            {
                List<SysLogDB> lstSysLogDB = new List<SysLogDB>();

                foreach (var oSysLog in lstSysLog)
                {
                    SysLogDB oSysLogDB = new SysLogDB();

                    // get all properties from oSysLog
                    oSysLogDB.Map(oSysLog);

                    // get other information for the log
                    //oSysLogDB.UserId = TU_GetUserName();
                    //oSysLogDB.MachineName = Environment.MachineName;

                    var AppConn = ConfigurationManager.ConnectionStrings["DBConnectionString"].ConnectionString;
                    //var DBName = TU_GetValueFromConnectionStringByKey(AppConn, "Database");
                    
                    lstSysLogDB.Add(oSysLogDB);
                }

                DataTable sysLogResult = lstSysLogDB.ToDataTable();

                SaveSyslogEventToDB(sysLogResult);
            }
            catch (Exception ex)
            {
                // Do nothing
                // DB Method will write to a file if there is an exception
                Console.WriteLine("Error in Syslogger.saveSyslogEvents: " + ex.ToString());
            }
        }

        private static void SaveSyslogEventToDB(DataTable sysLogResult)
        {
            SqlConnection conn;
            conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DBConnectionString"].ConnectionString);

            try
            {
                using ((conn))
                {
                    SqlCommand comm = new SqlCommand();
                    comm.Connection = conn;
                    comm.CommandText = "dbo.Proc_Save_Syslog";
                    comm.CommandType = CommandType.StoredProcedure;
                    SqlParameter param = comm.Parameters.Add("@ptblSyslog", SqlDbType.Structured);
                    param.Value = sysLogResult;

                    conn.Open();
                    comm.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                try
                {
                    var msg = string.Join(",", sysLogResult.AsEnumerable().Select(x => "(" + string.Join(",", x.ItemArray) + ")"));
                    SaveLogToFile(msg);
                    SaveLogToFile("DB Syslog save error: " + ex.Message);
                }
                catch (Exception wtl)
                {
                    Console.WriteLine("LoggingService.SaveSyslogEvent: " + wtl.Message);
                }
            }
        }

        private static void SaveLogToFile(string Message)
        {
            string mydocpath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            using (System.IO.StreamWriter outputFile = new System.IO.StreamWriter(mydocpath + string.Format(@"\log{0}.txt", DateTime.Now.ToLongDateString()), true))
            {
                outputFile.WriteLine(DateTime.Now.ToString() + ": " + Message);
            }
        }
    }
}
