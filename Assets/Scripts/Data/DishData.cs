using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewDish", menuName = "RecipeCatcher/Dish")]
public class DishData : ScriptableObject
{
    public string dishName;
    public Sprite completedDishImage;
    public List<IngredientData> requiredIngredients; // Bu yemeğin tarifi
}
