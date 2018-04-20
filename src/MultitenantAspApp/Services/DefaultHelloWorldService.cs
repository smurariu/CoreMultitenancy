namespace MultitenantAspApp
{
    public class DefaultHelloWorldService : IHelloWorldService
    {
        public string GetHelloWorld()
        {
            return "Hello World";
        }
    }
}