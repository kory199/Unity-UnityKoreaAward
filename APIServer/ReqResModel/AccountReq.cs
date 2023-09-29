using System.ComponentModel.DataAnnotations;

namespace APIServer.ReqResMondel;

public class AccountReq
{
    [Required]
    [MinLength(1, ErrorMessage = "ID CONNOT BE EMPTY")]
    [StringLength(12, ErrorMessage = "ID IS SO LONG")]
    [RegularExpression(@"^(?=.*[a-zA-Z])(?=.*[0-9])[a-zA-Z0-9]+$", 
        ErrorMessage = "ID MUST CONTAIN BOTH ALPHANUMERIC CHARACTERS")]
    public String ID { get; set; } = "";

    [Required]
    [MinLength(1, ErrorMessage = "PASSWORD CANNOT BE EMPTY")]
    [StringLength(12, ErrorMessage = "PASSWORE IS SO LONG")]
    [DataType(DataType.Password)]
    [RegularExpression(".*[!@#$%^&*()_+].*", 
        ErrorMessage = "PASSWORD MUST CONTAIN AT LEAST ONE SPECIAL CHARACTER")]
    public String Password { get; set; } = "";
}