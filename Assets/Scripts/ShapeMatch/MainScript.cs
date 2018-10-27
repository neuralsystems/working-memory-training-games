using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainScript : MonoBehaviour
{
    public GameObject Box;
    [HideInInspector]public int Tap_Count=0;
    [HideInInspector]public int Wrong_Tap_Count=0;
    public int RepeatLevel;
    public GameObject SampleObject;
    private Sprite[] SpritesArray;
    public int LevelNumber;
    public int GameObjectsInLevel;
    public int ShouldMove;
    public int ShouldHide;
    [HideInInspector]public GameObject ob;
    [HideInInspector]public List<Vector2> MoveAgainList = new List<Vector2>();
    public const string DATABASE_NAME = "WorkingMemoryGames_DB1.db";
    public void StartMainLevel()
    {
        LevelNumber = ShapeMatch_mainLevelStart.level_number;
        //LevelNumber = 5;
        GetLevelInformation();                                       // for getting level information for the selected user
        Box.GetComponent<Renderer>().enabled = true;                // enabling the box  
        List<Vector2> randomizedList = new List<Vector2>();         // randomized list is for adding new positions in the gr
        int GO = GameObjectsInLevel;                                // this will give the no. of game objects present in that level number

        int SizeX = 1;                                              // initial grid size
        int SizeY = 2;
        // arraySizeX and ArraySizeY are for initializing the no. of coordinates required in x and y
        //their ith element will contain no. of coordinates reqired in respective axes in the ith level
        int[] ArraySizeX = new int[GO];
        int[] ArraySizeY = new int[GO];

        // loop for initializing their values

        for(int z=0;z<GO;z++)
        {
            if (z%2==0)
            {
                SizeX += 1;
                ArraySizeX[z] = SizeX;
                ArraySizeY[z] = SizeY;
            }
            else
            {
                SizeY += 1;
                ArraySizeX[z] = SizeX;
                ArraySizeY[z] = SizeY;
            }
        }

        // array x and y are for storing the coordinates
        float[] x = new float[ArraySizeY[GO-1]];
        float[] y = new float[ArraySizeX[GO-1]];

        
        FindPosition(GO, x, y, randomizedList, MoveAgainList);              // for finding positions of coordinates of grid and storing then in world points in randomized list
        LoadSprites();                                                      // loading sprites in an array
        int temp = GameObjectsInLevel;                                      // used for instantiating the no. of gameobjects required
        if (RepeatLevel == 0)
        {
            ShuffleSpriteArray(SpritesArray);
            while (temp > 0)
            {
                int flag = 2;
                while (flag > 0)
                {

                    Inst(temp, GO, x, y,randomizedList);
                    flag--;
                }
                temp--;

            }
        }
        else if(RepeatLevel!=0 && LevelNumber==0)
        {
            while (temp > 0)
            {
                int flag = 2;
                while (flag > 0)
                {

                    Inst(temp, GO, x, y, randomizedList);
                    flag--;
                }
                temp--;
                if(RepeatLevel>0)
                {
                    RepeatLevel--;
                }

            }
        }
        
    }
    void FindPosition(int GO, float[] x, float[] y, List<Vector2> randomizedList, List<Vector2> MoveAgainList)      // finding positions in the grid for randomization
    {
        float ScreenHeight, ScreenWidth;
        int i;
        ScreenHeight = Camera.main.pixelHeight;             // finding screen height in pixels
        ScreenWidth = Camera.main.pixelWidth;               // fing screen width in pixels
        ScreenWidth -= 0.1f*ScreenWidth;
        for (i = 0; i < y.Length; i++)                      // finding positions along y axis
        {
            y[i] = (ScreenHeight * (i + 1)) / (y.Length + 1);
            if (i == 0)
            {
                y[i] -= (0.12f*ScreenHeight - 5 * GO);
            }
            if (i == y.Length - 1)
            {
                y[i] += (0.12f * ScreenHeight - 5 * GO);
            }
        }
        for (i = 0; i < x.Length; i++)                                              // finding positions along x axis
        {
            x[i] = (ScreenWidth * (i + 1)) / (x.Length + 1);
            if (i == 0)
            {
                x[i] -= (0.15f*ScreenWidth - 7 * GO);
            }
            if (i == x.Length - 1)
            {
                x[i] += (0.15f*ScreenHeight - 7 * GO);
            }
        }
        for (i = 0; i < x.Length; i++)                                                      // storing those positions in the vector 3 list
        {
            for (int j = 0; j < y.Length; j++)
            {
                Vector2 pos = new Vector3(x[i], y[j]);
                Vector2 screenPos = Camera.main.ScreenToWorldPoint(pos);
                randomizedList.Add( new Vector2(screenPos.x,screenPos.y));
                MoveAgainList.Add(new Vector2(screenPos.x, screenPos.y));
            }
        }
    }
    void LoadSprites()                                                                              // fuction for loading all the sprites in ythe array
    {
        object[] loadedIcons = Resources.LoadAll("ShapeMatch/Icons", typeof(Sprite));
        SpritesArray = new Sprite[loadedIcons.Length];
        for (int x = 0; x < loadedIcons.Length; x++)
        {
            SpritesArray[x] = (Sprite)loadedIcons[x];
        }
    }
    void ShuffleSpriteArray(Sprite[] SpriteArray)                                                   // for shuffling the sprites array
    {
        for (int i = 0; i < SpriteArray.Length; i++)
        {
            Sprite temp = SpriteArray[i];
            int randomIndex = Random.Range(0, SpriteArray.Length);                              //for choosing random item and swapping it
            SpriteArray[i] = SpriteArray[randomIndex];
            SpriteArray[randomIndex] = temp;
        }
    }
    void Inst(int temp,int GO, float[] x, float[] y, List<Vector2> randomizedList)      // for instantiating the objects
    {
        GameObject temp_GO = SampleObject.GetComponent<SimpleObjectPool>().GetObject();  // getting the gameobject to be instantiated
        if (RepeatLevel == 0)
        {
            temp_GO.GetComponent<SpriteRenderer>().sprite = SpritesArray[temp-1];
        }
        else
        {
            temp_GO.GetComponent<SpriteRenderer>().sprite = SpritesArray[RepeatLevel];
        }
        float Scale = Scale_size();                                                                // scalling the game object according to the no. of game objects present in that level
        int index = Random.Range(0, randomizedList.Count);                                          // getting random index of sprite for the instantiated gameObject
        Vector3 playerpos = new Vector3(randomizedList[index].x, randomizedList[index].y, 0);       // defining the position for the instantiated gameobject
        StartCoroutine(backAgain(playerpos, temp_GO, GO));                                          // moving the instantiated gameobject to that selected position
        StartCoroutine(ScaleObject(Scale, temp_GO));                                                // function for calling scalling 
        StartCoroutine(pump(temp_GO, GO));                                                          // starting the pump function
        randomizedList.RemoveAt(index);                                                             // removing the used position vector from the list so that it cannot be repeated
        
    }
    IEnumerator backAgain(Vector3 playerpos, GameObject temp_Go, int GO)        // for moving the object to a particular position with some speed
    {
        float speed = Camera.main.pixelWidth * 0.06f;
        var step = speed * Time.deltaTime;
        while ((Vector3.Distance(temp_Go.transform.position, playerpos)) >= 0.5)
        {
            temp_Go.transform.position = Vector3.MoveTowards(temp_Go.transform.position, playerpos, step);
            yield return null;
            if (Vector3.Distance(temp_Go.transform.position, playerpos) <= 0.5)
            {
                temp_Go.transform.position = playerpos;
            }
        }
        
    }
    IEnumerator ScaleObject(float maxSize, GameObject temp_GO1)        // for scaling the object
    {
        while (maxSize > temp_GO1.transform.localScale.x)
        {
            temp_GO1.transform.localScale += new Vector3(1, 1, 1) * Time.deltaTime * 0.8f;
            yield return null;
        }

    }
    IEnumerator pump(GameObject temp_Go, int GO)            // for calling the pumping script 
    {
        yield return new WaitForSeconds(0.5f);
        temp_Go.GetComponent<Scalling>().SetScale(true, GO);
        yield return new WaitForSeconds(0.5f);
        Box.GetComponent<Renderer>().enabled = false;
    }

    public void GetLevelInformation()                       // for getting Details about the previously completed levels by that user
    {
        Debug.Log("getting level info");
        var ds = new ShapeMatch_DataService(DATABASE_NAME);
        //var Level_completed_Details = ds.GetCompletedLevel(username02);
        //LevelNumber = GetLevelDetail(Level_completed_Details);
        var Level_Info = ds.GetLevelsInfo(LevelNumber);
        GameObjectsInLevel = GetGameObjects(Level_Info);
        ShouldMove = GetIsMove(Level_Info);
        ShouldHide = GetIsHide(Level_Info);
        Debug.Log("info = "+ GameObjectsInLevel+ ShouldHide + ShouldMove);

    }
    int go, move, hide;
    private int GetGameObjects(IEnumerable<ShapeMatch_levels> Level_info)    //for getting the no. of game objects in that level 
    {
        foreach (var level_obj in Level_info)
        {
            go = level_obj.GetgameObjects();
        }
        return go;
    }
    private int GetIsMove(IEnumerable<ShapeMatch_levels> Level_info)   // for getting the value of is move 
    {
        foreach (var level_obj in Level_info)
        {
            move = level_obj.GetIsMove();
        }
        return move;
    }
    private int GetIsHide(IEnumerable<ShapeMatch_levels> Level_info)    // for getting the value of is hide
    {
        foreach (var level_obj in Level_info)
        {
            hide = level_obj.GetIsHide();
        }
        return hide;
    }
    float Scale_size()
    {
        int GO = GameObjectsInLevel;
        float maxSize;
        if (GO <= 3)
        {
            maxSize = 5 - 0.7f * GO;
        }
        else if (GO > 3 && GO <= 5)
        {
            maxSize = 5 - 0.6f * GO;
        }
        else
        {
            maxSize = 5 - 0.5f * GO;
        }
        return maxSize;
    }
}
        
