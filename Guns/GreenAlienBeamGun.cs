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

namespace GunMania
{
    public class Gal : AdvancedGunBehaviour
    {
        public static void Add()
        {

            Gun gun = ETGMod.Databases.Items.NewGun("Chernobyl", "gal");
            Game.Items.Rename("outdated_gun_mods:chernobyl", "gunmania:chernobyl");
            var behav = gun.gameObject.AddComponent<Gal>();
            
            behav.preventNormalFireAudio = true;
            gun.SetShortDescription("idk");
            gun.SetLongDescription("idk");

            gun.SetupSprite(null, "gal_idle_001", 4);

            gun.SetAnimationFPS(gun.shootAnimation, 8);
            gun.SetAnimationFPS(gun.reloadAnimation, 8);
            gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(20) as Gun, true, false);
            gun.GetComponent<tk2dSpriteAnimator>().GetClipByName(gun.shootAnimation).wrapMode = tk2dSpriteAnimationClip.WrapMode.LoopSection;
            gun.GetComponent<tk2dSpriteAnimator>().GetClipByName(gun.shootAnimation).loopStart = 1;

            //GUN STATS
            gun.doesScreenShake = true;
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.angleVariance = 3;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.Beam;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
                gun.muzzleFlashEffects = VFXToolbox.CreateVFXPool("Green Beam",
            new List<string>()
         {
                    "GunMania/Resources/MuzzleFlashes/GreenBeamMuzzleFlash",

         },
                 1, //FPS
                 new IntVector2(27, 16), //Dimensions
                 tk2dBaseSprite.Anchor.MiddleLeft, //Anchor
                 false, //Uses a Z height off the ground
                 0, //The Z height, if used
                 false,
                 VFXAlignment.Fixed,
                                -1,
            null,
            VFXPoolType.All,
            false
         );
            gun.reloadTime = 1f;
            gun.DefaultModule.cooldownTime = 0.01f;
            gun.DefaultModule.numberOfShotsInClip = 40;
            gun.DefaultModule.ammoType = GameUIAmmoType.AmmoType.CUSTOM;
            gun.DefaultModule.customAmmoType = "green_beam";
            gun.barrelOffset.transform.localPosition = new Vector3(1.625f, 0.6875f, 0f);
            gun.SetBaseMaxAmmo(200);


            List<string> BeamAnimPaths = new List<string>()
            {
                "GunMania/Resources/Beams/gal_beam_002",
                

            };
           

            //BULLET STATS
            Projectile projectile = UnityEngine.Object.Instantiate<Projectile>((PickupObjectDatabase.GetById(20) as Gun).DefaultModule.projectiles[0]);

            BasicBeamController beamComp = projectile.GenerateBeamPrefab(
                "GunMania/Resources/Beams/gal_beam_001",
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
            projectile.SetProjectileSpriteRight("gal_beam_002", 7, 9, false, tk2dBaseSprite.Anchor.MiddleCenter, 16, 16);
            FakePrefab.MarkAsFakePrefab(projectile.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(projectile);
            projectile.baseData.damage = 33f;
            projectile.baseData.force *= 1f;
            projectile.baseData.range = 999f;
            projectile.baseData.speed = 50f;
            gun.gunClass = GunClass.BEAM;
            beamComp.penetration = 100;
            beamComp.boneType = BasicBeamController.BeamBoneType.Straight;
            beamComp.DamageModifier = 1f;
            beamComp.interpolateStretchedBones = true;
            gun.DefaultModule.projectiles[0] = projectile;
            gun.quality = PickupObject.ItemQuality.B;

            ETGMod.Databases.Items.Add(gun, null, "ANY");

        }
        public override void OnPostFired(PlayerController player, Gun gun)
        {
            //This determines what sound you want to play when you fire a gun.
            //Sounds names are based on the Gungeon sound dump, which can be found at EnterTheGungeon/Etg_Data/StreamingAssets/Audio/GeneratedSoundBanks/Windows/sfx.txt
            gun.PreventNormalFireAudio = true;
            AkSoundEngine.PostEvent("", gameObject);
        }

        private bool HasReloaded;
        //This block of code allows us to change the reload sounds.
        public override void Update()
        {
            if (gun.CurrentOwner)
            {

                if (!gun.PreventNormalFireAudio)
                {
                    this.gun.PreventNormalFireAudio = true;
                }
                if (!gun.IsReloading && !HasReloaded)
                {
                    this.HasReloaded = true;
                }
            }
        }

        public override void OnReloadPressed(PlayerController player, Gun gun, bool bSOMETHING)
        {
            if (gun.IsReloading && this.HasReloaded)
            {
                HasReloaded = false;
                AkSoundEngine.PostEvent("Stop_WPN_All", base.gameObject);
                base.OnReloadPressed(player, gun, bSOMETHING);
                AkSoundEngine.PostEvent("Play_WPN_trashgun_reload_01", base.gameObject);
            }
        }
        //Now add the Tools class to your project.
        //All that's left now is sprite stuff. 
        //Your sprites should be organized, like how you see in the mod folder. 
        //Every gun requires that you have a .json to match the sprites or else the gun won't spawn at all
        //.Json determines the hand sprites for your character. You can make a gun two handed by having both "SecondaryHand" and "PrimaryHand" in the .json file, which can be edited through Notepad or Visual Studios
        //By default this gun is a one-handed weapon
        //If you need a basic two handed .json. Just use the jpxfrd2.json.
        //And finally, don't forget to add your Gun to your ETGModule class!
    }
}


