using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;
using Verse;
using Verse.Sound;

namespace Roos_Faun_Xenotype
{
    public class FaunSettings : ModSettings
    {
        // Default Mod Settings
        public const bool mechanophobiaAffectsFriendlyDefault = true;

        // setting variables to defaults
        public static bool mechanophobiaAffectsFriendly = mechanophobiaAffectsFriendlyDefault;

        // Writes settings to file. Note that saving is by ref.
        public override void ExposeData()
        {
            Scribe_Values.Look(ref mechanophobiaAffectsFriendly, "mechaphobiaAffectsFriendly", mechanophobiaAffectsFriendlyDefault);
            base.ExposeData();
        }
    }


    public class Roos_Faun_Xenotype : Mod
    {
        // reference to our settings.
        public FaunSettings settings;

        // constructor which resolves the reference to our settings.
        public Roos_Faun_Xenotype(ModContentPack content) : base(content)
        {
            this.settings = GetSettings<FaunSettings>();
        }

        // The GUI part to set our settings.
        public override void DoSettingsWindowContents(Rect inRect)
        {
            Listing_Standard listingStandard = new Listing_Standard();
            listingStandard.Begin(inRect);

            //listingStandard.Label("Lactation Ability Settings");
            listingStandard.CheckboxLabeled("Mechaphobia counts friendly mechs ", ref FaunSettings.mechanophobiaAffectsFriendly);
            listingStandard.End();
            base.DoSettingsWindowContents(inRect);
        }

        // Override SettingsCategory to show up in the list of settings.
        public override string SettingsCategory()
        {
            return "Roo's Faun Xenotype";
        }
    }
}

