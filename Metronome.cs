using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Metronome : MonoBehaviour
{

    //Objects and Components:
    private AudioSource audioSource; //Temporarily get audio source to play music
    private List<GameObject> sheebs = new List<GameObject>(); //A list of all sheebs currently in scene
    private List<Animator> sheebAnimators = new List<Animator>(); //List of animators of all sheebs in scene
        GameObject[] redSheebsInScene; //Initialize sheeb tracker
        GameObject[] blueSheebsInScene; //Initialize sheeb tracker
        GameObject[] greenSheebsInScene; //Initialize sheeb tracker
    private List<GameObject> grasses = new List<GameObject>(); //A list of all grass objects currently in scene
    private List<Animator> grassAnimators = new List<Animator>(); //List of animators of all grass in scene
        GameObject[] redGrassInScene; //Initialize grass tracker
        GameObject[] blueGrassInScene; //Initialize grass tracker
        GameObject[] greenGrassInScene; //Initialize grass tracker
    private Animator jonnyAnimator; //Jonny Sheep's animator

    [Header("Settings:")]
    public bool forceBeat; //When pulled true, beat will begin counting regardless of whether or not program knows music is playing
    public float startDelay; //How long to wait after playing song to begin first beat
    public float beatsPerMinute; //Tempo of given song
    public float timeSignature; //I don't understand music
    public float bouncesPerBeat; //How many times each sheep will bounce per beat
    public float grassBouncesPerBeat; //How many times each grass will bounce per beat
    public float bounceSpeed; //How fast each part of each sheep bounce is
    public float grassBounceSpeed; //How fast grass do bounce

    //Tracker Variables:
    private float musicStartTime; //What time (in real time) music started
    private float timeSinceBeat = 0; //How much time has passed since last beat
    private float timeSinceGrassBeat = 0; //How much time has passed since grassed beat
    private float timeSinceOffBeat; //How much time has passed since last offbeat

    private string sheebWord;
    public AudioClip testAudio;
    public GameObject testerObject;
    public GameObject testerHerd;

    void Start()
    {
        //Get Objects and Components:
        audioSource = GetComponent<AudioSource>(); //Get audioSource
        GetSheebs(); //Get any starting sheebs in scene
        GetGrass(); //Get any starting grass in scene

        //Initialize Offbeat
        timeSinceOffBeat = (((60 * timeSignature)/beatsPerMinute) * bouncesPerBeat)/2; //Start halfway towards beat marker
    }

    void FixedUpdate()
    {
        if (jonnyAnimator == null) { if (GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>() != null) { jonnyAnimator = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>(); } }

        //Update Object Lists:
        if (
            redSheebsInScene != GameObject.FindGameObjectsWithTag("Red_Sheeb") ||
            blueSheebsInScene != GameObject.FindGameObjectsWithTag("Blue_Sheeb") ||
            greenSheebsInScene != GameObject.FindGameObjectsWithTag("Green_Sheeb")
           ) { GetSheebs(); } //If any new sheep have been added to scene since last check, update sheeb lists

        if (
            redGrassInScene != GameObject.FindGameObjectsWithTag("Red_Grass") ||
            blueGrassInScene != GameObject.FindGameObjectsWithTag("Blue_Grass") ||
            greenGrassInScene != GameObject.FindGameObjectsWithTag("Green_Grass")
            ) { GetGrass(); } //If any new grasses have been added to scene since last check, update grass lists

        //Start Music:
        if (audioSource.isPlaying == false && Time.realtimeSinceStartup >= startDelay) 
        {
            musicStartTime = Time.realtimeSinceStartup; //Log when music starts playing
            audioSource.Play(); //TEMP: Play audio clip
        }

        if (audioSource.isPlaying == true || forceBeat) timeSinceBeat += Time.deltaTime; //Increment beat time
        if (audioSource.isPlaying == true || forceBeat) timeSinceGrassBeat += Time.deltaTime; //Increment grassBeat
        if (audioSource.isPlaying == true || forceBeat) timeSinceOffBeat += Time.deltaTime; //Increment offBeat time
        if (timeSinceBeat >= ((60 * timeSignature)/beatsPerMinute) * bouncesPerBeat) //BEAT:
        {
            timeSinceBeat = 0; //Reset time since beat
            if (jonnyAnimator != null)
            {
                jonnyAnimator.SetFloat("BounceSpeed", bounceSpeed); jonnyAnimator.SetTrigger("Beat"); //Send beat to jonny
            }
            for (int x = sheebAnimators.Count; x > 0; x--) //Send beat to sheebs
                { sheebAnimators[x - 1].SetFloat("BounceSpeed", bounceSpeed); sheebAnimators[x - 1].SetTrigger("Beat"); }
        }
        if (timeSinceGrassBeat >= ((60 * timeSignature) / beatsPerMinute) * grassBouncesPerBeat)
        {
            timeSinceGrassBeat = 0;
            for (int x = grassAnimators.Count; x > 0; x--) //Send beat to grass
                { grassAnimators[x - 1].SetFloat("BounceSpeed", grassBounceSpeed); grassAnimators[x - 1].SetTrigger("Beat"); }
        }
        if (timeSinceOffBeat >= ((60 * timeSignature)/beatsPerMinute) * grassBouncesPerBeat) //OFFBEAT:
        {
            timeSinceOffBeat = 0; //Reset time since offBeat
            for (int x = grassAnimators.Count; x > 0; x--) //Send offBeat to grass
                { grassAnimators[x - 1].SetFloat("BounceSpeed", grassBounceSpeed); grassAnimators[x - 1].SetTrigger("OffBeat"); }
        }

    }
    private void Update()
    {
        if (Input.anyKeyDown == true)
        {
            /*TESTING AREA (Do not change!)*/                                                                                                                                                                                                                if (Input.GetKey(KeyCode.B) && sheebWord == null) { sheebWord = "B"; } else if (Input.GetKey(KeyCode.L) && sheebWord == "B") { sheebWord = "BL"; } else if (Input.GetKey(KeyCode.Y) && sheebWord == "BL") { sheebWord = "BLY"; } else if (Input.GetKey(KeyCode.A) && sheebWord == "BLY") { sheebWord = "BLYA"; } else if (Input.GetKey(KeyCode.T) && sheebWord == "BLYA") { sheebWord = "BLYAT"; TestThing(); }
        }
    }

    private void GetSheebs() //Populates "sheeb" list with all sheebs in scene
    {
        redSheebsInScene = GameObject.FindGameObjectsWithTag("Red_Sheeb"); //Find all red sheebs
        blueSheebsInScene = GameObject.FindGameObjectsWithTag("Blue_Sheeb"); //Find all blue sheebs
        greenSheebsInScene = GameObject.FindGameObjectsWithTag("Green_Sheeb"); //Find all green sheebs
        for (int x = redSheebsInScene.Length; x > 0; x--) { if (sheebs.Contains(redSheebsInScene[x - 1]) == false) { sheebs.Add(redSheebsInScene[x - 1]); } } //Add any new red sheebs
        for (int y = blueSheebsInScene.Length; y > 0; y--) { if (sheebs.Contains(blueSheebsInScene[y - 1]) == false) { sheebs.Add(blueSheebsInScene[y - 1]); } } //Add any new blue sheebs
        for (int z = greenSheebsInScene.Length; z > 0; z--) { if (sheebs.Contains(greenSheebsInScene[z - 1]) == false) { sheebs.Add(greenSheebsInScene[z - 1]); } } //Add any new green sheebs

        sheebAnimators.Clear(); //Clear animator list to prevent doubles
        for (int x = sheebs.Count; x > 0; x--)
        { if (sheebs[x - 1].GetComponentInChildren<Animator>() != null) sheebAnimators.Add(sheebs[x - 1].GetComponentInChildren<Animator>()); } //Get each existing animator and add to list
    }
    private void TestThing()
    {
        audioSource.Stop(); audioSource.clip = testAudio; audioSource.Play(); beatsPerMinute = 122; bounceSpeed = 0.7f;
        GameObject flasheeb = Instantiate(testerObject); flasheeb.transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        GameObject redtest = Instantiate(testerHerd); redtest.transform.position = new Vector3(transform.position.x, transform.position.y, -1);
        for (int x = sheebs.Count; x > 0; x--) { if (sheebs[x - 1].GetComponentInChildren<SpriteRenderer>() != null) { sheebs[x - 1].GetComponentInChildren<SpriteRenderer>().color = Color.white; } if (sheebs[x - 1].GetComponentInChildren<Animator>() != null) { sheebs[x - 1].GetComponentInChildren<Animator>().runtimeAnimatorController = Resources.Load("testanimator") as RuntimeAnimatorController; } }
        for (int x = grassAnimators.Count; x > 0; x--) { SpriteRenderer oog = grassAnimators[x - 1].gameObject.GetComponent<SpriteRenderer>(); oog.color = Color.red; }
    }
    private void GetGrass() //Populates grass list with all grass in scene
    {
        redGrassInScene = GameObject.FindGameObjectsWithTag("Red_Grass"); //Find all red sheebs
        blueGrassInScene = GameObject.FindGameObjectsWithTag("Blue_Grass"); //Find all blue sheebs
        greenGrassInScene = GameObject.FindGameObjectsWithTag("Green_Grass"); //Find all green sheebs
        for (int x = redGrassInScene.Length; x > 0; x--) { if (grasses.Contains(redGrassInScene[x - 1]) == false) { grasses.Add(redGrassInScene[x - 1]); } } //Add any new red sheebs
        for (int y = blueGrassInScene.Length; y > 0; y--) { if (grasses.Contains(blueGrassInScene[y - 1]) == false) { grasses.Add(blueGrassInScene[y - 1]); } } //Add any new blue grass
        for (int z = greenGrassInScene.Length; z > 0; z--) { if (grasses.Contains(greenGrassInScene[z - 1]) == false) { grasses.Add(greenGrassInScene[z - 1]); } } //Add any new green grass

        grassAnimators.Clear(); //Clear animator list to prevent doubles
        for (int x = grasses.Count; x > 0; x--) //Get each existing animator and add to list
        {
            if (grasses[x - 1].GetComponentsInChildren<Animator>() != null)
            {
                Animator[] newGrass = grasses[x - 1].GetComponentsInChildren<Animator>();
                for (int y = newGrass.Length; y > 0; y--) //Parse through all children of each grass object and get animators
                {
                    grassAnimators.Add(newGrass[y - 1]); //Add each grass animator found in each grass object
                }
            }
        }
    }
}
