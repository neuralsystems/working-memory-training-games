using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using cakeslice;

// !*** not a working copy ***!
public class Shared_MoveOnRandomPath : MonoBehaviour {

    float angle = 20.0f; // Maximum angle offset for new point 
    float speed = 8.0f; // Units per second 
    private float pos= 0.0f;
    private float segLength = 1.0f;
    private Vector3[] path = new Vector3[100];
    
    // Use this for initialization
    void Start () {
        path[path.Length - 2] = Vector3.zero;
        path[path.Length - 1] = new Vector3(segLength, segLength, 0);
        RecalcPath();
    }
	
	// Update is called once per frame
	void Update () {
        iTween.PutOnPath(gameObject, path, pos);
        var v3 = iTween.PointOnPath(path, Mathf.Clamp01(pos + .01f));
        transform.LookAt(v3);
        pos = pos + Time.deltaTime * speed / path.Length;
        if (pos >= 1.0) {
            pos -= 1.0f; RecalcPath();
        }
    }

    

     void RecalcPath() {
        var v3 = path[path.Length - 1] - path[path.Length - 2];
        path[0] = path[path.Length - 1];
        for (var i = 1; i < path.Length; i++)
        {
            //var q = Quaternion.AngleAxis(Random.Range(-angle, angle), Vector3.up);
            //v3 = q * v3;
            path[i] = path[i - 1] + v3;
        }
    }
}
