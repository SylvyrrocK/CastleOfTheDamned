using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunBarrelAmmoFeeder : MonoBehaviour
{
        [SerializeField] ShotgunItem2D shotgunItem2D;
        [SerializeField] Collider2D ammoFeedCollider;
        void OnCollisionEnter2D(Collision2D collision)
        {
                if (shotgunItem2D.shotgunItemData.ammo >= shotgunItem2D.shotgunItemData.maxAmmo)
                {
                        return;
                }
                if (collision.otherCollider == ammoFeedCollider)
                {
                        if (collision.collider.gameObject.GetComponent<AmmoItem>())
                        {
                                shotgunItem2D.shotgunItemData.ammo += 1;
                                Destroy(collision.collider.gameObject);
                        }
                }
        }
}
