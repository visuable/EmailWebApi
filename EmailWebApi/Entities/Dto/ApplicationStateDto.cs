namespace EmailWebApi.Entities.Dto
{
    public class ApplicationStateDto
    {
        public int Total { get; set; }
        public int Error { get; set; }
        public int Sent { get; set; }
        public int Query { get; set; }
    }
}