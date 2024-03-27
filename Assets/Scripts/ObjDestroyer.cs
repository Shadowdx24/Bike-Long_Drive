using UnityEngine;

public class ObjDestroyer : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("enemy"))
        {
            Destroy(collision.gameObject);
        }
        else if (collision.gameObject.CompareTag("fuel"))
        {
            Destroy(collision.gameObject);
        }
    }
}
