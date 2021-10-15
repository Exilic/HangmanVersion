using System;
using System.Text;
using System.Text.RegularExpressions;

namespace HangmanVersionTobias
{
    public static class MainClass
    {
        public static void Main(string[] args)
        {
            GameSession gameSession = NewGame();
            while (gameSession.Active)
            {
                gameSession
                .GetValidGuess()
                .DetermineNextState()
                .EstablishEndOfTurn();
            }
        }

        public static GameSession GetValidGuess(this GameSession gameSession)
        {
            do
            {
                PresentCurrentState(gameSession);
                gameSession.Guess = GetUserInput("Enter a single letter or the whole word:");
            } while (DetermineValidity(gameSession));           
            return gameSession;
        }

        public static GameSession DetermineNextState(this GameSession gameSession)
        {
            gameSession.GuessesLeft--;
            if (gameSession.Guess.Length == 1 && GuessIsCorrect(gameSession))
            {
                gameSession.Progression = InsertLetter(gameSession);
            }
            return gameSession;
        }

        public static GameSession EstablishEndOfTurn(this GameSession gameSession)
        {
            if (EndOfTurn(gameSession)) AskForNewTurn(gameSession);         
            return gameSession;
        }

        public static bool GuessIsCorrect(GameSession gameSession)
        {
            return gameSession.Progression.ToString()
                .Contains(gameSession.Guess)? true : false;
        }

        public static char[] InsertLetter(GameSession gameSession)
        {
            char guess = char.Parse(gameSession.Guess);
            char[] word = gameSession.CurrentWord.ToCharArray();
            for (int i = 0; i < gameSession.Progression.Length; i++)
            {
                if (word[i] == guess) gameSession.Progression[i] = guess;
            }
            return gameSession.Progression;
        }

        public static bool EndOfTurn(GameSession gameSession)
        {
            return RightGuess(gameSession)? true
                : gameSession.GuessesLeft == 0? true : false;
        }

        public static bool RightGuess(GameSession gameSession)
        {
            if (gameSession.Guess.Equals(gameSession.CurrentWord))
            {
                GetUserInput($"Your guess '{ gameSession.CurrentWord }' was the right word! You won! Press Enter to continue.");
                return true;
            }
            return false;
        }

        public static GameSession AskForNewTurn(GameSession gameSession)
        {
            string respons = GetUserInput($"Enter x to stop playing. Enter any other key to continue.");
            gameSession.Active = false;
            return respons.Equals("x")
                ? gameSession
                : new GameSession(gameSession.Name, GetRandomWord(NewWordList()));            
        }

        public static void PresentCurrentState(GameSession gameSession)
        {
            Console.Clear();
            Console.WriteLine($"The word to figure out: {gameSession.Progression}\n");
            Console.WriteLine($"Faulty letter guesses so far: {gameSession.IncorrectGuesses}");
            Console.WriteLine($"You have {gameSession.GuessesLeft} guesses left.\n");
        }

        public static string GetUserInput(string phrase)
        {
            Console.WriteLine(phrase + "\n");
            return Console.ReadLine();
        }

        public static bool DetermineValidity(GameSession gameSession)
        {            
            if(DetermineOnlyLetters(gameSession.Guess) == false) return false;
            if(RightAmountOfLetters(gameSession) == false) return false;
            if(NoRepeatGuess(gameSession) == false) return false;
            return true;
        }
        
        public static bool DetermineOnlyLetters(string guess)
        {
            if(Regex.IsMatch(guess, "[a-zA-Z]") == false)
            {
                GetUserInput("Only input letters, please. Press Enter to continue.");
                return false;
            }
            return true;
        }

        public static bool RightAmountOfLetters(GameSession gameSession)
        {
            if(gameSession.Guess.Length != gameSession.CurrentWord.Length
                && gameSession.Guess.Length > 1)
            {
                GetUserInput($"Word guesses should be {gameSession.CurrentWord.Length} letters long. Press Enter to continue.");
                return false;
            }
            return true;
        }

        public static bool NoRepeatGuess(GameSession gameSession)
        {
            if (gameSession.IncorrectGuesses.ToString().Contains(gameSession.Guess))
            {
                GetUserInput($"You have already guessed that letter. Press Enter to continue.");
                return false;
            }
            return true;
        }

        public static GameSession NewGame()
        {
            return new GameSession(GetName(), GetRandomWord(NewWordList()));
        }

        public static string GetName()
        {
            Console.WriteLine("What do you want to call yourself while playing Hangman?");
            string name = Console.ReadLine();
            Console.Clear();
            return name.Length > 0 ? name : "Player";
        }

        public static string GetRandomWord(string[] words)
        {
            return words[new Random().Next(0, words.Length)];
        }

        public static string[] NewWordList()
        {
            return new string[] { "Groda", "Fritid", "Klassiker", "Manuell", "Kloster", "Livbåt", "Motion" };
        }
    }
}
