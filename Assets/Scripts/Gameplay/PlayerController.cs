using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 10f;
    public float limitX = 2.5f; // Ekranın dışına çıkmaması için sınır

    void Update()
    {
        // Mobil için Drag (Sürükleme) kontrolü
        if (Input.GetMouseButton(0))
        {
            Vector3 touchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            touchPosition.z = 0f;

            // Karakteri (veya sepeti taşıyan aşçıyı) sadece X ekseninde dokunulan yere doğru yumuşakça hareket ettir
            float targetX = Mathf.Clamp(touchPosition.x, -limitX, limitX);
            Vector3 targetPosition = new Vector3(targetX, transform.position.y, transform.position.z);
            
            transform.position = Vector3.Lerp(transform.position, targetPosition, speed * Time.deltaTime);
        }
    }
}
