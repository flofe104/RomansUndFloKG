using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AssetMenue : MonoBehaviour
{

    private const string BASE_FOLDER_NAME = "ScriptableObjects/";

    [MenuItem("Assets/Create/Custom/Weapons/MeleeWeapons")]
    public static void NewMeleeWeapon()
    {
        AssetCreator.CreateAsset<InventoryMeleeWeapon>(BASE_FOLDER_NAME + "MeleeWeapons");
    }

    [MenuItem("Assets/Create/Custom/Weapons/RangedWeapons")]
    public static void NewRangedWeapon()
    {
        AssetCreator.CreateAsset<InventoryRangedWeapon>(BASE_FOLDER_NAME + "RangedWeapons");
    }

    [MenuItem("Assets/Create/Custom/Enemies/Default")]
    public static void NewDefaultEnemy()
    {
        AssetCreator.CreateAsset<EnemySciptableObject>(BASE_FOLDER_NAME + "DefaultEnemy");
    }


}
