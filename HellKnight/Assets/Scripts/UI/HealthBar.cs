using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public PlayerHealth playerHealth;
    public Image fillImage;

    void Update()
    {
        float fillValue = playerHealth.currentHealth / (float)playerHealth.maxHealth;
        fillImage.fillAmount = fillValue;
    }
}
