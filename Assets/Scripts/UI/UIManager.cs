using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("Can Sistemi")]
    public GameObject[] heartIcons; // Sahnede 3 tane kalp resmi olacak
    
    [Header("Tarif Barı")]
    public Transform recipeBarContainer; 
    public GameObject ingredientIconPrefab; // Üzerinde Image component'i olan Prefab
    
    private List<Transform> activeIcons = new List<Transform>();
    private List<IngredientData> activeIngredients = new List<IngredientData>();

    [Header("Oyun Sonu Panelleri")]
    public GameObject winPanel;
    public GameObject losePanel;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void SetupUI(List<IngredientData> requiredIngredients, int maxLives)
    {
        winPanel.SetActive(false);
        losePanel.SetActive(false);
        UpdateLives(maxLives);

        // Eski ikonları temizle
        foreach (Transform child in recipeBarContainer) Destroy(child.gameObject);
        activeIcons.Clear();
        activeIngredients.Clear();

        // Yeni yemeğin ikonlarını yukarıya diz
        foreach (var ing in requiredIngredients)
        {
            GameObject newIcon = Instantiate(ingredientIconPrefab, recipeBarContainer);
            newIcon.GetComponent<Image>().sprite = ing.icon;
            activeIcons.Add(newIcon.transform);
            activeIngredients.Add(ing);
        }
    }

    public void UpdateLives(int currentLives)
    {
        for (int i = 0; i < heartIcons.Length; i++)
        {
            heartIcons[i].SetActive(i < currentLives);
        }
    }

    public void OnIngredientCaught(IngredientData ing)
    {
        int index = activeIngredients.IndexOf(ing);
        if (index != -1)
        {
            // Yakalanan malzemeyi yeşil yap (veya tik işareti koydurabilirsin)
            activeIcons[index].GetComponent<Image>().color = Color.green; 
            
            // Listeden siliyoruz ki aynı malzemeden 2 tane varsa sıradakini de bulabilsin
            activeIngredients.RemoveAt(index);
            activeIcons.RemoveAt(index);
        }
    }

    public void ShowWinPanel()
    {
        winPanel.SetActive(true);
    }

    public void ShowLosePanel()
    {
        losePanel.SetActive(true);
    }
}
