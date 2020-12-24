namespace EmailWebApi.Objects
{
    public class Email
    {
        public EmailContent Content { get; set; }
        public EmailInfo Info { get; set; }
        public EmailState State { get; set; }
        public int Id { get; set; }
    }
}