using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUp : MonoBehaviour
{

    void Start()
    {
        Database database = FindObjectOfType<Database>();
        float targetScale = database.constants_faceComponentScale;
        float speed = database.constants_popSpeed;

        StartCoroutine(Pop(targetScale, speed));
    }

    IEnumerator Pop(float targetScale, float speed)
    {
        while (transform.localScale.x < targetScale)
        {
            transform.localScale += new Vector3(targetScale, targetScale) * Time.deltaTime * speed;
            yield return null;
        }

        if (transform.localScale.x > targetScale)
        {
            transform.localScale = new Vector3(targetScale, targetScale);
            yield break;
        }
    }
}
