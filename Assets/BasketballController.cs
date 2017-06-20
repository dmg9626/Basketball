using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]

public class BasketballController : MonoBehaviour 
{
    private Vector2 _screenPoint;
    private Vector2 _offset;

    private float _gravity = 2;

    void Start()
    {
        gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
    }

    void OnBecomeInvisible() // TOOD: when ball goes off screen instantiate new ball and destroy this one
    {
        Instantiate(GameObject.Find("Basketball"));
        Destroy(gameObject);
    }

    void OnMouseDown()
    {
        _screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);

        _offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(
            new Vector2(Input.mousePosition.x, Input.mousePosition.y));

        gameObject.GetComponent<Rigidbody2D>().gravityScale = 0; // disable gravity while mouse down
        gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0,0);
    }

    void OnMouseDrag()
    {
        Vector2 curScreenPoint = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        Vector2 curPosition = (Vector2) Camera.main.ScreenToWorldPoint(curScreenPoint) + _offset;

        gameObject.transform.position = curPosition;
    }

    void OnMouseUp()
    {
        gameObject.GetComponent<Rigidbody2D>().gravityScale = _gravity; // disable gravity while mouse down
    }
}