using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThornsScript : MonoBehaviour, IChangable
{
    public State CurrState { get; set; } = State.Normal;

    public Collider2D coll;

    public SpriteRenderer sprRenderer;
    public Sprite thornsSprite;
    public Sprite rosesSprite;

    public void UpdateClearState() //Roses
    {
        CurrState = State.Clear;
        coll.enabled = false;

        sprRenderer.sprite = rosesSprite;

        AstarPath.active.UpdateGraphs(GetComponent<Collider2D>().bounds); //does not work properly in free version...
    }

    public void UpdateNormalState() //Thorns
    {
        CurrState = State.Normal;
        coll.enabled = true;

        sprRenderer.sprite = thornsSprite;

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

