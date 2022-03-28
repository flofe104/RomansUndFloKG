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

    protected float scrollProgress;

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
        scrollProgress = activeWeaponIndex;
        EquippWeapon(ItemFromIndex());
    }

    private void Update()
    {
        if (Input.GetAxis("Mouse ScrollWheel") != 0f)
        {
            scrollProgress += Input.GetAxis("Mouse ScrollWheel");
            if((int)scrollProgress != activeWeaponIndex)
            {
                activeWeaponIndex = (int)scrollProgress;
                EquippWeapon(ItemFromIndex());
            }
        }
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
