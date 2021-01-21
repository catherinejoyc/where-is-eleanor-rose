using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using Newtonsoft.Json.Linq;

public class MinionScript : MonoBehaviour, IChangable, ISerializable
{
    public enum MinionState
    {
        Object,
        PickedUp,
        Idle,
        Following,
        Returning,
        Attacking,
        Dead
    }
    MinionState currMinionState;
    public State CurrState { get; set; }

    enum AttackState
    {
        Anticipation,
        Contact,
        Release,
        None //not currently attacking
    }
    AttackState currAttackState = AttackState.None;

    public Rigidbody2D rb;
    public Collider2D trigger;
    public Collider2D bodyColl;
    public Animator minionAnimator;

    #region following
    AIDestinationSetter destinationSetter;
    AIPath path;
    public float stoppingDistance;
    void UpdateFollowing(GameObject target_go)
    {
        //target = target_go;
        destinationSetter.target = target_go.transform;
        
        currMinionState = MinionState.Following;
        currAttackState = AttackState.None;
    }

    void Follow()
    {
        if (path.reachedEndOfPath && destinationSetter.target.gameObject.CompareTag("Player") && CurrState == State.Normal
            && currMinionState != MinionState.PickedUp)
            UpdateAttacking();
    }
    #endregion

    #region attack
    [Header("Attack")]
    float lastAttack = 0;

    [Tooltip("If this is changed, change also animation length")]
    public float attackCooldown;

    [Tooltip("If this is changed, change also red circle size")]
    public float attackRadius;

    public SpriteRenderer redCircle;
    float anticipationStartTime;

