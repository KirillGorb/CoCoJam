using UnityEngine;

public class ObjectTransfer
{
    private readonly Transform _transfer;
    private readonly float _speed;

    private Rigidbody2D _target;

    public void PuckUp(Rigidbody2D target)
    {
        _target = target;
        _target.angularVelocity = 0;
        _target.bodyType = RigidbodyType2D.Kinematic;
    }

    public void PuckDown()
    {
        _target.bodyType = RigidbodyType2D.Dynamic;
        _target = null;
    }

    public void Mover()
    {
        _target.MovePosition(Vector2.MoveTowards(_target.position, _transfer.position, Time.fixedDeltaTime * _speed));
    }
}