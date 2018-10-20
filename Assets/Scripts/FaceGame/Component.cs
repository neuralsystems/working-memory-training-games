using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Component : MonoBehaviour
{

    // Hierarchy of game objects : FaceComponent > Option[NO_OF_OPTIONS] > OptionBg

    public IEnumerator SelectOption(bool preLevel, bool tutorial, int mainIndex)
    {

        SpriteRenderer sprite;
        CircleCollider2D circ;
        SpriteRenderer optionBG;
        GameObject component;
        Database database = FindObjectOfType<Database>();
        float scaleToRadiusConversionFactor = Database.constants_scaleToRadiusConversionFactor;
        float sizeToScaleConversionFactor = Database.constants_sizeToScaleConversionFactor;
        float smoothTime = Database.constants_smoothTime;
        int count = transform.childCount;
              
		if (tutorial)
		{
			transform.GetChild(0).GetComponent<SmoothTransition>().enabled = true;
			optionBG = transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>();
			sprite = transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>();
			sprite.enabled = true;
			optionBG.enabled = true;
			float targetScale = sprite.bounds.size.x * sizeToScaleConversionFactor;
			optionBG.transform.localScale = new Vector3(targetScale, targetScale, 0f);
            
			if (mainIndex != 0)
			{
				transform.GetChild(0).GetComponent<SmoothTransition>().SetTarget(Database.constants_optionPosLevel[0], transform.GetChild(0).transform.localScale);
			}
			else
			{
				Vector3 temp = new Vector3(0f, 0f);
				for (int i = 0; i < Database.constants_optionPosLevel.Count; i++)
				{
					temp += Database.constants_optionPosLevel[i];
				}
				transform.GetChild(0).GetComponent<SmoothTransition>().SetTarget(temp/Database.constants_optionPosLevel.Count, transform.GetChild(0).transform.localScale);
			}
		}
		yield return new WaitForSeconds(1.5f);

		for (int i = 0; i < count; i++)
        {

            sprite = transform.GetChild(i).gameObject.GetComponent<SpriteRenderer>();
            circ = transform.GetChild(i).gameObject.GetComponent<CircleCollider2D>();
            optionBG = transform.GetChild(i).GetChild(0).GetComponent<SpriteRenderer>();

            //enable sprite and collider of options, enable option background sprite
            sprite.enabled = true;
            optionBG.enabled = true;
            circ.enabled = true;

            //dynamically allocate size of option background based on option size
            float targetScale = sprite.bounds.size.x * sizeToScaleConversionFactor;
            optionBG.transform.localScale = new Vector3(targetScale, targetScale, 0f);
            circ.radius = targetScale * scaleToRadiusConversionFactor;
        }

		yield return new WaitUntil(() => database.ifOptionSelected);
        for (int i = 0; i < count; i++)
        {
            component = transform.GetChild(i).gameObject;
            //if (!(component.GetComponent<Option>().selectedKey))
            //{
                component.GetComponent<Option>().SetTouch(false);
            //}
        }

        //Setting false for next iteration
        database.ifOptionSelected = false;


        yield return new WaitForSeconds(3 * smoothTime);     //Wait for Object to reach facebase

        //Destroy Objects Not Selected
        for (int i = 0; i < count; i++)
        {
            component = transform.GetChild(i).gameObject;
            if (!(component.GetComponent<Option>().selectedKey))
            {
                Destroy(component);
            }
        }
    }

}
