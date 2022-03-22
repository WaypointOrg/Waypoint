using System;
using System.Dynamic;
using System.Collections.Generic;
using System.IO;
using YamlDotNet.Serialization;
using System.Linq;
using System.Numerics;

namespace GameServer 
{
    class SceneParser 
    {
        public static List<RectCollider> ParseScene(string path)
        {
            string file = File.ReadAllText(path);
            Dictionary<string, dynamic> objects = ParseFiles(file);
            List <dynamic> transforms = GetTransforms(objects);
            return TransfromToCollider(transforms);
        }

        public static dynamic ParseYaml(string s)
        {
            var deserializer = new DeserializerBuilder().Build();
            return deserializer.Deserialize<ExpandoObject>(s);
        }

        public static Dictionary<string, dynamic> ParseFiles(string file)
        {
            string[] files_array = file.Split("--- ");
            List<string> files = files_array.ToList<string>();
            files.RemoveAt(0);

            
            Dictionary<string, dynamic> objects = new Dictionary<string, dynamic>();
            foreach (var f in files)
            {
                string[] splits = f.Split(Environment.NewLine, 2);
                string header = splits[0];
                string document = splits[1];
                string object_id = header.Split("&")[1];

                objects[object_id] = ParseYaml(document);
            }

            return objects;
        }

        public static List<dynamic> GetTransforms(Dictionary<string, dynamic> objects){
            List<dynamic> transfroms = new List<dynamic>();
            foreach (KeyValuePair<string, dynamic> element in objects)
            {
                string type = ((IDictionary<String, Object>) element.Value).Keys.First();

                if (type == "GameObject")
                {
                    dynamic gameObject = element.Value.GameObject;

                    if (gameObject["m_TagString"] != "ServerCollideable")
                    {
                        continue;
                    }

                    foreach (var component in gameObject["m_Component"])
                    {
                        string fileID = component["component"]["fileID"];
                        dynamic component_object = objects[fileID];
                        string component_type = ((IDictionary<String, Object>) component_object).Keys.First();

                        if (component_type == "Transform"){
                            transfroms.Add(component_object.Transform);
                        }
                    }
                }
            }
            return transfroms;
        }

        public static List<RectCollider> TransfromToCollider(List <dynamic> transforms)
        {
            List<RectCollider> colliders = new List<RectCollider>();
            foreach (var transform in transforms)
            {
                colliders.Add(new RectCollider(
                    new Vector2(float.Parse(transform["m_LocalPosition"]["x"]), float.Parse(transform["m_LocalPosition"]["y"])),
                    new Vector2(float.Parse(transform["m_LocalScale"]["x"]),    float.Parse(transform["m_LocalScale"]["y"]))
                ));
            }

            return colliders;
        }
    }
}