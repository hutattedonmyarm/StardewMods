using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Pathoschild.Stardew.DataLayers.Framework;
using StardewValley;

namespace Pathoschild.Stardew.DataLayers.Layers
{
    internal class AnimalPetLayer : BaseLayer
    {
        /*********
        ** Fields
        *********/
        /// <summary>The legend entry for petted animals.</summary>
        private readonly LegendEntry Petted;

        // <summary>The legend entry for not yet petted animals.</summary>
        private readonly LegendEntry NotPetted;


        /*********
        ** Public methods
        *********/
        /// <summary>Construct an instance.</summary>
        /// <param name="config">The data layer settings.</param>
        /// <param name="mods">Handles access to the supported mod integrations.</param>
        public AnimalPetLayer(LayerConfig config)
            : base(I18n.Animalpet_Name(), config)
        {
            this.Legend = new[]
            {
                this.Petted = new LegendEntry(I18n.Keys.Animalpet_Petted, Color.Green),
                this.NotPetted = new LegendEntry(I18n.Keys.Animalpet_NotPetted, Color.Red)
            };
        }

        /// <summary>Get the updated data layer tiles.</summary>
        /// <param name="location">The current location.</param>
        /// <param name="visibleArea">The tile area currently visible on the screen.</param>
        /// <param name="visibleTiles">The tile positions currently visible on the screen.</param>
        /// <param name="cursorTile">The tile position under the cursor.</param>
        public override TileGroup[] Update(GameLocation location, in Rectangle visibleArea, in Vector2[] visibleTiles, in Vector2 cursorTile)
        {
            List<TileData> pettedAnimals = new List<TileData>();
            List<TileData> unpettedAnimals = new List<TileData>();
            foreach (FarmAnimal animal in (location as Farm)?.animals.Values ?? (location as AnimalHouse)?.animals.Values ?? Enumerable.Empty<FarmAnimal>())
            {
                Vector2 entityTile = animal.getTileLocation();
                if (visibleTiles.Contains(entityTile))
                {
                    if(animal.wasPet.Value)
                    {
                        pettedAnimals.Add(new TileData(entityTile, this.Petted));
                    } else
                    {
                        unpettedAnimals.Add(new TileData(entityTile, this.NotPetted));
                    }
                }
            }
            return new[] {
                new TileGroup(pettedAnimals, outerBorderColor: this.Petted.Color),
                new TileGroup(unpettedAnimals, outerBorderColor: this.NotPetted.Color)
            };
        }
    }
}
