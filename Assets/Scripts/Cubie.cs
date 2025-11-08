using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Cubie : MonoBehaviour {
   
    private Rigidbody2D rb;
    public float fallMultiplier = 4.5f;
    private bool Grounded = true;
    private Animator anim;
    public GameObject dustParticle;
    private bool firstJump = true;
    private float prevYpos = -1000;
    public bool isDead = false;
    public GameObject GameOverScreen;
    Color[] standColor = new Color[] { Color.red, Color.green, Color.blue, Color.yellow };
    int colorIndex = 0;
    int Acheivemnt = 30;
    private char lastJump = 'N';
    // score text
    public Text scoreText;
    //
    private AudioSource audi;
    // Use this for initialization
    void Start () {
        audi = GetComponent<AudioSource>();

        Time.timeScale = 2f;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {

       
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
          
            Jump(true);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow)) {

          
                Jump(false);
        }
        if (transform.position.y + 5f < prevYpos)
        {
            if (!isDead)
                Death();
        }
       
    }
    void FixedUpdate()
    {
        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier) * Time.deltaTime;
        }

    }
    public void Jump(bool smalljump)
    {
        firstJump = false;
        if (!Grounded)
            return;

        //play animation
        anim.SetTrigger("jump");

        Grounded = false;
        if (smalljump)
        {
            lastJump = 'S';
            rb.AddForce(new Vector2(9.8f * 12f, 9.8f * 20f));

         
        }
        else {
            // do long jump
            lastJump = 'B';
            StartCoroutine(longJump());
        }
    }


    IEnumerator longJump()
    {

        rb.AddForce(new Vector2(0, 9.8f * 29f));

        yield return new WaitForSeconds(0.15f);

        rb.AddForce(new Vector2(9.8f * 12f, 0));
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        rb.velocity = Vector3.zero;

        if (lastJump == 'S' && col.gameObject.tag == "smallTile")
        {

        }
        else if (lastJump == 'B' && col.gameObject.tag == "bigTile")
        {

        }
        else if (lastJump == 'N')
        {

        }
        else {
            //print("wrong Jump  - Game Over!!!");
            GameOverScreen.SetActive(true);
        }



        if (col.gameObject.tag.Contains("Tile"))
        {
            // Trigger visual glow effect if in musical mode
            PianoTileVisuals tileVisuals = col.gameObject.GetComponent<PianoTileVisuals>();
            if (tileVisuals != null)
            {
                tileVisuals.TriggerGlow();
            }
            
            // Play the tile's assigned note in musical mode
            TileNotePlayer notePlayer = col.gameObject.GetComponent<TileNotePlayer>();
            if (notePlayer != null && notePlayer.assignedNote != null)
            {
                // Musical mode: play the specific note assigned to this tile
                notePlayer.PlayNote();
            }
            else
            {
                // Fallback to original piano notes if not in musical mode
                if (col.gameObject.CompareTag("smallTile"))
                {
                    // Play C3 for small tiles (white keys)
                    if (AudioManager.Instance != null)
                        AudioManager.Instance.PlayC3Note();
                }
                else if (col.gameObject.CompareTag("bigTile"))
                {
                    // Play C6 for big tiles (black keys)
                    if (AudioManager.Instance != null)
                        AudioManager.Instance.PlayC6Note();
                }
                else
                {
                    // Default sound for any other tiles
                    audi.Play();
                }
            }
            
            //update the score
            scoreText.text = (int.Parse(scoreText.text) + 1).ToString();
            
            // change color (only in normal mode, musical mode uses piano key colors)
            if (GameModeManager.Instance == null || !GameModeManager.Instance.IsMusicalMode())
            {
                GameObject cube = col.gameObject;
                Renderer cr = cube.GetComponent<Renderer>();
                cr.material.SetColor("_Color", standColor[colorIndex]);
            }

            prevYpos = transform.position.y;

            GameObject temp = Instantiate(dustParticle, new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z),dustParticle.transform.rotation);
            Destroy(temp, 1.5f);

            
            Grounded = true;
            transform.position = new Vector3(col.gameObject.transform.position.x-0.2f,transform.position.y,transform.position.z);

            if (firstJump)
                return;
            StartCoroutine(FallTile(col.gameObject));


        }
        AcheivementAcheived();

    }

    void AcheivementAcheived()
    {
        if (int.Parse(scoreText.text) == Acheivemnt)
        {
            if (colorIndex >= standColor.Length - 1)
            {
                colorIndex = 0;
            }
            else
            {
                colorIndex++;
            }
            Acheivemnt = Acheivemnt * 2;
        }
    }

    IEnumerator FallTile(GameObject col)
    {
        yield return new WaitForSeconds(1f);
        if(col.gameObject != null)
            col.AddComponent<Rigidbody2D>();

    }

    void CheckHighScore()
{
    int currentScore = int.Parse(scoreText.text);
    bool isNewHighScore = HighScoreManager.Instance.CheckAndUpdateHighScore(currentScore);
    
    if (isNewHighScore)
    {
        // Optional: Show some visual feedback for new high score
        Debug.Log("New High Score: " + currentScore);
    }
}

    void Death()
    {
        GameOverScreen.SetActive(true);
        print("Player died .......");
        isDead = true;
        CheckHighScore();
    }
}