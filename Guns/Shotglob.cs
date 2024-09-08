using System;
using System.Collections;
using Gungeon;
using MonoMod;
using UnityEngine;
using Alexandria.ItemAPI;
using System.Collections.Generic;

namespace GunMania
{
    
    public class Shotglob : GunBehaviour
    {


        public static void Add()
        {
            // Get yourself a new gun "base" first.
            // Let's just call it "Basic Gun", and use "jpxfrd" for all sprites and as "codename" All sprites must begin with the same word as the codename. For example, your firing sprite would be named "jpxfrd_fire_001".
            Gun gun = ETGMod.Databases.Items.NewGun("Shotglob", "shotglob");
            // "kp:basic_gun determines how you spawn in your gun through the console. You can change this command to whatever you want, as long as it follows the "name:itemname" template.
            Game.Items.Rename("outdated_gun_mods:shotglob", "gs:shotglob");
            gun.gameObject.AddComponent<Shotglob>();
            //These two lines determines the description of your gun, ".SetShortDescription" being the description that appears when you pick up the gun and ".SetLongDescription" being the description in the Ammonomicon entry. 
            gun.SetShortDescription("So Cute!!");
            gun.SetLongDescription("The projectiles it fires are rather bouncy for a bullet.");
            // This is required, unless you want to use the sprites of the base gun.
            // That, by default, is the pea shooter.
            // SetupSprite sets up the default gun sprite for the ammonomicon and the "gun get" popup.
            // WARNING: Add a copy of your default sprite to Ammonomicon Encounter Icon Collection!
            // That means, "sprites/Ammonomicon Encounter Icon Collection/defaultsprite.png" in your mod .zip. You can see an example of this with inside the mod folder.
            gun.SetupSprite(null, "shotglob_idle_001", 8);
            // ETGMod automatically checks which animations are available.
            // The numbers next to "shootAnimation" determine the animation fps. You can also tweak the animation fps of the reload animation and idle animation using this method.
            gun.SetAnimationFPS(gun.shootAnimation, 12);
            // Every modded gun has base projectile it works with that is borrowed from other guns in the game. 
            // The gun names are the names from the JSON dump! While most are the same, some guns named completely different things. If you need help finding gun names, ask a modder on the Gungeon discord.
            gun.AddProjectileModuleFrom("ak-47", true, false);
            // Here we just take the default projectile module and change its settings how we want it to be.
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.SemiAutomatic;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.carryPixelOffset = new IntVector2(7, -4);
            gun.reloadTime = 1.1f;
            gun.DefaultModule.cooldownTime = 0.5f;
            gun.DefaultModule.numberOfShotsInClip = 3;
            gun.SetBaseMaxAmmo(200);
            // Here we just set the quality of the gun and the "EncounterGuid", which is used by Gungeon to identify the gun.
            gun.quality = PickupObject.ItemQuality.B;
            //This block of code helps clone our projectile. Basically it makes it so things like Shadow Clone and Hip Holster keep the stats/sprite of your custom gun's projectiles.
            Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(gun.DefaultModule.projectiles[0]);
            projectile.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(projectile.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(projectile);
            gun.DefaultModule.projectiles[0] = projectile;
            gun.muzzleFlashEffects = VFXToolbox.CreateVFXPool("Shotglob Flash",
    new List<string>()
    {
                    "GunMania/Resources/MuzzleFlashes/splatlamo001",
                    "GunMania/Resources/MuzzleFlashes/splatlamo002",
                    "GunMania/Resources/MuzzleFlashes/splatlamo003",


    },
    15, //FPS
    new IntVector2(27, 16), //Dimensions
    tk2dBaseSprite.Anchor.MiddleLeft, //Anchor
    false, //Uses a Z height off the ground
    0, //The Z height, if used
    false,
   VFXAlignment.Fixed
  );
            //projectile.baseData allows you to modify the base properties of your projectile module.
            //In our case, our gun uses modified projectiles from the ak-47.
            //You can modify a good number of stats but for now, let's just modify the damage and speed.
            for (int i = 0; i < 3; i++)
            {
                gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(157) as Gun, true, false);
            }
            foreach (ProjectileModule projectileModule in gun.Volley.projectiles)
            {
                projectileModule.ammoCost = 1;
                projectileModule.shootStyle = ProjectileModule.ShootStyle.Automatic;
                projectileModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
                BounceProjModifier bounce = projectile.gameObject.GetOrAddComponent<BounceProjModifier>();
                bounce.numberOfBounces = 1;
                projectileModule.cooldownTime = 0.7f;
                projectileModule.angleVariance = 7f;
                gun.barrelOffset.transform.localPosition = new Vector3(1.3125f, 0.4375f, 0f);
                projectileModule.numberOfShotsInClip = 3;
                projectile.gameObject.SetActive(false);
                projectileModule.projectiles[0] = projectile;
                projectile.baseData.damage = 7.25f;
                projectile.baseData.range = 50;
                projectile.baseData.speed = 10;
                projectile.AdditionalScaleMultiplier = .5f;
                projectile.SetProjectileSpriteRight("shotglobber_projectile_001", 10, 10, false, tk2dBaseSprite.Anchor.MiddleCenter, 16, 16);
                FakePrefab.MarkAsFakePrefab(projectile.gameObject);
                UnityEngine.Object.DontDestroyOnLoad(projectile);
                gun.DefaultModule.ammoType = GameUIAmmoType.AmmoType.CUSTOM;
                gun.DefaultModule.customAmmoType = CustomClipAmmoTypeToolbox.AddCustomAmmoType("shottyglob", "GunMania/Resources/AmmoTypes/shottyglob_clipfull", "GunMania/Resources/AmmoTypes/shottyglob_clipempty");
                gun.DefaultModule.projectiles[0] = projectile;
                bool flag = projectileModule != gun.DefaultModule;
                if (flag)
                {
                    projectileModule.ammoCost = 0;
                }
                //This determines what sprite you want your projectile to use. Note this isn't necessary if you don't want to have a custom projectile sprite.
                //The x and y values determine the size of your custom projectile
                ETGMod.Databases.Items.Add(gun, null, "ANY");

            }
        }

        public override void OnPostFired(PlayerController player, Gun gun)
        {
            //This determines what sound you want to play when you fire a gun.
            //Sounds names are based on the Gungeon sound dump, which can be found at EnterTheGungeon/Etg_Data/StreamingAssets/Audio/GeneratedSoundBanks/Windows/sfx.txt
            gun.PreventNormalFireAudio = true;
            AkSoundEngine.PostEvent("slime_blast_001", gameObject);
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
				AkSoundEngine.PostEvent("Play_WPN_SAA_reload_01", base.gameObject);
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
