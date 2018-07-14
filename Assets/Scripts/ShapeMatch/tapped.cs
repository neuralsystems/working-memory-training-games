using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tapped : MonoBehaviour
{ 
    void Update ()
    {
		if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if(Physics.Raycast(ray, out hit))
            {
                transform.gameObject.GetComponent<tapDetect>().OnMouseDown(); // calling on mouse down fuction whenever touched
            }
        }
	}
}
