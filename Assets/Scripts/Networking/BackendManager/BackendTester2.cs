using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json.Linq;


public class BackendTester2 : MonoBehaviour {

    public string AdminUsername = "admin";
    public string AdminPassword = "autismsurbhit";
    public string AlternateBackendUrl = "";
    private BackendManager backendManager;
    private float totaltime;

    private void Start()
    {
        backendManager = GetComponent<BackendManager>();
        if (backendManager == null)
        {
            backendManager = gameObject.AddComponent<BackendManager>();
        }
        if (AlternateBackendUrl != "")
        {
            backendManager.DevelopmentUrl = backendManager.ProductionUrl = AlternateBackendUrl;
        }
        Test_1();
    }



    private void Test_1()
    {
        WWWForm form = new WWWForm();
        form.AddField("username", "testuser");
        form.AddField("password", "superpassword");
        //form.AddField("email", "superpassword@gmail.com");
        backendManager.Send(RequestType.Post, "users/", form, OnBackendResponse);
    }

    private void Test_2()
    {
        WWWForm form = new WWWForm();
        form.AddField("username", AdminUsername);
        form.AddField("password", "superpassword");
        form.AddField("email", "test@test.com");
        backendManager.Send(RequestType.Post, "users/", form, OnBackendResponse);
    }


    private void OnBackendResponse(ResponseType responseType, JToken responseData, string callee)
    {
        var r_dat= new object[] { responseType, responseData };
        var msg = r_dat.ToString();
        Debug.Log(msg);

        //foreach
    }
}
