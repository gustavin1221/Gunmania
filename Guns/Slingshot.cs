using System;
using System.Collections;
using Gungeon;
using MonoMod;
using UnityEngine;
using Alexandria.ItemAPI;
using System.Collections.Generic;

namespace HelloPretzel
{

    class Slingshot : GunBehaviour
    {


        public static void Add()
        {
            // Get yourself a new gun "base" first.
            // Let's just call it "Basic Gun", and use "jpxfrd" for all sprites and as "codename" All sprites must begin with the same word as the codename. For example, your firing sprite would be named "jpxfrd_fire_001".
            Gun gun = ETGMod.Databases.Items.NewGun("Slingshot", "abgun");
            // "kp:basic_gun determines how you spawn in your gun through the console. You can change this command to whatever you want, as long as it follows the "name:itemname" template.
            Game.Items.Rename("outdated_gun_mods:slingshot", "gustavin:slingshot");
            gun.gameObject.AddComponent<Slingshot>();
            //These two lines determines the description of your gun, ".SetShortDescription" being the description that appears when you pick up the gun and ".SetLongDescription" being the description in the Ammonomicon entry. 
            gun.SetShortDescription("Those birds are angry!");
            gun.SetLongDescription("Back then gungeoneers fired Gigi eggs with this thing. Turns out, using a handy subspecies of this gross bird is a way better idea.");
            // This is required, unless you want to use the sprites of the base gun.
            // That, by default, is the pea shooter.
            // SetupSprite sets up the default gun sprite for the ammonomicon and the "gun get" popup.
            // WARNING: Add a copy of your default sprite to Ammonomicon Encounter Icon Collection!
            // That means, "sprites/Ammonomicon Encounter Icon Collection/defaultsprite.png" in your mod .zip. You can see an example of this with inside the mod folder.
            gun.SetupSprite(null, "abgun_idle_001", 8);
            // ETGMod automatically checks which animations are available.
            // The numbers next to "shootAnimation" determine the animation fps. You can also tweak the animation fps of the reload animation and idle animation using this method.
            gun.SetAnimationFPS(gun.shootAnimation, 24);
            gun.SetAnimationFPS(gun.reloadAnimation, 10);
            // Every modded gun has base projectile it works with that is borrowed from other guns in the game. 
            // The gun names are the names from the JSON dump! While most are the same, some guns named completely different things. If you need help finding gun names, ask a modder on the Gungeon discord.
            gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(15) as Gun, true, false);
            // Here we just take the default projectile module and change its settings how we want it to be.
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.Charged;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = 1;
            gun.DefaultModule.cooldownTime = 0.5f;
            gun.DefaultModule.numberOfShotsInClip = 3;
            gun.SetBaseMaxAmmo(75);
            gun.gunHandedness = GunHandedness.TwoHanded;
            gun.barrelOffset.transform.localPosition = new Vector3(1.375f, 1.5625f, 0f);
            gun.carryPixelOffset += new IntVector2(5, 0);
            gun.GetComponent<tk2dSpriteAnimator>().GetClipByName(gun.chargeAnimation).wrapMode = tk2dSpriteAnimationClip.WrapMode.LoopSection;
            gun.GetComponent<tk2dSpriteAnimator>().GetClipByName(gun.chargeAnimation).loopStart = 5;
            // Here we just set the quality of the gun and the "EncounterGuid", which is used by Gungeon to identify the gun.
            gun.quality = PickupObject.ItemQuality.A;
            //This block of code helps clone our projectile. Basically it makes it so things like Shadow Clone and Hip Holster keep the stats/sprite of your custom gun's projectiles.
            Projectile projectile = UnityEngine.Object.Instantiate<Projectile>((PickupObjectDatabase.GetById(15) as Gun).DefaultModule.projectiles[0]);
            projectile.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(projectile.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(projectile);
            gun.DefaultModule.projectiles[0] = projectile;
            //projectile.baseData allows you to modify the base properties of your projectile module.
            //In our case, our gun uses modified projectiles from the ak-47.
            //You can modify a good number of stats but for now, let's just modify the damage and speed.
            projectile.baseData.damage = 10;
            projectile.baseData.speed = 15f;
            projectile.transform.parent = gun.barrelOffset;
            //This determines what sprite you want your projectile to use. Note this isn't necessary if you don't want to have a custom projectile sprite.
            //The x and y values determine the size of your custom projectile
            projectile.SetProjectileSpriteRight("abgun_projectile_001", 26, 16, false, tk2dBaseSprite.Anchor.MiddleCenter, 16, 16);
            projectile.shouldRotate = true;
            ProjectileModule.ChargeProjectile item = new ProjectileModule.ChargeProjectile
            {
                Projectile = projectile,
                ChargeTime = 0f,
            };
            Projectile projectile2 = UnityEngine.Object.Instantiate<Projectile>((PickupObjectDatabase.GetById(15) as Gun).DefaultModule.projectiles[0]);
            projectile2.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(projectile2.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(projectile2);
            gun.DefaultModule.projectiles[0] = projectile2;
            //projectile.baseData allows you to modify the base properties of your projectile module.
            //In our case, our gun uses modified projectiles from the ak-47.
            //You can modify a good number of stats but for now, let's just modify the damage and speed.
            projectile2.baseData.damage = 20;
            projectile2.baseData.speed = 17.5f;
            projectile2.transform.parent = gun.barrelOffset;
            //This determines what sprite you want your projectile to use. Note this isn't necessary if you don't want to have a custom projectile sprite.
            //The x and y values determine the size of your custom projectile
            projectile2.SetProjectileSpriteRight("abgun_projectile_005", 13, 16, false, tk2dBaseSprite.Anchor.MiddleCenter, 16, 16);
            projectile2.shouldRotate = true;
            ProjectileModule.ChargeProjectile item2 = new ProjectileModule.ChargeProjectile
            {
                Projectile = projectile2,
                ChargeTime = 0.35f,
            };
            Projectile projectile3 = UnityEngine.Object.Instantiate<Projectile>((PickupObjectDatabase.GetById(15) as Gun).DefaultModule.projectiles[0]);
            projectile3.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(projectile3.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(projectile3);
            //projectile.baseData allows you to modify the base properties of your projectile module.
            //In our case, our gun uses modified projectiles from the ak-47.
            //You can modify a good number of stats but for now, let's just modify the damage and speed.
            projectile3.baseData.damage = 20;
            projectile3.baseData.speed = 35f;
            projectile3.transform.parent = gun.barrelOffset;
            //This determines what sprite you want your projectile to use. Note this isn't necessary if you don't want to have a custom projectile sprite.
            //The x and y values determine the size of your custom projectile
            projectile3.SetProjectileSpriteRight("abgun_projectile_002", 15, 15, false, tk2dBaseSprite.Anchor.MiddleCenter, 16, 16);
            projectile3.shouldRotate = true;
            ProjectileModule.ChargeProjectile item3 = new ProjectileModule.ChargeProjectile
            {
                Projectile = projectile3,
                ChargeTime = 1.20f,
            };
            Projectile projectile4 = UnityEngine.Object.Instantiate<Projectile>((PickupObjectDatabase.GetById(15) as Gun).DefaultModule.projectiles[0]);
            projectile4.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(projectile4.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(projectile4);
            //projectile.baseData allows you to modify the base properties of your projectile module.
            //In our case, our gun uses modified projectiles from the ak-47.
            //You can modify a good number of stats but for now, let's just modify the damage and speed.
            projectile4.baseData.damage = 40;
            projectile4.baseData.speed = 16f;
            projectile4.transform.parent = gun.barrelOffset;
            //This determines what sprite you want your projectile to use. Note this isn't necessary if you don't want to have a custom projectile sprite.
            //The x and y values determine the size of your custom projectile
            projectile4.SetProjectileSpriteRight("abgun_projectile_003", 15, 15, false, tk2dBaseSprite.Anchor.MiddleCenter, 16, 16);
            projectile4.shouldRotate = true;
            ProjectileModule.ChargeProjectile item4 = new ProjectileModule.ChargeProjectile
            {
                Projectile = projectile4,
                ChargeTime = 1.95f,
            };
            Projectile projectile5 = UnityEngine.Object.Instantiate<Projectile>((PickupObjectDatabase.GetById(656) as Gun).singleModule.chargeProjectiles[0].Projectile);
            projectile5.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(projectile5.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(projectile5);
            //projectile.baseData allows you to modify the base properties of your projectile module.
            //In our case, our gun uses modified projectiles from the ak-47.
            //You can modify a good number of stats but for now, let's just modify the damage and speed.
            projectile5.baseData.damage = 30;
            projectile5.baseData.speed = 13.5f;
            projectile5.transform.parent = gun.barrelOffset;
            //This determines what sprite you want your projectile to use. Note this isn't necessary if you don't want to have a custom projectile sprite.
            //The x and y values determine the size of your custom projectile
            projectile5.SetProjectileSpriteRight("abgun_projectile_004", 15, 15, false, tk2dBaseSprite.Anchor.MiddleCenter, 16, 16);
            projectile5.shouldRotate = true;
            ProjectileModule.ChargeProjectile item5 = new ProjectileModule.ChargeProjectile
            {
                Projectile = projectile5,
                ChargeTime = 2.70f,
            };
            gun.DefaultModule.chargeProjectiles = new List<ProjectileModule.ChargeProjectile> { item, item2, item3, item4, item5 };
            {

            };
            gun.DefaultModule.ammoType = GameUIAmmoType.AmmoType.CUSTOM;
            gun.DefaultModule.customAmmoType = "duck";
            gun.carryPixelOffset = new IntVector2(0, 0);
            gun.gunSwitchGroup = null;
            ETGMod.Databases.Items.Add(gun, null, "ANY");
            ExplosionData Boom = new ExplosionData()
            {
                effect = (PickupObjectDatabase.GetById(108) as SpawnObjectPlayerItem).objectToSpawn.GetComponent<ProximityMine>().explosionData.effect,


                damageRadius = 3.5f,
                damageToPlayer = 0f,
                doDamage = true,
                damage = 15,
                doDestroyProjectiles = false,
                doForce = true,
                debrisForce = 20f,
                preventPlayerForce = true,
                explosionDelay = 0.1f,
                usesComprehensiveDelay = false,
                doScreenShake = true,
                playDefaultSFX = false,
                force = 20,
                breakSecretWalls = false,

            };
            ExplosiveModifier boom = projectile4.gameObject.GetOrAddComponent<ExplosiveModifier>();
            boom.explosionData = Boom;
            boom.doExplosion = true;

        }

        public override void PostProcessProjectile(Projectile projectile)
        {
            AkSoundEngine.PostEvent("Play_WPN_megablaster_shot_02", base.gameObject);
            base.PostProcessProjectile(projectile);
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
                AkSoundEngine.PostEvent("Play_WPN_megablaster_reload_01", base.gameObject);
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