using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class MApp_UserInforFormScript : MonoBehaviour {

    public InputField username;
    public Dropdown day;
    public Dropdown month;
    public Dropdown year;
    public InputField diagnosis;
    public InputField iq_value;
    public InputField first_name;
    public InputField last_name;
    public Text OutputText;
    public Text dob;
    const string all_ok = "Form Submitted Successfully. Thank you for your participation!";
    public const string database_Name = "WorkingMemoryGames_DB1.db";
    const int deafult_iq = 20;
    
    public void AddUserDetails()
    {
        var val_form = validateform();
        if (val_form == all_ok)
        {
            Debug.Log("all ok here");
            MApp_DataServices ds = new MApp_DataServices(database_Name);
            var _username = username.text.ToString();
            //var _dob = dob.text.ToString();
            var _day = day.options[day.value].text;
            var _month = month.options[month.value].text;
            var _year = year.options[year.value].text;
            var _diagnosis = diagnosis.text.ToString();
            var _iq_value = Convert.ToInt32(iq_value.text); 
            var _first_name = first_name.text.ToString();
            var _last_name = last_name.text.ToString();
            System.DateTime dDate;
            //System.DateTime.TryParse(dob.text, out dDate);
            //string.Format("{0:yyyy/mm/dd}", dDate);
            
            var _age = DateTime.Today.Year - GetDOB().Year;
            //try
            //{
            var users = ds.GetAllUsers();
            foreach(var user in users)
            {
                Debug.Log(user.Username);
            }
            ds.CreateUser(_username, _age* 1.0, _diagnosis, _iq_value, _first_name, _last_name);
            //} catch(Exception e)
            //{
            //OutputText.text = e.ToString();
            //Debug.Log(e.ToString());
            //}

            //Debug.Log("Entered user: " + _age);
        }
        else {
            //debug.log();
            OutputText.text = val_form;
        }
    }

    public void DisplayDOB()
    {
        dob.text = "born on: " + GetDOB().ToString("yyyy-MM-dd");
    }
    DateTime GetDOB()
    {
        return DateTime.Parse(day.options[day.value].text + "-" + month.options[month.value].text + "-" + year.options[year.value].text);
    }
    string validateform()
    {
        if (string.IsNullOrEmpty(username.text))
        {
            return "Username can not be empty";
        }


        //if (string.IsNullOrEmpty(dob.text))
        //{
        //    return "Date of birth can not be empty";
        //}else
        //{
        //    System.DateTime dDate;
        //    if (System.DateTime.TryParse(dob.text, out dDate))
        //    {
        //        string.Format("{0:yyyy/mm/dd}", dDate);
        //    }
        //    else
        //    {
        //        return "Invalid date of birth"; // <-- Control flow goes here
        //    }
        //}
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
            Debug.Log("format error in IQ value");
            return "format error in IQ value";
        }
        return all_ok;
    }
}
