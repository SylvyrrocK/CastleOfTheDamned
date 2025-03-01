using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemItem : SimpleItem
{
        [SerializeField] float crashSpeed;
        [SerializeField] float crashForce;
        [SerializeField] GameObject[] shards;
        bool isDestroyed = false;

        void OnCollisionEnter2D(Collision2D collision)
        {
                if (collision.relativeVelocity.magnitude > crashSpeed)
                {
                        if (!isDestroyed)
                        {
                                isDestroyed = true;
                                DestroyGem();
                        }
                }
        }

        public virtual void DestroyGem()
        {
                foreach (var shard in shards)
                {
                        var obj = Instantiate(shard, transform.position, Quaternion.identity);
                        var rb = obj.GetComponent<Rigidbody2D>();

                        float randomAngle = Random.Range(0f, 360f);
                        Vector2 randomMovement = new Vector2(Mathf.Sin(randomAngle * Mathf.Deg2Rad), Mathf.Cos(randomAngle * Mathf.Deg2Rad));
                        rb.AddForce(randomMovement * crashForce);
                }
                Destroy(gameObject);
        }
}
