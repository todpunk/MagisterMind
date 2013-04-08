﻿using System;
using System.Collections.Generic;
using System.Collections;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;


namespace ConsoleMagisterMind
{
	class Program
	{
		public static ArrayList Guesses;
		public static CodeSet TheCode;
		public static int GuessesMade;
		public static string LastLetterChoosable;

		static void Main(string[] args) {
			bool playing = true;
			LastLetterChoosable = CodeSet.CodeItems[ProgramConfig.CodesPossible].ToString();

			while (playing) {
				SetupNewGame();
				Console.WriteLine("Guess the code!  We need " + ProgramConfig.CodeSize + " guesses from A to " + LastLetterChoosable);
				Console.WriteLine("You have " + ProgramConfig.Guesses + " guesses.");
				while (GuessesMade < ProgramConfig.Guesses) {
					Console.WriteLine("What's your guess? ");
					GetGuess();
					GuessesMade++;
					// TODO: How best to check if we've won, and how do we want to output the Right color/spot results?

				}
				Console.Write("Continue playing? (Y/N)");
				if ("Y" == Console.ReadLine().ToUpper()) {
					playing = true;
					Console.WriteLine("OK, we'll setup another round!");
				}
				else {
					playing = false;
					Console.WriteLine("Thanks for playing!");
				}
			}

		}

		public static void GetGuess() {
			bool guessing = true;
			while (guessing) {
				string playerGuess = Console.ReadLine();
				Console.WriteLine("Your guess: " + playerGuess);
                // We could get fancy here and compare that their guess is valid before cleanup, but we're just prototyping
				playerGuess = CleanInput(playerGuess);
				if (playerGuess.Length != ProgramConfig.CodeSize) {
					Console.WriteLine("That's not 4 characters long.  Your guess: " + playerGuess);
					Console.WriteLine("Try again.");
				}
				Console.WriteLine("Your guess: " + playerGuess);
				Guesses.Insert(GuessesMade, new CodeSet(ProgramConfig.CodeSize));
				try {
					((CodeSet) Guesses[GuessesMade]).SetFromString(playerGuess);
				}
				// The thought here is that we'll only get an exception if it's the wrong length.
				// This is, of course, rather silly of us and should probably be cleaned up in the future
				// ToDo: Somday, make this NOT a travesty of exception handling laziness
				catch (Exception e) {
					Console.WriteLine(e.Message);
					Guesses[GuessesMade] = null;
				}
				if (Guesses[GuessesMade] != null) {
					guessing = false;
				}
			}
		}

		public static void SetupNewGame() {
			Guesses = new ArrayList(ProgramConfig.Guesses);
			TheCode = new CodeSet();
			GuessesMade = 0;
		}

		private static string CleanInput(string strIn) {
            // Replace invalid characters with empty strings.
            //return Regex.Replace(strIn, "[a-zA-Z0-9 -]", "");
			return Regex.Replace(strIn.ToUpper(), "[^A-" + LastLetterChoosable + "]", "");
		}
	}
}
