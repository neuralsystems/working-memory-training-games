using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomizeArray : MonoBehaviour {

	public void Randomize<T>(List<T> list) {
		for (int i = list.Count - 1; i > 0; i--) {
			int r = Random.Range(0, i + 1);
			T tmp = list[i];
			list[i] = list[r];
			list[r] = tmp;
		}
	}

}
