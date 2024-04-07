using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;        //Allows us to use Lists. 

public class GameManager : MonoBehaviour
{
    //Static instance of GameManager which allows it to be accessed by any other script.
    public static GameManager instance = null;
    //Store a reference to our BoardManager which will set up the level.
    private BoardManager boardScript;
    //Current level number, expressed in game as "Day  1".
    public int gold = 25;

    [SerializeField] private TMP_Text goldText;

    public enum Selectables { None = 0, Water, Hoe, Wheat, Carrots, Corn, Tomatos}

    public Selectables selected = Selectables.None;

    //Awake is always called before any Start functions
    void Awake()
    {
        //Check if instance already exists
        if (instance == null)

            //if not, set instance to this
            instance = this;

        //If instance already exists and it's not this:
        else if (instance != this)

            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);

        //Sets this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);

        //Get a component reference to the attached BoardManager script
        boardScript = GetComponent<BoardManager>();

        //Call the InitGame function to initialize the first level 
        InitGame();
    }

    //Initializes the game for each level.
    void InitGame()
    {
        updateGold();
        //Call the SetupScene function of the BoardManager script, pass it current level number.
        boardScript.SetupScene();

    }

    public void updateGold()
    {
        instance.goldText.text = GameManager.instance.gold.ToString();
    }

    //Update is called every frame.
    void Update()
    {

    }
}