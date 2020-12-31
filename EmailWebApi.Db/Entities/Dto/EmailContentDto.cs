namespace EmailWebApi.Db.Entities.Dto
{
    public class EmailContentDto
    {
        public string Address { get; set; }
        public EmailBodyDto Body { get; set; }
        public string Title { get; set; }
    }
}