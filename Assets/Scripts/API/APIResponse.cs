public class APIResponse<T>
{
    public int Result { get; set; }
    public string ResultMessage { get; set; }
    public T Data { get; set; }
}