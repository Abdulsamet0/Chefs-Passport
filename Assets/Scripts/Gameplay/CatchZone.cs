using UnityEngine;

public class CatchZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        IngredientDrop drop = collision.GetComponent<IngredientDrop>();
        if (drop != null)
        {
            // Yakalanan objeyi GameManager'a bildir
            GameManager.Instance.IngredientCaught(drop.ingredientData);
            
            // Sepete giren fiziksel objeyi yok et
            Destroy(collision.gameObject); 
        }

        ObstacleDrop bomb = collision.GetComponent<ObstacleDrop>();
        if (bomb != null)
        {
            GameManager.Instance.ObstacleCaught();
            Destroy(collision.gameObject);
        }
    }
}
