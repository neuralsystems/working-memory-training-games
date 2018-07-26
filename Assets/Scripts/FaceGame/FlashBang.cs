using UnityEngine;
using UnityEngine.UI;  // add to the top
using System.Collections;

public class FlashBang : MonoBehaviour
{
    public CanvasGroup myCG;
    private bool flash = false;

    void Update()
    {
        if (flash)
        {
            myCG.alpha = myCG.alpha - Time.deltaTime;
            if (myCG.alpha <= 0)
            {
                myCG.alpha = 0;
                flash = false;
            }
        }
    }

    public void EmitFlash()
    {
        flash = true;
        myCG.alpha = 1;
    }
}