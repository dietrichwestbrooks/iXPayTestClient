using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Wayne.Payment.Tools.iXPayTestClient.Business.Messaging
{
    public abstract class TerminalObjectCollection<TObject> : IEnumerable<TObject>
        where TObject : INamedObject
    {
        protected TerminalObjectCollection()
        {
            
        }

        protected TerminalObjectCollection(IEnumerable<TObject> objects)
        {
            AddRange(objects);
        }

        private List<TObject> _objects = new List<TObject>();

        public TObject this[string name]
        {
            get
            {
                var @object = _objects.FirstOrDefault(o => o.Name == name);

                if (@object == null)
                    throw new IndexOutOfRangeException(name);

                return @object;
            }
        }

        public void AddRange(IEnumerable<TObject> objects)
        {
            if (objects == null)
                return;

            foreach (var o in objects)
            {
                Add(o);
            }
        }

        public void Add(TObject @object)
        {
            if (@object == null)
                return;

            _objects.Add(@object);
            OnObjectAdded(@object);
        }

        public IEnumerator<TObject> GetEnumerator()
        {
            return _objects.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        protected virtual void OnObjectAdded(TObject @object)
        {
        }
    }
}
