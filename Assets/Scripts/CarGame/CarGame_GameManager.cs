using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class CarGame_GameManager : MonoBehaviour {

	public Transform demoTile;
	string subFolder = "CarFragments/";
	private Vector3 velocity = Vector3.zero;
	Text scoreText;
	public int size = 4;
	public int PPU = 32;
	Sprite[] fragmentSprite = new Sprite[4];
	public Transform PARKING_SLOT;
	// Use this for initialization
	void Start () {
		if (scoreText == null) {
			scoreText = GameObject.Find (CarGame_SceneVariables.scoretext).GetComponent<Text>() as Text;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (CarGame_SceneVariables.presentCue) {
			Debug.Log ("created ");
			CarGame_SceneVariables.presentCue = false;
			Debug.Log ("presentCue " + CarGame_SceneVariables.presentCue);
			Instantiate (demoTile, CarGame_SceneVariables.initVector, Quaternion.identity);

		}

	}

	public void Match(){
		GameObject.FindGameObjectWithTag (CarGame_SceneVariables.matchedTag).GetComponent<CarGame_DetectTouch> ().SetTouch (false);
		GameObject.FindGameObjectWithTag (CarGame_SceneVariables.matchedTag).GetComponent<Scalling> ().SetScale(false);
		AfterSelection ();

	}



	public void MisMatch(){
		Debug.Log ("Didnt match");
		GameObject.FindGameObjectWithTag (CarGame_SceneVariables.selectedTileTag).GetComponent<CarGame_DetectTouch> ().SetTouch (false);
		GameObject.FindGameObjectWithTag (CarGame_SceneVariables.selectedTileTag).GetComponent<Scalling> ().SetScale(false);
		AfterSelection ();
		StartCoroutine (DispalyMisMatch ());


//		Vector3 eor = GameObject.Find (SceneVariables.eor[0]).gameObject.transform.position;
//		CueBehavior go = GameObject.FindGameObjectWithTag (SceneVariables.cueTag).GetComponent<CueBehavior>();
//		StartCoroutine(go.MoveCue (eor));


	}

	// mismatch will cause the breaking of the cue sprite into 4 parts
	public void Break(){
		
		string parentSprite = GameObject.FindGameObjectWithTag(CarGame_SceneVariables.cueTag).GetComponent<SpriteRenderer> ().sprite.name;
		string folderName = Camera.main.GetComponent<ArrangeTiles> ().ImageFolder;
		GameObject[] fragments = GameObject.FindGameObjectsWithTag (CarGame_SceneVariables.fragmentTag);
		foreach (GameObject fragment in fragments) {
			string imageName = parentSprite + "_" + fragment.name;
			fragment.GetComponent<SpriteRenderer> ().sprite = Resources.Load (folderName + subFolder + parentSprite + "/" + imageName, typeof(Sprite)) as Sprite;
			fragment.GetComponent<FragmentScript> ().enabled = true;
		}
		GameObject.FindGameObjectWithTag (CarGame_SceneVariables.cueTag).GetComponent<SpriteRenderer> ().sprite = null;
		BreakUp ();
	}

	public void DynamicBreak(){
		BreakUp ();
//		Texture2D sourceTexture = GetComponent<SpriteRenderer>().sprite.texture;
//		Debug.Log ("called the dynamic one");
		var cueTile = GameObject.FindGameObjectWithTag(CarGame_SceneVariables.cueTag);
//		var ImageWidth = Mathf.FloorToInt(cueTile.GetComponent<SpriteRenderer> ().sprite.rect.width);
//		var ImageHeight = Mathf.FloorToInt(cueTile.GetComponent<SpriteRenderer> ().sprite.rect.height);
//		string folderName = Camera.main.GetComponent<ArrangeTiles> ().ImageFolder;
		int numOfFragments = cueTile.transform.childCount;
		for(int i = 0;i< numOfFragments; i++) {
			cueTile.transform.GetChild (i).GetComponent<SpriteRenderer> ().sprite = fragmentSprite [i];
//			string imageName = parentSprite + "_" + fragment.name;
//			int iIndex = i/2, jIndex = i%2;
//			cueTile.transform.GetChild(i).GetComponent<SpriteRenderer>().sprite = Sprite.Create(cueTile.GetComponent<SpriteRenderer>().sprite.texture, new Rect( iIndex*( ImageWidth/ numOfFragments), jIndex*(ImageHeight/ numOfFragments), ImageWidth/ numOfFragments, ImageWidth/ numOfFragments), new Vector2(0.5f, 0.5f));
//			cueTile.transform.GetChild(i).GetComponent<SpriteRenderer>().material.mainTexture = CopyPixels(cueTile.GetComponent<SpriteRenderer>().sprite.texture);
			cueTile.transform.GetChild (i).GetComponent<FragmentScript> ().enabled = true;

		}
		GameObject.FindGameObjectWithTag (CarGame_SceneVariables.cueTag).GetComponent<SpriteRenderer> ().sprite = null;
	}

	void BreakUp(){
		// Get sprite texture


		var cueTile = GameObject.FindGameObjectWithTag(CarGame_SceneVariables.cueTag);
		// Get sprite width and height
		Texture2D sourceTexture = cueTile.GetComponent<SpriteRenderer>().sprite.texture;
		int width = Mathf.FloorToInt( cueTile.GetComponent<SpriteRenderer>().sprite.rect.width );
		int height = Mathf.FloorToInt( cueTile.GetComponent<SpriteRenderer>().sprite.rect.height );

		int partNumber = 0;
		int k = 0;
		for(int i = 0; i < width/size; i++)
		{
			for(int j = 0; j < height/size; j++)
			{
				// Cut out the needed part from the texture
				fragmentSprite[k] = Sprite.Create(sourceTexture, new Rect(i*width/(width/size),j*height/(height/size), size, size), new Vector2(0f, 0f) , PPU);
				k++;
//				GameObject n = new GameObject();
//				SpriteRenderer sr = n.AddComponent<SpriteRenderer>();
//				sr.sprite = newSprite; //A kivágott sprite-ot megkapja az új GameObject
//				sr.name = "part " + partNumber;
//				// Place every GameObject as it was in the original sprite
//				n.transform.position = new Vector3((GetComponent<SpriteRenderer>().bounds.min.x + (sr.sprite.rect.width/PPU)*i), (GetComponent<SpriteRenderer>().bounds.min.y + (sr.sprite.rect.width/PPU)*j) , transform.position.z);
//				n.transform.parent = transform; // Place the parts inside the parent object
//
//				partNumber++;
			}
		}
		scoreText.text = "abc "+k;
	}

	Texture2D CopyPixels(Texture2D sourceTexture)
	{
		
		int x = 0, y = 0;
		int width = 210, height = 72;
		Color[] pix = sourceTexture.GetPixels (x, y, width, height);
		Debug.Log (pix.Length + " = length of pix");
		Texture2D destTex = new Texture2D (width, height);
		destTex.SetPixels (pix);
		destTex.Apply ();
		return destTex;
	}
	public void AfterSelection(){
		GameObject[] go = GameObject.FindGameObjectsWithTag (CarGame_SceneVariables.OptionTileTag);
		foreach (GameObject g in go) {
			g.GetComponent<CarGame_DetectTouch> ().SetTouch (false);
			g.GetComponent<Scalling> ().SetScale (false);
		}
	}

	public IEnumerator DispalyMisMatch(){
		Vector3 cuePosition = GameObject.FindGameObjectWithTag (CarGame_SceneVariables.cueTag).transform.position;
		GameObject selected = GameObject.FindGameObjectWithTag (CarGame_SceneVariables.selectedTileTag);
		while (true) {
			selected.transform.position = Vector3.SmoothDamp (selected.transform.position, cuePosition, ref velocity ,CarGame_SceneVariables.speed * Time.deltaTime);
//			yield return new WaitForSeconds (selected.GetComponent<MergeOptionCue> ().waitTime);
			yield return null;
			if (selected.transform.position == cuePosition)
				break;
		}
		Break ();
	}

	public IEnumerator MoveBack(){
		GameObject selected = GameObject.FindGameObjectWithTag (CarGame_SceneVariables.selectedTileTag);
		Vector3 cuePosition = selected.GetComponent<CarGame_DetectTouch> ().origin;
		while (true) {
			selected.transform.position = Vector3.SmoothDamp (selected.transform.position, cuePosition, ref velocity ,CarGame_SceneVariables.speed * Time.deltaTime);
//			yield return new WaitForSeconds (selected.GetComponent<MergeOptionCue> ().waitTime);
			yield return null;
			if (selected.transform.position == cuePosition)
				break;
		}
		selected.tag = CarGame_SceneVariables.OptionTileTag;
		Camera.main.GetComponent<CarGame_SceneVariables> ().ResetorRestart ();
		Destroy(GameObject.FindGameObjectWithTag(CarGame_SceneVariables.cueTag));
	}

	public void UpdateScoreText(int matched){
		CarGame_DataService ds = new CarGame_DataService (CarGame_SceneVariables.databaseName);
		var scores = ds.GetScoreValue (Camera.main.GetComponent<ArrangeTiles> ().level, Camera.main.GetComponent<Timer>().time, matched);
		float scoreToAdd = 0f;
		foreach (var score in scores) {
			scoreToAdd = score.GetScoreValue ();
		}
		scoreText.text = Mathf.Max(Convert.ToInt32 (scoreText.text) + scoreToAdd, Camera.main.GetComponent<CarGame_SceneVariables>().minScoreToShow).ToString();
		Debug.Log ("Time it took: "+ Camera.main.GetComponent<Timer>().time);

	}

	public void RandomizeObjects(){
		GameObject[] go = GameObject.FindGameObjectsWithTag (CarGame_SceneVariables.OptionTileTag);
		Sprite[] sprites = new Sprite[go.Length];
		int i = 0;
		foreach(var sprite in sprites){
			sprites [i] = go [i].GetComponent<ImageEffect> ().oldSprite;
			i++;
		}
//		RandomizingArray ra = new RandomizingArray ();
		var old_sprites = RandomizingArray.RandomizeSprite (sprites);
		i = 0;
		foreach (var g in go) {
			go [i].GetComponent<ImageEffect> ().oldSprite = old_sprites [i];
			i++;
		}
	}




}
