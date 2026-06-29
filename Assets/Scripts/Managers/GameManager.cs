using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement; // Haritaya dönmek için eklendi

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    // Haritadan seçilen yemeği buraya aktarmak için Static değişken kullanıyoruz
    public static DishData SelectedDish; 

    [Header("Level Data")]
    public DishData currentDish;
    public List<IngredientData> allPossibleIngredients;
    public Spawner spawner;
    
    [Header("Game Status")]
    public int lives = 3;
    private List<IngredientData> collectedIngredients = new List<IngredientData>();
    private List<IngredientData> requiredIngredientsRemaining = new List<IngredientData>();
    private bool isGameOver = false; // Oyun bittiyse hareket etmesini engellemek için

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        // Eğer MapScene üzerinden bir yemek seçilip geldiyse onu kullan
        if (SelectedDish != null)
        {
            currentDish = SelectedDish;
        }
        
        if(currentDish != null)
        {
            StartLevel(currentDish);
        }
    }

    public void StartLevel(DishData dish)
    {
        isGameOver = false;
        currentDish = dish;
        collectedIngredients.Clear();
        requiredIngredientsRemaining = new List<IngredientData>(dish.requiredIngredients);
        lives = 3; 
        
        // UI'ı kur
        UIManager.Instance.SetupUI(dish.requiredIngredients, lives);
        spawner.SetupSpawner(requiredIngredientsRemaining, allPossibleIngredients);
    }

    public void IngredientCaught(IngredientData caughtIngredient)
    {
        if (isGameOver) return; // Oyun bittiyse puan hesaplama

        if (requiredIngredientsRemaining.Contains(caughtIngredient))
        {
            requiredIngredientsRemaining.Remove(caughtIngredient);
            collectedIngredients.Add(caughtIngredient);
            
            // UI tarafında yeşil yap
            UIManager.Instance.OnIngredientCaught(caughtIngredient);

            if (requiredIngredientsRemaining.Count == 0)
            {
                DishCompleted();
            }
            else
            {
                spawner.SetupSpawner(requiredIngredientsRemaining, allPossibleIngredients);
            }
        }
        else
        {
            lives--;
            UIManager.Instance.UpdateLives(lives); // UI'dan kalbi sil
            
            if (lives <= 0)
            {
                GameOver();
            }
        }
    }

    public void ObstacleCaught()
    {
        if (isGameOver) return;
        
        // Bomba yakalanırsa 1 can gitsin
        lives--;
        UIManager.Instance.UpdateLives(lives);
        
        if (lives <= 0)
        {
            GameOver();
        }
    }

    private void DishCompleted()
    {
        isGameOver = true;
        CollectionManager.Instance.UnlockDish(currentDish);
        UIManager.Instance.ShowWinPanel(); // Kazanma ekranını aç
    }

    private void GameOver()
    {
        isGameOver = true;
        UIManager.Instance.ShowLosePanel(); // Kaybetme ekranını aç
    }
    
    // UI'daki butonlara atanacak fonksiyonlar (OnClick)
    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ReturnToMap()
    {
        SceneManager.LoadScene("MapScene"); // MapScene adında bir sahnemiz olacak
    }
}
