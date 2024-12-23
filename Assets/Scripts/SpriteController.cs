using UnityEngine;
using UnityEngine.Serialization;

public abstract class SpriteController : MonoBehaviour
{
    [SerializeField] protected SpritePhysicsSpecs physicsSpecs;
    [SerializeField] private Vector2 velocity;
    private BoxCollider2D _boxCollider;

    private void Start()
    {
        _boxCollider = GetComponent<BoxCollider2D>();
    }

    protected bool TopTouchingGround()
    {
        float height = _boxCollider.size.y * transform.lossyScale.y;
        float width = _boxCollider.size.x * transform.lossyScale.x;
        float top = _boxCollider.offset.y * transform.lossyScale.y + height / 2 + transform.position.y;
        float left = _boxCollider.offset.x * transform.lossyScale.x - width / 2 + transform.position.x;

        for (float x = left; x < left + width; x += physicsSpecs.CollisionCheckSpace)
        {
            if (Ground.PointOnGround(x, top))
            {
                return true;
            }
        }

        return Ground.PointOnGround(left + width, top);
    }

    protected bool BottomTouchingGround()
    {
        float height = _boxCollider.size.y * transform.lossyScale.y;
        float width = _boxCollider.size.x * transform.lossyScale.x;
        float top = _boxCollider.offset.y * transform.lossyScale.y + height / 2 + transform.position.y;
        float left = _boxCollider.offset.x * transform.lossyScale.x - width / 2 + transform.position.x;

        for (float x = left; x < left + width; x += physicsSpecs.CollisionCheckSpace)
        {
            if (Ground.PointOnGround(x, top - height))
            {
                return true;
            }
        }

        return Ground.PointOnGround(left + width, top - height);
    }

    protected bool LeftTouchingGround()
    {
        float height = _boxCollider.size.y * transform.lossyScale.y;
        float width = _boxCollider.size.x * transform.lossyScale.x;
        float top = _boxCollider.offset.y * transform.lossyScale.y + height / 2 + transform.position.y;
        float left = _boxCollider.offset.x * transform.lossyScale.x - width / 2 + transform.position.x;

        for (float y = top; y > top - height; y -= physicsSpecs.CollisionCheckSpace)
        {
            if (Ground.PointOnGround(left, y))
            {
                return true;
            }
        }

        return Ground.PointOnGround(left, top - height);
    }

    protected bool RightTouchingGround()
    {
        float height = _boxCollider.size.y * transform.lossyScale.y;
        float width = _boxCollider.size.x * transform.lossyScale.x;
        float top = _boxCollider.offset.y * transform.lossyScale.y + height / 2 + transform.position.y;
        float left = _boxCollider.offset.x * transform.lossyScale.x - width / 2 + transform.position.x;

        for (float y = top; y > top - height; y -= physicsSpecs.CollisionCheckSpace)
        {
            if (Ground.PointOnGround(left + width, y))
            {
                return true;
            }
        }

        return Ground.PointOnGround(left + width, top - height);
    }

    protected void SetXVelocity(float vel)
    {
        velocity.x = vel;
    }

    protected void ChangeXVelocity(float vel)
    {
        velocity.x += vel;
    }

    protected void SetYVelocity(float vel)
    {
        velocity.y = vel;
    }

    protected void ChangeYVelocity(float vel)
    {
        velocity.y += vel;
    }

    protected CollisionInfo ApplyPhysics()
    {
        velocity.x = Mathf.Lerp(velocity.x, 0,
            physicsSpecs.HorizontalDrag * Time.deltaTime);
        velocity.y += physicsSpecs.Gravity * Time.deltaTime;
        velocity.x = Mathf.Clamp(velocity.x, -physicsSpecs.MaxVelocity.x, physicsSpecs.MaxVelocity.x);
        velocity.y = Mathf.Clamp(velocity.y, -physicsSpecs.MaxVelocity.y, physicsSpecs.MaxVelocity.y);

        Vector2 timeScaledVelocity = velocity * Time.deltaTime;

        bool topCollision = false;
        bool bottomCollision = false;
        bool leftCollision = false;
        bool rightCollision = false;

        transform.Translate(0, timeScaledVelocity.y, 0);
        if (velocity.y != 0 && (velocity.y > 0 ? TopTouchingGround() : BottomTouchingGround()))
        {
            transform.Translate(0, -timeScaledVelocity.y, 0);
            velocity.y = 0;

            if (timeScaledVelocity.y < 0) bottomCollision = true;
            else topCollision = true;
        }

        transform.Translate(timeScaledVelocity.x, 0, 0);
        if (velocity.x != 0 && (velocity.x > 0 ? RightTouchingGround() : LeftTouchingGround()))
        {
            transform.Translate(-timeScaledVelocity.x, 0, 0);
            velocity.x = 0;

            if (timeScaledVelocity.x < 0) leftCollision = true;
            else rightCollision = true;
        }

        return new CollisionInfo(topCollision, bottomCollision, leftCollision, rightCollision);
    }

    protected struct CollisionInfo
    {
        public bool Top { get; private set; }
        public bool Bottom { get; private set; }
        public bool Left { get; private set; }
        public bool Right { get; private set; }

        public bool X => Left || Right;
        public bool Y => Top || Bottom;

        public CollisionInfo(bool top, bool bottom, bool left, bool right)
        {
            Top = top;
            Bottom = bottom;
            Left = left;
            Right = right;
        }

        public override string ToString()
        {
            return $"Top: {Top}, Bottom: {Bottom}, Left: {Left}, Right: {Right}";
        }
    }
}