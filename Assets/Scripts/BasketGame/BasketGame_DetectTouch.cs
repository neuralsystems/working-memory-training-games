using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasketGame_DetectTouch : MonoBehaviour {
	// Use this for initialization

	public AudioClip bubbleBurst;
    public bool shouldTouch = false;

    void Start () {
//		Debug.Log ("in start");
	}
	
	// Update is called once per frame
	void Update () {
		// when the object is Touched


			if (Input.touchCount == 1)
			{
				Vector3 wp = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
				Vector2 touchPos = new Vector2(wp.x, wp.y);
				if (GetComponent<Collider2D>() == Physics2D.OverlapPoint(touchPos))
				{
					// add the code for execuation  on tap 
//					Destroy(this.gameObject);
                    
                   OnMouseDown();
                   
					

				}
			}
		}
		


	public void OnMouseDown(){
        if (gameObject.tag == BasketGame_SceneVariables.fruitTag)
        {
            shouldTouch = true;
        }
        if (shouldTouch)
        {
            
            if (tag == BasketGame_SceneVariables.baloonTag)
            {
                Debug.Log("vame in of");
                shouldTouch = false;
                StartCoroutine(BalloonClick());
            }
            else
            {
                //		Debug.Log ("touched on the bubble");
                //		Destroy (this.gameObject);
                //		GetComponent<Rigidbody2D>().isKinematic = true;
                gameObject.AddComponent<Rigidbody2D>();
                //		GetComponent<SpriteRenderer>().sortingLayerName = "Game";
                foreach (Transform child in gameObject.transform)
                {
                    Destroy(child.gameObject);
                    //			StartCoroutine(child.GetComponent<BasketGame_BubbleBehavior> ().BeforeGoDestroy ());
                }
                GetComponent<AudioSource>().PlayOneShot(bubbleBurst);
                GetComponent<ParticleSystem>().Play();
                iTween.Stop(gameObject);
                SetBoxCollider(.2f);
            }
        }
//		StartCoroutine(GetComponent<FruitBehavior> ().Fall ());
	}

	public void SetBoxCollider(float percent = 1f){
		Vector2 s = GetComponent<SpriteRenderer> ().bounds.size;
		s.x *= percent;
		s.y *= percent;
		GetComponent<CircleCollider2D>().radius = s.x;
	}

    IEnumerator BalloonClick()
    {
        GetComponent<Scalling>().SetScale(false);
        var ps = GetComponent<ParticleSystem>();
        var sr = GetComponent<SpriteRenderer>().bounds;
        var main = ps.main;
        iTween.Stop(gameObject);
        //main.startColor = GetComponent<SpriteRenderer>().sprite.texture.GetPixel(Mathf.RoundToInt(sr.size.x / 2), Mathf.RoundToInt(sr.size.y / 2));
        //ps.Play();
        if (bubbleBurst)
        {
            GetComponent<AudioSource>().PlayOneShot(bubbleBurst);
            yield return new WaitForSeconds(bubbleBurst.length);
        }
        GetComponent<BasketGame_PreBaloonScript>().ConvertToFruit();
    }

    public void SetTouch(bool value)
    {
        Debug.Log("Set Values with " + value);
        //		GetComponent<TrainGame_DetectTouch> ().enabled = value;
        shouldTouch = value;
        //GetComponent<BoxCollider2D>().enabled = value;
        
    }

    public void Flip()
    {
        SetTouch(!shouldTouch);
    }
}
