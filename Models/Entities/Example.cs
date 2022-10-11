using System.ComponentModel.DataAnnotations;
using Monolithic.Models.Common;

namespace Monolithic.Models.Entities;

public class Example : EntityBase
{
    [MaxLength(255)]
    public string FirstName { get; set; }

    [MaxLength(255)]
    public string LastName { get; set; }
}
