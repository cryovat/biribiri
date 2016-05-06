using Bridge;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiriBiri
{
    public abstract class EntityBase : IManaged, IUpdatable
    {
        private object _components = new object();
        private ComponentBase[] _allComponents = new ComponentBase[0];

        protected void CopyComponentsTo(EntityBase copy, Action<ComponentBase> setOwnerCallback)
        {
            for (int i = 0; i < _allComponents.Length; i++)
            {
                var component = _allComponents[i].Copy();
                Script.Set(copy._components, component.GetClassName(), component);
                setOwnerCallback(component);
                copy._allComponents.Push(component);
            }
        }

        protected TComponent ResolveComponent<TComponent>() where TComponent : ComponentBase, new()
        {
            var id = typeof(TComponent).GetClassName();
            var component = Script.Get<TComponent>(_components, id);
            if (component == null)
            {
                component = new TComponent();
                Script.Set(_components, id, component);

                _allComponents.Push(component);
            }

            return component;
        }

        public void Setup()
        {
            for (int i = 0; i < _allComponents.Length; i++)
            {
                _allComponents[i].Setup();
            }
        }

        public void Update(double delta)
        {
            for (int i = 0; i < _allComponents.Length; i++)
            {
                _allComponents[i].Update(delta);
            }
        }

        public void Teardown()
        {
            for (int i = 0; i < _allComponents.Length; i++)
            {
                _allComponents[i].Teardown();
            }
        }
    }
}
