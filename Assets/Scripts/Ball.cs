using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private bool _hasCollided;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!_hasCollided)
        {
            if (collision.gameObject.GetComponent<Wall>() != null)
            {
                GameManager.instance.OnBallCollided();
            }
            _hasCollided = true;
        }
    }

    public void BallStart()
    {
        _hasCollided = false;
        this.transform.position = new Vector2(-Camera.main.orthographicSize * Screen.width / Screen.height * 0.65f, 0);
    }

    public void Move(float speed)
    {
        Vector3 translation = new Vector3(0, -speed, 0);
        if (Input.GetKey(KeyCode.UpArrow) || Input.GetMouseButton(0)) translation = new Vector3(0, speed, 0);
        
        this.transform.Translate(translation * Time.deltaTime);
    }
}
