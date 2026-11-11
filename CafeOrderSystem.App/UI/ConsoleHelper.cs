namespace Software_Design_Exam.App.UI;

public static class ConsoleHelper
{
    public static void RunSafely(Action action)
    {
        try
        {
            action();
        }
        catch(Exception ex) when (ex is ArgumentException or InvalidOperationException)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}