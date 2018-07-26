using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeItRainObjects : MonoBehaviour {

	public GameObject rainingObjectPrefab;
	private GameObject rainingObject;
	private float timer = 0f;
	private List<float> rainingObjectPosition = new List<float>();
	private int count = 0;
    private float rainingTime;
    private float waitingTime;
    private float positionsCount;
    private Vector3 screenSize;

    void Start() {

        Database database = FindObjectOfType<Database>();
        rainingTime = database.constants_rainingTime;
        waitingTime = database.constants_rainingIntervalTime;
        positionsCount = (float)database.constants_rainingPositionsCount;

		screenSize = Camera.main.ViewportToWorldPoint(new Vector3(database.viewPortHeight, database.viewPortWidth));

        //fixing positions from which objects fall
        for(float i=0f; i<positionsCount; i = i+1) {
            rainingObjectPosition.Add(((2 * i + 1)/(positionsCount))*(screenSize.x));
		}
	}

	void Update() {
	
		timer += Time.deltaTime;

        //count denotes no of raining objects instantiated by far
		if(count*waitingTime < rainingTime)
        {
			// Spawn a new block repeatedly
			if (timer > waitingTime)
            {
                //Instantiate object
				rainingObject = Instantiate(rainingObjectPrefab);
                count++;
				
                //Set position : x is random(from a list), y is just above screen
                float aboveScreenPosition = rainingObject.transform.GetComponent<SpriteRenderer>().bounds.size.y + screenSize.y; 
                int i = Random.Range (0, (int)positionsCount-1);
                rainingObject.transform.position = new Vector3(rainingObjectPosition[i] - screenSize.x, aboveScreenPosition, 0);
				
                //reset timer
                timer -= waitingTime;
			}
		}
	}

}
