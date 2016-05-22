using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KCVDB.LocalAnalyze.IO
{
    internal sealed class ArrayBuilder<T>
    {
        public void Append(T[] sourceArray, int sourceOffset, int sourceCount)
        {
            if (this.array == null)
            {
                this.array = new T[this.Count + sourceCount];
            }
            else if (this.array.Length < this.count + sourceCount)
            {
                var bytes = new T[Math.Max(this.array.Length * 2, this.count + sourceCount)];
                System.Array.Copy(this.array, 0, bytes, 0, this.count);
                this.array = bytes;
            }
            System.Array.Copy(sourceArray, sourceOffset, this.array, this.count, sourceCount);
            this.count += sourceCount;
        }

        public void Clear()
        {
            this.count = 0;
        }

        public T[] Array
        {
            get
            {
                return this.array;
            }
        }

        public int Count
        {
            get
            {
                return this.count;
            }
        }

        private T[] array;
        private int count;
    }
}
