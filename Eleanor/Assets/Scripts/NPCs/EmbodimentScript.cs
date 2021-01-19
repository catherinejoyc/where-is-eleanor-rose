using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Pathfinding;
using Newtonsoft.Json.Linq;

public class EmbodimentScript : MonoBehaviour, ISerializable
{
    AIDestinationSetter destinationSetter;
    AIPath aiPath;
    public Animator embodimentAnimator;

    #region pick up
    public Animation anim;

    public void PickUp()
    {
        destinationSetter.target = PlayerScript.Instance.transform;
    }
    #endregion

    #region place
    bool placed = false;
    public SpriteRenderer sprRenderer;
    public Sprite transformedSpr;

    public void Place()
    {
        placed = true;

        GetComponent<Rigidbody2D>().isKinematic = true;
        destinationSetter.target = null;

        embodimentAnimator.SetTrigger("transformed");
        sprRenderer.sprite = transformedSpr;
    }
    #endregion

    #region save data
    public string _json;
    public class SaveData
    {
        public Vector2 _position;
        public bool _isFollowing;
        public bool _isPlaced;

        public SaveData(Vector2 pos, bool isFollowing, bool isPlaced)
        {
            _position = pos;
            _isFollowing = isFollowing;
            _isPlaced = isPlaced;
        }
    }

    public string Serialize()
    {
        JObject jObj = new JObject();

        jObj.Add("componentName", GetType().Name); //script/ component name

        SaveData sd = new SaveData(this.transform.position, (destinationSetter.target == null) ? false : true, placed); //add data
        jObj.Add("data", JObject.Parse(JsonUtility.ToJson(sd)));

        _json = jObj.ToString();
        return _json;
    }
    public void Deserialize(string json)
    {
        JObject jObj = JObject.Parse(json);

        SaveData sd = JsonUtility.FromJson<SaveData>(jObj["data"].ToString());

        this.transform.position = sd._position;
        if (sd._isPlaced)
        {
            Place();
        }
        else
        {
            destinationSetter.target = (sd._isFollowing) ? PlayerScript.Instance.transform : null;
        }
    }
    #endregion

    private void Update()
    {
        embodimentAnimator.SetFloat("Horizontal", aiPath.desiredVelocity.x);
        embodimentAnimator.SetFloat("Vertical", aiPath.desiredVelocity.y);
        embodimentAnimator.SetFloat("Speed", aiPath.desiredVelocity.sqrMagnitude);

        if (destinationSetter.target != null
             &&
             aiPath.remainingDistance > 20) //if embodiment is too far away
        {
            gameObject.transform.position = destinationSetter.target.position; // teleport to player
        }
    }

    private void Start()
    {
        destinationSetter = GetComponent<AIDestinationSetter>();
        aiPath = GetComponent<AIPath>();
        embodimentAnimator = GetComponentInChildren<Animator>();
    }
}
