namespace SysLogger
{
    public class SysLog
    {
        // Severity of log
        public string Severity { get; set; }

        // Erroring code file name
        public string FileName { get; set; }

        // Error throwing function
        public string FunctionName { get; set; }

        // Description of error
        public string Description { get; set; }

        // Error details
        public string Detail { get; set; }

        // Set to true if further interaction is required by sending an email or running a web script
        // Default is false
        public bool NotificationRequired { get; set; } = false;
    }

}
