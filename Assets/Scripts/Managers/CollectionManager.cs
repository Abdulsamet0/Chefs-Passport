using UnityEngine;

public class CollectionManager : MonoBehaviour
{
    public static CollectionManager Instance;

    private void Awake()
    {
        // Singleton
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    // Yemeğin kilidini açıp cihaz hafızasına kaydeder
    public void UnlockDish(DishData dish)
    {
        // Yemeğin adını kullanarak PlayerPrefs'e 1 değerini atıyoruz (1: açık, 0: kapalı)
        PlayerPrefs.SetInt("Unlocked_" + dish.dishName, 1);
        PlayerPrefs.Save();
        
        Debug.Log(dish.dishName + " koleksiyona eklendi!");
    }

    // Yemek koleksiyonda var mı diye kontrol eder
    public bool IsDishUnlocked(DishData dish)
    {
        return PlayerPrefs.GetInt("Unlocked_" + dish.dishName, 0) == 1;
    }
    
    // Words of Wonders tarzı haritada, bir ülkedeki yemeklerin ne kadarının tamamlandığını vs.
    // sorgulamak için bu metotlar çoğaltılabilir.
}
