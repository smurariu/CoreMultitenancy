namespace MultitenantAspApp
{
    public class SzHelloWorldService : IHelloWorldService
    {
        public string GetHelloWorld()
        {
            return "SZ: Hello World!";
        }
    }
}