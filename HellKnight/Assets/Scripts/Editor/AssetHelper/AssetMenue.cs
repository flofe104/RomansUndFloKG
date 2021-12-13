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


}
