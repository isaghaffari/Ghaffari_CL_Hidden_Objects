using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{

    // A shortcut any script can use (GameManager.Instance) to reach this GameManager
    public static GameManager Instance;

    // Slot in the Inspector where you drag your on-screen score text
    public TMP_Text ScoreText;

    // The current score. Declared here so it remembers its value between frames
    private int score = 0;

    //slot for the win message that appears when every hidden object has been found in the game
    public GameObject winText;

    //how many hidden objects there are in total. worked out automatically in Start
    private int totalObjects;

    //how many have been found so far
    private int foundCount = 0;

    public Transform waiter;
    public float waiterSpeed = 2f;
    public float waiterDistance = 10f;

    private Vector3 waiterStartPos;
    private float waiterLastX;
    
    //the sound that plays when the player clicks a decoy
    public AudioClip wrongSound;
    
    //the neon sign's Sprite Renderer, so we can swap its image
    public SpriteRenderer neonSign;
    //the sign's normal "on" image
    public Sprite signOnSprite;
    //the darkened "off" image
    public Sprite signOffSprite;
    //the buzz sound when it flickers
    public AudioClip buzzSound;
    //optional: the sign's Light 2D, so the glow switches off too
    public GameObject signLight;
    
    void Awake()
    {
        Instance = this;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //remember where the waiter started
        waiterStartPos = waiter.position;
        
        //start lastX at his current position
        waiterLastX = waiter.position.x;
        
        //count every object in the scene that has a HiddenObject script
        totalObjects = FindObjectsByType<HiddenObject>(FindObjectsSortMode.None).Length;
        
        UpdateScoreText();
    }

    //called by MouseMove when a decoy is clicked
    public void WrongObject()
    {
        //play the wrong sound once, on top of anything else playing
        GetComponent<AudioSource>().PlayOneShot(wrongSound);
    }
    void Update()
    {
        //PingPong counts up from 0 to waiterDistance, then back down to 0, forever
        //adding it to the start X makes him walk right, then left, then right again, etc etc
        float x = waiterStartPos.x + Mathf.PingPong(Time.time * waiterSpeed, waiterDistance);
        
        //move himto the new X, keeping his Y and Z the same
        waiter.position = new Vector3(x, waiterStartPos.y, waiterStartPos.z);
        
        //if he moved left this frame, flip the sprite so he faces left; otherwise face right
        waiter.GetComponent<SpriteRenderer>().flipX = x < waiterLastX;
        
        //remember this frame's X for the next frame
        waiterLastX = x;
    }

    //public void addScore(int amount)
    //{
        //score += amount;
        //UpdateScoreText();
    //}

    //void UpdateScoreText()
    //{
        //ScoreText.text = "Score: " + score;
    //}

    public void ObjectFound()
    {
        AddScore(1);
        GetComponent<AudioSource>().Play();
        
        //add one to the number of objects found
        foundCount += 1;

        if (foundCount >= totalObjects)
        {
            EndGame();
        }
    }
    
    //called by MouseMove when the sign is clicked
    public void FlickerSign()
    {
        //play the buzz once, on top of anything else playing
        GetComponent<AudioSource>().PlayOneShot(buzzSound);

        //switch off right away
        SignOff();
        //then schedule a quick on-off stutter, and finally back on for good
        Invoke(nameof(SignOn), 0.08f);
        Invoke(nameof(SignOff), 0.16f);
        Invoke(nameof(SignOn), 0.45f);
    }

    //swap to the dark image and kill the glow
    void SignOff()
    {
        neonSign.sprite = signOffSprite;
        if (signLight != null) signLight.SetActive(false);
    }

    //swap back to the lit image and bring the glow back
    void SignOn()
    {
        neonSign.sprite = signOnSprite;
        if (signLight != null) signLight.SetActive(true);
    }

    void EndGame()
    {
        //show the win message
        winText.SetActive(true);
        waiter.gameObject.SetActive(false);
    }

    public void AddScore(int amount)
    {
        //add the amount to the score
        score += amount;

        //refresh the on-screen text so it shows the new score
        UpdateScoreText();
    }

    void UpdateScoreText()
    {
        //join the word "Score: " with the number and display it 
        ScoreText.text = "Score: " + score;
    }
}