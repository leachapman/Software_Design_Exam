using System.Globalization;
using Software_Design_Exam.Domain.Enums;

namespace Software_Design_Exam.App.UI;

public static class ConsoleInput
{
    public static int ReadInt(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            if (int.TryParse(Console.ReadLine(), out var value))
                return value;
            Console.WriteLine("Please enter a valid number.");
        }
    }

    public static decimal ReadDecimal(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            if (decimal.TryParse(Console.ReadLine(), out var value))
                return value;
            Console.WriteLine("Please enter a valid number (e.g. 3.50).");
        }
    }

    public static bool ReadYesNo (string prompt)
    {
        string? input = null;
        while (input != "y" && input != "n")
        {
            Console.WriteLine(prompt);
            input = Console.ReadLine()?.Trim().ToLower();
        }
        return input == "y";
    }

    public static ProductCategory ReadCategory()
    {
        Console.WriteLine("Category: ");
        foreach (var c in Enum.GetValues<ProductCategory>())
            {
                Console.WriteLine($"  {(int)c} = {c}");
            }

        while (true)
        {
            var number = ReadInt("Category number: ");
               if (Enum.IsDefined(typeof(ProductCategory), number))
                {
                    return (ProductCategory)number;
                }
            Console.WriteLine("Invalid category.");
        }
    }
}