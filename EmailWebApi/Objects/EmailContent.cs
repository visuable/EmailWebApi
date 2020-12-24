namespace EmailWebApi.Objects
{
    public class EmailContent
    {
        public string Address { get; set; }
        public EmailBody Body { get; set; }
        public string Title { get; set; }
        public int Id { get; set; }
    }
}