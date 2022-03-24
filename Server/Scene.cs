using System.Collections.Generic;

namespace GameServer
{
    class Scene
    {
        public List<RectCollider> obstacles;

        public void LoadFromFile(string path)
        {
            SceneParser parser = new SceneParser();
            obstacles = parser.ParseScene(path);
        }
    }
}
