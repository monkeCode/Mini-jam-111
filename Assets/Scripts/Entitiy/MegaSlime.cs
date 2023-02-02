using UnityEngine;

public class MegaSlime : Slime
{
    public override void TakeDamage(uint damage, IDamageable source = null)
    {
        base.TakeDamage(damage, source);
        var directions = new[] {Vector2.up, Vector2.down, Vector2.left, Vector2.right};
        Vector2 direction;
        do
        { 
            direction = directions[Random.Range(0, 3)];
            
        } while (!GameManager.Instance.ActiveRoom.CanMove((Vector2) transform.position + direction, out var tile));

        hitPoints /= 2;
       var newSlime = Instantiate(this, transform.position + (Vector3) direction, Quaternion.identity);
       newSlime.transform.localScale = transform.localScale / 1.2f;
       transform.localScale /= 1.2f;
       GameManager.Instance.ActiveRoom.AddEntity(newSlime);
    }
}
