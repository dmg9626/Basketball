using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]

public class BasketballController : MonoBehaviour 
{
    Vector2 offset;
    Vector2 lastPosition;
    Vector2 currentPosition;
    Vector2 startPosition = new Vector2(0, -2);

    Rigidbody2D basketball;

    public float maxVelocity = 10;
    float gravity = 2;

    float screenBoundBottom = -8;
    float screenBoundTop = 12;
    float screenBoundLeft = -10;
    float screenBoundRight = 10;

    bool thrown = false;

    void Start()
    {
        basketball = gameObject.GetComponent<Rigidbody2D>();
        basketball.gravityScale = 0;

        gameObject.transform.position = startPosition;
        currentPosition = transform.position;
    }

    void Update()
    {
        lastPosition = currentPosition;
        currentPosition = transform.position;

        if (transform.position.y < screenBoundBottom)
            Reset();
        else if (transform.position.x < screenBoundLeft || transform.position.x > screenBoundRight)
            Reset();
        
        /* make ball smaller as it goes away
        else
        {
            if((currentPosition - lastPosition).y > 0F) // ball moving up
            {
                gameObject.transform.localScale.Scale(new Vector3(.001, .001));
            }
        }
        */
    }

    void Reset()
    {
        gameObject.transform.position = startPosition;
        basketball.gravityScale = 0;
        basketball.velocity = Vector2.zero;
        thrown = false;
    }

    void OnMouseDown()
    {
        if(!thrown)
        {
            //_screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);

            offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(
                new Vector2(Input.mousePosition.x, Input.mousePosition.y));

            basketball.gravityScale = 0; // disable gravity while mouse down
            basketball.velocity = new Vector2(0,0);
        }

    }

    void OnMouseDrag()
    {
        if(!thrown)
        {
            Vector2 curScreenPoint = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            Vector2 curPosition = (Vector2) Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;

            gameObject.transform.position = curPosition;
        }
    }

    void OnMouseUp()
    {
        // get direction of movement in last frame
        // move object in that direction at velocity
        if(!thrown)
        {
            Vector2 velocity = (currentPosition - lastPosition) * 20;
            if (velocity.y > maxVelocity)
                velocity.y = maxVelocity;

            basketball.velocity = velocity;
            basketball.gravityScale = gravity; // disable gravity while mouse down

            thrown = true;
        }
    }
}