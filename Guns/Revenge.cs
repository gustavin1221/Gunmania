using System;
using System.Collections;
using Gungeon;
using MonoMod;
using UnityEngine;
using Alexandria.ItemAPI;
using System.Collections.Generic;

namespace HelloPretzel
{

    class Revenge : GunBehaviour
    {


        public static void Add()
        {
            // Get yourself a new gun "base" first.
            // Let's just call it "Basic Gun", and use "jpxfrd" for all sprites and as "codename" All sprites must begin with the same word as the codename. For example, your firing sprite would be named "jpxfrd_fire_001".
            Gun gun = ETGMod.Databases.Items.NewGun("Revenge", "rvg");
            // "kp:basic_gun determines how you spawn in your gun through the console. You can change this command to whatever you want, as long as it follows the "name:itemname" template.
            Game.Items.Rename("outdated_gun_mods:revenge", "gustavin:revenge");
            gun.gameObject.AddComponent<Revenge>();
            //These two lines determines the description of your gun, ".SetShortDescription" being the description that appears when you pick up the gun and ".SetLongDescription" being the description in the Ammonomicon entry. 
            gun.SetShortDescription("Thief!");
            gun.SetLongDescription("This gun was forged by an ancient stonesmith, i wonder where he is now. Maybe Bello knows something about it.");
            // This is required, unless you want to use the sprites of the base gun.
            // That, by default, is the pea shooter.
            // SetupSprite sets up the default gun sprite for the ammonomicon and the "gun get" popup.
            // WARNING: Add a copy of your default sprite to Ammonomicon Encounter Icon Collection!
            // That means, "sprites/Ammonomicon Encounter Icon Collection/defaultsprite.png" in your mod .zip. You can see an example of this with inside the mod folder.
            gun.SetupSprite(null, "rvg_idle_001", 8);
            // ETGMod automatically checks which animations are available.
            // The numbers next to "shootAnimation" determine the animation fps. You can also tweak the animation fps of the reload animation and idle animation using this method.
            gun.SetAnimationFPS(gun.shootAnimation, 20);
            gun.SetAnimationFPS(gun.reloadAnimation, 14); // Every modded gun has base projectile it works with that is borrowed from other guns in the game.     
            // The gun names are the names from the JSON dump! While most are the same, some guns named completely different things. If you need help finding gun names, ask a modder on the Gungeon discord.
            gun.muzzleFlashEffects = VFXToolbox.CreateVFXPool("Revenge Muzzle Flash",
               new List<string>()
            {
                    "HelloPretzel/Resources/MuzzleFlashes/RevengeMuzzleFlash001",
                    "HelloPretzel/Resources/MuzzleFlashes/RevengeMuzzleFlash002",
                    "HelloPretzel/Resources/MuzzleFlashes/RevengeMuzzleFlash003",
                    "HelloPretzel/Resources/MuzzleFlashes/RevengeMuzzleFlash004",
                    "HelloPretzel/Resources/MuzzleFlashes/RevengeMuzzleFlash005",
                    "HelloPretzel/Resources/MuzzleFlashes/RevengeMuzzleFlash006",
                    "HelloPretzel/Resources/MuzzleFlashes/RevengeMuzzleFlash007",
                    "HelloPretzel/Resources/MuzzleFlashes/RevengeMuzzleFlash008",

            },
                    16, //FPS
                    new IntVector2(27, 16), //Dimensions
                    tk2dBaseSprite.Anchor.MiddleLeft, //Anchor
                    false, //Uses a Z height off the ground
                    0, //The Z height, if used
                    false,
                    VFXAlignment.Fixed,
                                   -1,
               null,
               VFXPoolType.All,
               false,
               true
            );
            for (int i = 0; i < 4; i++)
            {
                gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(601) as Gun, true, false);
            }
            foreach (ProjectileModule projectileModule in gun.Volley.projectiles)
            {
                projectileModule.ammoCost = 1;
                projectileModule.shootStyle = ProjectileModule.ShootStyle.SemiAutomatic;
                projectileModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
                projectileModule.cooldownTime = 1.1f;
                projectileModule.angleVariance = 8f;
                gun.barrelOffset.transform.localPosition = new Vector3(2.6875f, 0.6875f, 0f);
                projectileModule.numberOfShotsInClip = 2;
                Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(projectileModule.projectiles[0]);
                projectile.gameObject.SetActive(false);
                projectileModule.projectiles[0] = projectile;
                projectile.baseData.damage = 8f;
                projectile.baseData.range = 20;
                projectile.baseData.speed = 18;
                projectile.AdditionalScaleMultiplier = 1f;
                projectile.SetProjectileSpriteRight("bs_projectile_001", 12, 12, false, tk2dBaseSprite.Anchor.MiddleCenter, 16, 16);
                FakePrefab.MarkAsFakePrefab(projectile.gameObject);
                UnityEngine.Object.DontDestroyOnLoad(projectile);
                gun.DefaultModule.ammoType = GameUIAmmoType.AmmoType.SHOTGUN;
                gun.DefaultModule.projectiles[0] = projectile;
                BounceProjModifier bounce = projectile.gameObject.GetOrAddComponent<BounceProjModifier>();
                bounce.numberOfBounces = 1;
                bounce.damageMultiplierOnBounce = 1.15f;
                bounce.bouncesTrackEnemies = true;
                bounce.bounceTrackRadius = 5f;
                bounce.TrackEnemyChance = 1f;
                ExplosionData Boom = new ExplosionData()
                {
                    effect = (PickupObjectDatabase.GetById(108) as SpawnObjectPlayerItem).objectToSpawn.GetComponent<ProximityMine>().explosionData.effect,


                    damageRadius = 3.5f,
                    damageToPlayer = 0f,
                    doDamage = true,
                    damage = 5,
                    doDestroyProjectiles = false,
                    doForce = true,
                    debrisForce = 20f,
                    preventPlayerForce = true,
                    explosionDelay = 0.1f,
                    usesComprehensiveDelay = false,
                    doScreenShake = true,
                    playDefaultSFX = true,
                    force = 20,
                    breakSecretWalls = false,

                };
                ExplosiveModifier boom = projectile.gameObject.GetOrAddComponent<ExplosiveModifier>();
                boom.explosionData = Boom;
                boom.doExplosion = true;

            bool flag = projectileModule != gun.DefaultModule;
                if (flag)
                {
                    projectileModule.ammoCost = 0;
                }

            }

            gun.reloadTime = 1;
            gun.SetBaseMaxAmmo(75);
            gun.carryPixelOffset = new IntVector2(8, -2);
            ETGMod.Databases.Items.Add(gun, null, "ANY");


        }

        public override void OnPostFired(PlayerController player, Gun gun)
        {
            //This determines what sound you want to play when you fire a gun.
            //Sounds names are based on the Gungeon sound dump, which can be found at EnterTheGungeon/Etg_Data/StreamingAssets/Audio/GeneratedSoundBanks/Windows/sfx.txt
            gun.PreventNormalFireAudio = true;
            AkSoundEngine.PostEvent("Play_WPN_shotgun_shot_01", gameObject);
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
                AkSoundEngine.PostEvent("m_WPN_frostgiant_reload_02", base.gameObject);
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