# Arena - Konzolos Viadal Szimuláló Játék

Az "Arena" egy C# konzolos játék alkalmazás, amely egy egyszerű viadalt szimulál, ahol hősök harcolnak egymással. A játékban a játékos lehetőséget kap arra, hogy kiválassza a harcosok számát, majd a hősök különböző osztályokba tartoznak (Íjász, Kardos, Lovas) és egymás ellen harcolnak. A harcok során a harcosok egymásra támadnak, és a végeredményt a támadás típusa és a védekezés alapján határozzák meg.

## Fájlstruktúra

- `Program.cs`: A játék indító pontja, ami a menüt és a játék inicializálását tartalmazza.
- `Welcome.cs`: Üdvözlő szövegek és hang lejátszásáért felelős modul.
- `Hero.cs`: A `Hero` osztályt tartalmazza, ami a hősök tulajdonságait és állapotát reprezentálja.
- `HeroGenerator.cs`: Hősök generálásáért felelős osztály, amely generálja a neveket és tulajdonságokat.
- `Battle.cs`: A játék fő részét alkotja, ahol a harcok zajlanak le és az eredmények kerülnek kiértékelésre.
- `SelectFighters.cs`: Két véletlenszerű harcos kiválasztásáért felelős osztály.
- `Table.cs`: A hősök táblázatos megjelenítését kezeli.
- `MUSICS` mappa: Hangfájlok tárolására szolgál.
- `ARTS` mappa: ASCII művészeti alkotásokat tartalmaz, amelyek a játék megjelenítéséhez szükségesek.

## Játékmenü és Torna

A játék menüjében új tornát indíthat a játékos, ahol kiválaszthatja a résztvevő harcosok számát. A harcosok véletlenszerű neveket és osztályokat kapnak. A harcok szimulálásában a támadási típusok és védekezés alapján dől el, hogy ki marad életben.
