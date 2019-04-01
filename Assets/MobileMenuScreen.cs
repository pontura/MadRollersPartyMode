using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobileMenuScreen : MonoBehaviour
{
    public void EditUser()
    {
        Data.Instance.LoadLevel("Registration");
    }
}
