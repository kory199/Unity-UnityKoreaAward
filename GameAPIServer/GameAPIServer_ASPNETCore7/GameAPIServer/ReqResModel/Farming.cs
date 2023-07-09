using System.ComponentModel.DataAnnotations;
using GameAPIServer.DBModel;

namespace GameAPIServer.ReqResModel;

public class PkFarmingItemRequest
{
    [Required] public String Id { get; set; }
    [Required] public String AuthToken { get; set; }
    [Required] public Int32 ItemCode { get; set; }
}