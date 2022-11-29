using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MyUserControl : MonoBehaviour
{
    public static bool iAmThisUser = false;
    [SerializeField] private InputActionReference _space;

    private void Start()
    {
        _space.action.performed += ctx =>
        {
            iAmThisUser = true;
        };
    }
}
