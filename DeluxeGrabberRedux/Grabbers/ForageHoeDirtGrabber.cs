﻿using Microsoft.Xna.Framework;
using StardewValley;
using StardewValley.TerrainFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SObject = StardewValley.Object;

namespace DeluxeGrabberRedux.Grabbers
{
    class ForageHoeDirtGrabber : TerrainFeaturesMapGrabber
    {
        public ForageHoeDirtGrabber(ModEntry mod, GameLocation location) : base(mod, location)
        {
        }

        public override bool GrabFeature(Vector2 tile, TerrainFeature feature)
        {
            if (feature is HoeDirt dirt && IsForeageableHoeDirt(feature)) {
                SObject crop = null;

                // impl @ StardewValley::Crop::harvest
                if (dirt.crop.whichForageCrop.Value == Crop.forageCrop_springOnion)
                {
                    crop = Helpers.SetForageStatsBasedOnProfession(Player, new SObject(ItemIds.SpringOnion, 1), tile, true);
                }
                // impl @ StardewValley::Crop::hitWithHoe
                else if (dirt.crop.whichForageCrop.Value == Crop.forageCrop_ginger)
                {
                    crop = new SObject(ItemIds.Ginger, 1);
                }

                if (crop != null && TryAddItem(crop))
                {
                    if (dirt.crop.whichForageCrop.Value == Crop.forageCrop_springOnion)
                    {
                        GainExperience(Skills.Foraging, 3);
                    }
                    dirt.destroyCrop(tile, false, Location);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        private bool IsForeageableHoeDirt(TerrainFeature feature)
        {
            var dirt = feature is HoeDirt ? feature as HoeDirt : null;
            if (dirt != null)
            {
                return dirt.crop != null && dirt.crop.forageCrop.Value;
            }
            else
            {
                return false;
            }
        }
    }
}
