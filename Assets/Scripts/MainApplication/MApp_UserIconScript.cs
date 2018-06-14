using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MApp_UserIconScript : MonoBehaviour {

    //public Button User_detail;
    private User user_gameobject;
    private MApp_RegistedUserListScript scrollList;
	// Use this for initialization
	void Start () {
		
	}

    public void SetUp(UserIcon reward_object, MApp_RegistedUserListScript current_scrolllist)
    {
        user_gameobject = reward_object.user;
        scrollList = current_scrolllist;
        transform.GetChild(0).GetComponent<Text>().text = reward_object.user.Username;
        UnityEngine.Events.UnityAction set = () => { Camera.main.GetComponent<MApp_GameManager>().SetUser(reward_object.user); };
        GetComponent<Button>().onClick.AddListener(set);
        //transform.GetChild(0).GetComponent<Text>().text = "abc";
        //Debug.Log((transform.GetChild(0).GetComponent<Text>().text == "") );
    }
}
