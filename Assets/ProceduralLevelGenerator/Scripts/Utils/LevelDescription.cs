﻿using System;
using System.Collections.Generic;
using System.Linq;
using Assets.ProceduralLevelGenerator.Scripts.Data.Graphs;
using GeneralAlgorithms.DataStructures.Common;
using GeneralAlgorithms.DataStructures.Graphs;
using MapGeneration.Core.MapDescriptions;
using MapGeneration.Interfaces.Core.MapDescriptions;
using UnityEngine;

namespace Assets.ProceduralLevelGenerator.Scripts.Utils
{
    // TODO: where to place this file?
    public class LevelDescription<TRoom, TConnection>
        where TConnection : IConnection<TRoom>
    {
        private readonly List<TConnection> connections = new List<TConnection>();
        private readonly List<CorridorRoomDescription> corridorRoomDescriptions = new List<CorridorRoomDescription>();
        private readonly TwoWayDictionary<TRoom, TConnection> corridorToConnectionMapping = new TwoWayDictionary<TRoom, TConnection>();
        private readonly MapDescription<TRoom> mapDescription = new MapDescription<TRoom>();
        private readonly TwoWayDictionary<GameObject, IRoomTemplate> prefabToRoomTemplateMapping = new TwoWayDictionary<GameObject, IRoomTemplate>();

        private readonly RoomShapesLoader roomShapesLoader = new RoomShapesLoader();

        public void AddRoom(TRoom room, List<GameObject> roomTemplates)
        {
            if (room == null) throw new ArgumentNullException(nameof(room));

            mapDescription.AddRoom(room, GetBasicRoomDescription(roomTemplates));
        }

        public void AddRoom(TRoom room, GameObject roomTemplate)
        {
            if (room == null) throw new ArgumentNullException(nameof(room));

            AddRoom(room, new List<GameObject> {roomTemplate});
        }

        public void AddConnection(TConnection connection)
        {
            if (connection == null) throw new ArgumentNullException(nameof(connection));

            connections.Add(connection);
            mapDescription.AddConnection(connection.From, connection.To);
        }

        public void AddCorridorConnection(TConnection connection, List<GameObject> corridorRoomTemplates, TRoom corridorRoom)
        {
            if (connection == null) throw new ArgumentNullException(nameof(connection));
            if (corridorRoom == null) throw new ArgumentNullException(nameof(corridorRoom));

            connections.Add(connection);
            corridorToConnectionMapping.Add(corridorRoom, connection);

            var corridorRoomDescription = GetCorridorRoomDescription(corridorRoomTemplates);
            mapDescription.AddRoom(corridorRoom, corridorRoomDescription);
            mapDescription.AddConnection(connection.From, corridorRoom);
            mapDescription.AddConnection(connection.To, corridorRoom);
        }

        private BasicRoomDescription GetBasicRoomDescription(List<GameObject> roomTemplatePrefabs)
        {
            return new BasicRoomDescription(roomTemplatePrefabs.Select(GetRoomTemplate).ToList());
        }

        private CorridorRoomDescription GetCorridorRoomDescription(List<GameObject> roomTemplatePrefabs)
        {
            foreach (var existingRoomDescription in corridorRoomDescriptions)
            {
                var existingPrefabs = existingRoomDescription
                    .RoomTemplates
                    .Select(x => prefabToRoomTemplateMapping.GetByValue(x))
                    .ToList();

                if (existingPrefabs.SequenceEqual(roomTemplatePrefabs))
                {
                    return existingRoomDescription;
                }
            }

            var corridorRoomDescription = new CorridorRoomDescription(roomTemplatePrefabs.Select(GetRoomTemplate).ToList());
            corridorRoomDescriptions.Add(corridorRoomDescription);

            return corridorRoomDescription;
        }

        private IRoomTemplate GetRoomTemplate(GameObject roomTemplatePrefab)
        {
            if (prefabToRoomTemplateMapping.ContainsKey(roomTemplatePrefab))
            {
                return prefabToRoomTemplateMapping[roomTemplatePrefab];
            }

            var roomTemplate = roomShapesLoader.GetRoomTemplate(roomTemplatePrefab);
            prefabToRoomTemplateMapping.Add(roomTemplatePrefab, roomTemplate);

            return roomTemplate;
        }

        // TODO: how to name this?
        public IMapDescription<TRoom> GetMapDescription()
        {
            return mapDescription;
        }

        public IGraph<TRoom> GetGraph()
        {
            return mapDescription.GetGraph();
        }

        public IGraph<TRoom> GetGraphWithoutCorridors()
        {
            return mapDescription.GetStageOneGraph();
        }

        public TwoWayDictionary<GameObject, IRoomTemplate> GetPrefabToRoomTemplateMapping()
        {
            return prefabToRoomTemplateMapping;
        }

        public TwoWayDictionary<TRoom, TConnection> GetCorridorToConnectionMapping()
        {
            return corridorToConnectionMapping;
        }
    }

    public class LevelDescription : LevelDescription<Room, Connection>
    {
    }
}