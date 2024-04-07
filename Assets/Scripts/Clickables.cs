using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clickables : Singleton<Clickables>
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //[SerializeField]
    public void OnCarrotClick()
    {
        GameManager.instance.selected = GameManager.Selectables.Carrots;
    }
}


