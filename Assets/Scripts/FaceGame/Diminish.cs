using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Diminish : MonoBehaviour
{

    void Start()
    {
		Database database = new Database();
        float speed = database.constants_diminishSpeed;

        StartCoroutine(Dim(speed));
    }

    IEnumerator Dim(float speed)
    {

        while (transform.localScale.x > 0)
        {
            transform.localScale -= new Vector3(1, 1, 1) * Time.deltaTime * speed;
            yield return null;
        }

        if (transform.localScale.x < 0)
        {
            transform.localScale = new Vector3(0, 0, 0);
            yield break;
        }

    }
}
