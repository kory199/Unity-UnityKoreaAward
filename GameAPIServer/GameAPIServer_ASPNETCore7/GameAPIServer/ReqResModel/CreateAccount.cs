using System.ComponentModel.DataAnnotations;

namespace GameAPIServer.ReqResModel;

public class PkCreateAccountRequest
{
    [Required]
    [MinLength(2, ErrorMessage = "ID CONNOT BE EMPTY")]
    [StringLength(11, ErrorMessage = "ID IS SO LONG")]
    public string Id { get; set; }

    [Required]
    [MinLength(1, ErrorMessage = "PASSWORD CANNOT BE EMPTY")]
    [StringLength(30, ErrorMessage = "PASSWORD IS SO LONG")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = "";
}
