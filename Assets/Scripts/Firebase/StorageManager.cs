using System.IO;
using UnityEngine;

public class StorageManager
{
    private readonly string cacheDirectory;

    public StorageManager()
    {
        cacheDirectory = Path.Combine(Application.persistentDataPath, "ImageCache");
        if (!Directory.Exists(cacheDirectory))
        {
            Directory.CreateDirectory(cacheDirectory);
        }
    }

    public void SaveImage(string url, byte[] imageData, float percentage)
    {
        string filename = SanitizeFileName(url);
        string imagePath = Path.Combine(cacheDirectory, filename);
        File.WriteAllBytes(imagePath, imageData);

        // Save the percentage in a separate JSON file
        string jsonPath = Path.Combine(cacheDirectory, filename + ".json");
        var data = new CachedImageData { Percentage = percentage };
        File.WriteAllText(jsonPath, JsonUtility.ToJson(data));
    }

    public Texture2D LoadImage(string url, out float percentage)
    {
        string filename = SanitizeFileName(url);
        string imagePath = Path.Combine(cacheDirectory, filename);
        string jsonPath = Path.Combine(cacheDirectory, filename + ".json");

        Texture2D texture = null;
        percentage = 0f;

        if (File.Exists(imagePath))
        {
            byte[] imageData = File.ReadAllBytes(imagePath);
            texture = new Texture2D(2, 2);
            texture.LoadImage(imageData);

            if (File.Exists(jsonPath))
            {
                string jsonData = File.ReadAllText(jsonPath);
                var data = JsonUtility.FromJson<CachedImageData>(jsonData);
                percentage = data.Percentage;
            }
        }

        return texture;
    }

    public bool ImageExists(string url)
    {
        string filename = SanitizeFileName(url);
        string imagePath = Path.Combine(cacheDirectory, filename);
        return File.Exists(imagePath);
    }

    private string SanitizeFileName(string url)
    {
        // Remove the query string from the URL (everything after and including '?')
        string filename = Path.GetFileName(url.Split('?')[0]);

        // Replace invalid characters with an underscore or another safe character
        foreach (char c in Path.GetInvalidFileNameChars())
        {
            filename = filename.Replace(c, '_');
        }

        return filename;
    }

    [System.Serializable]
    private class CachedImageData
    {
        public float Percentage;
    }
}
