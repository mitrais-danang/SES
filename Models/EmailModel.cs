using System.ComponentModel.DataAnnotations;

public class EmailModel
{
    public string Destination { get; set; }
    public string CC { get; set; }
    public string Body { get; set; }
    public string Subject { get; set; }
}