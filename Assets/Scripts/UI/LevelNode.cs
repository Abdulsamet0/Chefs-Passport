using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelNode : MonoBehaviour
{
    public Image nodeImage;
    public Button nodeButton;
    private DishData myDish;

    public void Setup(DishData dish, bool canPlay, bool isCompleted)
    {
        myDish = dish;

        if (isCompleted)
        {
            nodeImage.color = Color.green; // Tamamlananlar yeşil (İstersen buraya tik işareti de koydurabilirsin)
            nodeButton.interactable = true;
        }
        else if (canPlay)
        {
            nodeImage.color = Color.white; // Oynanabilir durumda (sıra onda)
            nodeButton.interactable = true;
        }
        else
        {
            nodeImage.color = Color.gray;  // Kilitli (sıra daha gelmemiş)
            nodeButton.interactable = false;
        }

        // Butona tıklandığında OnNodeClicked çalışsın
        nodeButton.onClick.AddListener(OnNodeClicked);
    }

    private void OnNodeClicked()
    {
        // Seçilen yemeği GameManager'ın bileceği static değişkene atıyoruz
        GameManager.SelectedDish = myDish; 
        
        // Oyun sahnesine geçiş yapıyoruz
        SceneManager.LoadScene("GameScene");
    }
}
