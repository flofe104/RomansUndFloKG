using System.Collections;
using System.Collections.Generic;
using Testing;
using UnityEngine;

[TestMonoBehaviour(CallStartBeforeTesting = true)]
public class SwapWeapon : MonoBehaviour
{

    public PlayerWeaponUser player;

    public List<BaseInventoryWeapon> weapons;


    protected int activeWeaponIndex;


    protected BaseInventoryWeapon ItemFromIndex()
    {
        int totalItems = weapons.Count;
        //modulo but turn negative index into positive
        activeWeaponIndex %= totalItems;
        activeWeaponIndex += totalItems;
        activeWeaponIndex %= totalItems;
        return weapons[activeWeaponIndex];
    }

    private void Start()
    {
        EquippWeapon(ItemFromIndex());
    }

    private void Update()
    {
        int nextWeaponIndex = activeWeaponIndex + GetMouseScrollDirection();
       
        if(Input.GetKeyDown(KeyCode.Q))
        {
            nextWeaponIndex -= 1;
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            nextWeaponIndex += 1;
        }

        if (nextWeaponIndex != activeWeaponIndex)
        {
            activeWeaponIndex = nextWeaponIndex;
            EquippWeapon(ItemFromIndex());
        }
    }

    protected int GetMouseScrollDirection()
    {
        float scrollDir = Input.GetAxis("Mouse ScrollWheel");
        if (scrollDir > 0f)
            return 1;
        else if (scrollDir < 0f)
            return -1;
        else
            return 0;
    }

    protected void EquippWeapon(BaseInventoryWeapon weapon)
    {
        player.DestroyEquipedWeapon();
        player.EquipWeapon(weapon);
    }

    #region tests

    [Test]
    protected void TestHasInitialWeaponEquipped()
    {
        Assert.IsTrue(player.enabled && player.HasWeaponEquipped);
    }


    [TestEnumerator]
    protected IEnumerator TestPlayerBehaviourAfterWeaponSwap()
    {
        do
        {
            yield return null;
        } while (ItemFromIndex() is InventoryMeleeWeapon);

        Assert.IsTrue(player.enabled && player.HasWeaponEquipped);
    }

    #endregion

}
