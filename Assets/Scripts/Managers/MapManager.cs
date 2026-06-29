using UnityEngine;
using UnityEngine.UI;

public class MapManager : MonoBehaviour
{
    public CountryData currentCountry; // Ekranda gösterilen ülke
    public Image backgroundImage;
    
    public Transform nodesContainer; // Butonların (düğümlerin) dizileceği UI nesnesi
    public GameObject nodePrefab; // Üzerinde LevelNode.cs olan buton prefabı

    private void Start()
    {
        if (currentCountry != null)
        {
            LoadCountry(currentCountry);
        }
    }

    public void LoadCountry(CountryData country)
    {
        currentCountry = country;
        
        if (backgroundImage != null && country.background != null)
        {
            backgroundImage.sprite = country.background;
        }

        // Eski düğümleri temizle
        foreach (Transform child in nodesContainer) Destroy(child.gameObject);

        bool previousUnlocked = true; // İlk bölüm her zaman açıktır

        // WoW tarzı harita düğümlerini oluştur
        for (int i = 0; i < country.dishes.Count; i++)
        {
            DishData dish = country.dishes[i];
            
            GameObject newNode = Instantiate(nodePrefab, nodesContainer);
            LevelNode nodeScript = newNode.GetComponent<LevelNode>();
            
            bool isThisDishUnlocked = CollectionManager.Instance.IsDishUnlocked(dish);
            
            // Bölüm kilitli mi açık mı?
            bool canPlay = previousUnlocked; 
            
            nodeScript.Setup(dish, canPlay, isThisDishUnlocked);

            // Eğer bu bölüm henüz bitirilmediyse, bundan sonrakiler kilitli kalacak
            if (!isThisDishUnlocked)
            {
                previousUnlocked = false; 
            }
        }
    }
}
