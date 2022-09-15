using System;
using System.Dynamic;
using System.Collections.Generic;
using System.IO;
using YamlDotNet.Serialization;
using System.Linq;
using System.Numerics;
using System.Globalization;

namespace GameServer
{
    class Map
    {
        public List<RectCollider> obstacles = new List<RectCollider>();

        // Optional: Waiting room is the only room with trigger
        public RectCollider trigger = null;
        public List<Vector2> mapSpawns = new List<Vector2>();

        private string path;
        private Dictionary<string, dynamic> objects;

        public Map(string _path)
        {
            path = _path;
            Utilities.Log("Loading scene " + path);
            LoadFromFile();
        }

        private void LoadFromFile()
        {
            ParseObjects();
            ParseScene();
        }

        private dynamic ParseYaml(string s)
        {
            var deserializer = new DeserializerBuilder().Build();
            return deserializer.Deserialize<ExpandoObject>(s);
        }

        private void ParseObjects()
        {
            string file = File.ReadAllText(path);
            string[] files_array = file.Split("--- ");
            List<string> files = files_array.ToList<string>();
            files.RemoveAt(0);

            objects = new Dictionary<string, dynamic>();
            foreach (var f in files)
            {
                string[] splits = f.Split("\n", 2);
                string header = splits[0];
                string document = splits[1];
                string object_id = header.Split("&")[1];

                objects[object_id] = ParseYaml(document);
            }
        }

        private void ParseScene()
        {
            foreach (KeyValuePair<string, dynamic> element in objects)
            {
                string type = ((IDictionary<String, Object>)element.Value).Keys.First();

                if (type == "GameObject")
                {
                    dynamic gameObject = element.Value.GameObject;
                    string tag = gameObject["m_TagString"];

                    switch (tag)
                    {
                        case "ServerCollideable":
                            obstacles.Add(ObjectToCollider(gameObject));
                            break;
                        case "ServerTrigger":
                            trigger = ObjectToCollider(gameObject);
                            break;
                        // case "ServerMapCenters":
                        //     Vector2 center_position = ObjectToVector2(gameObject);
                        //     string center_name = gameObject["m_Name"];
                        //     int center_index = int.Parse(center_name.Substring(center_name.Length - 1));
                        //     mapCenters[center_index] = center_position;
                        //     break;
                        case "ServerSpawn":
                            Vector2 spawn_position = ObjectToVector2(gameObject);                            
                            mapSpawns.Add(spawn_position);
                            break;
                    }

                }
            }
            if (!obstacles.Any())
            {
                throw new Exception($"No obstacles found in scene {path}");
            }
        }

        private RectCollider ObjectToCollider(dynamic gameObject)
        {
            foreach (var component in gameObject["m_Component"])
            {
                string fileID = component["component"]["fileID"];
                dynamic component_object = objects[fileID];
                string component_type = ((IDictionary<String, Object>)component_object).Keys.First();

                if (component_type == "Transform")
                {
                    dynamic transform = component_object.Transform;
                    if (float.Parse(transform["m_LocalRotation"]["x"]) != 0 ||
                        float.Parse(transform["m_LocalRotation"]["y"]) != 0 ||
                        float.Parse(transform["m_LocalRotation"]["z"]) != 0)
                    {
                        throw new Exception($"[Import Error]: GameObject {gameObject["m_Name"]} is rotated. This is not supported");
                    }

                    return new RectCollider(
                        new Vector2(float.Parse(transform["m_LocalPosition"]["x"], CultureInfo.InvariantCulture), float.Parse(transform["m_LocalPosition"]["y"], CultureInfo.InvariantCulture)),
                        new Vector2(float.Parse(transform["m_LocalScale"]["x"], CultureInfo.InvariantCulture), float.Parse(transform["m_LocalScale"]["y"], CultureInfo.InvariantCulture))
                    );
                }
            }
            throw new Exception($"[Import Error]: GameObject {gameObject["m_Name"]} does not have a transform component");
        }

        private Vector2 ObjectToVector2(dynamic gameObject)
        {
            foreach (var component in gameObject["m_Component"])
                {
                    string fileID = component["component"]["fileID"];
                    dynamic component_object = objects[fileID];
                    string component_type = ((IDictionary<String, Object>)component_object).Keys.First();

                    if (component_type == "Transform")
                    {
                        dynamic position = component_object.Transform["m_LocalPosition"];
                        return new Vector2(float.Parse(position["x"], CultureInfo.InvariantCulture), float.Parse(position["y"], CultureInfo.InvariantCulture));
                    }
                }
                throw new Exception($"[Import Error]: GameObject {gameObject["m_Name"]} does not have a transform component");
            }
    }
}
