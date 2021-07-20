using System;

namespace BattleShipLibrary
{
    public class PlayerSettings
    {
        public static void WelcomeMessage()
        {
            TextPosition.SetCoordTextMessage("Welcome to Battleship Console App");
            Console.WriteLine("");
        }

        public static string GetAndVerifyName(string consoleLineName)
        {
            Console.Write("{0}: ", consoleLineName);
            string name = Console.ReadLine();
            Console.WriteLine();

            return name;
        }

        public static void GetErrorName(ErrorType error)
        {
            switch (error)
            {
                case ErrorType.ErrorName:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Input Error!The input line must consist only letter caracters!");
                    Console.ResetColor();
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.ReadKey();
                    break;

                case ErrorType.NoError:
                    break;
            }
        }
    }
}
