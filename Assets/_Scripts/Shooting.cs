using UnityEngine;

public class Shooting : MonoBehaviour
{
    [SerializeField] private GameObject prefab;
    public float shotSpeed = 10f;
    public float shotRPS = 2f;
    float lastShot = 0f;
    public float shotLifetime = 3f;
    public KeyCode shotKey = KeyCode.LeftControl;

    // Update is called once per frame
    void Update()
    {
        if (prefab == null)
        {
            return;
        }

        if (Input.GetKey(shotKey) && (lastShot + 1f/shotRPS <= Time.time))
        {
            lastShot = Time.time;
            GameObject shot = Instantiate(prefab, transform.position, Quaternion.identity);
            if (shot.GetComponent<Rigidbody2D>() != null)
            {
                shot.GetComponent<Rigidbody2D>().linearVelocity = transform.right * shotSpeed;
            }
            Destroy(shot, shotLifetime);
        }
    }
}