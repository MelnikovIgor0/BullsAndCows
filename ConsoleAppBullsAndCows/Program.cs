using System;

namespace ConsoleAppBullsAndCows
{
    class Program
    {
        /// <summary>
        /// The function displays the rules.
        /// </summary>
        /// <param name="backWhere">String indicating where the user was before calling the rules.</param>
        static void ShowRules(string backWhere)
        {
            Console.WriteLine("  The program selects a number of user specified length");
            Console.WriteLine("  (there are no repeating digits and leading zeroes in");
            Console.WriteLine("  the hidden number). User is trying to guess the hidden");
            Console.WriteLine("  number, and until it is guessed, he enters a number");
            Console.WriteLine("  (of the same length), and in response receives the");
            Console.WriteLine("  amount of cows (digits that are in both numbers, but");
            Console.WriteLine("  in different places) and bulls (digits that are in");
            Console.WriteLine("  both numbers and are in the same places). At any");
            Console.WriteLine("  point in the game you can surrender by typing 'surrender'");
            Console.WriteLine("  instead of a number. Due to the fact that digits in");
            Console.WriteLine("  hidden number are not repeated, you are also required");
            Console.WriteLine("  to enter numbers with non-repeating digits. You can");
            Console.WriteLine("  return to viewing rules at any point during the game.");
            Console.WriteLine();
            Console.WriteLine("  You are back to " + backWhere);
        }

        /// <summary>
        /// The function displays the difficulty selection menu, returns the selected by user difficulty.
        /// Can handle rules menu invocation and incorrect input.
        /// </summary>
        /// <returns>Uint value, hard level of game.</returns>
        static uint ShowStartMenu()
        {

            Console.WriteLine("You are playing the 'bulls and cows' game.");
            Console.WriteLine("To read the rules of the game type 'rules'.");
            Console.WriteLine("To start the game, type number corresponding to difficulty level (2-10).");
            uint hardLevel = 0;
            do
            {
                Console.Write(">>> ");
                string comand = Console.ReadLine();
                if (comand == "rules") ShowRules("choosing difficulty.");
                else
                {
                    uint inputNumber;
                    if (uint.TryParse(comand, out inputNumber))
                    {
                        if (2 <= inputNumber && inputNumber <= 10) hardLevel = inputNumber;
                        else Console.WriteLine("Typed difficulty level is not correct!!!");
                    }
                    else Console.WriteLine("Typed command was not recognized!!!");
                }
            } while (hardLevel == 0);
            return hardLevel;
        }

