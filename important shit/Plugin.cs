using BepInEx;
using Alexandria;
using Alexandria.ItemAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using HarmonyLib;
using HutongGames.PlayMaker.Actions;

namespace GunMania
{
    [BepInDependency(Alexandria.Alexandria.GUID)] // this mod depends on the Alexandria API: https://enter-the-gungeon.thunderstore.io/package/Alexandria/Alexandria/
    [BepInDependency(ETGModMainBehaviour.GUID)]
    [BepInPlugin(GUID, NAME, VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        public const string GUID = "gustavin.etg.gunmania";
        public const string NAME = "GunMania";
        public const string VERSION = "1.0.0";
        public const string TEXT_COLOR = "#00FFFF";

        public void Start()
        {
            ETGModMainBehaviour.WaitForGameManagerStart(GMStart);
        }
        public void GMStart(GameManager g)
        {
            Slingshot.Add();
            Frostbite.Add();
            SniperShotty.Add();
            DessertEagle.Add();
            SludgeMaker.Add();
            Dick.Add();
            KittyCannon.Add();
            BlunderBus.Add();
            Revenge.Add();
            RainbuddySpecial.Add();
            Gal.Add();
            FrogRing.Add();
            FlubberRing.Add();
            CaseyRing.Add();
            EncheesedLead.Add();
            MucousLead.Add();

            Log($"{NAME} v{VERSION} started succesfully", ("#dd00ff"));

            PickupObjectDatabase.GetById(368).quality = PickupObject.ItemQuality.B;
            PickupObjectDatabase.GetById(535).quality = PickupObject.ItemQuality.A;
            PickupObjectDatabase.GetById(747).quality = PickupObject.ItemQuality.S;
            Log($"GunMania - Cut guns succesfully reimplemented! (El Tigre, Real Cool Bow, Turtine Gun)", ("#dd00ff"));

        }

        public static void Log(string text, string color = "#FFFFFF")
        {
            ETGModConsole.Log($"<color={color}>{text}</color>");


        }
    }
}
