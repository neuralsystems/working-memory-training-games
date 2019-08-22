using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenUrl : MonoBehaviour
{
    public void OpenURL(string url)
      {
          Application.OpenURL(url);
          Debug.Log("is this working?");
      }
}
