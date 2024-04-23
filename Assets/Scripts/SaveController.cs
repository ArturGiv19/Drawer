using Es.InkPainter;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

public class SaveController : MonoBehaviour
{
    public static SaveController instance;
    public SaveController() { instance = this; }

    private string folderPath;
    private RenderTexture renderTexture;
    private int numberOfTexture = 0;
    private string[] files;

    private void Start()
    {
        folderPath = Application.persistentDataPath + "/Assets/Save/";
        Load();
    }

    private Texture2D RenderTextureTo2DTexture(RenderTexture rt)
    {
        var texture = new Texture2D(rt.width, rt.height, rt.graphicsFormat, 0, TextureCreationFlags.None);
        RenderTexture.active = rt;
        texture.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
        texture.Apply();

        RenderTexture.active = null;

        return texture;
    }
    public void Save()
    {
        Save(RenderTextureTo2DTexture((RenderTexture)painter.GetComponent<Renderer>().sharedMaterial.mainTexture));
    }
    public void Save(Texture2D texture)
    {
        if (texture != null)
        {
            byte[] bytes = texture.EncodeToPNG();

            files = Directory.GetFiles(folderPath);
            numberOfTexture = 0;
            if (files.Length != 0)
                numberOfTexture = int.Parse(Path.GetFileName(files[files.Length - 1]).Split('.')[0]) + 1;
            

            string filePath = folderPath + numberOfTexture + ".png";
            PreviewController.instance.AddPreview(texture, filePath);

            File.WriteAllBytes(filePath, bytes);

            Debug.Log("Изображение сохранено по пути: " + filePath);
        }
        else
        {
            Debug.LogError("Текстура для сохранения не установлена!");
        }
    }
    [SerializeField] private Material material;
    [SerializeField] private Texture _texture;
    [SerializeField] private Texture2D _texture2d;

    public void Delete(Preview _forDelete)
    {
        File.Delete(_forDelete.fileName);
    }
    [SerializeField] private InkCanvas painter;
    [SerializeField] Texture[] loadedTextures;

    public void ClearImage()
    {
        _texture = new Texture2D(1024, 1024);
        Color[] pixels = new Color[1024 * 1024];
        for (int i = 0; i < pixels.Length; i++)
        {
            pixels[i] = Color.white;
        }

        ((Texture2D)_texture).SetPixels(pixels);
        ((Texture2D)_texture).Apply();

        painter.GetComponent<Renderer>().material.SetTexture("_MainTex", _texture);
        painter.enabled = true;
        painter.Init();
    }
    public void Load()
    {
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        files = Directory.GetFiles(folderPath);

        ClearImage();
        if (files.Length == 0)
            return;

        loadedTextures = new Texture[files.Length];
        numberOfTexture = int.Parse(Path.GetFileName(files[files.Length - 1]).Split('.')[0]) + 1;
        for (int i = 0; i < files.Length; i++)
        {
            loadedTextures[i] = LoadTexture(Path.GetFileName(files[i]));
        }

        PreviewController.instance.LoadPreviews(loadedTextures, files);


    }
    private Texture2D LoadTexture(string _name)
    {
        string texturePath = Path.Combine(folderPath, _name);

        byte[] fileData = File.ReadAllBytes(texturePath);

        Texture2D texture = new Texture2D(2, 2);
        texture.LoadImage(fileData);
        texture.name = _name.Split('.')[0];

        return texture;

    }

}
