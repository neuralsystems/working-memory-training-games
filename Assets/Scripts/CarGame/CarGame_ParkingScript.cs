using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CarGame_ParkingScript : MonoBehaviour {

	public Button ParkingSlab;
	public Image ParkedImage;

	private Parking parking_object;
	private CarGame_ParkingList parkinglist_gameobject;
	// Use this for initialization
	void Start () {
		
	}

	public void SetUp(Parking currentparking, CarGame_ParkingList curren_parkinglist){
		parking_object = currentparking;
		ParkedImage.sprite = currentparking.vehicleImage;
		if (currentparking.vehicleImage == null) {
			ParkedImage.enabled = false;
		}
		parkinglist_gameobject = curren_parkinglist;
	}

}
