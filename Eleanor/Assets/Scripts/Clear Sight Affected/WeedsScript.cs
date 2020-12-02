using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeedsScript : MonoBehaviour, IChangable
{
    public State CurrState { get; set; }

    public Collider2D coll;

    public SpriteRenderer sprRenderer;
    public Sprite weedsSprite;
    public Sprite taresSprite;

    public void UpdateClearState() //Tares
    {
        CurrState = State.Clear;
        coll.enabled = true;

        sprRenderer.sprite = taresSprite;

        AstarPath.active.UpdateGraphs(GetComponent<Collider2D>().bounds);
    }
    public void UpdateNormalState() //Weeds
    {
        CurrState = State.Normal;
        coll.enabled = false;

        sprRenderer.sprite = weedsSprite;

        AstarPath.active.UpdateGraphs(GetComponent<Collider2D>().bounds);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject go = collision.gameObject;

        if (go.CompareTag("Player"))
        {
            go.GetComponent<PlayerScript>().Daisies--;
        }
    }
}
