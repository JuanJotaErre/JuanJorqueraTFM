using UnityEngine;
using UnityEngine.SceneManagement;

public class HealthController : MonoBehaviour
{

    [SerializeField]
    public int maxHealth = 10;
    
    public int currentHealth;

    float lastDamage = 0;
    float invicibilitySeconds = 1f;

    void Start()
    {
        currentHealth = maxHealth;
    }


    public void Damage(int strength, GameObject collider)
    {
        if(Time.time - lastDamage > invicibilitySeconds)
        {
            currentHealth -= strength;
            lastDamage = Time.time;

            CheckHealth();
            
        }
    }

    private void CheckHealth()
    {
        if(currentHealth <= 0)
        {
            if(gameObject.CompareTag("Player"))
            {
                SceneManager.LoadScene("SampleScene");
            }
            Destroy(gameObject);
        }
    }

    internal void Heal(int healStrength)
    {
        currentHealth += healStrength;
    }
}
