// File: core/Engine.cs (Updated to handle non-removing Morties)
using System;

namespace Rick_Morty_console_game.core
{
    public class Engine
    {
        Statistics game_statistics = new Statistics();
        private int switchedRound = 0;
        private int stayedRound = 0;
        private int switchedWins = 0;
        private int stayedWins = 0;

        public void Game(int amountOfBoxes, Type mortyType)
        {
            Console.Clear();
            Console.WriteLine($"Welcome to the Rick And Morty guess game!");

            while (true)
            {
                if (!PlayRound(amountOfBoxes, mortyType))
                    break;
            }

            Console.WriteLine("Morty: Okay... uh, bye!");

            var mortyInstance = (IMorty)Activator.CreateInstance(mortyType, new KeyManager(), amountOfBoxes)!;
            game_statistics.DisplayStatistics(mortyInstance, amountOfBoxes, stayedRound, switchedRound, stayedWins, switchedWins);
        }

        private bool PlayRound(int amountOfBoxes, Type mortyType)
        {
            int N = amountOfBoxes;
            var keyManager = new KeyManager();

            keyManager.GenerateKey(N);
            int round1 = keyManager.GetCurrentRound() - 1;

            var morty = (IMorty)Activator.CreateInstance(mortyType, keyManager, N)!;

            Console.WriteLine($"\nMorty: Oh geez, Rick, I'm gonna hide your portal gun in one of the {N} boxes, okay?");
            Console.WriteLine($"Morty: HMAC1={morty.GetHMAC(round1)}");

            int userChoice1 = GetUserChoice(N, $"Morty: Rick, enter your number [0,{N}) so you don't whine later that I cheated, alright?");
            int portalGunBox = keyManager.CalculateFairValue(userChoice1, N, round1);

            int selectedBox = GetUserChoice(N, $"Morty: Okay, okay, I hid the gun. What's your guess [0,{N})?");

            bool switched = false;
            int finalSelectedBox = selectedBox;
            bool won;
            if (morty.ShouldRemoveBoxes(selectedBox, portalGunBox))
            {
                var boxesToKeep = morty.SelectBoxesToKeep(selectedBox, portalGunBox, N, keyManager);

                Console.WriteLine($"Morty: I'm keeping the box you chose, I mean {selectedBox}, and the box {boxesToKeep[1]}.");
                while (true)
                {
                    Console.Write($"Morty: You can switch your box (enter {boxesToKeep[1]}), or, you know, stick with it (enter {selectedBox}).\nRick: ");
                    if (int.TryParse(Console.ReadLine(), out int finalChoice))
                    {
                        if (finalChoice == selectedBox)
                        {
                            switched = false;
                            finalSelectedBox = selectedBox;
                            break;
                        }
                        else if (finalChoice == boxesToKeep[1])
                        {
                            switched = true;
                            finalSelectedBox = boxesToKeep[1];
                            break;
                        }
                    }
                    Console.WriteLine($"Please enter either {selectedBox} or {boxesToKeep[1]}");
                }
            }
            else
            {
                Console.WriteLine($"Morty: Well Rick, looks like you're stuck with box {selectedBox}. Let's see what's inside!");
                switched = false;
                finalSelectedBox = selectedBox;
            }

            RevealGenerationDetails(keyManager, userChoice1, N);

            Console.WriteLine($"Morty: Your portal gun is in the box {portalGunBox}.");

            won = finalSelectedBox == portalGunBox;

            if (switched)
            {
                switchedRound++;
                if (won) switchedWins++;
            }
            else
            {
                stayedRound++;
                if (won) stayedWins++;
            }

            if (won)
            {
                Console.WriteLine("Morty: Aww geez, you won, Rick! That's... that's great, I guess.");
            }
            else
            {
                Console.WriteLine("Morty: Aww man, you lost, Rick. Now we gotta go on one of *my* adventures!");
            }

            while (true)
            {
                Console.Write("Morty: D-do you wanna play another round (y/n) or Exit?\nRick: ");
                string? response = Console.ReadLine();
                response = response?.ToLower().Trim();

                if (response == "y" || response == "yes")
                    return true;
                else if (response == "n" || response == "no" || response == "exit")
                    return false;

                Console.WriteLine("Please enter 'y', 'n', or 'exit'");
            }
        }

        private void RevealGenerationDetails(KeyManager keyManager, int userChoice1, int N)
        {
            for (int i = 0; i < keyManager.GetRoundCount(); i++)
            {
                Console.WriteLine($"Morty: Aww man, my {GetOrdinal(i + 1)} random value is {keyManager.GetMortyValue(i)}.");
                Console.WriteLine($"Morty: KEY{i + 1}={BitConverter.ToString(keyManager.GetKey(i)).Replace("-", "")}");

                if (i == 0)
                {
                    int fairValue = keyManager.CalculateFairValue(userChoice1, N, i);
                    Console.WriteLine($"Morty: So the {GetOrdinal(i + 1)} fair number is ({keyManager.GetMortyValue(i)} + {userChoice1}) % {N} = {fairValue}.");
                }
                else
                {
                    int range = (i == 1) ? N - 1 : N;
                    Console.WriteLine($"Morty: Uh, okay, the {GetOrdinal(i + 1)} fair number is used for box selection with range {range}.");
                }
            }
        }

        private string GetOrdinal(int number)
        {
            return number switch
            {
                1 => "1st",
                2 => "2nd",
                3 => "3rd",
                _ => $"{number}th"
            };
        }

        private int GetUserChoice(int maxValue, string message)
        {
            while (true)
            {
                Console.WriteLine(message);
                Console.Write("Rick: ");
                if (int.TryParse(Console.ReadLine(), out int choice) && choice >= 0 && choice < maxValue)
                    return choice;
                Console.WriteLine($"Incorrect input, enter number [0,{maxValue})");
            }
        }
    }
}