﻿/*
 * Author: Shon Verch
 * File Name: DungeonInstance.cs
 * Project Name: TheDungeonMaster
 * Creation Date: 12/28/2017
 * Modified Date: 1/18/2018
 * Description: The top-level manager for a dungeon.
 */

using System.Collections;
using System.Linq;
using UnityEngine;

/// <summary>
/// The top-level manager for a dungeon.
/// </summary>
public class DungeonInstance : MonoBehaviour
{
    [SerializeField]
    private Room entryRoom;
    [SerializeField]
    private EncounterRoom[] encounters;
    [SerializeField]
    private EncounterRoom bossRoom;
    [SerializeField]
    private GameObject dungeonDoor;
    [SerializeField]
    private GameObject bossDoor;

    private bool isBossDoorOpen;

    /// <summary>
    /// Called every frame.
    /// </summary>
    private void Update()
    {
        // If we have completed all encounters and the boss door hasn't been opened yet, let's open the boss door!
        if (IsReadyForBoss() && !isBossDoorOpen)
        {
            isBossDoorOpen = false;
            bossDoor.SetActive(false);
        }

        // If the boss encounter is completed and we haven't opened the dungeon door yet, let's open it!
        if (!bossRoom.IsComplete || !dungeonDoor.activeInHierarchy) return;
        dungeonDoor.SetActive(false);
    }

    /// <summary>
    /// Indicates whether the player is ready to fight the boss.
    /// The player is ready when all encounters in the dungeon are complete.
    /// </summary>
    /// <returns></returns>
    private bool IsReadyForBoss() => encounters.All(encounterRoom => encounterRoom.IsComplete);
}
