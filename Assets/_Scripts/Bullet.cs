using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEditor.Experimental.GraphView.GraphView;

public class Bullet : MonoBehaviour
{
    public LayerMask destroyableLayer = 1 << 7;
    private void OnCollisionEnter2D(Collision2D other) {
        Debug.Log("Layer: " + (other.gameObject.layer) + " " + other.gameObject.name + " Destroyable Layer: " + destroyableLayer.ToString());

        if (other.collider.CompareTag("Player")) {
            if (RespawnSystem.instance != null) {
                RespawnSystem.instance.PlayerDied();
            }
            else {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
        else {
            int collidingLayer = (1 << destroyableLayer);
            if ((other.gameObject.layer & collidingLayer) != 0) {
                Debug.Log("Layer: " + (other.gameObject.layer) + " " + other.gameObject.name);
                Destroy(other.gameObject);
            }
        }
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        
        Debug.Log("Layer: " + (other.gameObject.layer) + " " + other.gameObject.name + " Destroyable Layer: " + (destroyableLayer).ToString());

        if (other.CompareTag("Player")) {
            if (RespawnSystem.instance != null) {
                RespawnSystem.instance.PlayerDied();
            }
            else {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
        else {
            int collidingLayer = (1 << destroyableLayer);
            if ((other.gameObject.layer & collidingLayer) != 0) {
                Debug.Log("Layer: " + (other.gameObject.layer) + " " + other.gameObject.name);
                Destroy(other.gameObject);
            }
        }
        Destroy(gameObject);
    }
}
