using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShotgunItem3D : Item3D
{
        [SerializeField] ShotgunItemData shotgunItemData;
        public override ItemData Data
        {
                get { return shotgunItemData; }
                set
                {
                        shotgunItemData = value is ShotgunItemData ? (ShotgunItemData)value : throw new System.Exception("Trying to set ItemData of a wrong type");
                }
        }

        [SerializeField] int bulletsPerShot;
        [SerializeField] int damagePerBullet;
        [SerializeField] float spreadAngle;
        [SerializeField] float shootRange;
        [SerializeField] float recoilVertical;
        [SerializeField] float recoilHorizontal;
        [SerializeField] Material loadedBarrelMaterial;
        [SerializeField] Material unloadedBarrelMaterial;
        [SerializeField] MeshRenderer barrelRenderer;
        [SerializeField] LayerMask hitLayers;

        public override void OnUse(Player player)
        {
                if (shotgunItemData.ammo > 0)
                {
                        shotgunItemData.ammo -= 1;
                        Shoot();
                }
        }

        public override void OnUseCancel(Player player)
        {

        }

        public void Update()
        {
                if (shotgunItemData.ammo > 0)
                {
                        barrelRenderer.material = loadedBarrelMaterial;
                }
                else
                {
                        barrelRenderer.material = unloadedBarrelMaterial;
                }
        }
        public void Shoot()
        {
                GameManager.Instance.player.playerController.ForceRotateCamera(new Vector2(Random.Range(-recoilHorizontal, recoilHorizontal), recoilVertical));
                for (int i = 0; i < bulletsPerShot; i++)
                {
                        Quaternion spread = Quaternion.Euler(Random.Range(-spreadAngle, spreadAngle), Random.Range(-spreadAngle, spreadAngle), Random.Range(-spreadAngle, spreadAngle));
                        bool hit = Physics.Raycast(transform.position, spread * -transform.right, out RaycastHit rayResult, shootRange, hitLayers);
                        if (hit)
                        {
                                if (rayResult.collider != null)
                                {
                                        Health health = rayResult.collider.gameObject.GetComponent<Health>();
                                        if (health != null)
                                        {
                                                health.Damage(damagePerBullet);
                                        }
                                }
                        }
                }
        }
}
