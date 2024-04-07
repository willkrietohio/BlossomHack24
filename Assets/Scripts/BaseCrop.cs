using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class BaseCrop : MonoBehaviour
{
    //enum Crop { None, Wheat, Carrots, Corn, Tomatos}
    int state = -1;
    //Crop crop;
    long growthStart;
    int growthTime;
    int buyPrice;
    int sellPrice;
    string name;
    // Start is called before the first frame update
    public virtual void Start()
    {
        // 0 is empty, 1-5 is growth stage
        state = 0;
        growthTime = 0;
        UpdateVisual();
    }

    // Update is called once per frame
    public virtual void Update()
    {
        long currentTimestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        if (currentTimestamp >= (growthStart + growthTime)) 
        {
            if (state < 5) state++;
            UpdateVisual();
            growthStart = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        }
    }

    public virtual void UpdateVisual()
    {
        //pass
    }
    public virtual void OnClick()
    {
        if (state == 0) 
        {
            if (GameManager.instance.gold >= buyPrice)
            {
                GameManager.instance.gold -= buyPrice;
            }
            state = 1;
            growthStart = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        }
        else if (state == 5) 
        {
            state = 0; 
            GameManager.instance.gold += sellPrice;
        }
    }
}


public class Wheat:BaseCrop
{
    public virtual void UpdateVisual()
    {
        //pass
    }
}