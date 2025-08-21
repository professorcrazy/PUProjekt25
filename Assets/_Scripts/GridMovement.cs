using System.Collections;
using UnityEngine;

public class GridMovement : MonoBehaviour
{
    public bool isUsingContinuedMovement = true;
    bool isMoving = false;
    public float moveDuration = 0.25f;
    public float gridSize = 1f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!isMoving)
        {
            System.Func<KeyCode, bool> inputFunction;
            if (isUsingContinuedMovement)
            {
                inputFunction = Input.GetKey;
            }
            else
            {
                inputFunction = Input.GetKeyDown;
            }


            if (inputFunction(KeyCode.UpArrow) || inputFunction(KeyCode.W))
            {
                StartCoroutine(Move(Vector2.up));
            } else if (inputFunction(KeyCode.DownArrow) || inputFunction(KeyCode.S))
            {
                StartCoroutine(Move(Vector2.down));
            }
            else if (inputFunction(KeyCode.RightArrow) || inputFunction(KeyCode.D))
            {
                StartCoroutine(Move(Vector2.right));
            }
            else if (inputFunction(KeyCode.LeftArrow) || inputFunction(KeyCode.A))
            {
                StartCoroutine(Move(Vector2.left));
            }
        }

    }

    IEnumerator Move(Vector2 dir)
    {
        isMoving = true;
        Vector2 startPos = transform.position;

        Vector2 endPos = startPos + (dir * gridSize);

        float elapsedTime = 0;
        while (elapsedTime < moveDuration)
        {
            elapsedTime += Time.deltaTime;
            float percent = elapsedTime / moveDuration;
            transform.position = Vector2.Lerp(startPos, endPos, percent);
            yield return null;
        }
        transform.position = endPos;

        isMoving = false;
    }
}
