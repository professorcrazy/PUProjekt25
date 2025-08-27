using UnityEngine;

public class Pickup : MonoBehaviour
{
    public int point = 1;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (ScoreSystem.instance != null)
            {
                ScoreSystem.instance.UpdateScore(point);
            }
            Destroy(gameObject);
        }
    }
}