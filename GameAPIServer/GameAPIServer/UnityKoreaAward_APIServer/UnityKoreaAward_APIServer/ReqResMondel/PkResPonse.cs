using System.ComponentModel.DataAnnotations;

namespace UnityKoreaAward_APIServer.ReqResMondel;

public class PkResPonse
{
    [Required]
    public ErrorCode Result { get; set; } = ErrorCode.None;
}
