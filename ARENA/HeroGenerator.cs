using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace ArenaGame
{
    class HeroGenerator
    {
        private const string NAMING_API_URL = "https://randommer.io/Name";
        private readonly string[] Classes = { "Íjász", "Kardos", "Lovas" };

        public async Task<List<Hero>> GenerateHeroes(int count)
        {
            List<Hero> heroes = new List<Hero>();

            using (HttpClient client = new HttpClient())
            {
                var formData = new MultipartFormDataContent();
                formData.Add(new StringContent("firstname"), "type");
                formData.Add(new StringContent(count.ToString()), "number");

                try
                {
                    var response = await client.PostAsync(NAMING_API_URL, formData);

                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        var nameArray = content.Replace("\"", "").Split(',');

                        Random random = new Random();

                        for (int i = 0; i < nameArray.Length; i++)
                        {
                            string randomClass = Classes[random.Next(0, Classes.Length)];

                            heroes.Add(new Hero
                            {
                                Name = nameArray[i].Trim('[', ']'),
                                Class = randomClass,
                                UniqueID = i + 1,
                                CurrentHealth = GetInitialHealthByClass(randomClass),
                                MaxHealth = GetInitialHealthByClass(randomClass)
                            });
                        }
                    }
                    else
                    {
                        Console.WriteLine("Kedves közönség a krónikásunk elkeverte a feljegyzéseit, amik a harcosok neveit tartalmazzák, így a feltüntet nevek csak ideiglenesek.");
                        GenerateRandomHeroes(heroes, count);
                    }
                }
                catch (HttpRequestException)
                {
                    Console.WriteLine("Kedves közönség a krónikásunk elkeverte a feljegyzéseit, amik a harcosok neveit tartalmazzák, így a feltüntet nevek csak ideiglenesek.");
                    GenerateRandomHeroes(heroes, count);
                }
            }

            return heroes;
        }

        private void GenerateRandomHeroes(List<Hero> heroes, int count)
        {
            Random random = new Random();

            for (int i = 0; i < count; i++)
            {
                string randomName = random.Next(2) == 0 ? "John Doe" : "Jane Doe";
                string randomClass = Classes[random.Next(0, Classes.Length)];

                heroes.Add(new Hero
                {
                    Name = randomName,
                    Class = randomClass,
                    UniqueID = i + 1,
                    CurrentHealth = GetInitialHealthByClass(randomClass),
                    MaxHealth = GetInitialHealthByClass(randomClass)
                });
            }
        }

        private int GetInitialHealthByClass(string heroClass)
        {
            switch (heroClass)
            {
                case "Íjász":
                    return 100;
                case "Kardos":
                    return 120;
                case "Lovas":
                    return 150;
                default:
                    return 100;
            }
        }
    }
}
