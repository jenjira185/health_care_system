using System.Runtime.CompilerServices;

namespace HealthCareSystem
{
    class Utils
    {
        public static int GetIndexAddOne(List<User> users)
        {
            return users.Last().Id + 1;
        }

        public static string GetRequiredInput(string promptMessage)
        {
            string? input = null;

            while (string.IsNullOrWhiteSpace(input))
            {
                Console.Write(promptMessage);
                input = Console.ReadLine() ?? "".Trim();

                if (string.IsNullOrWhiteSpace(input))
                {
                    DisplayAlertText("The input can not be empty. PLease try again...");
                }
            }

            return input;
        }

        public static void DisplaySucessText(string text, int delay = 40)
        {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"{text}");
            Console.ResetColor();
        }

        public static void DisplayAlertText(string text, int delay = 40)
        {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"{text}");
            Console.ResetColor();
        }

        public static int GetIntegerInput(string promptMessage)
        {
            while(true)
            {
                Console.Write(promptMessage);
                string? input = Console.ReadLine();

                if (int.TryParse(input, out int result))
                {
                    return result;
                }
                else
                {
                    DisplayAlertText("Invalifd input. Use only numbers. Please try again...");
                }
            }
        }
    }
}