using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Media;
using System.Text;

namespace ArenaGame
{
    public static class Battle
    {
        public static void StartBattle(List<Hero> heroes)
        {
            Console.WriteLine(Table.DisplayHeroesTable(heroes));
            Console.WriteLine("Válassz csata típust:");
            Console.WriteLine("1. Rendes csata (Harcok végig nézése)");
            Console.WriteLine("2. Gyors csata (Csak az eredmények érdekelnek)");

            char choice = Console.ReadKey().KeyChar;
            bool isFastBattle = (choice == '2');

            Console.Clear();

            Random random = new Random();
            var aliveHeroes = heroes.Where(hero => hero.CurrentHealth > 0).ToList();

            int round = 1;
            while (aliveHeroes.Count > 1)
            {
                WriteLog($"\n{round}. kör:");


                (Hero attacker, Hero defender) = SelectFighters.SelectRandomFighters(aliveHeroes);

                attacker.Battle = 1;
                defender.Battle = 1;
                attacker.BattleCounter += 1;
                defender.BattleCounter += 1;

                if (!isFastBattle)
                {
                    string attackerClassName = RemoveAccents(attacker.Class);
                    string defenderClassName = RemoveAccents(defender.Class);

                    string battleFileName = $"{attackerClassName.Substring(0, 1)}{defenderClassName.Substring(0, 1)}.txt";

                    string battleFilePath = Path.Combine("BARTS", battleFileName);
                    if (File.Exists(battleFilePath))
                    {
                        string battleContent = File.ReadAllText(battleFilePath);
                        Console.WriteLine($"\n{battleContent}");
                    }
                    PlayClassSound(attacker.Class);
                }

                

                WriteLog($"\nTámadó: (#{attacker.UniqueID}) {attacker.Name} ({attacker.Class}) | Védő: (#{defender.UniqueID}) {defender.Name} ({defender.Class})");

                string attackType = GetAttackType(attacker.Class, defender.Class);

                if (attackType == "AttackerWins")
                {
                    int previousHealth = defender.CurrentHealth;
                    defender.CurrentHealth = 0;
                    WriteLog($"{attacker.Name} sikeresen eltalálta {defender.Name}-t, aki meghalt.");
                    string logDead = $"{defender.Name} (#{defender.UniqueID}) életerő változás: {previousHealth} -> 0 (-{previousHealth})";
                    WriteLog(logDead, false);
                }
                else if (attackType == "DefenderWins")
                {
                    int previousHealth = attacker.CurrentHealth;
                    attacker.CurrentHealth = 0;
                    WriteLog($"{defender.Name} sikeresen eltalálta {attacker.Name}-t, aki meghalt.");
                    string logDead = $"{attacker.Name} (#{attacker.UniqueID}) életerő változás: {previousHealth} -> 0 (-{previousHealth})";
                    WriteLog(logDead, false);
                }
                else if (attackType == "ILrando")
                {
                    double chance = random.NextDouble();
                    if (chance <= 0.4) // 40% esély
                    {
                        int previousHealth = defender.CurrentHealth;
                        defender.CurrentHealth = 0;
                        WriteLog($"{attacker.Name} sikeresen eltalálta {defender.Name}-t, aki meghalt.");
                        string logDead = $"{defender.Name} (#{defender.UniqueID}) életerő változás: {previousHealth} -> 0 (-{previousHealth})";
                        WriteLog(logDead, false);
                    }
                    else
                    {
                        WriteLog($"{attacker.Name} próbálkozott, de {defender.Name} kivédekezte a támadást.");
                    }
                }
                else if (attackType == "Draw")
                {
                    WriteLog($"{attacker.Name} és {defender.Name} harca egyenlő erővel zajlott, senki sem sérült.");
                }

                foreach (var hero in aliveHeroes)
                {
                    if (hero != attacker && hero != defender)
                    {
                        int previousHealth = hero.CurrentHealth;
                        int newHealth = hero.CurrentHealth + 10;
                        hero.CurrentHealth = Math.Min(newHealth, hero.MaxHealth);

                        int healedAmount = hero.CurrentHealth - previousHealth;

                        string logMessage = $"{hero.Name} (#{hero.UniqueID}) életerő változás: {previousHealth} -> {hero.CurrentHealth} (+{healedAmount})";
                        WriteLog(logMessage, false); // Csak a log fájlba írjuk ki
                    }
                }


                foreach (var hero in aliveHeroes)
                {
                    if (hero.Battle == 1 && hero.CurrentHealth > 0)
                    {
                        int previousHealth = hero.CurrentHealth;

                        int newHealth = hero.CurrentHealth / 2;
                        int quarterMaxHealth = hero.MaxHealth / 4;

                        if (newHealth < quarterMaxHealth)
                        {
                            WriteLog($"{hero.Name} meghalt a csata után, mivel túlságosan megsérült.", false);
                            hero.CurrentHealth = 0;
                        }
                        else
                        {
                            hero.CurrentHealth = newHealth;
                        }

                        int damageTaken = previousHealth - hero.CurrentHealth;

                        string logMessage = $"{hero.Name} (#{hero.UniqueID}) életerő változás: {previousHealth} -> {hero.CurrentHealth} (-{damageTaken})";
                        WriteLog(logMessage, false);
                    }
                }


                attacker.Battle = 0;
                defender.Battle = 0;

                aliveHeroes = aliveHeroes.Where(hero => hero.CurrentHealth > 0).ToList();
                if ((!isFastBattle && aliveHeroes.Count > 1) || (isFastBattle && aliveHeroes.Count > 1))
                {
                    WriteLog("-----------------------------Kör végi eredmény---------------------------");
                    WriteLog(Table.DisplayHeroesTable(heroes)); // Kiírja az aktuális hőstáblázatot és kimenti
                }
                else if (!isFastBattle)
                {
                    Console.WriteLine("-----------------------------Kör végi eredmény---------------------------");
                    Console.WriteLine(Table.DisplayHeroesTable(heroes)); // Kiírja az aktuális hőstáblázatot és kimenti
                }



                if (!isFastBattle)
                {
                    Console.SetCursorPosition(0, 0); // Kurzort az első sor elejére, ez azért kellet, hogy nagy számú harcos esetén a lista miatt a lényeg látszódjon.
                    Console.WriteLine("Nyomd le a 'Space' billentyűt a tovább lépéshez...");
                    while (Console.ReadKey().Key != ConsoleKey.Spacebar) { }
                    Console.Clear();

                }
                round++;
            }

            if (aliveHeroes.Count == 1)
            {
                WriteLog($"{aliveHeroes[0].Name} a torna győztese!");
                var winners = aliveHeroes.Where(hero => hero.BattleCounter > 0).ToList();

                if (winners.Count == 0)
                {
                    WriteLog("Ráadásul harc nélkül!!!");
                }

                Console.WriteLine("--------------------------------Végső eredmény---------------------------");
                Console.WriteLine(Table.DisplayHeroesTable(heroes));
                Console.WriteLine("Nyomd meg az 'Enter' billentyűt a kilépéshez...");
                while (Console.ReadKey(true).Key != ConsoleKey.Enter) { }
                Environment.Exit(0);

            }
            else if (aliveHeroes.Count == 0)
            {
                WriteLog($"{heroes.Last().Name} volt az utolsó csata nyertese, de végül meghalt a győzelem ellenére. Hiába nyerte meg az utolsó csatát, nem lett a tornának győztese. Így {heroes.Last().Name} csak tiszteletbeli győztes.");
            }
            WriteLog("--------------------------------Végső eredmény---------------------------");
            WriteLog(Table.DisplayHeroesTable(heroes));
            Console.WriteLine("Nyomd meg az 'Enter' billentyűt a kilépéshez...");
            while (Console.ReadKey(true).Key != ConsoleKey.Enter) { }
            Environment.Exit(0);
        }

