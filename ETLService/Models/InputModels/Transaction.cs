using System.ComponentModel.DataAnnotations;

namespace ETLService.Models.InputModels;

public class Transaction
{
    [Required]
    public string FirstName { get; set; }

    [Required]
    public string LastName { get; set; }

    [Required]
    public string Address { get; set; }

    [Required]
    public decimal Payment { get; set; }

    [Required]
    [DataType(DataType.Date)]
    public DateTime Date { get; set; }

    [Required]
    public long AccountNumber { get; set; }

    [Required]
    public string Service { get; set; }
}