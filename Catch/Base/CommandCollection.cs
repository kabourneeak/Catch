﻿using System.Collections;
using System.Collections.Generic;

namespace Catch.Base
{
    public class CommandCollection : ICollection<IAgentCommand>, IUpdatable
    {
        private readonly List<IAgentCommand> _commands;

        public CommandCollection()
        {
            _commands = new List<IAgentCommand>();
        }

        #region IUpdatable implementation

        public void Update(float ticks)
        {
            _commands.ForEach(c => c.Update(ticks));
        }

        #endregion

        #region ICollection implementation

        public IEnumerator<IAgentCommand> GetEnumerator() => _commands.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public void Add(IAgentCommand item) => _commands.Add(item);

        public void Clear() => _commands.Clear();

        public bool Contains(IAgentCommand item) => _commands.Contains(item);

        public void CopyTo(IAgentCommand[] array, int arrayIndex) => _commands.CopyTo(array, arrayIndex);

        public bool Remove(IAgentCommand item) => _commands.Remove(item);

        public int Count => _commands.Count;

        public bool IsReadOnly => false;

        #endregion

        public void AddRange(IEnumerable<IAgentCommand> collection)
        {
            foreach (var item in collection)
                _commands.Add(item);
        }
    }
}
