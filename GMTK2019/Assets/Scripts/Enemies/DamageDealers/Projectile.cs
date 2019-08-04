using UnityEngine;

public class Projectile : DamageDealer
{


    private Vector2 speed;

    public float multiplier;
    public void Update()
    {
        transform.Translate(speed.normalized * multiplier * Time.deltaTime,Space.World);
    }

    public virtual void  Init(Vector2 initialSpeed)
    {
        Destroy(gameObject,15);
        speed = initialSpeed;
        var dir = new Vector2(Mathf.RoundToInt(speed.x), Mathf.RoundToInt(speed.y));
        float z = 0;
        if (dir.x == 1)
        {
            if (dir.y == 1)
            {
                z = 45;
            }
            else if (dir.y == -1)
            {
                z = -45;
            }
            else
            {
                z = 0;
            }

        }
        else if (dir.y == 1)
        {
            if (dir.x == 1)
            {
               z = 45;
            }
            else if (dir.x == -1)
            {
                z = 115;
            }
            else
            {
                z = 90;
            }
        }
        else if (dir.x == -1)
        {
            if (dir.y == 1)
            {
                z = 115;
            }
            else if (dir.y == -1)
            {
                z = -115;
            }
            else
            {
                z = 180;
            }
        }
        else if (dir.y == -1)
        {
            if (dir.x == 1)
            {
                z = -45;
            }
            else if (dir.x == -1)
            {
                z = -115;
            }
            else
            {
                z = 270;
            }
        }
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, z));
    }

}