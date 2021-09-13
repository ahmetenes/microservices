namespace CommandService.DTOs
{
    public class CommandRead
    {
        
        public int Id { get; set; }
        
        public string HowTo { get; set; }
        
        public string CommandString { get; set; }
        public int PlatformId { get; set; }

    }
}