using System;
using System.Linq;
using System.Threading.Tasks;

namespace ArenaGame
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await StartGameAsync();
        }

        static async Task StartGameAsync()
        {
            Console.WindowHeight = 50;
            Console.WindowWidth = 130;
            Welcome.DisplayWelcomeBanner();
            await Welcome.PlayWelcomeSoundAsync();
            await Welcome.DisplayWelcomeMessageAsync();

            int numberOfHeroes;

            string[] menuOptions = { "Új torna", "Kilépés" };
            int selectedOption = 0;

            do
            {
                Console.SetCursorPosition((Console.WindowWidth - 40) / 2, Console.WindowHeight - 1);
                for (int i = 0; i < menuOptions.Length; i++)
                {
                    if (i == selectedOption)
                    {
                        Console.BackgroundColor = ConsoleColor.Blue;
                        Console.ForegroundColor = ConsoleColor.Black;
                    }
                    Console.Write(menuOptions[i].PadRight(20));
                    Console.ResetColor();
                }

                ConsoleKeyInfo keyInfo = Console.ReadKey(true);

                if (keyInfo.Key == ConsoleKey.LeftArrow && selectedOption > 0)
                {
                    selectedOption--;
                }
                else if (keyInfo.Key == ConsoleKey.RightArrow && selectedOption < menuOptions.Length - 1)
                {
                    selectedOption++;
                }
                else if (keyInfo.Key == ConsoleKey.Enter)
                {
                    if (selectedOption == 0)
                    {
                        // Új torna választva, hősök számának bekérése
                        Console.Clear();
                        bool validInput = false;

                        do
                        {
                            Console.Write("Kérem, add meg, hányan harcolnak az idei játékokon (legalább 2): ");
                            string userInput = Console.ReadLine();

                            if (int.TryParse(userInput, out numberOfHeroes) && numberOfHeroes >= 2)
                            {
                                Console.WriteLine($"\nKöszönöm a választ! Az idei játékokon {numberOfHeroes} harcos van.\n");
                                validInput = true;
                            }
                            else
                            {
                                Console.WriteLine("Érvénytelen bemenet. Kérem, adj meg egy érvényes pozitív egész számot (legalább 2).\n");
                            }
                        } while (!validInput);

                        HeroGenerator heroGenerator = new HeroGenerator();
                        var heroes = await heroGenerator.GenerateHeroes(numberOfHeroes);

                        Table.DisplayHeroesTable(heroes);

                        // Kiválasztás és csata indítása
                        Battle.StartBattle(heroes);

                        break;
                    }
                    else if (selectedOption == 1)
                    {
                        // Kilépés választva
                        Environment.Exit(0);
                    }
                }
            } while (true);
        }
    }
}
