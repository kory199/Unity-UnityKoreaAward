using System.ComponentModel.DataAnnotations;

namespace APIServer.ReqResMondel;

public class AccountReq
{
    [Required]
    [MinLength(2, ErrorMessage = "ID CONNOT BE EMPTY")]
    [StringLength(20, ErrorMessage = "ID IS SO LONG")]
    public String ID { get; set; } = "";

    [Required]
    [MinLength(1, ErrorMessage = "PASSWORD CANNOT BE EMPTY")]
    [StringLength(20, ErrorMessage = "PASSWORE IS SO LONG")]
    [DataType(DataType.Password)]
    public String Password { get; set; } = "";
}