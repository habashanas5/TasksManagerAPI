using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TaskManagerAPI.Models
{
    public class UserTask
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "The Title is required.")]
        [StringLength(100, ErrorMessage = "The Title must be less than 100 characters.")]
        public string Title { get; set; }

        [StringLength(500, ErrorMessage = "The Description must be less than 500 characters.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "The Due Date is required.")]
        [DataType(DataType.Date, ErrorMessage = "The Due Date must be a valid date.")]
        public DateTime DueDate { get; set; }

        [Required(ErrorMessage = "The Priority is required.")]
        [RegularExpression("Low|Medium|High", ErrorMessage = "Priority must be Low, Medium, or High.")]
        public string Priority { get; set; }

        [Required(ErrorMessage = "The Status is required.")]
        [RegularExpression("Pending|In Progress|Completed", ErrorMessage = "Status must be Pending, In Progress, or Completed.")]
        public string Status { get; set; }

        [StringLength(50, ErrorMessage = "The Category must be less than 50 characters.")]
        public string Category { get; set; }

    }
}
