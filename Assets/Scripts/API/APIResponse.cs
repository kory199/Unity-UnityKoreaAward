public class APIResponse<T>
{
    public string responseBody { get; set; }
    public T Data { get; set; }
}