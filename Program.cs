using System.IO.Compression;

namespace Incremental
{
    class Incremental
    {
        public static int level = 0;
        public static double experience = GetExperienceRequiredForLevel(level);
        public static double money = 0.00D;
        public static double incrementAmount = 0.00D;
        public static double experienceBase = 1.00D;
        public static string[,] upgrades = {
            {"Increment Amount ", "Increases money received, costs: ", "20"},
            {"Experience Base ", "Increases the base experience gain, costs: ", "50"}
            };

        public static void Main(string[] args)
        {
            const int loopCount = 50;
            int currentLoop = 0;

            if (File.Exists("save.txt"))
            {
                SetVariables("save.txt");
            }
            else
            {
                File.Create("save.txt");
                UpdateFile();
            }

            while (true)
            {
                currentLoop++;
                double increaseBy = Math.Log10(level + 1 + incrementAmount + 1);
                experience += experienceBase + level / 10.0D;

                if (experience >= GetExperienceRequiredForLevel(level + 1))
                {
                    experience = 0;
                    level++;
                    Print($"You have leveled up to level {level}! Congrats, press ANYTHING to continue!", ConsoleColor.Yellow, true);
                    Console.ReadLine();
                }

                money += increaseBy;
                string moneyString = money.ToString("C");
                string increasedByString = increaseBy.ToString("C");

                Print($"Money increased by {increasedByString}\n Money:{moneyString}", ConsoleColor.Cyan, true);
                Console.WriteLine(string.Format("\nExperience: {0:#.##}", experience));
                string? input = Console.ReadLine();

                if (input != "" && input != null)
                {
                    if (input.StartsWith("upgrade"))
                    {
                        if (input.EndsWith(""))
                        {
                            DoUpgrade();
                        }
                        else
                        {
                            
                        }
                    }
                }

                if (currentLoop >= loopCount)
                {
                    AskUpgrade();
                    currentLoop = 0;
                }
            }
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
            string moneyString = money.ToString("C");
            bool answer = AskYesNo(
            $"You have: {moneyString}\n Would you like to Upgrade something?\n(y/N)\n",
            ConsoleColor.Green);

            if (answer)
                DoUpgrade();

        }

        public static void DoUpgrade(int? index = -1)
        {
            Console.WriteLine($"Chose from 1-{upgrades.Length / 3}");
            int currentIndex = 0;

            for (int i = 0; i < upgrades.Length; i++)
            {
                if (i == 3)
                {
                    currentIndex += 1;
                    Console.WriteLine();
                    if (currentIndex >= upgrades.Length / 3)
                        break;

                    i = 0;
                }
                else if (i == 2)
                {
                    float cost = float.Parse(upgrades[currentIndex, i]) * (level + 1);
                    Print($"{cost}", ConsoleColor.Blue);
                }
                else
                    Print($"{upgrades[currentIndex, i]}", ConsoleColor.Blue);
            }

            index ??= -1;
            if (index == -1)
            {
                string? answer = Console.ReadLine();
                answer ??= "1";
                index = int.Parse(answer) - 1;
            }

            string chosenUpgrade;
            float chosenUpgradeCost;

            try
            {
                chosenUpgrade = upgrades[(int)index, 0];
                chosenUpgradeCost = float.Parse(upgrades[(int)index, 2]);
            }
            catch (System.Exception)
            {
                throw;
            }

            if (money >= chosenUpgradeCost)
            {
                switch (chosenUpgrade)
                {
                    case "Increment Amount ":
                        incrementAmount++;
                        Print($"{chosenUpgrade} is now {incrementAmount}");
                        break;
                    case "Experience Base ":
                        experienceBase++;
                        Print($"{chosenUpgrade} is now {experienceBase}");
                        break;
                    default:
                        ERROR($"chosenUpgrade: {chosenUpgrade} NOT FOUND");
                        break;
                }
            }
            else
                Print("Not enough money", ConsoleColor.Red);

            Console.ReadLine();

        }

        /// <summary>
        /// Input a message such as: "Continue?\n (Y/n)
        /// and ensure that the last argument is "y" and not "n" as "y" is the default in this case. 
        /// </summary>
        /// <param name="msg">The "question" to ask</param>
        /// <param name="color">Color of the text to print, can be empty</param>
        /// <param name="def">y or n, make sure to capitalise it appropriately in your msg</param>
        /// <returns></returns>
        public static bool AskYesNo(string msg, ConsoleColor? color)
        {
            color ??= ConsoleColor.White;
            Print(msg, color, true);
            string? answer = Console.ReadLine();
            return answer?.Trim().ToLower() == "y";
        }

        /// <summary>
        /// Faster way to print to console/display text.
        /// </summary>
        /// <param name="msg">Message to be printed</param>
        /// <param name="color">Color of message, can be empty</param>
        /// <param name="clear">Whether or not to clear console, defaults to false</param>
        public static void Print(string msg, ConsoleColor? color = ConsoleColor.White, bool? clear = false)
        {

            clear ??= false;

            if ((bool)clear)
                Console.Clear();

            color ??= ConsoleColor.White;

            Console.ForegroundColor = (ConsoleColor)color;
            Console.Write(msg);
            Console.ResetColor();

        }

        public static void ERROR(string msg)
        {
            Print(msg, ConsoleColor.Red);
            Environment.Exit(1);
        }

        public static void SetVariables(string? filePath = "./save.txt/")
        {
            filePath ??= "./save.txt/";
            string fileString = File.ReadAllText(filePath);

            Print($"File text: {fileString}");
            Console.ReadLine();
            /*string[] saveInfo = fileString[0].Split(';'); ;

            // 0;0;0.00;0.00;1.00 Default Info

            level = int.Parse(saveInfo[0]);
            money = double.Parse(saveInfo[1]);
            experience = GetExperienceRequiredForLevel(level);
            incrementAmount = double.Parse(saveInfo[2]);
            experienceBase = double.Parse(saveInfo[3]);
            */
        }

        public static void UpdateFile(string? filePath = "save.txt")
        {
            filePath ??= "save.txt";
            string save = $"{level};{money};{incrementAmount};{experienceBase}";
            File.WriteAllText(filePath, save);
        }

    }
}