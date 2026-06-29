using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;

public class CSVDataImporter : EditorWindow
{
    private static string csvPath = "LevelData.csv";

    [MenuItem("Chef's Passport/CSV'den Bölümleri Yükle (Otomatik İkonlu)")]
    public static void ImportCSVData()
    {
        string fullPath = Path.Combine(Application.dataPath, "../", csvPath);
        
        if (!File.Exists(fullPath))
        {
            Debug.LogError("CSV dosyası bulunamadı! Lütfen ana klasörde LevelData.csv olduğundan emin ol. Yol: " + fullPath);
            return;
        }

        EnsureFolderExists("Assets/GameData");
        EnsureFolderExists("Assets/GameData/Ingredients");
        EnsureFolderExists("Assets/GameData/Dishes");
        EnsureFolderExists("Assets/GameData/Countries");
        
        // Kullanıcının belirttiği ikon klasörlerini yarat/kontrol et
        EnsureFolderExists("Assets/GameData/Icons");
        EnsureFolderExists("Assets/GameData/Icons/Ingredient Icons");
        EnsureFolderExists("Assets/GameData/Icons/Dish Icons");

        // Ortak düşme şablonunu (IngredientDrop olan Prefab) projeden bul
        GameObject commonDropPrefab = FindCommonDropPrefab();
        if (commonDropPrefab == null)
        {
            Debug.LogWarning("Projede 'IngredientDrop' scripti içeren bir Prefab bulunamadı! Lütfen malzemeler için bir Prefab oluşturun.");
        }

        string[] lines = File.ReadAllLines(fullPath);
        int successCount = 0;

        foreach (string line in lines)
        {
            if (string.IsNullOrWhiteSpace(line)) continue;
            
            string[] parts = line.Split(';');
            if (parts.Length < 3) continue;

            string countryName = parts[0].Trim();
            string dishName = parts[1].Trim();
            string[] ingredientNames = parts[2].Split(',');

            List<IngredientData> dishIngredients = new List<IngredientData>();
            foreach (string ing in ingredientNames)
            {
                string cleanIng = ing.Trim();
                if (string.IsNullOrEmpty(cleanIng)) continue;
                
                IngredientData ingData = GetOrCreateAsset<IngredientData>($"Assets/GameData/Ingredients/{cleanIng}.asset");
                ingData.ingredientName = cleanIng;
                
                // OTOMATİK İKON ATAMA (Sadece Ingredient Icons klasöründe ara)
                if (ingData.icon == null)
                {
                    ingData.icon = FindSprite(cleanIng, "Assets/GameData/Icons/Ingredient Icons");
                }
                
                // OTOMATİK ŞABLON ATAMA
                if (ingData.dropPrefab == null && commonDropPrefab != null)
                {
                    ingData.dropPrefab = commonDropPrefab;
                }
                
                EditorUtility.SetDirty(ingData);
                dishIngredients.Add(ingData);
            }

            DishData dishData = GetOrCreateAsset<DishData>($"Assets/GameData/Dishes/{dishName}.asset");
            dishData.dishName = dishName;
            dishData.requiredIngredients = dishIngredients;
            
            // Yemek bitmiş resmi (Opsiyonel) için de otomatik atama yap (Sadece Dish Icons klasöründe ara)
            if (dishData.completedDishImage == null)
            {
                dishData.completedDishImage = FindSprite(dishName, "Assets/GameData/Icons/Dish Icons");
            }
            
            EditorUtility.SetDirty(dishData);

            CountryData countryData = GetOrCreateAsset<CountryData>($"Assets/GameData/Countries/{countryName}.asset");
            countryData.countryName = countryName;
            if (countryData.dishes == null) countryData.dishes = new List<DishData>();
            
            if (!countryData.dishes.Contains(dishData))
            {
                countryData.dishes.Add(dishData);
            }
            EditorUtility.SetDirty(countryData);
            
            successCount++;
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        
        Debug.Log($"<color=green>Başarılı!</color> İkonlar otomatik tarandı ve {successCount} satır veri işlendi!");
    }

    private static void EnsureFolderExists(string folderPath)
    {
        if (!AssetDatabase.IsValidFolder(folderPath))
        {
            int lastSlash = folderPath.LastIndexOf('/');
            string parent = folderPath.Substring(0, lastSlash);
            string newFolder = folderPath.Substring(lastSlash + 1);
            AssetDatabase.CreateFolder(parent, newFolder);
        }
    }

    private static T GetOrCreateAsset<T>(string path) where T : ScriptableObject
    {
        T asset = AssetDatabase.LoadAssetAtPath<T>(path);
        if (asset == null)
        {
            asset = ScriptableObject.CreateInstance<T>();
            AssetDatabase.CreateAsset(asset, path);
        }
        return asset;
    }
    
    // Proje içindeki belirli bir klasörde resimleri (Sprite) ismine göre tarar
    private static Sprite FindSprite(string name, string folderPath)
    {
        // Klasörün var olduğundan emin ol (Hata vermemesi için)
        if (!AssetDatabase.IsValidFolder(folderPath)) return null;

        // t:Sprite yazarak ve belirli bir klasörü vererek aramayı hızlandırıyoruz
        string[] searchFolders = new string[] { folderPath };
        string[] guids = AssetDatabase.FindAssets(name + " t:Sprite", searchFolders);
        
        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            // Sadece ismi birebir aynı olanı al (Örn: DomatesSosu.png gelmemesi için)
            if (Path.GetFileNameWithoutExtension(path).Equals(name, System.StringComparison.OrdinalIgnoreCase))
            {
                return AssetDatabase.LoadAssetAtPath<Sprite>(path);
            }
        }
        return null;
    }

    // Projedeki Prefab'ları tarar ve içinde IngredientDrop scripti olan bir şablon bulur
    private static GameObject FindCommonDropPrefab()
    {
        string[] guids = AssetDatabase.FindAssets("t:Prefab");
        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            if (prefab != null && prefab.GetComponent<IngredientDrop>() != null)
            {
                return prefab; // İlk bulduğu malzeme düşme şablonunu döndürür
            }
        }
        return null;
    }
}
