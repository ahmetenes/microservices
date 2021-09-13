using System.ComponentModel.DataAnnotations;

namespace CommandService.DTOs
{
    public class CommandWrite
    {
        
        [Required]        
        public string HowTo { get; set; }
        [Required]
        public string CommandString { get; set; }


    }
}