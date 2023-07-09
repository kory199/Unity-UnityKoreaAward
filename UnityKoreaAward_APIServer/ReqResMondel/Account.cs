using System.ComponentModel.DataAnnotations;

namespace UnityKoreaAward_APIServer.ReqResMondel;

public class PkAccountRequest
{
    [Required]
    [MinLength(2, ErrorMessage = "ID CONNOT BE EMPTY")]
    [StringLength(11, ErrorMessage = "ID IS SO LONG")]
    public String ID { get; set; } = "";

    [Required]
    [MinLength(1, ErrorMessage = "PASSWORD CANNOT BE EMPTY")]
    [StringLength(20, ErrorMessage = "PASSWORE IS SO LONG")]
    [DataType(DataType.Password)]
    public String Password { get; set; } = "";
}