        /// <summary>
        /// The function returns a random number (in string type) consisting of different digits without leading zeros.
        /// </summary>
        /// <param name="hardLevel">Uint value, hard level of game.</param>
        /// <returns>String value, number with different digits.</returns>
        static string GenerateNumber(uint hardLevel)
        {
            //Works like this: in an ordered array of digits it swaps two random digits.
            //1000 times, then creates a string of the required length from the first.
            //digits of the array.
            uint[] uniqueDigits = new uint[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            Random shuffleRand = new Random();
            for (uint iteration = 0; iteration < 1000; iteration++)
            {
                uint index1 = (uint)shuffleRand.Next(0, 10), index2 = (uint)shuffleRand.Next(0, 10);
                while (index1 == index2) index2 = (uint)shuffleRand.Next(0, 10);
                // Swapping two elements.
                uint swapBuffer = uniqueDigits[index1];
                uniqueDigits[index1] = uniqueDigits[index2];
                uniqueDigits[index2] = swapBuffer;
            }
            //Handling the case where the number should start from 0.
            if (uniqueDigits[0] == 0)
            {
                uint index1 = (uint)shuffleRand.Next(0, 10);
                while (index1 == 0) index1 = (uint)shuffleRand.Next(0, 10);
                //Swapping two elements.
                uint swapBuffer = uniqueDigits[0];
                uniqueDigits[0] = uniqueDigits[index1];
                uniqueDigits[index1] = swapBuffer;
            }
            string totalNumber = "";
            //Collecting the whole number.
            for (uint iteration = 0; iteration < hardLevel; iteration++)
                totalNumber += (char)('0' + uniqueDigits[iteration]);
            return totalNumber;
        }

        /// <summary>
        /// The function counts amount of bulls.
        /// </summary>
        /// <param name="selectedNumber">String value, generated by program number.</param>
        /// <param name="guessNumber">String value, typed by user number.</param>
        /// <returns>Uint value, amount of bulls.</returns>
        static uint CountBulls(string selectedNumber, string guessNumber)
        {
            uint bulls = 0;
            for (int index = 0; index < selectedNumber.Length; index++)
                if (selectedNumber[index] == guessNumber[index])
                    bulls++;
            return bulls;
        }

        /// <summary>
        /// The function counts amount of cows.
        /// </summary>
        /// <param name="selectedNumber">String value, generated by program number.</param>
        /// <param name="guessNumber">String value, typed by user number.</param>
        /// <returns>Uint value, amount of cows.</returns>
        static uint CountCows(string selectedNumber, string guessNumber)
        {
            uint cows = 0;
            for (int indexSelected = 0; indexSelected < selectedNumber.Length; indexSelected++)
            {
                //If it's definitely a bull, it's definitely not a cow.
                if (selectedNumber[indexSelected] == guessNumber[indexSelected]) continue;
                for (int indexGuess = 0; indexGuess < guessNumber.Length; indexGuess++)
                    if (indexSelected != indexGuess && selectedNumber[indexSelected] == guessNumber[indexGuess])
                    {
                        cows++;
                        break;
                    }
            }
            return cows;
        }

        /// <summary>
        /// The function checks that all digits in the number are different.
        /// </summary>
        /// <param name="number">String type number.</param>
        /// <returns>Bool value, does number have different digits.</returns>
        static bool CheckNumber(string number)
        {
            bool[] usedDigits = new bool[10];
            for (int index = 0; index < 10; index++) usedDigits[index] = false;
            for (int index = 0; index < number.Length; index++)
            {
                //If same digit has been met before, then the number is not correct.
                if (usedDigits[(int)number[index] - (int)'0']) return false;
                usedDigits[(int)number[index] - (int)'0'] = true;
            }
            return true;
        }

        /// <summary>
        /// Function that implements one game.
        /// </summary>
        static void GameBody()
        {
            uint hardLevel = ShowStartMenu();
            string selectedNumber = GenerateNumber(hardLevel);
            Console.WriteLine("The program chose a number of length " + hardLevel + ".");
            bool gameWin = false, gameLose = false;
            do
            {
                Console.Write("Your guess >>> ");
                string comand = Console.ReadLine();
                if (comand == "rules") ShowRules("game.");
                else if (comand == "surrender") gameLose = true;
                else
                {
                    ulong inputNumber;
                    if (!ulong.TryParse(comand, out inputNumber))
                        Console.WriteLine("Guess is not a number!!! Input was ignored.");
                    else if (comand[0] == '0')
                        Console.WriteLine("Because of hidden number does not start at digit 0, " +
                            "you cannot type numbers starting at digit 0!!!");
                    else if (inputNumber.ToString().Length != selectedNumber.Length)
                        Console.WriteLine("The guess is incorrect length (must be " + hardLevel + " digits)!!!");
                    else if (!CheckNumber(comand)) Console.WriteLine("Number contains the same digits!!!");
                    else
                    {
                        Console.WriteLine("Bulls: " + CountBulls(selectedNumber, comand));
                        Console.WriteLine("Cows: " + CountCows(selectedNumber, comand));
                        if (selectedNumber == comand) gameWin = true;
                    }
                }
            } while (!gameWin && !gameLose);
            if (gameLose) Console.WriteLine("You lose :( Number was " + selectedNumber);
            if (gameWin) Console.WriteLine("You won!!!");
        }

        /// <summary>
        /// Function that checks if the user needs to restart the game.
        /// </summary>
        /// <returns>Bool value, does user want restart game.</returns>
        static bool Restart()
        {
            Console.WriteLine("Restart game?(y/n)");
            do
            {
                Console.Write(">>> ");
                string comand = Console.ReadLine();
                if (comand == "y") return true;
                if (comand == "n") return false;
            } while (true);
        }

        /// <summary>
        /// Main, just main.
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            do
            {
                GameBody();
            } while (Restart());
        }
    }
}