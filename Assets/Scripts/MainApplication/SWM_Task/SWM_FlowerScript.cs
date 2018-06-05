using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SWM_FlowerScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public IEnumerator MoveToTarget(GameObject target)
    {
        float smoothTime = .1f;
        Vector3 velocity = Vector3.zero;
        while(Vector3.Distance(transform.position, target.transform.position) > 0.01f)
        {
            transform.position = Vector3.SmoothDamp(transform.position, target.transform.position, ref velocity, smoothTime);
            yield return null;
        }
        transform.position = target.transform.position;
        transform.parent = target.transform;
        target.GetComponent<SpriteRenderer>().enabled = false;
        Camera.main.GetComponent<SWM_GameManager>().SetScene();
    }
}
