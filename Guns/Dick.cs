using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Gungeon;
using Dungeonator;
using MonoMod;
using UnityEngine;
using Alexandria.ItemAPI;

namespace HelloPretzel
{
    public class Dick : AdvancedGunBehaviour
    {
        public static void Add()
        {

            Gun gun = ETGMod.Databases.Items.NewGun("MY HUGE PENIS", "dick");
            Game.Items.Rename("outdated_gun_mods:my_huge_penis", "gustavin:dick");
            var behav = gun.gameObject.AddComponent<Dick>();
            
            behav.preventNormalFireAudio = true;
            gun.SetShortDescription("mine");
            gun.SetLongDescription("OOOOOOOOOOH IM BOUT TO BUST");

            gun.SetupSprite(null, "dick_idle_001", 4);

            gun.SetAnimationFPS(gun.shootAnimation, 8);
            gun.SetAnimationFPS(gun.reloadAnimation, 8);
            gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(86) as Gun, true, false);
            gun.GetComponent<tk2dSpriteAnimator>().GetClipByName(gun.shootAnimation).wrapMode = tk2dSpriteAnimationClip.WrapMode.LoopSection;
            gun.GetComponent<tk2dSpriteAnimator>().GetClipByName(gun.shootAnimation).loopStart = 1;

            //GUN STATS
            gun.doesScreenShake = false;
            gun.DefaultModule.ammoCost = 2;
            gun.DefaultModule.angleVariance = 0;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.Beam;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;

            gun.reloadTime = 1f;
            gun.muzzleFlashEffects.type = VFXPoolType.None;
            gun.DefaultModule.cooldownTime = 0.01f;
            gun.DefaultModule.numberOfShotsInClip = 10;
            gun.DefaultModule.ammoType = GameUIAmmoType.AmmoType.CUSTOM;
            
            gun.DefaultModule.customAmmoType = "white";
            gun.barrelOffset.transform.localPosition = new Vector3(1.6875f, 0.5625f, 0f);
            gun.SetBaseMaxAmmo(200);


            List<string> BeamAnimPaths = new List<string>()
            {
                "HelloPretzel/Resources/cum",
                

            };
           

            //BULLET STATS
            Projectile projectile = UnityEngine.Object.Instantiate<Projectile>((PickupObjectDatabase.GetById(86) as Gun).DefaultModule.projectiles[0]);

            BasicBeamController beamComp = projectile.GenerateBeamPrefab(
                "HelloPretzel/Resources/cum",
                new Vector2(10, 8),
                new Vector2(0, 1),
                BeamAnimPaths,
                1,
                //Impact
                null,
                -1,
                null,
                null,
                //End
                null,
                -1,
                null,
                null,
                //Beginning
                null,
                -1,
                null,
                null
                );

            projectile.gameObject.SetActive(false);
            projectile.SetProjectileSpriteRight("cum", 100, 55, false, tk2dBaseSprite.Anchor.MiddleCenter, 16, 16);
            FakePrefab.MarkAsFakePrefab(projectile.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(projectile);
            projectile.baseData.damage = 100f;
            projectile.baseData.force *= 10f;
            projectile.baseData.range = 999f;
            projectile.baseData.speed = 25f;
            gun.gunClass = GunClass.BEAM;
            beamComp.penetration = 100;
            beamComp.boneType = BasicBeamController.BeamBoneType.Projectile;
            beamComp.DamageModifier = 1f;
            beamComp.interpolateStretchedBones = false;
            gun.DefaultModule.projectiles[0] = projectile;

            gun.quality = PickupObject.ItemQuality.B;

           

            ETGMod.Databases.Items.Add(gun, null, "ANY");
            Dick.mbID = gun.PickupObjectId;

        }
        public static int mbID;
        public System.Random rng = new System.Random();