        private static string GetAttackType(string attackerClass, string defenderClass)
        {
            if (attackerClass == "Íjász")
            {
                if (defenderClass == "Lovas") return "ILrando";
                else if (defenderClass == "Kardos") return "AttackerWins";
                else if (defenderClass == "Íjász") return "AttackerWins";
            }
            else if (attackerClass == "Kardos")
            {
                if (defenderClass == "Lovas") return "Draw";
                else if (defenderClass == "Kardos") return "DefenderWins";
                else if (defenderClass == "Íjász") return "AttackerWins";
            }
            else if (attackerClass == "Lovas")
            {
                if (defenderClass == "Lovas") return "AttackerWins";
                else if (defenderClass == "Kardos") return "DefenderWins";
                else if (defenderClass == "Íjász") return "AttackerWins";
            }

            return "Draw";
        }

        public static string RemoveAccents(string input)
        {
            input = input.Normalize(NormalizationForm.FormD);
            var sb = new StringBuilder();

            foreach (char c in input)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(c);
                }
            }

            return sb.ToString();
        }

        private static void PlayClassSound(string className)
        {
            string soundFileName = $"{RemoveAccents(className.Substring(0, 1))}.wav";
            string soundFilePath = Path.Combine("MUSICS", soundFileName);

            if (File.Exists(soundFilePath))
            {
                using (var player = new SoundPlayer(soundFilePath))
                {
                    player.Play();
                }
            }
        }

        private static void WriteLog(string text, bool writeToConsole = true)
        {
            if (writeToConsole)
            {
                Console.WriteLine(text);
            }

            DateTime currentTime = DateTime.Now;
            string logFileName = $"{currentTime.Year}_{currentTime.Month:00}_{currentTime.Day:00}_{currentTime.Hour:00}_{currentTime.Minute:00}.txt";
            string logFilePath = Path.Combine("LOG", logFileName);
            Directory.CreateDirectory(Path.GetDirectoryName(logFilePath)); // Ellenőrzi és létrehozza a LOG mappát szükség esetén

            using (StreamWriter writer = File.AppendText(logFilePath))
            {
                writer.WriteLine(text);
            }
        }


    }
}