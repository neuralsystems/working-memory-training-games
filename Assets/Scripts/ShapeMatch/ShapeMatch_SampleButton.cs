using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ShapeMatch_SampleButton : MonoBehaviour
{
    public Button buttonComponent;
    public Text nameLabel;

    public void Setup(string item)
    {
        nameLabel.text = item;
    }
}
