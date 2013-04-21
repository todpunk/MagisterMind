using System;
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
		public static List<CodeSet> Guesses;
		public static CodeSet TheCode;
		public static int GuessesMade;
		public static string LastLetterChoosable;

		static void Main(string[] args) {
			bool playing = true;
			LastLetterChoosable = CodeSet.CodeItems[ProgramConfig.CodesPossible].ToString();

			while (playing) {
				SetupNewGame();
				Console.WriteLine("Guess the code!  We need " + ProgramConfig.CodeSize + " guesses from A to " + LastLetterChoosable);
				Console.WriteLine("For each guess, we'll tell you R or C.  Either you have the (R)ight color in the right place, or at least the (C)olor is right.  We won't tell you which spot is R or C.");
				Console.WriteLine("You have " + ProgramConfig.Guesses + " guesses.");
				while (GuessesMade < ProgramConfig.Guesses) {
					Console.WriteLine("What's your guess? ");
					GetGuess();
					// TODO: How best to check if we've won, and how do we want to output the Right color/spot results?
					if (Guesses[GuessesMade].Equals(TheCode)) {
						Console.WriteLine("Pins: RRRR");
						Console.WriteLine("Congratulations, you guessed the code!");
						GuessesMade = ProgramConfig.Guesses;
					}
					else {
						Console.WriteLine(GetPins());
					}
					GuessesMade++;
				}
				Console.Write("Continue playing? (Y/N)");
				if ("Y" == Console.ReadLine().ToUpper()) {
					playing = true;
					Console.WriteLine("OK, we'll setup another round!");
				}
				else {
					playing = false;
					Console.WriteLine("Thanks for playing!\n(Press any key to end)");
					Console.ReadKey();
				}
			}

		}

		private static string GetPins() {
			int[] rightPins = new int[ProgramConfig.CodeSize];
			int[] closePins = new int[ProgramConfig.CodeSize];
			int codeSpot = 0;
			int guessSpot = 0;
			CodeSet Guess = Guesses[GuessesMade];
			string ret = "";
			
			// First compare a set for Right Pins
			while (codeSpot != ProgramConfig.CodeSize) {
				if (TheCode.Items[codeSpot] == Guess.Items[codeSpot]) {
					rightPins[codeSpot] = 1;
					ret = ret + "R ";
				}
				codeSpot++;
			}

			// Then recheck for correct color but not correct spot
			while (guessSpot != ProgramConfig.CodeSize) {
				if (rightPins[guessSpot] == 1 || guessSpot == codeSpot)
				{
					guessSpot++;
					continue;
				}

				codeSpot = 0;
				while (codeSpot != ProgramConfig.CodeSize) {
					if (closePins[codeSpot] == 1 || rightPins[codeSpot] == 1) {
						codeSpot++;
						continue;
					}
					if (TheCode.Items[codeSpot] == Guess.Items[guessSpot]) {
						closePins[codeSpot] = 1;
						ret = ret + "C ";
					}
					codeSpot++;
				}
				guessSpot++;
			}

			return ret;
			throw new NotImplementedException();
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
					continue;
				}
				Console.WriteLine("Your guess: " + playerGuess);

				Guesses.Insert(GuessesMade, new CodeSet(ProgramConfig.CodeSize));
				try {
					((CodeSet) Guesses[GuessesMade]).SetFromString(playerGuess);
				}
				// We used to think this would only happen if this was the wrong length,
				// but now we enforce that so this is for problems we're not handling, because
				// we are lazy.
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
			Guesses = new List<CodeSet>(ProgramConfig.Guesses);
			TheCode = new CodeSet(ProgramConfig.CodeSize);
			TheCode.Randomize(ProgramConfig.CodesPossible);
			GuessesMade = 0;
		}

		private static string CleanInput(string strIn) {
            // Replace invalid characters with empty strings.
            //return Regex.Replace(strIn, "[a-zA-Z0-9 -]", "");
			return Regex.Replace(strIn.ToUpper(), "[^A-" + LastLetterChoosable + "]", "");
		}
	}
}