        public override void OnPostFired(PlayerController player, Gun gun)
        {
            PlayerController owner = this.gun.CurrentOwner as PlayerController;
            bool nanner = owner.PlayerHasActiveSynergy("Bananarmaments");

            if (nanner)
            {
                int doit;
                if(player.PlayerHasActiveSynergy("Apex Predator"))
                {
                    doit = rng.Next(1, 20);
                }
                else
                {
                    doit = rng.Next(1, 700);
                }
                
                if(doit == 1)
                {
                    Projectile projectile2 = ((Gun)ETGMod.Databases.Items[478]).DefaultModule.chargeProjectiles[0].Projectile;

                    GameObject gameObject = SpawnManager.SpawnProjectile(projectile2.gameObject, owner.sprite.WorldCenter, Quaternion.Euler(0f, 0f, (owner.CurrentGun == null) ? 0f : owner.CurrentGun.CurrentAngle), true);
                    Projectile component = gameObject.GetComponent<Projectile>();
                    bool flag = component != null;
                    if (flag)
                    {
                        component.Owner = owner;
                        component.Shooter = owner.specRigidbody;
                    }


                }

            }

            if (player.PlayerHasActiveSynergy("Apex Predator"))
            {
                int oo = rng.Next(1, 4);
                switch (oo)
                {
                    case 1:
                        AkSoundEngine.PostEvent("Play_Monkey_shoot_001", base.gameObject);

                        break;
                    case 2:
                        AkSoundEngine.PostEvent("Play_Monkey_shoot_002", base.gameObject);

                        break;
                    case 3:
                        AkSoundEngine.PostEvent("Play_Monkey_shoot_003", base.gameObject);

                        break;
                    default: // case 4
                        AkSoundEngine.PostEvent("Play_Monkey_shoot_004", base.gameObject);

                        break;
                }

            }

            if (owner.PlayerHasActiveSynergy("Infinite Monkey Theorem"))
            {
                this.gun.GainAmmo(2);
            }
            

            base.OnPostFired(player, gun);
        }

      

        private bool HasReloaded = true;
        public bool limiter = false;
       
        public override void  Update()
        {
            if (this.gun.CurrentOwner != null)
            {

                PlayerController owner = this.gun.CurrentOwner as PlayerController;
                if (!gun.PreventNormalFireAudio)
                {
                    this.gun.PreventNormalFireAudio = true;
                }
                if (!gun.IsReloading && !HasReloaded)
                {
                    this.HasReloaded = true;
                }
                if (gun.IsFiring && limiter == false && this.HasReloaded && Time.timeScale > 0f && !owner.PlayerHasActiveSynergy("Apex Predator"))
                {

                    AkSoundEngine.PostEvent("Play_monkey_barrel_laser_fire",base.gameObject);
                    limiter = true;

                }
                if (!this.gun.IsFiring && limiter)
                {

                    AkSoundEngine.PostEvent("Stop_monkey_barrel_laser_fire", base.gameObject);

                    limiter = false;
                }

                if (this.gun.IsFiring && owner.IsDodgeRolling && limiter)
                {
                    AkSoundEngine.PostEvent("Stop_monkey_barrel_laser_fire", base.gameObject);


                    limiter = false;
                }

                
            }
        }


        public override void OnReloadPressed(PlayerController player, Gun gun, bool bSOMETHING)
        {
            if (gun.IsReloading && this.HasReloaded)
            {
                this.gun.Update();
                AkSoundEngine.PostEvent("Stop_monkey_barrel_laser_fire", base.gameObject);
                if(player.PlayerHasActiveSynergy("Apex Predator"))
                {
                    AkSoundEngine.PostEvent("Play_WPN_m1911_reload_01", base.gameObject);
                }
                else
                {
                    AkSoundEngine.PostEvent("Play_WPN_mailbox_reload_01", base.gameObject);
                }

                HasReloaded = false;

                base.OnReloadPressed(player, gun, bSOMETHING);


            }

        }

        public Dick()
        {

        }

       
    }
}


