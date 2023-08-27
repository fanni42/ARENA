using System;
using System.Collections.Generic;
using System.Linq;

namespace ArenaGame
{
    public static class SelectFighters
    {
        public static (Hero attacker, Hero defender) SelectRandomFighters(List<Hero> heroes)
        {
            Random random = new Random();

            // Véletlenszerűen kiválasztunk egy támadó és egy védekező hőst
            Hero attacker = heroes[random.Next(0, heroes.Count)];
            Hero defender = heroes.Where(h => h.UniqueID != attacker.UniqueID).ToList()[random.Next(0, heroes.Count - 1)];

            return (attacker, defender);
        }
    }
}
