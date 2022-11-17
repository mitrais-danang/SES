namespace SESDemo.Models
{
    public class MailSetting
    {
        public string SMTPHost { get; set; }
        public int SMTPPort { get; set; }
        public string SMTPUserName { get; set; }
        public string SMTPPassword { get; set; }
        public string SMTPEmailFrom { get; set; }

    }
}
