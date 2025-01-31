﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewModdingAPI.Utilities;
using StardewValley;
using StardewValley.Events;
using StardewValley.Locations;
using StardewValley.Menus;
using xTile;

namespace WeddingMod
{
    /// <summary>The main entry point for the mod.</summary>
    public class ModEntry : Mod
    {
        /*********
        ** Fields
        *********/
        /// <summary>The path to the wedding map file, relative to the mod folder.</summary>
        private string MapPath = "assets/wedding.tbin";

        /// <summary>The player's tile position in the wedding map.</summary>
        private readonly Vector2 PlayerPixels = new Vector2(1732, 4052);


        /// <summary>The question before the wedding.</summary>

        public void setup(object sender, DayStartedEventArgs e)
        {
            if (Game1.weddingToday)
            {
                //Lets clean this up some
                string question = Helper.Translation.Get("Wedding_Question");
                List<Response> options = new List<Response>()
                {
                    new Response("On the Beach.",this.Helper.Translation.Get("Wedding_Beach")),
                    new Response("In the Forest, near the river.",this.Helper.Translation.Get("Wedding_Forest")),
                    new Response("In the Secret Woods.",this.Helper.Translation.Get("Wedding_Wood")),
                    new Response("Where the Flower Dance takes place!",this.Helper.Translation.Get("Wedding_Flower"))
                };
                /*
                   I changed up the createQuestionDialogue, to make it easier to see. Basically, it pulls the question from the question string, and the responses from the options list, then turns it into an array. The answer part is a method that is called when the player clicks on a response.
                */
                Game1.player.currentLocation.createQuestionDialogue(
                    question,
                    options.ToArray(),
                    answer
                );
            }
        }


        /*********
        ** Public methods
        *********/
        /// <summary>The mod entry point, called after the mod is first loaded.</summary>
        /// <param name="helper">Provides simplified APIs for writing mods.</param>
        public override void Entry(IModHelper helper)
        {
            //helper.Events.Player.Warped += Player_Warped;
            helper.Events.GameLoop.DayStarted += setup;

        }

        /// <summary>The method called after a new day starts.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void Player_Warped(object sender, WarpedEventArgs e)
        {
            if (Game1.weddingToday && e.NewLocation.Name == "Town" && e.NewLocation.currentEvent != null)
            {
                this.Monitor.Log("The wedding event has started!");

                // create temporary location
                this.Helper.Content.Load<Map>(this.MapPath); // make sure map is loaded before game accesses it
                string mapKey = this.Helper.Content.GetActualAssetKey(this.MapPath);
                var newLocation = new GameLocation(mapKey, "Temp");

                // move everything to new location
                var oldLocation = Game1.currentLocation;
                Game1.player.currentLocation = Game1.currentLocation = newLocation;
                newLocation.resetForPlayerEntry();
                newLocation.currentEvent = oldLocation.currentEvent;
                newLocation.Map.LoadTileSheets(Game1.mapDisplayDevice);

                // move player position within map (if needed)
                Game1.player.Position = this.PlayerPixels;
            }
        }

        /// <summary>The method that is called inside of the createQuestionDialogue</summary>
        /// <param name="who">The player.</param>
        /// <param name="ans">The answer key.</param>
        private void answer(Farmer who, string ans)
        {
            switch (ans)
            {
                case "On the Beach.":
                    MapPath = "assets/WeddingBeach.tbin";
                    SetUpMaps();
                    // Load Beach
                    break;
                case "In the Forest, near the river.":
                    MapPath = "assets/WeddingForest.tbin";
                    SetUpMaps();
                    // Load Forest
                    break;
                case "In the Secret Woods.":
                    MapPath = "assets/WeddingWoods.tbin";
                    SetUpMaps();
                    // Load Wood
                    break;
                case "Where the Flower Dance takes place!":
                    MapPath = "assets/WeddingFlower.tbin";
                    SetUpMaps();
                    // Load Flower Dance
                    break;
            }
            Game1.showGlobalMessage(MapPath);
        }

        ///<summary></summary>
        /// 
        private void SetUpMaps()
        {
            this.Monitor.Log("The wedding event has started!");

            // create temporary location
            this.Helper.Content.Load<Map>(this.MapPath); // make sure map is loaded before game accesses it
            string mapKey = this.Helper.Content.GetActualAssetKey(this.MapPath);
            var newLocation = new GameLocation(mapKey, "Temp");

            // move everything to new location
            var oldLocation = Game1.currentLocation;
            Game1.player.currentLocation = Game1.currentLocation = newLocation;
            newLocation.resetForPlayerEntry();
            newLocation.currentEvent = oldLocation.currentEvent;
            newLocation.Map.LoadTileSheets(Game1.mapDisplayDevice);

            // move player position within map (if needed)
            Game1.player.Position = this.PlayerPixels;

            //temp sprites

            newLocation.TemporarySprites.Add(new TemporaryAnimatedSprite("LooseSprites\\Cursors", new Microsoft.Xna.Framework.Rectangle(540, 1196, 98, 54), 99999f, 1, 99999, new Vector2(25f, 60f) * 64f + new Vector2(0.0f, -64f), false, false, 1f, 0.0f, Color.White, 4f, 0.0f, 0.0f, 0.0f, false));
            newLocation.TemporarySprites.Add(new TemporaryAnimatedSprite("LooseSprites\\Cursors", new Microsoft.Xna.Framework.Rectangle(540, 1250, 98, 25), 99999f, 1, 99999, new Vector2(25f, 60f) * 64f + new Vector2(0.0f, 54f) * 4f + new Vector2(0.0f, -64f), false, false, 0.0f, 0.0f, Color.White, 4f, 0.0f, 0.0f, 0.0f, false));
        }
    }
}

