namespace EmailWebApi.Objects
{
    public class ApplicationState
    {
        public int Total { get; set; }
        public int Error { get; set; }
        public int Sent { get; set; }
        public int Query { get; set; }
    }
}