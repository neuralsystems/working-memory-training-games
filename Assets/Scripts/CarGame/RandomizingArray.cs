using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;
class RandomizingArray
{
	static Random _random = new Random();

	public static List<string> RandomizeStrings(string[] arr)
	{
		List<KeyValuePair<int, string>> list =
			new List<KeyValuePair<int, string>>();
		// Add all strings from array.
		// ... Add new random int each time.
		foreach (string s in arr)
		{
			list.Add(new KeyValuePair<int, string>(_random.Next(), s));
		}
		// Sort the list by the random number.
		var sorted = from item in list
			orderby item.Key
			select item;
		// Allocate new string array.
		string[] result = new string[arr.Length];
		// Copy values to array.
		int index = 0;
		foreach (KeyValuePair<int, string> pair in sorted)
		{
			result[index] = pair.Value;
			index++;
		}
		// Return copied array.
		return result.ToList();
	}

	public static List<Sprite> RandomizeSprite(Sprite[] arr)
	{
		List<KeyValuePair<int, Sprite>> list =
			new List<KeyValuePair<int, Sprite>>();
		// Add all strings from array.
		// ... Add new random int each time.
		foreach (Sprite s in arr)
		{
			list.Add(new KeyValuePair<int, Sprite>(_random.Next(), s));
		}
		// Sort the list by the random number.
		var sorted = from item in list
			orderby item.Key
			select item;
		// Allocate new string array.
		Sprite[] result = new Sprite[arr.Length];
		// Copy values to array.
		int index = 0;
		foreach (KeyValuePair<int, Sprite> pair in sorted)
		{
			result[index] = pair.Value;
			index++;
		}
		// Return copied array.
		return result.ToList();
	}

//	static void Main()
//	{
//		
//	}
}
