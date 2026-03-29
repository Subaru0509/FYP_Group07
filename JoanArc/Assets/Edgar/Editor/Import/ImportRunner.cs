using System.Collections.Generic;
using System.IO;
using System.Linq;
using Edgar.Unity.Export;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Edgar.Unity.Editor
{
    #if OndrejNepozitekEdgar
    public class ImportRunner
    {
        private readonly ExportDto _export;
        private readonly string _directory;
        private readonly TileBase _floorTile;
        private readonly TileBase _wallTile;

        public ImportRunner(ExportDto export, string directory)
        {
            _export = export;
            _directory = directory;
            _floorTile = AssetDatabase.LoadAssetAtPath<Tile>(@"Assets\Edgar\Examples\Grid2D\Example1\Tiles\example1_79.asset");
            _wallTile = AssetDatabase.LoadAssetAtPath<Tile>(@"Assets\Edgar\Examples\Grid2D\Example1\Tiles\example1_69.asset");
        }

        public void Run()
        {
            HandleRoomTemplates();
            HandleLevelGraph();
            HandleScene();
        }

        private void HandleScene()
        {
            var directoryName = Path.GetFileName(_directory);
            var scenePath = Path.Combine(_directory, $"{directoryName}.unity");
            
            if (File.Exists(scenePath))
            {
                File.Delete(scenePath);
            }
            
            var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            EditorSceneManager.SaveScene(scene, scenePath);
            
            var generator = new GameObject("Generator");
            var dungeonGenerator = generator.AddComponent<DungeonGeneratorGrid2D>();
            generator.AddComponent<ExportPostProcessing>();
            
            var levelGraph = AssetDatabase.LoadAssetAtPath<LevelGraph>(GetLevelGraphPath());
            dungeonGenerator.FixedLevelGraphConfig = new FixedLevelGraphConfigGrid2D()
            {
                LevelGraph = levelGraph,
            };
            
            EditorSceneManager.SaveScene(scene);
        }

        private void HandleRoomTemplates()
        {
            var roomTemplatesDirectory = GetRoomTemplatesFolder();
            if (File.Exists(roomTemplatesDirectory))
            {
                Directory.Delete(roomTemplatesDirectory, true);
            }
            Directory.CreateDirectory(roomTemplatesDirectory);

            foreach (var roomTemplateDto in _export.RoomTemplates)
            {
                CreateRoomTemplate(roomTemplateDto);
            }
        }

        private void CreateRoomTemplate(RoomTemplateDto roomTemplateDto)
        {
            var path = Path.Combine(GetRoomTemplatesFolder(), $"{roomTemplateDto.Name}.prefab");
            
            var go = new GameObject(roomTemplateDto.Name);
            
            var roomTemplateSettings = go.AddComponent<RoomTemplateSettingsGrid2D>();
            roomTemplateSettings.RepeatMode = roomTemplateDto.RepeatMode;
            go.AddComponent<DoorsGrid2D>();

            var tilemaps = new GameObject(GeneratorConstantsGrid2D.TilemapsRootName);
            tilemaps.transform.SetParent(go.transform);
            tilemaps.AddComponent<Grid>();

            {
                var layer = new GameObject("Floor");
                layer.transform.SetParent(tilemaps.transform);
                var tilemap = layer.AddComponent<Tilemap>();
                var tilemapRenderer = layer.AddComponent<TilemapRenderer>();
                tilemapRenderer.sortingOrder = 0;
                
                foreach (var position in roomTemplateDto.Tiles)
                {
                    tilemap.SetTile(position, _floorTile);
                }
            }
            
            {
                var layer = new GameObject("Walls");
                layer.transform.SetParent(tilemaps.transform);
                var tilemap = layer.AddComponent<Tilemap>();
                var tilemapRenderer = layer.AddComponent<TilemapRenderer>();
                tilemapRenderer.sortingOrder = 1;
                
                foreach (var position in roomTemplateDto.OutlineTiles)
                {
                    tilemap.SetTile(position, _wallTile);
                }
            }
            
            PrefabUtility.SaveAsPrefabAsset(go, path);
            
            Object.DestroyImmediate(go);
            
            // For some reason, the nested doors data are not saved the first time
            var loadedAgain = PrefabUtility.LoadPrefabContents(path);
            var doors = loadedAgain.GetComponent<DoorsGrid2D>();
            var doorsDto = roomTemplateDto.Doors;
            doors.SelectedMode = doorsDto.SelectedMode;
            doors.HybridDoorModeData = doorsDto.HybridDoorModeData;
            doors.ManualDoorModeData = doorsDto.ManualDoorModeData;
            doors.SimpleDoorModeData = doorsDto.SimpleDoorModeData;
            PrefabUtility.SaveAsPrefabAsset(loadedAgain, path);
            
            // Remove game object from scene
            Object.DestroyImmediate(loadedAgain);
        }

        private void HandleLevelGraph()
        {
            var levelGraphDto = _export.LevelGraph;
            var levelGraphPath = GetLevelGraphPath();

            if (File.Exists(levelGraphPath))
            {
                File.Delete(levelGraphPath);
            }
            
            var levelGraph = ScriptableObject.CreateInstance<LevelGraph>();
            AssetDatabase.CreateAsset(levelGraph, levelGraphPath);
            
            var roomTemplatesFolder = GetRoomTemplatesFolder();
            
            levelGraph.name = "Level graph";
            levelGraph.EditorData = new LevelGraphEditorData()
            {
                Zoom = 0.7f,
                PanOffset = new Vector2(222f, -55f),
            };
            levelGraph.Rooms = new List<RoomBase>();
            levelGraph.Connections = new List<ConnectionBase>();
            
            levelGraphDto.DefaultRoomTemplates.ForEach(x =>
            {
                var prefabPath = Path.Combine(roomTemplatesFolder, $"{x}.prefab");
                var roomTemplate = AssetDatabase.LoadAssetAtPath(prefabPath, typeof(GameObject));
                levelGraph.DefaultIndividualRoomTemplates.Add(roomTemplate as GameObject);
            });
            
            levelGraphDto.DefaultCorridorRoomTemplates.ForEach(x =>
            {
                var prefabPath = Path.Combine(roomTemplatesFolder, $"{x}.prefab");
                var roomTemplate = AssetDatabase.LoadAssetAtPath(prefabPath, typeof(GameObject));
                levelGraph.CorridorIndividualRoomTemplates.Add(roomTemplate as GameObject);
            });

            foreach (var roomDto in levelGraphDto.Rooms)
            {
                var room = ScriptableObject.CreateInstance<Room>();
                AssetDatabase.AddObjectToAsset(room, levelGraph);
                
                room.Name = roomDto.DisplayName;
                room.name = roomDto.Id;
                room.Position = roomDto.Position;
                
                roomDto.RoomTemplates.ForEach(x =>
                {
                    var prefabPath = Path.Combine(roomTemplatesFolder, $"{x}.prefab");
                    var roomTemplate = AssetDatabase.LoadAssetAtPath(prefabPath, typeof(GameObject));
                    room.IndividualRoomTemplates.Add(roomTemplate as GameObject);
                });

                levelGraph.Rooms.Add(room);
            }

            foreach (var connectionDto in levelGraphDto.Connections)
            {
                var connection = ScriptableObject.CreateInstance<ExportConnection>();
                AssetDatabase.AddObjectToAsset(connection, levelGraph);

                connection.From = levelGraph.Rooms.Single(x => x.name == connectionDto.From);
                connection.To = levelGraph.Rooms.Single(x => x.name == connectionDto.To);
                connection.name = $"C: {connectionDto.From} -> {connectionDto.To}";
                
                connectionDto.RoomTemplates.ForEach(x =>
                {
                    var prefabPath = Path.Combine(roomTemplatesFolder, $"{x}.prefab");
                    var roomTemplate = AssetDatabase.LoadAssetAtPath(prefabPath, typeof(GameObject));
                    connection.RoomTemplates.Add(roomTemplate as GameObject);
                });
                
                levelGraph.Connections.Add(connection);
            }
            
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
        
        private string GetLevelGraphPath()
        {
            return Path.Combine(_directory, $"Level graph.asset");
        }
        
        private string GetRoomTemplatesFolder()
        {
            return Path.Combine(_directory, "Room templates");
        }
    }
    #endif
}