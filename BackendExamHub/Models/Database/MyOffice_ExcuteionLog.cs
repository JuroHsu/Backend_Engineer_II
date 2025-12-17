using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackendExamHub.Models.Database;

/// <summary>
/// MyOffice 執行日誌
/// </summary>
[Table("MyOffice_ExcuteionLog")]
public class MyOfficeExecutionLog
{
    [Key]
    [Column("DeLog_AutoID")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long DeLogAutoId { get; set; }

    [Required]
    [Column("DeLog_StoredPrograms")]
    [MaxLength(120)]
    public string DeLogStoredPrograms { get; set; } = string.Empty;

    [Required]
    [Column("DeLog_GroupID")]
    public Guid DeLogGroupId { get; set; }

    [Required]
    [Column("DeLog_isCustomDebug")]
    public bool DeLogIsCustomDebug { get; set; } = false;

    [Required]
    [Column("DeLog_ExecutionProgram")]
    [MaxLength(120)]
    public string DeLogExecutionProgram { get; set; } = string.Empty;

    [Column("DeLog_ExecutionInfo")]
    public string? DeLogExecutionInfo { get; set; }

    [Column("DeLog_verifyNeeded")]
    public bool DeLogVerifyNeeded { get; set; } = false;

    [Required]
    [Column("DeLog_ExDateTime")]
    public DateTime DeLogExDateTime { get; set; } = DateTime.Now;
}