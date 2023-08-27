using System;
using System.Collections.Generic;
using System.Text;

namespace ArenaGame
{
    public static class Table
    {
        public static string DisplayHeroesTable(List<Hero> heroes)
        {
            StringBuilder tableBuilder = new StringBuilder();

            tableBuilder.AppendLine("|   Azonosító   |       Név       |       Kaszt       |      Életerő    |");
            tableBuilder.AppendLine("|---------------|-----------------|-------------------|-----------------|");

            foreach (var hero in heroes)
            {
                tableBuilder.AppendLine($"| {hero.UniqueID,-13} | {hero.Name,-15} | {hero.Class,-17} | {hero.CurrentHealth,11}/{hero.MaxHealth,-3} |");
            }

            tableBuilder.AppendLine();

            string tableContent = tableBuilder.ToString();

            return tableContent; // Visszaadás string-ként
        }
    }
}
