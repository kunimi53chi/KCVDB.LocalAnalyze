using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KCVDB.LocalAnalyze.IO
{
    sealed class Grouping<TKey, TElement> : IGrouping<TKey, TElement>
    {
        public TKey Key
        {
            get
            {
                return this.key;
            }
        }

        public IEnumerator<TElement> GetEnumerator()
        {
            return this.elements.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)this.elements).GetEnumerator();
        }

        public Grouping(TKey key, IEnumerable<TElement> elements)
        {
            this.key = key;
            this.elements = elements;
        }

        private readonly TKey key;
        private readonly IEnumerable<TElement> elements;
    }
}
