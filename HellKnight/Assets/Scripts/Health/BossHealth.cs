using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHealth : BaseHealth
{



    protected override void OnEntityDied()
    {
        UIManager uIManager= FindObjectOfType<UIManager>();

        uIManager.activateOnBossDeath.SetActive(true);
        uIManager.deactivateOnBossDeath.SetActive(false);

        foreach(MonoBehaviour m in GetComponents<MonoBehaviour>())
            m.enabled = false;

        Time.timeScale = 0;
    }

}
