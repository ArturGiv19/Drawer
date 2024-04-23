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
    [SerializeField] private Image colorPicker;

    private Vector2Int pixelCoords;
    private float brushSize;
    private Color color;
    private Vector2 size;
    private Texture2D spriteTexture;

    private void Awake()
    {
        colorPreview.color = color;
        MousePainter.instance.SetBrushSettings(color, brushSize);
        size = colorPicker.rectTransform.sizeDelta;
        spriteTexture = ((Texture2D)colorPicker.mainTexture);
    }
    public void Done()
    {
        MousePainter.instance.SetBrushSettings(color, brushSize);
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {


            if (RectTransformUtility.RectangleContainsScreenPoint(colorPicker.rectTransform, Input.mousePosition))
            {
                Vector2 localPosition;
                if (RectTransformUtility.ScreenPointToLocalPointInRectangle(colorPicker.rectTransform, Input.mousePosition, null, out localPosition))
                {
                    Vector2 spriteSize = colorPicker.sprite.rect.size;

                    Vector2 normalizedPosition = new Vector2(localPosition.x / spriteSize.x, localPosition.y / spriteSize.y);

                    Texture2D texture = colorPicker.sprite.texture;

                    int x = Mathf.FloorToInt(normalizedPosition.x * texture.width) + (int)size.x / 2;
                    int y = Mathf.FloorToInt(normalizedPosition.y * texture.height) + (int)size.y / 2;

                    color = texture.GetPixel(x * texture.width / (int)size.x, y * texture.height / (int)size.y);
                    color.a = 1;
                    colorPreview.color = color;
                }
            }
        }


        brushSize = sliderSize.value;
        colorPreview.transform.localScale = Vector3.one * brushSize;
    }
}