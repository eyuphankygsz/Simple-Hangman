using System.Text;

namespace HangmanGame
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Hangman hangman = new Hangman();
            hangman.MainMenu();
        }
    }
    class Hangman
    {
        int health;
        bool correctInput;
        StringBuilder errorMessage = new StringBuilder(null);
        char input;
        bool won;
        List<char> triedLetters = new List<char>();

        StringBuilder hangmanSentence = new StringBuilder(null);
        StringBuilder guessedString = new StringBuilder(null);

        StringBuilder textFile = new StringBuilder(@"C:\Users\AmiralSincap\Documents\MEGA\source\repos\HangmanGame\");

        public void MainMenu()
        {

            while (!correctInput)
            {
                won = false;
                errorMessage.Clear();
                triedLetters.Clear();
                SetErrorMessage(null);

                PrintMainMenu();
                GetInput(1, 3);
                if (correctInput)
                {
                    Console.Clear();
                    MenuRouter("MainMenu");
                }
                Console.Clear();
            }
            Console.ReadLine();
        }

        void StartGame()
        {
            health = 5;
            while (true)
            {
                while (true)
                {
                    PrintGameScreen();
                    if (correctInput) break;
                    GetInput(1, 2);
                    if (correctInput)
                    {
                        Console.Clear();
                        MenuRouter("StartGame");
                        if (health == 0)
                            break;
                        
                    }
                    Console.Clear();
                }
                while (true)
                {
                    EndScreen();
                    GetInput(1, 2);
                    if (correctInput)
                    {
                        Console.Clear();
                        MenuRouter("EndScreen");
                        break;
                    }
                    Console.Clear();

                }
                break;
            }
        }


        int SetHealth(bool increase)
        {
            return increase ? health + 1 : health - 1;
        }
        void SetErrorMessage(string error)
        {
            errorMessage.Clear();
            errorMessage.Append(error);
        }
        void PrintMainMenu()
        {
            Console.WriteLine("Welcome to the Hangman Game!");
            Console.WriteLine("Please select an option:");
            Console.WriteLine("1-) Write your own sentence / word");
            Console.WriteLine("2-) Select randomly");
            Console.WriteLine("3-) Exit");
            PrintErrorMessage();
        }

        void PrintGameScreen()
        {
            Console.Clear();
            Console.WriteLine($"Health: {health}");
            for (int i = 0; i < 3; i++)
            {
                if (i != 1)
                    for (int j = 0; j < hangmanSentence.Length; j++)
                        Console.Write('*');
                else
                    CheckLetters();
                Console.Write('\n');
                Console.Write('\n');
            }
            Console.WriteLine("Please select an option:");
            Console.WriteLine("1-) Guess a letter/number");
            Console.WriteLine("2-) Guess the whole sentence/word");
            PrintErrorMessage();
        }

        void MenuRouter(string from)
        {
            correctInput = false;
            SetErrorMessage("");
            switch (input)
            {
                case '1':
                    if (from == "MainMenu")
                        UserSentence();
                    else if (from == "StartGame")
                        GuessLetter();
                    break;
                case '2':
                    if (from == "MainMenu")
                        FileSentence();
                    else if (from == "StartGame")
                        GuessWhole();
                    else if (from == "EndScreen")
                        Environment.Exit(0);
                    break;
                case '3':
                    if (from == "MainMenu")
                        Environment.Exit(0);
                    break;
            }
        }

        void GuessLetter()
        {
            do
            {
                PrintErrorMessage();
                CheckLetters();
                Console.Write('\n');
                Console.Write("Enter any letter/number:");
                input = Console.ReadKey().KeyChar;
                if (triedLetters.Contains(char.ToLower(input)) || triedLetters.Contains(char.ToLower(input)))
                {
                    SetErrorMessage("ERROR: You already tried this letter!");
                    input = '*';
                }
                Console.Clear();

            } while (input == '*');

            triedLetters.Add(input);
            if (!hangmanSentence.ToString().Contains(Char.ToUpper(input)) && !hangmanSentence.ToString().Contains(Char.ToLower(input)))
            {
               health = SetHealth(false);
            }
            SetErrorMessage(null);

        }
        void GuessWhole()
        {
            CheckLetters();
            Console.Write('\n');
            Console.Write("Enter your guess!:");
            string guess = Console.ReadLine();

            if (hangmanSentence.ToString().ToLower().Equals(guess.ToLower()))
            {
                won = true; 
                correctInput = true;
            }
            else
            {
                health = 0;
            }
            
            Console.Clear();
        }

        void CheckLetters()
        {
            guessedString.Clear();

            for (int j = 0; j < hangmanSentence.Length; j++)
            {
                char next = '_';
                if (triedLetters.Contains(char.ToUpper(@hangmanSentence[j])) || triedLetters.Contains(char.ToLower(@hangmanSentence[j])) || hangmanSentence[j] == ' ')
                    next = hangmanSentence[j];
                Console.Write(next);
                guessedString.Append(next);
            }
            if (hangmanSentence.Equals(guessedString))
            {
                won = true;
                correctInput = true;
            }
        }
        void UserSentence()
        {
            Console.Write("Write anything:");
            hangmanSentence.Clear();
            hangmanSentence.Append(Console.ReadLine());
            StartGame();
        }
        void FileSentence()
        {
            textFile.Clear();
            textFile.Append(@"C:\Users\AmiralSincap\Documents\MEGA\source\repos\HangmanGame\");
            while (true)
            {
                Console.WriteLine("Select a category:");
                Console.WriteLine("1-) Celebrities");
                Console.WriteLine("2-) Games");
                Console.WriteLine("3-) Movies");
                PrintErrorMessage();
                GetInput(1, 3);
                if (correctInput)
                {
                    switch (input)
                    {
                        case '1':
                            textFile.Append("celebrities.txt");
                            break;
                        case '2':
                            textFile.Append("games.txt");
                            break;
                        case '3':
                            textFile.Append("movies.txt");
                            break;
                    }
                    break;
                }
            }
            correctInput = false;
            if (File.Exists(textFile.ToString()))
            {
                List<string> sentences = @File.ReadAllLines(textFile.ToString()).ToList();
                Random rand = new Random();
                hangmanSentence.Clear();
                hangmanSentence.Append(@sentences[rand.Next(0, sentences.Count)]);
                StartGame();
            }
        }



        void GetInput(int min, int max)
        {
            input = Console.ReadKey().KeyChar;

            if (input >= '0' + min && input <= '0' + max)
            {
                SetErrorMessage(null);
                correctInput = true;
            }
            else
            {
                SetErrorMessage($"ERROR: Please select a correct option! (Between {min} to {max})");
                correctInput = false;
            }
        }

        void EndScreen()
        {
            Console.Clear();
            if (won)
                Console.WriteLine("Congrats! You've won!");
            else
            {
                Console.WriteLine("You've lost!");
                Console.WriteLine(@$"The Sentence was {hangmanSentence}");
            }

            Console.WriteLine("Do you want to start again?");
            Console.WriteLine("1:Yes");
            Console.WriteLine("2:No");
        }

        void PrintErrorMessage()
        {
            if (!string.IsNullOrEmpty(errorMessage.ToString()))
                Console.WriteLine(errorMessage);
        }
    }
}