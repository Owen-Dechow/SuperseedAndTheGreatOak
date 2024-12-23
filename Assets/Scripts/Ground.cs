using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour
{
    private static Ground _i;
    private SpriteRenderer _sr;

    private void Awake()
    {
        _i = this;
        _sr = GetComponent<SpriteRenderer>();
    }

    public static Vector2Int PositionToPixel(Vector2 position)
    {
        Vector2 normalPosition = position / (_i._sr.sprite.bounds.size * _i.transform.lossyScale.x);
        normalPosition.x += .5f;
        normalPosition.y += .5f;

        Vector2 worldPixelSize = _i._sr.sprite.textureRect.size;
        return Vector2Int.RoundToInt(normalPosition * worldPixelSize);
    }

    public static Vector2 PixelToPosition(Vector2 pixel)
    {
        Vector2 normalPosition = pixel / _i._sr.sprite.textureRect.size;
        normalPosition.x -= .5f;
        normalPosition.y -= .5f;

        return normalPosition * _i._sr.sprite.bounds.size * _i.transform.lossyScale.x;
    }

    public static bool PointOnGround(Vector2 point)
    {
        Vector2Int pixel = PositionToPixel(point);
        return _i._sr.sprite.texture.GetPixel((int)pixel.x, (int)pixel.y).a > 0;
    }

    public static bool PointOnGround(float x, float y) => PointOnGround(new Vector2(x, y));

}