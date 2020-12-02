using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public enum State
{
    Normal,
    Clear
}

public class PlayerScript : MonoBehaviour
{
    static PlayerScript instance;
    public static PlayerScript Instance
    {
        get { return instance; }
    }

    public Rigidbody2D rb;
    public Collider2D bodyColl;
    public Animator playerAnimator;

    #region daisies - life
    [SerializeField]
    int daisies;
    public int Daisies
    {
        get
        {
            return daisies;
        }
        set
        {
            //paralysation for 1 sec //taking damage
            if (value < daisies)
            {
                if (!paralyzed && !invincible)
                {
                    switch (value)
                    {
                        case 2:
                            daisies = 2;

                            //fill 2 sprites
                            UIManager.Instance.UpdateDaisies(2);
                            break;
                        case 1:
                            daisies = 1;

                            //fill 1 sprite
                            UIManager.Instance.UpdateDaisies(1);
                            break;
                        case 0:
                            daisies = 0;

                            //die
                            UIManager.Instance.UpdateDaisies(0);
                            Die();
                            break;
                        default:
                            daisies = 0;

                            //die
                            UIManager.Instance.UpdateDaisies(0);
                            Die();
                            break;
                    }

                    StartInvincibility();
                }

                StartParalysis();
            }
            else //adding life
            {
                if (value >= 3)
                {
                    daisies = 3;
                    UIManager.Instance.UpdateDaisies(3);
                }
                else
                {
                    daisies = value;
                    UIManager.Instance.UpdateDaisies(daisies);
                }
            }
        }
    }

    Vector2 spawnPoint;

    bool invincible = false; //is invincible for 2 seconds after taking damage and releasing minions
    float startTimeOfInvincibility;
    float invicibilityCooldown = 2f;

    public void StartInvincibility()
    {
        invincible = true;
        startTimeOfInvincibility = Time.time;

        //start sprite blinking
        playerAnimator.SetBool("Damaged", true);
    }
    void EndInvincibility()
    {
        invincible = false;

        //stop sprite blinking
        playerAnimator.SetBool("Damaged", false);
    }

    public bool dead = false;
    void Die()
    {
        // don't call again if already dead
        if (dead)
            return;

        //stop movement
        StartParalysis();

        dead = true;

        UIManager.Instance.StartDeathTransition(1f);
    }
    void ResetGameplay()
    {
        //respawn @ Hub
        transform.position = spawnPoint;

        //fill up Daisies
        Daisies = 3;

        //fill up SightPoints
        SightPoints = maxSightPoints;

        //stop clear sight
        UpdateNormalState();

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Respawn()
    {
        dead = false;

        //refill lives
        Daisies = 3;

        //refill clear sight
        SightPoints = maxSightPoints;
    }

    #endregion

    #region movement
    public float moveSpeed;
    public float csMoveSpeed;
    Vector2 movement;

    void SetMovementVector()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        movement = movement.normalized; //normalizes movement to avoid making diagonal movement faster
    }
    void StopMovement()
    {
        movement.x = 0;
        movement.y = 0;
        movement = movement.normalized;
    }
    void Move()
    {
        if (currState == State.Normal)
            rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
        else if (currState == State.Clear)
            rb.MovePosition(rb.position + movement * csMoveSpeed * Time.fixedDeltaTime);
    }

    float paralyzeStart;
    float paralyzeCooldown = 0.5f;
    bool paralyzed = false;

    public void StartParalysis()
    {
        paralyzed = true;
        paralyzeStart = Time.time;
    }
    public void StopParalysis()
    {
        paralyzeStart = 0;
        paralyzed = false;
    }
    public void EndOfLevelParalysis()
    {
        paralyzed = true;
        paralyzeStart = Time.time + 10;
    }
    #endregion

    #region  clear sight
    [Tooltip("How long can the player use Clear Sight (in seconds)")]
    public float maxSightPoints;

    public GlitchEffect glitchEffect;

