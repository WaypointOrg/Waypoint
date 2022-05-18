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
    class Scene
    {
        public List<RectCollider> obstacles;
        public RectCollider trigger;

        public float width;
        public float height;

        private string path;
        private Dictionary<string, dynamic> objects;

        public Scene(string _path)
        {
            path = _path;
            LoadFromFile();
        }

        private void LoadFromFile()
        {
            ParseObjects();
            ParseColliders();
            GetWidth();
        }

        private void GetWidth()
        {
            float minX = 0;
            float minY = 0;
            float maxX = 0;
            float maxY = 0;
            foreach (RectCollider obstacle in obstacles)
            {
                minX = Math.Min(minX, obstacle.position.X + obstacle.scale.X / 2);
                minY = Math.Min(minY, obstacle.position.Y + obstacle.scale.Y / 2);
                maxX = Math.Max(maxX, obstacle.position.X - obstacle.scale.X / 2);
                maxY = Math.Max(maxY, obstacle.position.Y - obstacle.scale.Y / 2);

                Console.WriteLine($"Obstacle: {obstacle.position.X}, {obstacle.position.Y}, {obstacle.scale.X}, {obstacle.scale.Y}");
            }

            width = maxX - minX;
            height = maxY - minY;

            Console.WriteLine($"Width: {width}, Height: {height}");
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
                string[] splits = f.Split(Environment.NewLine, 2);
                string header = splits[0];
                string document = splits[1];
                string object_id = header.Split("&")[1];

                objects[object_id] = ParseYaml(document);
            }
        }
    
        private void ParseColliders(){
            obstacles = new List<RectCollider>();
            foreach (KeyValuePair<string, dynamic> element in objects)
            {
                string type = ((IDictionary<String, Object>) element.Value).Keys.First();

                if (type == "GameObject")
                {
                    dynamic gameObject = element.Value.GameObject;

                    if (gameObject["m_TagString"] == "ServerCollideable")
                    {
                        RectCollider collider = ObjectToCollider(gameObject);
                        if (collider != null)
                        {
                            obstacles.Add(collider);
                        }
                    } else if (gameObject["m_TagString"] == "ServerTrigger")
                    {
                        trigger = ObjectToCollider(gameObject);
                    }
                }
            }
            if (trigger == null)
            {
                throw new Exception($"No trigger found in scene {path}");
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
                string component_type = ((IDictionary<String, Object>) component_object).Keys.First();

                if (component_type == "Transform"){
                    dynamic transform = component_object.Transform;
                    if (float.Parse(transform["m_LocalRotation"]["x"]) != 0 || 
                        float.Parse(transform["m_LocalRotation"]["y"]) != 0 || 
                        float.Parse(transform["m_LocalRotation"]["z"]) != 0)
                        {
                            Console.WriteLine($"[Import Error]: GameObject {gameObject["m_Name"]} is rotated. This is not supported. The object was ignored");
                            return null;
                        }

                    return new RectCollider(
                        new Vector2(float.Parse(transform["m_LocalPosition"]["x"], CultureInfo.InvariantCulture), float.Parse(transform["m_LocalPosition"]["y"], CultureInfo.InvariantCulture)),
                        new Vector2(float.Parse(transform["m_LocalScale"]["x"],    CultureInfo.InvariantCulture), float.Parse(transform["m_LocalScale"]["y"],    CultureInfo.InvariantCulture))
                    );
                }
            }
            Console.WriteLine($"[Import Error]: GameObject {gameObject["m_Name"]} is rotated. This is not supported. The object was ignored");
            return null;
        }
    }
}
