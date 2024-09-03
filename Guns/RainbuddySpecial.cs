using System;
using System.Collections;
using Gungeon;
using MonoMod;
using UnityEngine;
using Alexandria.ItemAPI;

namespace GunMania
{

    class RainbuddySpecial : GunBehaviour
    {


        public static void Add()
        {
            // Get yourself a new gun "base" first.
            // Let's just call it "Basic Gun", and use "jpxfrd" for all sprites and as "codename" All sprites must begin with the same word as the codename. For example, your firing sprite would be named "jpxfrd_fire_001".
            Gun gun = ETGMod.Databases.Items.NewGun("Rainbuddy Special", "bwlr");
            // "kp:basic_gun determines how you spawn in your gun through the console. You can change this command to whatever you want, as long as it follows the "name:itemname" template.
            Game.Items.Rename("outdated_gun_mods:rainbuddy_special", "gunmania:rainbuddy_special");
            gun.gameObject.AddComponent<RainbuddySpecial>();
            //These two lines determines the description of your gun, ".SetShortDescription" being the description that appears when you pick up the gun and ".SetLongDescription" being the description in the Ammonomicon entry. 
            gun.SetShortDescription("-The Rain Train");
            gun.SetLongDescription("Bowler's trusty ol' shotgun. It can only kill those who are rainbow.");
            // This is required, unless you want to use the sprites of the base gun.
            // That, by default, is the pea shooter.
            // SetupSprite sets up the default gun sprite for the ammonomicon and the "gun get" popup.
            // WARNING: Add a copy of your default sprite to Ammonomicon Encounter Icon Collection!
            // That means, "sprites/Ammonomicon Encounter Icon Collection/defaultsprite.png" in your mod .zip. You can see an example of this with inside the mod folder.
            gun.SetupSprite(null, "bwlr_idle_001", 8);
            // ETGMod automatically checks which animations are available.
            // The numbers next to "shootAnimation" determine the animation fps. You can also tweak the animation fps of the reload animation and idle animation using this method.
            gun.SetAnimationFPS(gun.shootAnimation, 20);
            gun.SetAnimationFPS(gun.reloadAnimation, 6); // Every modded gun has base projectile it works with that is borrowed from other guns in the game.                                   // The gun names are the names from the JSON dump! While most are the same, some guns named completely different things. If you need help finding gun names, ask a modder on the Gungeon discord.
            gun.muzzleFlashEffects = (PickupObjectDatabase.GetById(601) as Gun).muzzleFlashEffects;
            for (int i = 0; i < 4; i++)
            {
                gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(157) as Gun, true, false);
            }
            foreach (ProjectileModule projectileModule in gun.Volley.projectiles)
            {
                projectileModule.ammoCost = 1;
                projectileModule.shootStyle = ProjectileModule.ShootStyle.Automatic;
                projectileModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
                projectileModule.cooldownTime = 0.7f;
                projectileModule.angleVariance = 12f;
                gun.barrelOffset.transform.localPosition = new Vector3(1.375f, 0.25f, 0f);
                projectileModule.numberOfShotsInClip = 7;
                Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(projectileModule.projectiles[0]);
                projectile.gameObject.SetActive(false);
                projectileModule.projectiles[0] = projectile;
                projectile.baseData.damage = 7f;
                projectile.baseData.range = 20;
                projectile.baseData.speed = 15;
                projectile.AdditionalScaleMultiplier = .5f;
                projectile.SetProjectileSpriteRight("bwlr_projectile_001", 9, 9, false, tk2dBaseSprite.Anchor.MiddleCenter, 16, 16);
                FakePrefab.MarkAsFakePrefab(projectile.gameObject);
                UnityEngine.Object.DontDestroyOnLoad(projectile);
                gun.DefaultModule.ammoType = GameUIAmmoType.AmmoType.SHOTGUN;
                gun.DefaultModule.projectiles[0] = projectile;
                gun.gunClass = GunClass.SHOTGUN;
                bool flag = projectileModule != gun.DefaultModule;
                if (flag)
                {
                    projectileModule.ammoCost = 0;
                }

            }
            gun.reloadTime = 1.7f;
            gun.SetBaseMaxAmmo(125);
            gun.carryPixelOffset = new IntVector2(4, -1);

            
            ETGMod.Databases.Items.Add(gun, null, "ANY");
            gun.PreventNormalFireAudio = true;
            gun.gunSwitchGroup = (PickupObjectDatabase.GetById(157) as Gun).gunSwitchGroup;
        }

        public override void PostProcessProjectile(Projectile projectile)
        {
            base.PostProcessProjectile(projectile);
            projectile.OnHitEnemy += this.OnHitEnemy;
        }

        private void OnHitEnemy(Projectile p, SpeculativeRigidbody enemy, bool killed)
        {
            if (enemy == null || enemy.healthHaver == null || enemy.healthHaver.bodySprites == null)
                return;

            foreach (var s in enemy.healthHaver.bodySprites)
            {
                s.usesOverrideMaterial = true;
                s.renderer.material.shader = ShaderCache.Acquire("Brave/Internal/RainbowChestShader");
            }
        }
        //Now add the Tools class to your project.
        //All that's left now is sprite stuff. 
        //Your sprites should be organized, like how you see in the mod folder. 
        //Every gun requires that you have a .json to match the sprites or else the gun won't spawn at all
        //.Json determines the hand sprites for your character. You can make a gun two handed by having both "SecondaryHand" and "PrimaryHand" in the .json file, which can be edited through Notepad or Visual Studios
        //By default this gun is a one-handed weapona
        //If you need a basic two handed .json. Just use the jpxfrd2.json.
        //And finally, don't forget to add your Gun to your ETGModule class!
    }
}