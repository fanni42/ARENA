namespace ArenaGame
{
    public class Hero
    {
        public required string Name { get; set; }
        public required string Class { get; set; }
        public int MaxHealth { get; set; }
        public int CurrentHealth { get; set; }
        public int Battle { get; set; }
        public int BattleCounter { get; set; }
        public int UniqueID { get; set; }
    }
}
