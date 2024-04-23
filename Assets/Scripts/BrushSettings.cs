using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Es.InkPainter.Sample;
public class BrushSettings : MonoBehaviour
{
    public static BrushSettings instance;
    public BrushSettings() { instance = this; }

    [SerializeField] private Slider sliderSize;
    [SerializeField] private Image colorPreview;
    [SerializeField] private Image previewImage;

    private Vector2Int pixelCoords;
    private float brushSize;
    private Color color;
    private Vector2 size;
    private Texture2D spriteTexture;

    private void Awake()
    {
        colorPreview.color = color;
        MousePainter.instance.SetBrushSettings(color, brushSize);
        size = previewImage.rectTransform.sizeDelta;
        spriteTexture = ((Texture2D)previewImage.mainTexture);
    }
    public void Done()
    {
        MousePainter.instance.SetBrushSettings(color, brushSize);
    }

    private void Update()
    {
        if (Input.GetMouseButton(0) && Input.mousePosition.y > Screen.height / 2 - size.y / 2f)
        {
            pixelCoords = new Vector2Int((int)((Math.Clamp(Input.mousePosition.x - Screen.width / 2, -size.x / 2f, size.y / 2f) + size.x / 2f) / size.x * spriteTexture.width), (int)((Math.Clamp(Input.mousePosition.y - Screen.height / 2 - 200, -size.x / 2f, size.y / 2f) + size.y / 2f) / size.y * spriteTexture.height));

            color = spriteTexture.GetPixel(pixelCoords.x, pixelCoords.y);
            color.a = 1;
            colorPreview.color = color;
        }
        brushSize = sliderSize.value;
        colorPreview.transform.localScale = Vector3.one * brushSize;
    }

    
}
