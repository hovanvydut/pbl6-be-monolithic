using System.ComponentModel.DataAnnotations.Schema;
using Monolithic.Models.Common;
using Monolithic.Constants;
using Microsoft.EntityFrameworkCore;

namespace Monolithic.Models.Entities;

[Table(TableName.BANK_CODE)]
[Index(nameof(Code), IsUnique = true)]
public class BankCodeEntity : EntityBase
{
    [Column("id")]
    public int Id { get; set; }

    [Column("code")]
    public string Code { get; set; }

    [Column("description")]
    public string Description { get; set; }
}