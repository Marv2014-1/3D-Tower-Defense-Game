
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public int maxHealth=100;
    public int currentHealth;

    public Image Healthbar;
    public TextMeshProUGUI healthText;

    // [SerializeField] int Damage;
    void Start()
    {
        currentHealth= maxHealth;
        UpdateHealth();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Enemy"))
        {
            TakeDamage(10);
        }
    }

    private void TakeDamage(int Damage)
    {
        currentHealth-= Damage;
        currentHealth= Mathf.Clamp(currentHealth,0,maxHealth);

        UpdateHealth();

        if (currentHealth <= 0)
        {
            Die();
        }

    }



    public void UpdateHealth()
    {
        Healthbar.fillAmount= (float)currentHealth/ maxHealth;
        healthText.text= currentHealth+ "/" +maxHealth;
    }

    private void Die()
    {
        Debug.Log("You DeD");
    }
}
