using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackendExamHub.Models.Database;

/// <summary>
/// MyOffice 帳號人員資料
/// </summary>
[Table("MyOffice_ACPD")]
public class MyOfficeAcpd
{
    [Key]
    [Column("ACPD_SID")]
    [MaxLength(20)]
    [StringLength(20)]
    public string AcpdSid { get; set; } = string.Empty;

    [Column("ACPD_Cname")]
    [MaxLength(60)]
    public string? AcpdCname { get; set; }

    [Column("ACPD_Ename")]
    [MaxLength(40)]
    public string? AcpdEname { get; set; }

    [Column("ACPD_Sname")]
    [MaxLength(40)]
    public string? AcpdSname { get; set; }

    [Column("ACPD_Email")]
    [MaxLength(60)]
    public string? AcpdEmail { get; set; }

    [Column("ACPD_Status")]
    public byte? AcpdStatus { get; set; } = 0;

    [Column("ACPD_Stop")]
    public bool? AcpdStop { get; set; } = false;

    [Column("ACPD_StopMemo")]
    [MaxLength(60)]
    public string? AcpdStopMemo { get; set; }

    [Column("ACPD_LoginID")]
    [MaxLength(30)]
    public string? AcpdLoginId { get; set; }

    [Column("ACPD_LoginPWD")]
    [MaxLength(60)]
    public string? AcpdLoginPwd { get; set; }

    [Column("ACPD_Memo")]
    [MaxLength(600)]
    public string? AcpdMemo { get; set; }

    [Column("ACPD_NowDateTime")]
    public DateTime? AcpdNowDateTime { get; set; } = DateTime.Now;

    [Column("ACPD_NowID")]
    [MaxLength(20)]
    public string? AcpdNowId { get; set; }

    [Column("ACPD_UPDDateTime")]
    public DateTime? AcpdUpdDateTime { get; set; } = DateTime.Now;

    [Column("ACPD_UPDID")]
    [MaxLength(20)]
    public string? AcpdUpdId { get; set; }
}
