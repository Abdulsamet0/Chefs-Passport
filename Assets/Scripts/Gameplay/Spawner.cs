using UnityEngine;
using System.Collections.Generic;

public class Spawner : MonoBehaviour
{
    public float spawnInterval = 1.5f;
    public float spawnLimitX = 2.5f;
    
    // Oyun dünyasında düşecek malzemelerin sabit durmasını istediğimiz boyutu
    public float targetVisualSize = 0.6f;
    
    [Header("Obstacles")]
    public GameObject bombPrefab;
    [Range(0f, 1f)] public float bombSpawnChance = 0.2f;
    
    private List<IngredientData> currentRecipe;
    private List<IngredientData> allPossibleIngredients; 
    
    private float timer;

    public void SetupSpawner(List<IngredientData> recipe, List<IngredientData> possibleIngredients)
    {
        currentRecipe = recipe;
        allPossibleIngredients = possibleIngredients;
    }

    void Update()
    {
        if (currentRecipe == null || currentRecipe.Count == 0) return;

        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            SpawnIngredient();
            timer = 0f;
        }
    }

    void SpawnIngredient()
    {
        IngredientData ingredientToSpawn = null;
        if (Random.value > 0.3f && currentRecipe.Count > 0)
        {
            ingredientToSpawn = currentRecipe[Random.Range(0, currentRecipe.Count)];
        }
        else if (allPossibleIngredients != null && allPossibleIngredients.Count > 0)
        {
            ingredientToSpawn = allPossibleIngredients[Random.Range(0, allPossibleIngredients.Count)];
        }

        if (ingredientToSpawn != null && ingredientToSpawn.dropPrefab != null)
        {
            float randomX = Random.Range(-spawnLimitX, spawnLimitX);
            Vector3 spawnPos = new Vector3(randomX, transform.position.y, 0f);

            if (bombPrefab != null && Random.value < bombSpawnChance)
            {
                GameObject bomb = Instantiate(bombPrefab, spawnPos, Quaternion.identity);
                
                SpriteRenderer bombSr = bomb.GetComponent<SpriteRenderer>();
                if (bombSr != null && bombSr.sprite != null)
                {
                    Vector2 spriteSize = bombSr.sprite.bounds.size;
                    if (spriteSize.x > 0 && spriteSize.y > 0)
                    {
                        float scaleFactorX = targetVisualSize / spriteSize.x;
                        float scaleFactorY = targetVisualSize / spriteSize.y;
                        float finalScale = Mathf.Min(scaleFactorX, scaleFactorY);
                        bomb.transform.localScale = new Vector3(finalScale, finalScale, 1f);
                    }
                }
                else
                {
                    // Eğer SpriteRenderer yoksa (sadece kareyse) varsayılan küçültmeyi uygula
                    bomb.transform.localScale = new Vector3(targetVisualSize, targetVisualSize, 1f); 
                }

                // Bombanın diğerlerinden 1.2 kat daha hızlı düşmesini sağla
                Rigidbody2D rb = bomb.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    rb.gravityScale *= 1.2f; 
                }

                return; // Bomba attıysa malzeme atma
            }
            
            GameObject spawnedObj = Instantiate(ingredientToSpawn.dropPrefab, spawnPos, Quaternion.identity);
            
            IngredientDrop dropScript = spawnedObj.GetComponent<IngredientDrop>();
            if (dropScript != null)
            {
                dropScript.ingredientData = ingredientToSpawn;
            }
            
            SpriteRenderer sr = spawnedObj.GetComponent<SpriteRenderer>();
            if (sr != null && ingredientToSpawn.icon != null)
            {
                sr.sprite = ingredientToSpawn.icon;
                
                // GÖRSEL OTOMATİK ÖLÇEKLENDİRME: Resim 4K da olsa, 10 piksel de olsa aynı boyutta görünmesini sağlar
                Vector2 spriteSize = sr.sprite.bounds.size;
                if(spriteSize.x > 0 && spriteSize.y > 0)
                {
                    // Resmin orijinal oranını (en/boy) bozmamak için eşit oranda küçült/büyült
                    float scaleFactorX = targetVisualSize / spriteSize.x;
                    float scaleFactorY = targetVisualSize / spriteSize.y;
                    float finalScale = Mathf.Min(scaleFactorX, scaleFactorY);
                    
                    spawnedObj.transform.localScale = new Vector3(finalScale, finalScale, 1f);
                }
            }
        }
    }
}
