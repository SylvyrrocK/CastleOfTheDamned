using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BarControl : MonoBehaviour
{
    // Start is called before the first frame update
    public PlayerController player;
    public Health health;

    public Slider healthSlider;
    public Slider staminaSlider;


    public void SetHealthSlider()
    {
        healthSlider.value = health.health / health.maxHealth;
    }
    public void SetStaminaSlider()
    {
        staminaSlider.value = player.currentStamina / player.maxStamina;
    }

    // Update is called once per frame
    void Update()
    {
        if (healthSlider != null && health != null) {
            SetHealthSlider();
        }
        else
        {
            Debug.Log($"Ты что-то забыл!");
        }

        if (staminaSlider != null && player != null)
        {
            SetStaminaSlider();
        }
        else
        {
            Debug.Log($"Ты что-то забыл!");
        }

    }
}
