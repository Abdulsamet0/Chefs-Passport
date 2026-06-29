using UnityEngine;

[CreateAssetMenu(fileName = "NewIngredient", menuName = "RecipeCatcher/Ingredient")]
public class IngredientData : ScriptableObject
{
    public string ingredientName;
    public Sprite icon; // UI'da göstermek için
    public GameObject dropPrefab; // Gökyüzünden düşecek fiziksel obje
}
