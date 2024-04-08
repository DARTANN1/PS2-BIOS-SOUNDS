using System.IO;
using System.Reflection;
using Aki.Reflection.Patching;
using BepInEx;
using EFT.UI;
using UnityEngine;

namespace SamSWAT.PS2BIOS
{
    [BepInPlugin("com.samswat.ps2bios", "SamSWAT.PS2BIOS", "1.0.0")]
    public class Plugin : BaseUnityPlugin
    {
        internal static AudioClip ButtonOver;
        internal static AudioClip ButtonClick;
        
        private void Awake()
        {
            LoadSoundBundle();
            new Patch().Enable();
        }

        private void LoadSoundBundle()
        {
            var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "/bundles/ps2bios.bundle";
            var bundle = AssetBundle.LoadFromFile(path);
            var assets = bundle.LoadAllAssets();
            ButtonOver = assets[0] as AudioClip;
            ButtonClick = assets[1] as AudioClip;
        }
    }
    
    public class Patch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return typeof(UISoundsWrapper).GetMethod(nameof(UISoundsWrapper.GetUIClip));
        }

        [PatchPostfix]
        private static void PatchPostfix(ref AudioClip __result, EUISoundType soundType)
        {
            switch (soundType)
            {
                case EUISoundType.ButtonClick:
                    __result = Plugin.ButtonClick;
                    break;
                case EUISoundType.ButtonOver:
                    __result = Plugin.ButtonOver;
                    break;
            }
        }
    }
}