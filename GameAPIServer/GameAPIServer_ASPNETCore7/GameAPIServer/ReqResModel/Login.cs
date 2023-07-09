using System.ComponentModel.DataAnnotations;

namespace GameAPIServer.ReqResModel;

public class PkLoginRequest
{
    [Required] 
    [MinLength(1, ErrorMessage = "ID CONNOT BE EMPTY")]
    [StringLength(40, ErrorMessage = "ID IS SO LONG")]
    public String Id { get; set; }

    [Required]
    [MinLength(1, ErrorMessage = "PASSWORD CANNOT BE EMPTY")]
    [StringLength(30, ErrorMessage = "PASSWORD IS SO LONG")]
    [DataType(DataType.Password)]
    public String Password { get; set; }
}

public class PkLoginResponse
{
    [Required] public ErrorCode Result { get; set; } = ErrorCode.None;
    [Required] public String AuthToken { get; set; } = "";
    [Required] public String Notice { get; set; } = "";
}