using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewCountry", menuName = "RecipeCatcher/Country")]
public class CountryData : ScriptableObject
{
    public string countryName;
    public Sprite background;
    public List<DishData> dishes; // Words of Wonders tarzı: Her ülkede bir dizi yemek
}
