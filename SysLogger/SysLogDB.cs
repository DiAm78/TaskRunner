namespace SysLogger
{
    public class SysLogDB : SysLog
    {

        // These properties are set in the Syslogger service prior to saving to the database
        // logged in user ID
        public string UserId { get; set; }

        // user machine name
        public string MachineName { get; set; }

        // App Environment
        public string AppEnvironment { get; set; }

    }

}
