using System.Collections;
using System.Collections.Generic;
using System;

namespace ConsoleMagisterMind
{
	/// <summary>
	/// Represents a simple set of code items, either as the player's guess, or 
	/// the game's hidden code.
	/// </summary>
	public class CodeSet
	{
		public List<char> Items;
		// This is simply for translating between integers and whatever item I'll be outputting to the Console,
		// and taking in From there as well.
		// Used to be an Enum.  Harder to translate between in this console context.  We'll see about the GUI side.
		public static string CodeItems = "ABCDEFGHIJKL";


		public CodeSet(): this(CodeItems.Length) {
			// We sent this through the other Constructor so we have the flexibility but really only 
			// one code path to debug
		}

		public CodeSet(int CodeSize) {
			Items = new List<char>(CodeSize);
		}

		public CodeSet(string code) {
			Items = new List<char>(code.Length);
			SetFromString(code);
		}

		/// <summary>
		/// Set the CodeSet to a random set of items within the allowed set of results;
		/// </summary>
		public void Randomize(int possible) {
			int x = 0;
			Random ourRandom = new Random();
			while (x < Items.Capacity) {
				Items.Insert(x, CodeItems[ourRandom.Next(0, possible - 1)]);
				x++;
			}
		}

		/// <summary>
		/// Sets a CodeSet to an input String, converting to appropriate CodeItems
		/// </summary>
		/// <param name="input">The input string, which must be appropriate length</param>
		public void SetFromString(string input) {
			if (input.Length != Items.Capacity) {
				throw new Exception("Input string is not of appropriate length (" + Items.Capacity + ")");
			}
			int x = 0;
			input = input.ToUpper();
			while (x < Items.Capacity) {
				if (CodeItems.Contains(input[x].ToString())) {
					Items.Insert(x, input[x]);
					x++;
				}
				else {
					// We throw this exception because if we accept characters not in CodeItems, it will break other behaviors in unexpected ways
					throw new Exception("This character is not in CodeItems: " + input[x]);
				}
			}
		}

		/// <summary>
		///  Basic string representation for the console
		/// </summary>
		/// <returns>"Code:\tItems\n</returns>
		public override string ToString() {
			string returnValue = "Code:\t";

			int x = 0;
			while (x < Items.Capacity) {
				returnValue = returnValue + CodeItems[(int)Items[x]] + ", ";
				x++;
			}
			
			return returnValue + "\n";
		}

		/// <summary>
		/// Compares two codes
		/// </summary>
		/// <param name="other"> The other code to compare.</param>
		/// <returns>true if they are the same code</returns>
		public override bool Equals(object obj) {
			int x = 0;
			CodeSet other;
			try {
				other = (CodeSet) obj;
			}
			catch (Exception) {
				return false;
			}
			
			if (this.Items.Capacity != other.Items.Capacity) {
				return false;
			}

			while (x < Items.Capacity) {
				if (this.Items[x] != other.Items[x]) {
					return false;
				}
				x++;
			}

			return true;
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}
	}
}