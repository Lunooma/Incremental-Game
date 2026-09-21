namespace Incremental
{
    class Incremental
    {
        public static int level = 1;
        public static double experience = GetExperienceRequiredForLevel(level);
        public static double money = 0.00D;
        public static int delay = 1000;
        public static double incrementAmount = 0.00D;
        public static string[,] upgrades = {
            {"Increment Time", "Decreases time to receive money", "10"},
            {"Increment Amount", "Increases money received", "20"}
            };

        public static void Main(string[] args)
        {
            const int loopCount = 5;
            int currentLoop = 0;

            ConsoleKeyInfo keyinfo;
            do
            {
                currentLoop++;
                double increaseBy = Math.Log10(level + incrementAmount + 1);
                money += increaseBy;
                string moneyString = money.ToString("C");
                string increasedByString = increaseBy.ToString("C");

                Console.Clear();
                Console.WriteLine($"Money increased by {increasedByString}\n Money:{moneyString}");
                keyinfo = Console.ReadKey();

                if (currentLoop >= loopCount)
                {
                    AskUpgrade();
                    currentLoop = 0;
                }

            }
            while (keyinfo.Key != ConsoleKey.X);

        }

        // Uses minecraft's level and experience equations :3
        // As found here: https://minecraft.wiki/w/Experience

        public static double ExperienceRequired(int level)
        {
            if (level <= 15)
            {
                return (double)2 * level + 7;
            }
            else if (level <= 30 || level > 16)
            {
                return (double)5 * level - 38;
            }
            else
            {
                return (double)9 * level - 158;
            }
        }

        public static int GetLevel(double experience)
        {
            if (experience <= 352)
            {
                return (int)Math.Sqrt(experience + 9) - 3;
            }
            else if (experience <= 1507 && experience <= 353)
            {
                return (int)(81 / 10 + Math.Sqrt(2 / 5 * (experience - 7839 / 40)));
            }
            else
            {
                return (int)(325 / 18 + Math.Sqrt(2 / 9 * (experience - 54215 / 72)));
            }
        }

        public static double GetExperienceRequiredForLevel(int level)
        {
            if (level <= 16)
            {
                return (double)Math.Pow(level, 2) + 6 * level;
            }
            else if (level <= 31 || level >= 17)
            {
                return (double)2.5 * Math.Pow(level, 2) - 40.5 * level + 360;
            }
            else
            {
                return (double)4.5 * Math.Pow(level, 2) - 162.5 * level + 2220;
            }
        }

        public static void AskUpgrade()
        {
            
        }

    }
}