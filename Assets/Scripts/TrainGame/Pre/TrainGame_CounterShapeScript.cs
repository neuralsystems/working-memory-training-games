using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using cakeslice;
public class TrainGame_CounterShapeScript : MonoBehaviour {

//	bool detectTouch = false;
	Vector3 velocity = Vector3.zero;
	public Vector3 original_position;
    private float minDistance = 0.1f;
	static bool Repeat = false;
    private string original_tag;
    private string hexForWrong = "#F80C1AFF";
	private string hexForNeutral = "#330CFF13";
	private string  hexForRight = "#72FF20FB";
	public AudioClip connect_sound;
    

	void Awake(){
		original_position = transform.position;
        original_tag = tag;
	}
	// Use this for initialization
	void Start () {
		
	}

	public void SetUp(Sprite shape){
        Reset();
        GetComponent<SpriteRenderer>().sprite = shape;
        transform.position = original_position;
        //SetTouchAndScale(true, true, true, tag);
        Repeat = false;
        foreach (Transform chi in transform)
        {
            chi.gameObject.GetComponent<TrainGame_CircleMaskScript>().SetColorAndVisibility(hexForNeutral, false);
        }
        var _is_correct = shape.name == TrainGame_PreGameManager.NAME_FOR_CORRECT;
        if (_is_correct)
        {
            var target_go = GameObject.Find(TrainGame_PreGameManager.SAMPLE_GO);
            transform.position = target_go.transform.position;
            GetComponent<SpriteRenderer>().enabled = true;
        }
        if(_is_correct)
        StartCoroutine(InitialSeperation(Camera.main.GetComponent<TrainGame_PreGameManager>().ShowInitialSeperation));


    }

    private void Reset()
    {
        GetComponent<SpriteRenderer>().enabled = false;
    }

    public IEnumerator InitialSeperation(bool val)
    {
        if (val)
        {
            var target = transform.position;
            target.x += GetComponent<SpriteRenderer>().bounds.size.x;
            yield return StartCoroutine(MoveToTarget(target, false, 2f));
            ResetPosition();
            
        }
        else
        {
            transform.position = original_position;
        }

        //SetTouchAndScale(true, true, true, tag);
    }
	
	public void OnMouseDown(){
        tag = TrainGame_SceneVariables.SELECTED_SHAPE_TAG;
        SetTouchAndScale(false, false, true, tag);
        StartCoroutine(MoveToCounter ());
	}

	IEnumerator MoveToCounter(){
		var target_go = GameObject.Find (TrainGame_PreGameManager.SAMPLE_GO);
		var target= target_go.transform.position;
		SetTouchAndScale(false,false,false ,original_tag);
		var is_correct = GetComponent<SpriteRenderer> ().sprite.name == TrainGame_PreGameManager.NAME_FOR_CORRECT;
        var smoothTime = .1f;
        if (is_correct)
        {
            smoothTime *= 5;
        }
		yield return StartCoroutine (MoveToTarget (target, is_correct, smoothTime));
		var sound_manager_go = GameObject.Find ("SoundManager");
		float extra_wait	= .2f;
		if (!is_correct) {
			yield return new WaitForSeconds (sound_manager_go.GetComponent<SoundManager_Script> ().PlaySadSound ());
			yield return new WaitForSeconds (1f);
			ResetPosition ();
			Camera.main.GetComponent<TrainGame_PreGameManager> ().ResetCorrect ();
			Repeat = true;
		} else {
			yield return new WaitForSeconds(sound_manager_go.GetComponent<SoundManager_Script>().PlaySound(connect_sound) + extra_wait);
			yield return new WaitForSeconds (sound_manager_go.GetComponent<SoundManager_Script> ().PlayHappySound () + extra_wait);
			if (!Repeat) {
				Camera.main.GetComponent<TrainGame_PreGameManager> ().IncrementOnCorrectMatch ();
			}
            tag = TrainGame_SceneVariables.COUNTER_SHAPE_OPTION_TAG;
            Camera.main.GetComponent<TrainGame_PreGameManager> ().LoadNext ();
		}
	}


	IEnumerator MoveToTarget(Vector3 target, bool is_correct, float smoothTime){
		while (Vector3.Distance (transform.position, target) > Mathf.Min (minDistance, 0.1f)) {
			transform.position = Vector3.SmoothDamp (transform.position, target, ref velocity, smoothTime);
			yield return null;
		}
		transform.position = target;
		if((Vector3.Distance(transform.position,original_position) < 0.01f)){
            tag = original_tag;
			foreach (Transform chi in transform) {
				chi.gameObject.GetComponent<TrainGame_CircleMaskScript> ().SetColorAndVisibility (hexForNeutral,false);
			}
            //SetTouchAndScale(true, true, true, tag);
        } else{
			if (!is_correct) {
				foreach (Transform chi in transform) {
					chi.gameObject.GetComponent<TrainGame_CircleMaskScript> ().SetColorAndVisibility (hexForWrong, false);
				}
			} else {
				foreach (Transform chi in transform) {
				chi.gameObject.GetComponent<TrainGame_CircleMaskScript> ().SetColorAndVisibility (hexForRight, true);
				}
			}
		}

	}


	void ResetPosition(){
        StartCoroutine(ResetPositionAndSet());
    }

    IEnumerator ResetPositionAndSet()
    {
        yield return StartCoroutine(MoveToTarget(original_position, true, .1f));
        SetTouchAndScale(true, true, true, tag);

    }

	void SetTouchAndScale(bool touch, bool scale, bool visible, string object_tag)
    {
        var all_obj = GameObject.FindGameObjectsWithTag(object_tag);
        foreach (var obj in all_obj)
        {
            //var obj = gameObject;
            obj.GetComponent<SpriteRenderer>().enabled = visible;
            obj.GetComponent<TrainGame_DetectTouch>().SetTouch(touch);
            obj.GetComponent<Scalling>().SetScale(scale);
            obj.GetComponent<Outline>().eraseRenderer = visible;
        }
	}
}
