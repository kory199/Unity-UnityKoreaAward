using System;

namespace GameAPIServer.DBModel;

public class PayInApp
{
    public Int64 player_id { get; set; }
    public Int64 receipt_number { get; set; }
    public Int32 item_code { get; set; }
    public Int64 count { get; set; }
}