    public ParticleSystem attackEffect;
    void UpdateAttacking()
    {
        currMinionState = MinionState.Attacking;

        switch (currAttackState)
        {
            case AttackState.None:
                currAttackState = AttackState.Anticipation;
                anticipationStartTime = 0; //set to zero
                break;
            case AttackState.Anticipation:
                Attack();
                break;
            case AttackState.Release:
                //go back to None
                currAttackState = AttackState.None;

                if (destinationSetter.target.CompareTag("Player"))
                {
                    UpdateFollowing(destinationSetter.target.gameObject);
                }
                else
                {
                    UpdateReturning();
                }

                break;
        }   
    }
    void Attack()
    {
        if (CurrState == State.Normal)
        {
            currAttackState = AttackState.Contact;

            //play animation
            minionAnimator.SetTrigger("Contact");

            //set cooldown
            lastAttack = Time.time;

            //check nearby colliders via OverlapCircle
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, attackRadius);
            if (colliders != null)
            {
                foreach (Collider2D nearbyColl in colliders)
                {
                    if (nearbyColl.CompareTag("Player"))
                    {
                        PlayerScript.Instance.Daisies--;
                        print(gameObject.name + " Attack!");
                    }
                }
            }

            currAttackState = AttackState.Release;
        }
    }
    #endregion

    #region returning
    GameObject spawnPointGO;
    void UpdateReturning()
    {
        currMinionState = MinionState.Returning;
        currAttackState = AttackState.None;

        //move to spawnPoint
        destinationSetter.target = spawnPointGO.transform;
    }
    void ReturnToSpawnPoint()
    {
        if (CurrState == State.Normal)
        {
            if (path.reachedEndOfPath)
            {
                UpdateIdle();
            }          
        }
    }
    #endregion

    #region idle
    void UpdateIdle()
    {
        currMinionState = MinionState.Idle;
        currAttackState = AttackState.None;
    }
    #endregion

    #region object state / clear and normal (denial)
    public Sprite objSpr;
    public Sprite normalSprite;
    [Header("Check only if in Level2!")]
    public bool notTouchable;
    void UpdateObject()
    {
        if (CurrState == State.Clear) //turns into object
        {
            GetComponent<SpriteRenderer>().sprite = objSpr;
            GetComponent<Animator>().enabled = false;

            //deactivate attacking shit
            this.gameObject.transform.localScale = new Vector2(1, 1);
            redCircle.color = new Color(1, 1, 1, 0);


            //if player is near/target, pick up
            if (destinationSetter.target != null)
            {
                if (destinationSetter.target.gameObject.CompareTag("Player") && Vector2.Distance(destinationSetter.target.position, transform.position) <= 1.5f)
                {
                    if (!notTouchable)
                        UpdatePickedUp();
                    else
                    {
                        //pop up
                        UIManager.Instance.StartPopUp(transformationMessage, messageSpeaker);
                    }
                }
            }
        }
        if (CurrState == State.Normal) //turns into minion/monster
        {
            GetComponent<SpriteRenderer>().sprite = normalSprite;
            GetComponent<Animator>().enabled = true;
        }
    }

    public void UpdateClearState()
    {
        CurrState = State.Clear;

        //does not affect the minion if picked up or transformed
        if (currMinionState == MinionState.PickedUp || transformed)
            return;

        rb.isKinematic = true;
        rb.velocity = new Vector2(0,0);

        //deactivate pathfinder
        path.enabled = false;

        UpdateObject();
    }

    public void UpdateNormalState()
    {
        CurrState = State.Normal;

        //does not affect the minion if picked up or transformed
        if (currMinionState == MinionState.PickedUp || transformed)
            return;

        rb.isKinematic = false;

        //activate pathfinder
        path.enabled = true;

        UpdateObject();
    }
    #endregion

    #region picked up
    [Header("Picked Up")]
    [Tooltip("How long does this stay in object state")]
    public float countdownTime;
    float currentPickUpTime; //time already gone

    //pop up text
    float popUpStart;

    void UpdatePickedUp()
    {
        if (PlayerScript.Instance.pickedUpObj != null)
        {
            UIManager.Instance.StartPopUp("My hands are full. I can't pick it up.", "Georgia Daisy");
        }
        else
        {
            //start countdown
            currentPickUpTime = 0;

            currMinionState = MinionState.PickedUp;

            //add to playerscript
            PlayerScript.Instance.pickedUpObj = this;

            //make sprite transparent
            GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);

            //deactivate collider
            bodyColl.enabled = false;

            //image
            UIManager.Instance.AddObjImage(objSpr, normalSprite);
        }    
    }

    void PickUpCountdown()
    {
        //add time if in normal state & alive & not tutorial & not visual novel
        if (CurrState == State.Normal && !PlayerScript.Instance.dead && !PlayerScript.Instance.tutorialMode && !PlayerScript.Instance.visualNovelMode)
        {
            currentPickUpTime += Time.deltaTime;

            //Update UI
            UIManager.Instance.UpdateObjCountdown(countdownTime, currentPickUpTime);
        }

        //"following"
        transform.position = destinationSetter.target.position;

        //transform back into monster if not already transformed
        if (currentPickUpTime >= countdownTime && !transformed)
        {
            //make player invincible
            PlayerScript.Instance.StartInvincibility();

            //remove from ui
            UIManager.Instance.DeactivateObjImage();

            //change minion state
            if (destinationSetter.target == null)
                UpdateReturning();
            else
                UpdateFollowing(destinationSetter.target.gameObject);

            //Update State
            UpdateNormalState();

            //remove from player
            PlayerScript.Instance.pickedUpObj = null;

            //show sprite
            GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);

            //activate collider
            bodyColl.enabled = true;
        }
    }
    #endregion

    #region death

    [Header("Transformation Message")]
    public string transformationMessage;
    public string messageSpeaker;
    bool transformed = false;

    public bool Die() //actually transforming
    {
        //if not picked up, it cannot transform
        if (currMinionState != MinionState.PickedUp)
        {
            return false;
        }
        Debug.LogWarning("transformed");

        //remove from player
        PlayerScript.Instance.pickedUpObj = null;

        currMinionState = MinionState.Dead;
        //set to transformed
        transformed = true;

        Destroy(spawnPointGO);

        GetComponent<SpriteRenderer>().sprite = objSpr;
        GetComponent<Animator>().enabled = false;

        trigger.enabled = false;
        bodyColl.enabled = false;



        StartCoroutine(FadeSprite());

        return true;
    }
    float deathFadeTime = 5.5f;
    IEnumerator FadeSprite()
    {
        // loop over 1 second backwards
        for (float i = deathFadeTime; i >= 0; i -= Time.deltaTime)
        {
            // set color with i as alpha
            GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, i);
            yield return null;
        }

        trigger.enabled = false;
        bodyColl.enabled = false;

        UIManager.Instance.StopPopUp();
    }
    #endregion

    #region save data
    public string _json;
    public class SaveData
    {
        public Vector2 _position;
        public int _minionState;
        public float _currentPickUpTime;

        public SaveData(Vector2 pos, MinionState minionState, float curPickUpTime)
        {
            _position = pos;
            switch(minionState)
            {
                case MinionState.Object:
                    _minionState = 0;
                    break;
                case MinionState.PickedUp:
                    _minionState = 1;
                    break;
                case MinionState.Idle:
                    _minionState = 2;
                    break;
                case MinionState.Following:
                    _minionState = 3;
                    break;
                case MinionState.Returning:
                    _minionState = 4;
                    break;
                case MinionState.Attacking:
                    _minionState = 5;
                    break;
                case MinionState.Dead:
                    _minionState = 6;
                    break;
            }
            _currentPickUpTime = curPickUpTime;
        }
    }

    public string Serialize()
    {
        JObject jObj = new JObject();

        jObj.Add("componentName", GetType().Name); //script/ component name

        Debug.Log(gameObject.name + ": " + currMinionState);

        SaveData sd = new SaveData(this.transform.position, currMinionState, currentPickUpTime);
        jObj.Add("data", JObject.Parse(JsonUtility.ToJson(sd)));

        _json = jObj.ToString();
        return _json;
    }
    public void Deserialize(string json)
    {
        JObject jObj = JObject.Parse(json);

        SaveData sd = JsonUtility.FromJson<SaveData>(jObj["data"].ToString());

        this.transform.position = sd._position;
        switch (sd._minionState)
        {
            case 0:
                UpdateObject();
                break;
            case 3:
                UpdateFollowing(PlayerScript.Instance.gameObject);
                break;
            case 5:
                UpdateAttacking();
                break;
            case 1:
                Debug.LogError("Minion was loaded as 'PickedUp' which should not be possible!");
                break;
            case 4:
                UpdateReturning();
                break;
            case 2:
                UpdateIdle();
                break;
            case 6:
                currMinionState = MinionState.Dead;
                //set to transformed
                transformed = true;

                Destroy(spawnPointGO);

                GetComponent<SpriteRenderer>().sprite = objSpr;
                GetComponent<Animator>().enabled = false;

                trigger.enabled = false;
                bodyColl.enabled = false;

                StartCoroutine(FadeSprite());
                break;
        }
    }
    #endregion

    private void Start()
    {
        //create spawnPoint GO (need the transform component for A*)
        spawnPointGO = new GameObject();
        spawnPointGO.transform.position = this.transform.position;

        CurrState = State.Normal;
        UpdateIdle();

        minionAnimator = GetComponent<Animator>();

        destinationSetter = GetComponent<AIDestinationSetter>();
        path = GetComponent<AIPath>();
    }

    private void Update()
    {
        switch (currMinionState)
        {
            case MinionState.Object:
                break;
            case MinionState.Following:
                Follow();
                break;
            case MinionState.Attacking:
                switch (currAttackState)
                {
                    case AttackState.Anticipation:                      
                        redCircle.color = Color.Lerp(new Color(1,1,1,0), new Color(1, 1, 1, 1), anticipationStartTime += Time.deltaTime);

                        if (anticipationStartTime >= 1)
                        {                          
                            UpdateAttacking();

                            //reset
                            anticipationStartTime = 0;
                            redCircle.color = new Color(1, 1, 1, 0);
                        }
                        break;
                    case AttackState.Release:
                        if (Time.time >= lastAttack + attackCooldown)
                        {
                            UpdateAttacking();
                        }
                        break;
                }
                break;
            case MinionState.PickedUp:
                //picked up countdown
                PickUpCountdown();

                //deactivate pop up
                if (popUpStart + 1.5f <= Time.time)
                {
                    UIManager.Instance.StopPopUp();
                }
                break;
            case MinionState.Dead:
                break;
        }

        //pathfinding
        switch (currMinionState)
        {
            case MinionState.Returning:
                ReturnToSpawnPoint();
                break;
            case MinionState.Following:
                Follow();
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && currMinionState != MinionState.Dead)
        {
            UpdateFollowing(collision.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && currMinionState != MinionState.Dead)
        {
            UpdateReturning();

            //stop pop up
            UIManager.Instance.StopPopUp();
        }
    }
}
