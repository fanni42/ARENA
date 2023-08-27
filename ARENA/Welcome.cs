using System;
using System.IO;
using System.Media;
using System.Threading;
using System.Threading.Tasks;

namespace ArenaGame
{
    public static class Welcome
    {
        public static void DisplayWelcomeBanner()
        {
            Console.ForegroundColor = ConsoleColor.Green; // Szöveg színe zöld
            Console.WriteLine(@"
     _        _______  _______  _______  _______             _______  _______    _        _______  _        _______ 
    ( \      (  ____ \(  ____ \(  ___  )(  ____ \|\     /|  (  ___  )(  ____ \  ( \      (  ____ \( (    /|(  ___  )
    | (      | (    \/| (    \/| (   ) || (    \/( \   / )  | (   ) || (    \/  | (      | (    \/|  \  ( || (   ) |
    | |      | (__    | |      | (___) || |       \ (_) /   | |   | || (__      | |      | (__    |   \ | || (___) |
    | |      |  __)   | | ____ |  ___  || |        \   /    | |   | ||  __)     | |      |  __)   | (\ \) ||  ___  |
    | |      | (      | | \_  )| (   ) || |         ) (     | |   | || (        | |      | (      | | \   || (   ) |
    | (____/\| (____/\| (___) || )   ( || (____/\   | |     | (___) || )        | (____/\| (____/\| )  \  || )   ( |
    (_______/(_______/(_______)|/     \|(_______/   \_/     (_______)|/         (_______/(_______/|/    )_)|/     \|
");
            Console.ResetColor(); // Alaphelyzetbe állítja a szöveg színét
        }

        public static async Task DisplayWelcomeMessageAsync()
        {
            Console.SetCursorPosition((Console.WindowWidth - "Üdvözöllek az idei aréna játékokon!".Length) / 2, Console.CursorTop);
            Console.WriteLine("Üdvözöllek az idei aréna játékokon!\n");

            Console.SetCursorPosition((Console.WindowWidth - "Minden nagy démon támadás előtt Lena hősnő emlékére rendezzük meg a viadalt, hogy kiválasztjuk a következő hőst.".Length) / 2, Console.CursorTop);
            Console.WriteLine("Minden nagy démon támadás előtt Lena hősnő emlékére rendezzük meg a viadalt, hogy kiválasztjuk a következő hőst.\n");
            await DisplayArenaArtAsync(); // ARENA.txt megjelenítése
        }

        public static async Task PlayWelcomeSoundAsync()
        {
            string currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string soundFileName = "arena.wav";
            string soundFilePath = Path.Combine(currentDirectory, "MUSICS", soundFileName);

            using (var player = new SoundPlayer(soundFilePath))
            {
                player.PlayLooping();
                await Task.Delay(2000); // Várakozás 2 másodpercig (változtatható)
            }
        }

        private static async Task DisplayArenaArtAsync()
        {
            string currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string artFilePath = Path.Combine(currentDirectory, "ARTS", "ARENA.txt");

            if (File.Exists(artFilePath))
            {
                using (StreamReader reader = new StreamReader(artFilePath))
                {
                    string line;
                    while ((line = await reader.ReadLineAsync()) != null)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow; // Szöveg színe zöld
                        Console.WriteLine(line);
                        Console.ResetColor(); // Alaphelyzetbe állítja a szöveg színét
                    }
                }
            }
        }
    }
}
