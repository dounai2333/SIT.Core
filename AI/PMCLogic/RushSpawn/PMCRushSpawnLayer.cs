﻿using BepInEx.Logging;
using DrakiaXYZ.BigBrain.Brains;
using DrakiaXYZ.Waypoints.BrainLogic;
using EFT;
using UnityEngine;

namespace SIT.Core.AI.PMCLogic.RushSpawn
{
    internal class PMCRushSpawnLayer : CustomLayer
    {
        protected ManualLogSource Logger;
        protected float nextRoamCheckTime = 0f;
        protected bool isActive = false;

        public PMCRushSpawnLayer(BotOwner botOwner, int priority) : base(botOwner, priority)
        {
            Logger = BepInEx.Logging.Logger.CreateLogSource(this.GetType().Name);
            Logger.LogInfo($"Added PMCRushSpawnLayer to {botOwner.name}");
        }

        public override string GetName()
        {
            return "PMCRushSpawn";
        }

        public override bool IsActive()
        {
            // If we're not in peace, we can't be roaming, otherwise we die
            if (!BotOwner.Memory.IsPeace)
            {
                return false;
            }

            // If we're active already, then stay active
            if (isActive)
            {
                return true;
            }

            // If it's been long enough, check if we should roam
            if (Time.time > nextRoamCheckTime)
            {
                Logger.LogDebug($"Checking if {BotOwner.name} should roam");
                nextRoamCheckTime = Time.time + 10f;

                if (Random.Range(0, 100) > 50)
                {
                    Logger.LogDebug("  Roaming");
                    isActive = true;
                    return true;
                }
                else
                {
                    Logger.LogDebug("  Not Roaming");
                }
            }

            return false;
        }

        public override Action GetNextAction()
        {
            return new Action(typeof(RoamingLogic), "PMCRushSpawn");
        }

        public override bool IsCurrentActionEnding()
        {
            // We only have one action, so it's never ending
            return false;
        }
    }
}
