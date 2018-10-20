using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothTransition : MonoBehaviour
{

	private Vector3 targetPosition;
	private Vector3 targetScale;
	private bool enable;
	private float smoothTime;
	private float speed;
	void SetValues()
	{

		smoothTime = Database.constants_smoothTime;
		speed = Database.constants_transitionSpeed;
	}

	public void SetTarget(Vector3 pos, Vector3 scal)
	{
		SetValues();

		targetPosition = pos;
		targetScale = scal;

		StartCoroutine(Transit());
	}

	public void SetTarget(Vector3 pos, Vector3 scal, float time, float speedVal)
    {
		SetValues();

        targetPosition = pos;
        targetScale = scal;
		smoothTime = time;
		speed = speedVal;

		StartCoroutine(Transit());
	}

	IEnumerator Transit()
	{
		Vector3 velocity = Vector3.zero;
		float epsilon = Database.constants_epsilon;

		while (new Vector3(Mathf.Abs(transform.position.x - targetPosition.x), Mathf.Abs(transform.position.y - targetPosition.y)).magnitude > epsilon)
		{
			transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
			transform.localScale = Vector3.Lerp(transform.localScale, targetScale, speed * Time.deltaTime);
			yield return null;
		}
		if (new Vector3(Mathf.Abs(transform.position.x - targetPosition.x), Mathf.Abs(transform.position.y - targetPosition.y)).magnitude > epsilon)
		{
			transform.position = targetPosition;
			transform.localScale = targetScale;
			yield break;
		}
	}

}
