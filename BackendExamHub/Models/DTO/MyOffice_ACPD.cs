using System.ComponentModel.DataAnnotations;

namespace BackendExamHub.Models.DTO;

public class AccountResponse
{
    public string ACPD_SID { get; set; } = string.Empty;
    public string? ACPD_Cname { get; set; }
    public string? ACPD_Ename { get; set; }
    public string? ACPD_Sname { get; set; }
    public string? ACPD_Email { get; set; }
    public byte? ACPD_Status { get; set; }
    public bool? ACPD_Stop { get; set; }
    public string? ACPD_StopMemo { get; set; }
    public string? ACPD_LoginID { get; set; }
    public string? ACPD_Memo { get; set; }
    public DateTime? ACPD_NowDateTime { get; set; }
    public string? ACPD_NowID { get; set; }
    public DateTime? ACPD_UPDDateTime { get; set; }
    public string? ACPD_UPDID { get; set; }
}

public class CreateAccountRequest
{
    [Required(ErrorMessage = "中文名稱為必填")]
    [MaxLength(60)]
    public string ACPD_Cname { get; set; } = string.Empty;

    [MaxLength(40)]
    public string? ACPD_Ename { get; set; }

    [MaxLength(40)]
    public string? ACPD_Sname { get; set; }

    [Required(ErrorMessage = "Email 為必填")]
    [EmailAddress(ErrorMessage = "Email 格式不正確")]
    [MaxLength(60)]
    public string ACPD_Email { get; set; } = string.Empty;

    public byte? ACPD_Status { get; set; } = 0;

    public bool? ACPD_Stop { get; set; } = false;

    [MaxLength(60)]
    public string? ACPD_StopMemo { get; set; }

    [Required(ErrorMessage = "登入帳號為必填")]
    [MaxLength(30)]
    public string ACPD_LoginID { get; set; } = string.Empty;

    [Required(ErrorMessage = "密碼為必填")]
    [MaxLength(60)]
    public string ACPD_LoginPWD { get; set; } = string.Empty;

    [MaxLength(600)]
    public string? ACPD_Memo { get; set; }
}

public class UpdateAccountRequest
{
    [Required(ErrorMessage = "帳號 ID 為必填")]
    [MaxLength(20)]
    public string ACPD_SID { get; set; } = string.Empty;

    [Required(ErrorMessage = "中文名稱為必填")]
    [MaxLength(60)]
    public string ACPD_Cname { get; set; } = string.Empty;

    [MaxLength(40)]
    public string? ACPD_Ename { get; set; }

    [MaxLength(40)]
    public string? ACPD_Sname { get; set; }

    [Required(ErrorMessage = "Email 為必填")]
    [EmailAddress(ErrorMessage = "Email 格式不正確")]
    [MaxLength(60)]
    public string ACPD_Email { get; set; } = string.Empty;

    public byte? ACPD_Status { get; set; }

    public bool? ACPD_Stop { get; set; }

    [MaxLength(60)]
    public string? ACPD_StopMemo { get; set; }

    [Required(ErrorMessage = "登入帳號為必填")]
    [MaxLength(30)]
    public string ACPD_LoginID { get; set; } = string.Empty;

    // 密碼為選填，如果有填才更新
    [MinLength(6, ErrorMessage = "密碼至少需要 6 個字元")]
    [MaxLength(60)]
    public string? ACPD_LoginPWD { get; set; }

    [MaxLength(600)]
    public string? ACPD_Memo { get; set; }
}