    public bool clearSightAvailable;

    [SerializeField]
    private float sightPoints;
    public float SightPoints
    {
        get
        {
            return sightPoints;
        }
        set
        {
            if (value > maxSightPoints)
            {
                sightPoints = maxSightPoints;

                //fill up sightBar
                UIManager.Instance.UpdateSightBar(value);
            }
            else if (value <= 0)
            {
                Die();
            }
            else
            {
                sightPoints = value;

                //set sightBar
                UIManager.Instance.UpdateSightBar(value);
            }
        }
    }

    public State currState = State.Normal;

    void UpdateClearState()
    {
        if (sightPoints > 0)
        {
            currState = State.Clear;

            //switch State of all IChangable
            foreach(IChangable obj in FindObjectsOfType<Object>().OfType<IChangable>())
            {
                obj.UpdateClearState();
            }

            //sight points reduce
            SightPoints -= Time.deltaTime;
        }
    }
    void UpdateNormalState()
    {
        currState = State.Normal;

        //switch State of all IChangableObstacles
        foreach (IChangable obj in FindObjectsOfType<Object>().OfType<IChangable>())
        {
            obj.UpdateNormalState();
        }
    }

    #endregion

    public bool visualNovelMode = false;
    public bool tutorialMode = false;

    public MinionScript pickedUpObj = null;

    private void Awake()
    {
        if (Instance == null)
            instance = this;
        else
            Debug.Log("PlayerScript already exists!");
    }

    private void Start()
    {
        //activate clear sight
        if (SceneManager.GetActiveScene().buildIndex == 2 || SceneManager.GetActiveScene().buildIndex == 1 || SceneManager.GetActiveScene().buildIndex == 6 || SceneManager.GetActiveScene().buildIndex == 7) //disable in Level1 and Level0 and Rooftop and Graveyard
            clearSightAvailable = false;
        else
            clearSightAvailable = true;

        //hp in Level1
        if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            daisies = 2;
            UIManager.Instance.UpdateDaisies(2);
        }

        //set sight Points      
        SightPoints = maxSightPoints;
        UIManager.Instance.sightBar.maxValue = maxSightPoints;
        spawnPoint = transform.position;
    }

    void Update()
    {
        if (!visualNovelMode && !tutorialMode && !dead)
            SetMovementVector();
        else
        {
            StopMovement();
        }

        //deparalyze after cooldown
        if (paralyzeStart + paralyzeCooldown <= Time.time && paralyzed)
        {
            StopParalysis();
        }

        #region invincibility
        if (Time.time > startTimeOfInvincibility + invicibilityCooldown)
        {
            EndInvincibility();
        }
        #endregion

        #region clear state
        if (clearSightAvailable)
        {
            if (Input.GetKeyDown(KeyCode.Space) && !visualNovelMode && !tutorialMode && !dead)
            {    
                UpdateClearState();

                AudioManager.Instance.PlayClearSight();
            }
            if (Input.GetKeyUp(KeyCode.Space))
            {
                UpdateNormalState();

                AudioManager.Instance.StopClearSight();
            }

            if (currState == State.Clear)
                UpdateClearState();
        }
        #endregion

        #region animator
        playerAnimator.SetFloat("Horizontal", movement.x);
        playerAnimator.SetFloat("Vertical", movement.y);
        playerAnimator.SetFloat("Speed", movement.sqrMagnitude);
        #endregion

        #region audio
        //footsteps
        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0) //moving
        {
            if (!visualNovelMode && !paralyzed && !dead && !tutorialMode)
                AudioManager.Instance.PlayWalking();
        }
        if ((Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0) || visualNovelMode || paralyzed || dead || tutorialMode) //stop
        {
            AudioManager.Instance.StopWalking();
        }

        #endregion

    }

    void FixedUpdate()
    {
        if (!paralyzed && !visualNovelMode && !tutorialMode && !dead)
            Move();
    }
}
