using BiriBiri.Utils;

namespace BiriBiri
{
    public class Scene : IManaged, IUpdatable, IDrawable
    {
        private QuadTree<Entity> _entities = new QuadTree<Entity>();
        private EntityPool _pool = new EntityPool();

        public readonly Game Game;

        public Scene(Game game)
        {
            Game = game;
        }

        public void AddEntity(Entity entity)
        {
            entity.Scene = this;
            entity.Setup();

            _entities.Add(entity);
        }

        public void AddEntity(string id, double x, double y)
        {
            var entity = _pool.Get(id);
            entity.Position.X = x;
            entity.Position.Y = y;

            entity.Scene = this;
            entity.Setup();

            _entities.Add(entity);
        }

        public void Draw()
        {

        }

        public void Clear()
        {

        }

        public void Setup()
        {
            _pool.PresetManager = Game.Presets;
        }

        public void Teardown()
        {
            _entities.Apply(e =>
            {
                e.Teardown();
                _pool.Return(e.EntityPoolKey, e);
            });

            _entities.Clear();

            _pool.PresetManager = null;
        }

        public void Update(double delta)
        {
            _entities.Apply(e => e.Update(delta));
        }
    }
}
