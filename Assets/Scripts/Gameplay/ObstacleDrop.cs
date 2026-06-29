using UnityEngine;

public class ObstacleDrop : MonoBehaviour
{
    // Ekranın altına düştüğünde kendini yok et
    private void Update()
    {
        if (transform.position.y < -6f)
        {
            Destroy(gameObject);
        }
    }
}
