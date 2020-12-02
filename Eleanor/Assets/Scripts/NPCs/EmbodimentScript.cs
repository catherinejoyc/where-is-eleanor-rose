using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Pathfinding;

public class EmbodimentScript : MonoBehaviour
{
    AIDestinationSetter destinationSetter;
    AIPath aiPath;
    public Animator embodimentAnimator;

    #region pick up
    public Animation anim;
    bool moving = false;

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
