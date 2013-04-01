using System.Collections;
using System;

namespace ConsoleMagisterMind
{
	/// <summary>
	/// Represents a simple set of code items, either as the player's guess, or 
	/// the game's hidden code.
	/// </summary>
	public class CodeSet
	{
		public ArrayList items;

		public CodeSet() : this(ProgramConfig.CodeSize) {
			// We sent this through the other Constructor so we have the flexibility but really only 
			// one code path to debug
		}

		public CodeSet(int CodeSize) {
			items = new ArrayList(CodeSize);
		}

		/// <summary>
		/// Set the CodeSet to a random set of items within the allowed set of results;
		/// </summary>
		public void Randomize() {
			int x = 0;
			while (x < items.Capacity) {
				items[x] = new Random().Next(0, ProgramConfig.CodesPossible - 1);
				x++;
			}
		}

		/// <summary>
		///  Basic string representation for the console
		/// </summary>
		/// <returns>"Code:\tItems\n</returns>
		public override string ToString() {
			string r = "Code:\t";

			int x = 0;
			while (x < items.Capacity) {
				r = r + items[x] + ", ";
			}
			
			return r + "\n";
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
			
			if (this.items.Capacity != other.items.Capacity) {
				return false;
			}

			while (x < items.Capacity) {
				if (this.items[x] != other.items[x]) {
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