using ScriptableArchitecture.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateSO : MonoBehaviour
{
    [SerializeField] private BoolReference var;
    [SerializeField] private bool activate;

    void Start()
    {
        if (var.Value)
            gameObject.SetActive(activate);
        else
            gameObject.SetActive(!activate);
    }
}
