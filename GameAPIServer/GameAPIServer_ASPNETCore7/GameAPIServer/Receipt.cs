namespace GameAPIServer;

public class Receipt
{
    public static PreceiptInfo NewReceipt()
    {
        PreceiptInfo newReceipt = new PreceiptInfo
        {
            payDate = PayDate(),
            //receiptNo = GenerateReceiptNum(),
            receiptNo = 1234, // 테스트용 
            code = 1,
        };

        Console.WriteLine($"[NEW RECEIPT] ReceiptNo : {newReceipt.receiptNo}, Date :{newReceipt.payDate}, code : {newReceipt.code} ");
        return newReceipt;
    }

    private static Int64 PayDate()
    {
        DateTime now = DateTime.Now;
        Int64 receiptNumber = Int64.Parse(string.Format("{0:yyyyMMddHHmm}", now));

        return receiptNumber;

    }

    private static Int64 GenerateReceiptNum()
    {
        DateTime now = DateTime.Now;
        Int64 receiptNumber = Int64.Parse(string.Format("{0:yyyyMMddHHmmssfff}", now));

        return receiptNumber;
    }
}

public class PreceiptInfo
{
    public Int64 payDate {get; set; }
    public Int64 receiptNo { get; internal set; }
    public Int32 code { get; set; }
}