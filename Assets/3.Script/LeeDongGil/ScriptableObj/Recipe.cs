using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RecipeData", menuName = "ScriptableObject/RecipeData")]
public class Recipe : ScriptableObject
{
    [Header("Materials order == MaterialCount order")]
    public List<ItemData> materials;
    public List<int> materialCount;
    public ItemData resultItem;

}
