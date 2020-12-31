using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UUID))]
public class PortalManager : MonoBehaviour, IDataPersistance
{
    public int AlertMeter;
    private List<Room> Rooms;

    private UUID uuid;

    struct Room
    {
        public PortalOcclusionVolume volume;
        public Portal[] portals;


        public bool isPlayerInRoom()
        {
            return volume.IsPlayerInRoom();
        }
    }

    void Awake()
    {
        uuid = GetComponent<UUID>();
    }

    // Start is called before the first frame update
    void Start()
    {
        UpdateRooms();
    }

    // Update is called once per frame
    void Update()
    {
        if (AlertMeter >= 100)
        {
            SwapPortals();
            AlertMeter = 0;
        }

    }

    void SwapPortals()
    {
        foreach (var room in Rooms)
        {
            foreach (var portal in room.portals)
            {
                if(portal.AffectedByBossAlert)
                    portal.TargetPortalIndex++;
            }
        }

        UpdateRooms();
    }

    void RandomPortals()
    {
        if (AlertMeter >= 100)
        {
            ClearPortalLinks();
            int randomRoomIndex;
            int previousIndex = 0;

            for (int i = 0; i < Rooms.Count;)
            {
                foreach (Portal p1 in Rooms[0].portals)
                {
                    if (p1.TargetPortal == null)
                    {
                        bool notLinked = true;


                        while (notLinked && Rooms.Count > 1)
                        {
                            randomRoomIndex = Random.Range(0, Rooms.Count);

                            if (randomRoomIndex != previousIndex)
                            {
                                foreach (var p2 in Rooms[randomRoomIndex].portals)
                                {
                                    if (p2.TargetPortal == null)
                                    {
                                        p1.TargetPortal[p1.TargetPortalIndex] = p2;
                                        p2.TargetPortal[p2.TargetPortalIndex] = p1;
                                        notLinked = false;
                                        previousIndex = randomRoomIndex;
                                        break;
                                    }

                                }

                                if (notLinked)
                                {
                                    Rooms.Remove(Rooms[randomRoomIndex]);
                                }
                            }
                        }

                    }
                }
                Rooms.Remove(Rooms[0]);
                previousIndex = 0;
            }

            AlertMeter = 0;
            UpdateRooms();
        }
    }

    void ClearPortalLinks()
    {
        for (int i = 0; i < Rooms.Count; i++)
        {
            foreach (Portal portal in Rooms[i].portals)
            {
                portal.TargetPortal = null;
            }
        }
    }


    void UpdateRooms()
    {
        var volumes = FindObjectsOfType<PortalOcclusionVolume>();

        Rooms = new List<Room>();

        foreach (var volume in volumes)
        {
            Room room;

            room.volume = volume;
            room.portals = volume.Portals;

            Rooms.Add(room);
        }
    }

    #region IDataPersistance
    public Dictionary<string, object> Save()
    {
        //create new dictionary to contain data for characterbase
        Dictionary<string, object> dataDictionary = new Dictionary<string, object>();
        if (!uuid)
            return dataDictionary;

        //Load currently saved values
        DataPersitanceHelpers.LoadDictionary(ref dataDictionary, uuid.ID);

        DataPersitanceHelpers.SaveValueToDictionary(ref dataDictionary, "AlertMeter", AlertMeter);

        //save json to file
        DataPersitanceHelpers.SaveDictionary(ref dataDictionary, uuid.ID);

        return dataDictionary;
    }

    public Dictionary<string, object> Load(bool destroyUnloaded = false)
    {
        //create new dictionary to contain data for characterbase
        Dictionary<string, object> dataDictionary = new Dictionary<string, object>();

        if (!uuid)
            return dataDictionary;

        //load dictionary
        DataPersitanceHelpers.LoadDictionary(ref dataDictionary, uuid.ID);

        AlertMeter = DataPersitanceHelpers.GetValueFromDictionary<int>(ref dataDictionary, "AlertMeter");

        return dataDictionary;
    }
    #endregion
}


