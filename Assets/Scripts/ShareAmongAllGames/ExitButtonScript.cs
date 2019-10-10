using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class ExitButtonScript : MonoBehaviour {

    float pressed_for = 0f;
    public Canvas levelCanvas;
    public Button other_button;
    bool isPressed = false;
    public bool is_pre_level;
    string default_scene = "MainApplication_HomeScreen";            // scene to load when the user exit from the pre level
    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        if (isPressed)
        {
            ExitProtocol();
        }
        //else if (!isPressed)
        //{
        //    Reset();
        //}
    }

    public void onPointerDownExitButton()
    {
        isPressed = true;
    }
    public void onPointerUpExitButton()
    {
        Reset();
        isPressed = false;
    }

    public void ExitProtocol()
    {
        Debug.Log("called exit protocol");
        pressed_for += Time.deltaTime;
        //GetComponentInChildren<Text>().text = pressed_for + "secs";
        if (IsPressedForLong() && other_button.GetComponent<ExitButtonScript>().IsPressedForLong())
        {
            if (!is_pre_level && !levelCanvas.gameObject.activeSelf)
            {
                ShowLevelScreen();
            }
            else
            {
                SceneManager.LoadScene(default_scene);
            }
        }

    }

    void ShowLevelScreen()
    {
        Reload();
    }

    public void Reset()
    {
        Debug.Log("time reset for " + name);
        pressed_for = 0f;
       
    }

    void Reload()
    {
        //levelCanvas.gameObject.SetActive(true);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public bool IsPressedForLong()
    {
        return pressed_for >= 3f;
    }

    IEnumerator ResetTime()
    {
        yield return new WaitForSeconds(5f);
        pressed_for = 0f;
        StartCoroutine(ResetTime());
    }
}
