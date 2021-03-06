﻿/*
 * Author: Shon Verch
 * File Name: RoomManager.cs
 * Project Name: TheDungeonMaster
 * Creation Date: 12/28/2017
 * Modified Date: 12/29/2017
 * Description: Stores all the rooms in the world.
 */

using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Stores all the rooms in the world.
/// </summary>
public class RoomManager : IEnumerable<Room>
{
    /// <summary>
    /// Gets a <see cref="Room"/> by index.
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public Room this[int index] => Get(index);

    private readonly List<Room> rooms;

    /// <summary>
    /// Initialize the <see cref="RoomManager"/>
    /// </summary>
    public RoomManager()
    {
        rooms = new List<Room>();
    }

    /// <summary>
    /// Add a <see cref="Room"/> to the manager.
    /// </summary>
    /// <param name="room">The <see cref="Room"/> to add.</param>
    public void Add(Room room)
    {
        rooms.Add(room);
    }

    /// <summary>
    /// Get a <see cref="Room"/> from index.
    /// </summary>
    /// <param name="index">The index of the <see cref="Room"/> to retrieve.</param>
    public Room Get(int index)
    {
        if (index < 0 || index > rooms.Count - 1) return null;
        return rooms[index];
    }

    /// <summary>
    /// Get the index of a <see cref="Room"/>.
    /// </summary>
    /// <param name="room">The room to retrieve the index of.</param>
    public int GetRoomIndex(Room room) => rooms.IndexOf(room);

    /// <summary>
    /// Retrieve the <see cref="IEnumerator{T}"/> for this <see cref="RoomManager"/> which iterates over the stored <see cref="Room"/> collection.
    /// </summary>
    public IEnumerator<Room> GetEnumerator() => ((IEnumerable<Room>)rooms).GetEnumerator();

    /// <summary>
    /// Retrieve the <see cref="IEnumerator{T}"/> for this <see cref="RoomManager"/> which iterates over the stored <see cref="Room"/> collection.
    /// </summary>
    IEnumerator IEnumerable.GetEnumerator() => rooms.GetEnumerator();
}
