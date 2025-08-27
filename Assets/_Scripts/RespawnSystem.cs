using UnityEngine;

public class RespawnSystem : MonoBehaviour
{
    public static RespawnSystem instance;
    Vector3 originalPlayerSpwan;
    Vector3 playerSpwan = Vector3.zero;
    Transform player;
    private void Awake() {
        if (instance != null) {
            Destroy(this);
        }
        instance = this;
    }

    private void Start() {
        if (GameObject.FindGameObjectWithTag("Player") != null) {
            player = GameObject.FindGameObjectWithTag("Player").transform;
            originalPlayerSpwan = player.position;
            playerSpwan = originalPlayerSpwan;
        }
    }
    public void PlayerDied() {
        if (player != null) {
            player.transform.position = playerSpwan;
            if (player.GetComponent<Rigidbody2D>()) { 
                player.GetComponent<Rigidbody2D>().linearVelocity= Vector2.zero;
            }
        }
    }

    public void UpdatePlayerSpawn(Vector3 pos) {
        playerSpwan = pos;
    }
}
