using System;
using UnityEngine;

public class AttackController : MonoBehaviour
{
    [SerializeField]
    int strength = 2;

    [SerializeField]
    float attackTime = .4f;

    [SerializeField]
    float cooldown = .3f;

    float lastAttack = 0;
    float lastCooldown = 0;
    GameObject parent;

    GameObject sprite;

    void Start()
    {
        parent = GetComponentInParent<PlayerController>().gameObject;
        sprite = GetComponentInChildren<SpriteRenderer>().gameObject;
        sprite.SetActive(false);
    }

    void Update()
    {
        if(sprite.activeSelf && Time.time - lastAttack > attackTime)
        {
            sprite.SetActive(false);
            lastCooldown = Time.time;
        }
    }


    public void Attack(Vector2Int direction)
    {
        if(!sprite.activeSelf && Time.time - lastCooldown > cooldown) {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            transform.rotation = Quaternion.Euler(0f, 0f, angle);
            sprite.SetActive(true);
            lastAttack = Time.time;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        Collide(collision);
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        Collide(collision);
    }

    private void Collide(Collider2D collision)
    {
        if(sprite.activeSelf && collision.gameObject != parent)
        {
            var entity = collision.GetComponent<HealthController>();
            if(entity) entity.Damage(strength, gameObject);
        }
    }


}
