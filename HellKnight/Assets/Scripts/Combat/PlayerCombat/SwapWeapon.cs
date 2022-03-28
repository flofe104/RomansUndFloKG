using System.Collections;
using System.Collections.Generic;
using Testing;
using UnityEngine;

[TestMonoBehaviour(CallStartBeforeTesting = true)]
public class SwapWeapon : MonoBehaviour
{

    public PlayerMeleeCombat meleeBehaviour;

    public PlayerRangedCombat rangeBehaviour;

    public List<InventoryMeleeWeapon> meleeItems;

    public List<InventoryRangedWeapon> rangedWeapon;

    protected int activeWeaponIndex;


    protected InstantiableInventoryItem ItemFromIndex()
    {
        int totalItems = meleeItems.Count + rangedWeapon.Count;
        //modulo but turn negative index into positive
        activeWeaponIndex %= totalItems;
        activeWeaponIndex += totalItems;
        activeWeaponIndex %= totalItems;
        if(activeWeaponIndex >= meleeItems.Count)
        {
            return rangedWeapon[activeWeaponIndex - meleeItems.Count];
        }
        else
        {
            return meleeItems[activeWeaponIndex];
        }
    }

    private void Start()
    {
        EquippWeapon(ItemFromIndex());
    }

    private void Update()
    {
        int nextWeaponIndex = activeWeaponIndex + GetMouseScrollDirection();
        if(nextWeaponIndex != activeWeaponIndex)
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

    protected void EquippWeapon(InstantiableInventoryItem weapon)
    {
        if (weapon is InventoryMeleeWeapon m)
            EquippWeapon(m);
        else
            EquippWeapon(weapon as InventoryRangedWeapon);
    }

    protected void EquippWeapon(InventoryMeleeWeapon weapon)
    {
        SetEnabledAndDestroyEquipedWeapons(true);
        meleeBehaviour.EquipWeapon(weapon);
    }

    protected void EquippWeapon(InventoryRangedWeapon weapon)
    {
        SetEnabledAndDestroyEquipedWeapons(false);
        rangeBehaviour.EquipWeapon(weapon);
    }

    public void SetEnabledAndDestroyEquipedWeapons(bool meleeEnabled)
    {
        meleeBehaviour.enabled = meleeEnabled;
        rangeBehaviour.enabled = !meleeEnabled;
        rangeBehaviour.DestroyEquipedWeapon();
        meleeBehaviour.DestroyEquipedWeapon();
    }

    #region tests

    [Test]
    protected void TestHasInitialWeaponEquipped()
    {

        Assert.IsTrue(!rangeBehaviour.enabled && !rangeBehaviour.HasWeaponEquipped);
        Assert.IsTrue(meleeBehaviour.enabled && meleeBehaviour.HasWeaponEquipped);
    }


    [TestEnumerator]
    protected IEnumerator TestPlayerBehaviourAfterWeaponSwap()
    {
        do
        {
            yield return null;
        } while (ItemFromIndex() is InventoryMeleeWeapon);

        Assert.IsTrue(rangeBehaviour.enabled && rangeBehaviour.HasWeaponEquipped);
        Assert.IsTrue(!meleeBehaviour.enabled && !meleeBehaviour.HasWeaponEquipped);
    }


    #endregion

}
