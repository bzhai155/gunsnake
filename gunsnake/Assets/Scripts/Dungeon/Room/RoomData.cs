﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomData : ScriptableObject
{
    [Header("Manually set")]
    public string roomName;
    public RoomType roomType;

    public bool isJungle;
    public bool isDungeon;
    public bool isTemple;

    public bool hasNorthDoor;
    public bool hasEastDoor;
    public bool hasSouthDoor;
    public bool hasWestDoor;

    [Header("Automatically set")]
    //public GameObject roomObject;
    public string roomObjectPath;

    [TextArea()]
    public string roomString;
    public int width;
    public int height;

    public const char WALLCHAR = '#';
    public const char FLOORCHAR = '.';
    public const char EMPTYCHAR = ' ';

    public void SetData(RoomData other)
    {
        roomName = other.roomName;
        roomType = other.roomType;
        hasNorthDoor = other.hasNorthDoor;
        hasEastDoor = other.hasEastDoor;
        hasSouthDoor = other.hasSouthDoor;
        hasWestDoor = other.hasWestDoor;

        //roomObject = other.roomObject;
        roomObjectPath = other.roomObjectPath;
        roomString = other.roomString;
        width = other.width;
        height = other.height;
    }
}
