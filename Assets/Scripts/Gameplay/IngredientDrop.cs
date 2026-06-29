using UnityEngine;

public class IngredientDrop : MonoBehaviour
{
    public IngredientData ingredientData; // Bu düşen obje hangi malzeme?

    // Spawner bu objeyi yarattığında otomatik çalışıp kimliğini verecek
    public void Setup(IngredientData data)
    {
        ingredientData = data;

        // Eğer objede resim gösterici (SpriteRenderer) varsa, resmini de otomatik güncelle!
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null && data.icon != null)
        {
            sr.sprite = data.icon;
        }
    }

    private void Update()
    {
        // Ekrandan çok aşağı düşerse yok et
        if (transform.position.y < -10f)
        {
            Destroy(gameObject);
        }
    }
}