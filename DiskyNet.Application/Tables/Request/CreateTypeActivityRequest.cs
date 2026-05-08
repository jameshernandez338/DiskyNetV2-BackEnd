using System.ComponentModel.DataAnnotations;

namespace DiskyNet.Application.Tables.Request
{
    public class CreateTypeActivityRequest
    {
        [Required(ErrorMessage = "Type activity name is required")]
        [StringLength(100, ErrorMessage = "Type activity name cannot exceed 100 characters")]
        public string TypeActivityName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Frequency days is required")]
        [Range(0, int.MaxValue, ErrorMessage = "Frequency days must be greater than or equal to 0")]
        public int TypeActivityFrecDays { get; set; }
    }
}
