using UnityEngine;

[CreateAssetMenu(fileName = "SpritePhysicsSpecs", menuName = "ScriptableObjects/SpritePhysicsSpecs")]
public class SpritePhysicsSpecs : ScriptableObject
{
    [SerializeField] public float CollisionCheckSpace = 0.1f;
    [SerializeField] public float Gravity = -5;
    [SerializeField] public float HorizontalDrag = 0.2f;
    [SerializeField] public Vector2 MaxVelocity = new Vector2(3, 3);
}