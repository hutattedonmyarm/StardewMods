using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Pathoschild.Stardew.DataLayers.Framework;
using StardewValley;

namespace Pathoschild.Stardew.DataLayers.Layers
{
    internal class ForageLayer : BaseLayer
    {
        /*********
        ** Fields
        *********/
        /// <summary>The legend entry for forageable items.</summary>
        private readonly LegendEntry Forageable;


        /*********
        ** Public methods
        *********/
        /// <summary>Construct an instance.</summary>
        /// <param name="config">The data layer settings.</param>
        /// <param name="mods">Handles access to the supported mod integrations.</param>
        public ForageLayer(LayerConfig config)
            : base(I18n.Forage_Name(), config)
        {
            this.Legend = new[]
            {
                this.Forageable = new LegendEntry(I18n.Keys.Forage_Foregeable, Color.Green)
            };
        }

        /// <summary>Get the updated data layer tiles.</summary>
        /// <param name="location">The current location.</param>
        /// <param name="visibleArea">The tile area currently visible on the screen.</param>
        /// <param name="visibleTiles">The tile positions currently visible on the screen.</param>
        /// <param name="cursorTile">The tile position under the cursor.</param>
        public override TileGroup[] Update(GameLocation location, in Rectangle visibleArea, in Vector2[] visibleTiles, in Vector2 cursorTile)
        {
            var forageItems =
                (
                    from Vector2 tile in visibleTiles
                    where location.objects.ContainsKey(tile)
                    let forageItem = location.objects[tile]
                    where forageItem != null && forageItem.isForage(location)
                    select new TileData(forageItem.TileLocation, this.Forageable)
                )
                .ToArray();
            return new[] { new TileGroup(forageItems, outerBorderColor: Color.Green) };
        }
    }
}
