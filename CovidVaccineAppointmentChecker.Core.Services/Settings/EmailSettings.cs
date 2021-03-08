namespace CovidVaccineAppointmentChecker.Settings
{
    public class EmailSettings
    {
        public SmtpServerSettings SmtpServer { get; set; }
        public string From { get; set; }
        public string To { get; set; }        
    }
    public class SmtpServerSettings
    {
        public bool UseDefaultCredentials { get; set; }
        public bool EnableSsl { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
