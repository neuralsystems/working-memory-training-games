using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Parking
{
	public Sprite vehicleImage;
}

public class CarGame_ParkingList : MonoBehaviour {

	public List<Parking> parking_list;
	public Transform contentPanel;
	public SimpleObjectPool carParkingPool;


	// Use this for initialization
	void Start () {
		RefreshDisplay ();		
	}

	public void RefreshDisplay (){

		AddParking ();
	}


	private void AddParking(){
		for (int i = 0; i < parking_list.Count; i++) {
			Parking parking_object = parking_list [i];
			GameObject newParkingSpot = carParkingPool.GetObject ();

			newParkingSpot.transform.SetParent (contentPanel);

			CarGame_ParkingScript parkingScript_object = newParkingSpot.GetComponent<CarGame_ParkingScript> ();
			parkingScript_object.SetUp (parking_object, this);
		}
	}

	// Update is called once per frame
	void Update () {
		
	}
}
