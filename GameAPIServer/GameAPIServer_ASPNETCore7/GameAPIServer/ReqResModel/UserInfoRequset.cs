using System.ComponentModel.DataAnnotations;

namespace GameAPIServer.ReqResModel;

public class UserInfoRequset
{
    [Required] public string Id { get; set; }
    [Required] public string AuthToken { get; set; }
}