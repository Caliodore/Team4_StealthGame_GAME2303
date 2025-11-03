using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Unity.Netcode;

public class Projectile : MonoBehaviour
{
    float damageAmount;
    float speed;
    float knockback;
    float lifetime;
    UnityAction<HitData> OnHit;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Initialize(float damage, float velocity, float life, float force, UnityAction<HitData> onHit)
    {
        damageAmount = damage;
        speed = velocity;
        lifetime = life;
        knockback = force;
        OnHit += onHit;

        GetComponent<Rigidbody>().linearVelocity = transform.forward * speed;//change this so the bullet movement would be better/real
        Destroy(gameObject, lifetime);
        
    }

    private void OnTriggerEnter(Collider other)
    {
        var target = other.gameObject.GetComponent<Guard_StateMachine>();
        if (target != null)
        {
            var direction = GetComponent<Rigidbody>().linearVelocity;
            direction.Normalize();

            Debug.Log("hit enemy!");
            target.TakeDamage(damageAmount);

            HitData hd = new HitData();
            hd.target = target;
            hd.direction = direction;
            hd.location = transform.position;

            OnHit?.Invoke(hd);
            
        }

        Destroy(gameObject);
    }
}

public class HitData
{
    public Vector3 location;
    public Vector3 direction;
    public Guard_StateMachine target; // NEED TO CHANGE THIS EVENTUALLY
}
