using UnityEngine;
using UnityEngine.SceneManagement;

public class ZombieTarget : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            Debug.Log("Zombie touché !");
            // SceneManager.LoadScene("City"); // remplace "City" par le vrai nom
        }
    }
}
