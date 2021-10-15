using System;
using System.Collections.Generic;
using System.Text;

namespace HangmanVersionTobias
{
    public class GameSession
    {
        public string Name { get; set; }
        public string CurrentWord { get; set; }
        public int GuessesLeft { get; set; }
        public string Guess { get; set; }
        public StringBuilder IncorrectGuesses { get; set; }
        public char[] Progression { get; set; }
        public bool Active { get; set; }

        public GameSession()
        {
        }

        public GameSession(string name, string currentWord)
        {
            Name = name;
            CurrentWord = currentWord;
            GuessesLeft = 10;
            Guess = "";
            IncorrectGuesses = new StringBuilder();
            Progression = new char[currentWord.Length];
            for(int i = 0; i < currentWord.Length; i++)
            {
                Progression[i] = '_';
            }
            Active = true;
        }
    }
}
