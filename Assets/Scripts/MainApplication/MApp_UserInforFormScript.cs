using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class MApp_UserInforFormScript : MonoBehaviour {

    public InputField username;
    public InputField dob;
    public InputField diagnosis;
    public InputField iq_value;
    public InputField first_name;
    public InputField last_name;
    public Text OutputText;
    const string all_ok = "Form Submitted Successfully. Thank you for your participation!";
    public const string database_Name = "WorkingMemoryGames_DB.db";
    const int deafult_iq = 20;
    public void AddUserDetails()
    {
        var val_form = validateform();
        if (val_form == all_ok)
        {
            MApp_DataServices ds = new MApp_DataServices(database_Name);
            var _username = username.text.ToString();
            var _dob = dob.text.ToString();
            var _diagnosis = diagnosis.text.ToString();
            var _iq_value = Convert.ToInt32(iq_value.text); 
            var _first_name = first_name.text.ToString();
            var _last_name = last_name.text.ToString();
            System.DateTime dDate;
            System.DateTime.TryParse(dob.text, out dDate);
            string.Format("{0:yyyy/mm/dd}", dDate);
            var _age =  DateTime.Today.Year - dDate.Year;
            //try
            //{
            ds.CreateUser(_username, _age, _diagnosis, _iq_value, _first_name, _last_name);
            //} catch(Exception e)
            //{
            //OutputText.text = e.ToString();
            //Debug.Log(e.ToString());
            //}

            Debug.Log("Entered user: " + _age);
        }
        else {
            //debug.log();
            OutputText.text = val_form;
        }
    }

    string validateform()
    {
        if (string.IsNullOrEmpty(username.text))
        {
            return "Username can not be empty";
        }


        if (string.IsNullOrEmpty(dob.text))
        {
            return "Date of birth can not be empty";
        }else
        {
            System.DateTime dDate;
            if (System.DateTime.TryParse(dob.text, out dDate))
            {
                string.Format("{0:yyyy/mm/dd}", dDate);
            }
            else
            {
                return "Invalid date of birth"; // <-- Control flow goes here
            }
        }
        if (string.IsNullOrEmpty(diagnosis.text))
        {
            return "Diagnosis can not be null";
        }
        if (string.IsNullOrEmpty(first_name.text))
        {
            return "First Name can not be null";
        }
        if (string.IsNullOrEmpty(last_name.text))
        {
            return "Last Name can not be null";
        }
        try
        {
            var _iq_value = Convert.ToInt32(iq_value.text);
           
        }
        catch (Exception e)
        {
            return "format error in IQ value";
        }
        return all_ok;
    }
}
