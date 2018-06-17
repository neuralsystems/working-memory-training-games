using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
[System.Serializable]
public class UserIcon
{
    public User user;
    //public UserIcon(User user)
    //{
    //    user = this.user;
    //}

}

public class MApp_RegistedUserListScript : MonoBehaviour {

    public SimpleObjectPool simpleGameObjectPool;
    public Transform contentPanel;
    public List<UserIcon> user_list;
    public Text msg_display;
 
    // Use this for initialization
	void Start () {
        AddUserToList();
	}

    void AddUserToList()
    {
        GetUsers();

        for (int i =0;i< user_list.Count; i++)
        {
            UserIcon u_icon = user_list[i];
            var _usericon = simpleGameObjectPool.GetObject();
            //_usericon.GuiButton.onClick.AddListener(() => { Function(param); OtherFunction(param); })
            _usericon.transform.SetParent(contentPanel);
            MApp_UserIconScript _user_icon_script = _usericon.GetComponent<MApp_UserIconScript>();
            Debug.Log("running the inner loop 1 " + u_icon.user.First_Name);
            _user_icon_script.SetUp(u_icon, this);

        }

    }

    void GetUsers()
    {
        try
        {
            Debug.Log("called getusers");
            MApp_DataServices ds = new MApp_DataServices(MApp_UserInforFormScript.database_Name);
            var all_users = ds.GetAllUsers();
            int i = 0;
            foreach (var user in all_users)
            {
                Debug.Log("running the inner loop " + user.First_Name);
                var new_user = new UserIcon();
                new_user.user = user;
                user_list.Add(new_user);
                i++;
            }

            msg_display.text = i.ToString() + "users ";
        } catch (Exception e)
        {
            msg_display.text = e.ToString();
        }
    }
	
}
